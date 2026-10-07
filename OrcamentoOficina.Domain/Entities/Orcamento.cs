using OrcamentoOficina.Domain.Enums;
using OrcamentoOficina.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrcamentoOficina.Domain.Entities
{
    public sealed class Orcamento
    {
        private readonly List<ItemOrcamento> _itens = [];

        private readonly List<EventoOrcamento> _eventos = [];

        public byte[] RowVersion { get; private set; } = null!;

        public Guid Id { get; private set; }

        public Guid ClienteId { get; private set; }

        public Guid VeiculoId { get; private set; }

        public Guid GrupoVersaoId { get; private set; }

        public int Versao { get; private set; }

        public Guid? OrcamentoOriginalId { get; private set; }

        public DateTimeOffset CriadoEm { get; private set; }

        public DateTimeOffset ValidadeEm { get; private set; }

        public DateTimeOffset? EnviadoEm { get; private set; }

        public StatusOrcamento Status { get; private set; }

        public Desconto DescontoGeral { get; private set; } = null!;

        public string? MotivoReprovacao { get; private set; }

        public string? UsuarioAutorizadorDescontoGeral
        {
            get;
            private set;
        }

        public IReadOnlyCollection<ItemOrcamento> Itens => _itens;

        public IReadOnlyCollection<EventoOrcamento> Eventos => _eventos;

        public decimal Subtotal => Math.Round(_itens.Sum(item => item.Subtotal), 2, MidpointRounding.AwayFromZero);

        public decimal ValorDescontoItens => Math.Round(_itens.Sum(item => item.ValorDesconto), 2, MidpointRounding.AwayFromZero);

        public decimal TotalAntesDescontoGeral => Subtotal - ValorDescontoItens;

        public decimal ValorDescontoGeral => DescontoGeral.Calcular(TotalAntesDescontoGeral);

        public decimal Total => Math.Round(TotalAntesDescontoGeral - ValorDescontoGeral, 2, MidpointRounding.AwayFromZero);

        private Orcamento(Guid clienteId, Guid veiculoId, DateTimeOffset criadoEm, DateTimeOffset validadeEm, int versao, Guid orcamentoOriginalId, Guid grupoVersaoId)
        {
            Id = Guid.NewGuid();

            ClienteId = clienteId;
            VeiculoId = veiculoId;

            CriadoEm = criadoEm;
            ValidadeEm = validadeEm;

            Versao = versao;
            OrcamentoOriginalId = orcamentoOriginalId;
            GrupoVersaoId = grupoVersaoId;

            Status = StatusOrcamento.Rascunho;

            DescontoGeral = new Desconto(TipoDesconto.ValorFixo, 0);
        }

        public Orcamento(Guid clienteId, Guid veiculoId, DateTimeOffset criadoEm, DateTimeOffset validadeEm)
        {
            if (clienteId == Guid.Empty)
                throw new ArgumentException("Cliente obrigatório.");

            if (veiculoId == Guid.Empty)
                throw new ArgumentException("Veículo obrigatório.");

            if (validadeEm <= criadoEm)
                throw new ArgumentException("A validade deve ser posterior à criação.");

            Id = Guid.NewGuid();
            ClienteId = clienteId;
            VeiculoId = veiculoId;            
            Versao = 1;
            GrupoVersaoId = Id;
            CriadoEm = criadoEm;
            ValidadeEm = validadeEm;
            Status = StatusOrcamento.Rascunho;
            DescontoGeral = new Desconto(TipoDesconto.ValorFixo, 0);
        }

        public void AdicionarItem(ItemOrcamento item)
        {
            GarantirRascunho();

            ArgumentNullException.ThrowIfNull(item);

            _itens.Add(item);
        }

        public void AlterarItem(Guid itemId, decimal quantidade, decimal precoUnitario, Desconto desconto, string? usuarioAutorizadorDesconto)
        {
            GarantirRascunho();

            var item = _itens.SingleOrDefault(x => x.Id == itemId) ?? throw new InvalidOperationException("Item não encontrado no orçamento.");

            item.Alterar(quantidade, precoUnitario, desconto, usuarioAutorizadorDesconto);
        }

        public void RemoverItem(Guid itemId)
        {
            GarantirRascunho();

            var item = _itens.SingleOrDefault(x => x.Id == itemId) ?? throw new InvalidOperationException("Item não encontrado no orçamento.");

            _itens.Remove(item);
        }

        public void DefinirDescontoGeral(Desconto desconto, string? usuarioAutorizador = null)
        {
            GarantirRascunho();
            ArgumentNullException.ThrowIfNull(desconto);

            if (desconto.PercentualEquivalente(TotalAntesDescontoGeral) > 15m
                && string.IsNullOrWhiteSpace(usuarioAutorizador))
            {
                throw new InvalidOperationException("Descontos gerais acima de 15% exigem autorização.");
            }

            DescontoGeral = desconto;
            UsuarioAutorizadorDescontoGeral = string.IsNullOrWhiteSpace(usuarioAutorizador) ? null : usuarioAutorizador.Trim();
        }

        public EventoOrcamento Enviar(string usuario, DateTimeOffset agora)
        {
            GarantirRascunho();

            if (string.IsNullOrWhiteSpace(usuario))
                throw new ArgumentException("O usuário é obrigatório.",
                    nameof(usuario));

            if (_itens.Count == 0)
                throw new InvalidOperationException("Não é possível enviar um orçamento sem itens.");

            if (agora > ValidadeEm)
                throw new InvalidOperationException("Não é possível enviar um orçamento expirado.");

            Status = StatusOrcamento.Enviado;
            EnviadoEm = agora;

            var evento = new EventoOrcamento(Id, "OrcamentoEnviado", usuario.Trim(), agora, null);

            _eventos.Add(evento);

            return evento;
        }

        public EventoOrcamento? AprovarItens(IReadOnlyCollection<Guid> itensIds, string usuario, DateTimeOffset agora, CanalAprovacao canal)
        {
            if (Status != StatusOrcamento.Enviado && Status != StatusOrcamento.AprovadoParcialmente && Status != StatusOrcamento.Aprovado)
                throw new InvalidOperationException("O orçamento não está em um estado que permita aprovação.");

            if (agora > ValidadeEm)
            {
                Status = StatusOrcamento.Expirado;
                throw new InvalidOperationException("Não é possível aprovar um orçamento expirado.");
            }
            

            if (itensIds is null || itensIds.Count == 0)
                throw new ArgumentException("Informe pelo menos um item para aprovação.", nameof(itensIds));
            
            if (string.IsNullOrWhiteSpace(usuario))
                throw new ArgumentException("O usuário da aprovação é obrigatório.",nameof(usuario));
            
            var idsDistintos = itensIds.Distinct().ToHashSet();

            var selecionados = _itens.Where(x => idsDistintos.Contains(x.Id)).ToList();

            if (selecionados.Count != idsDistintos.Count)
                throw new InvalidOperationException("Um ou mais itens informados não pertencem ao orçamento.");
                       
            var houveNovaAprovacao = selecionados.Any(x => !x.Aprovado);

            if (!houveNovaAprovacao)
                return null;
            
            foreach (var item in selecionados)
                item.Aprovar(usuario.Trim(), agora, canal);
            
            Status = _itens.All(x => x.Aprovado) ? StatusOrcamento.Aprovado : StatusOrcamento.AprovadoParcialmente;

            var evento = new EventoOrcamento(Id, Status == StatusOrcamento.Aprovado ? "OrcamentoAprovado" : "OrcamentoAprovadoParcialmente", usuario.Trim(), agora, $"Itens: {string.Join(",", idsDistintos)}");

            _eventos.Add(evento);

            return evento;
        }

        public EventoOrcamento Reprovar(string? motivo, string? usuario, DateTimeOffset agora)
        {
            if (Status != StatusOrcamento.Enviado)
                throw new InvalidOperationException("Somente orçamentos enviados podem ser reprovados.");
            
            if (string.IsNullOrWhiteSpace(motivo))
                throw new ArgumentException("O motivo da reprovação é obrigatório.", nameof(motivo));
            
            if (string.IsNullOrWhiteSpace(usuario))
                throw new ArgumentException("O usuário da reprovação é obrigatório.", nameof(usuario));
            
            Status = StatusOrcamento.Reprovado;
            MotivoReprovacao = motivo.Trim();

            var evento = new EventoOrcamento(Id, "OrcamentoReprovado", usuario.Trim(), agora, $"Motivo: {MotivoReprovacao}");

            _eventos.Add(evento);

            return evento;
        }

        public void AtualizarExpiracao(DateTimeOffset agora)
        {
            if (Status == StatusOrcamento.Enviado && agora >= ValidadeEm)
                Status = StatusOrcamento.Expirado;

            if (Status is not StatusOrcamento.Enviado and not StatusOrcamento.AprovadoParcialmente)
                return;
            
            if (agora < ValidadeEm)
                return;

            Status = StatusOrcamento.Expirado;

            RegistrarEvento("OrcamentoExpirado", "sistema", agora, $"Validade={ValidadeEm:O}");
        }

        private ItemOrcamento ObterItem(Guid itemId) => _itens.FirstOrDefault(item => item.Id == itemId) ?? throw new InvalidOperationException("Item não encontrado no orçamento.");

        private void GarantirRascunho()
        {
            if (Status != StatusOrcamento.Rascunho)
                throw new InvalidOperationException("O orçamento só pode ser alterado enquanto estiver em rascunho.");            
        }

        private void RegistrarEvento(string tipo, string usuario, DateTimeOffset ocorridoEm, string? dados = null)
        {
            _eventos.Add(new EventoOrcamento(Id, tipo, usuario, ocorridoEm, dados));
        }

        public Orcamento CriarRevisao(DateTimeOffset agora, DateTimeOffset novaValidade)
        {
            if (Status == StatusOrcamento.Rascunho)
                throw new InvalidOperationException("Um orçamento em rascunho não precisa ser revisado.");
            
            if (Status == StatusOrcamento.ConvertidoEmOrdemServico)
                throw new InvalidOperationException("Um orçamento convertido em ordem de serviço não pode ser revisado.");
            
            if (novaValidade <= agora)
                throw new ArgumentException("A validade da nova revisão deve ser futura.", nameof(novaValidade));
           
            var revisao = new Orcamento(clienteId: ClienteId, veiculoId: VeiculoId, criadoEm: agora, validadeEm: novaValidade, versao: Versao + 1, orcamentoOriginalId: Id, grupoVersaoId: GrupoVersaoId);

            foreach (var item in _itens)
                revisao.AdicionarItem(new ItemOrcamento(item.Descricao, item.Tipo, item.Quantidade, item.PrecoUnitario, new Desconto(item.Desconto.Tipo, item.Desconto.Valor), item.UsuarioAutorizadorDesconto, item.DisponivelEmEstoque));
            
            revisao.DefinirDescontoGeral(new Desconto(DescontoGeral.Tipo, DescontoGeral.Valor),UsuarioAutorizadorDescontoGeral);

            return revisao;
        }

        public EventoOrcamento RegistrarCriacaoRevisao(string usuario, DateTimeOffset agora)
        {
            if (string.IsNullOrWhiteSpace(usuario))
                throw new ArgumentException("O usuário é obrigatório.", nameof(usuario));
            
            var evento = new EventoOrcamento(Id, "RevisaoCriada", usuario.Trim(), agora, $"Revisão da versão anterior. Versão: {Versao}");

            _eventos.Add(evento);

            return evento;
        }

        public (OrdemServico OrdemServico, EventoOrcamento Evento) CriarOrdemServico(string usuario, DateTimeOffset agora)
        {
            if (Status != StatusOrcamento.Aprovado)
                throw new InvalidOperationException("Somente orçamentos totalmente aprovados podem ser convertidos em ordem de serviço.");
            
            if (string.IsNullOrWhiteSpace(usuario))
                throw new ArgumentException("O usuário é obrigatório.", nameof(usuario));
            
            var usuarioNormalizado = usuario.Trim();

            Status = StatusOrcamento.ConvertidoEmOrdemServico;

            var ordemServico = new OrdemServico(Id, agora, usuarioNormalizado);

            var evento = new EventoOrcamento(Id, "OrcamentoConvertidoEmOrdemServico", usuarioNormalizado, agora, $"OrdemServicoId: {ordemServico.Id}");

            _eventos.Add(evento);

            return (ordemServico, evento);
        }
    }
}
