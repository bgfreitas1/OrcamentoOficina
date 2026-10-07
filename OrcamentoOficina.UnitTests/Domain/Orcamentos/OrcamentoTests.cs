using OrcamentoOficina.Domain.Entities;
using OrcamentoOficina.Domain.Enums;
using OrcamentoOficina.Domain.ValueObjects;

namespace OrcamentoOficina.UnitTests.Domain.Orcamentos
{
    public sealed class OrcamentoTests
    {
        private static readonly DateTimeOffset Agora = new(2026, 10, 6, 12, 0, 0, TimeSpan.Zero);

        private static Orcamento CriarOrcamento()
        {
            return new Orcamento(Guid.NewGuid(), Guid.NewGuid(), Agora, Agora.AddDays(7));
        }

        private static ItemOrcamento CriarItem(decimal quantidade = 1, decimal precoUnitario = 100m, Desconto? desconto = null, string? autorizador = null)
        {
            return new ItemOrcamento("Pastilha de freio", TipoItemOrcamento.Peca, quantidade, precoUnitario, desconto, autorizador);
        }

        [Fact]
        public void CriarOrcamento_DeveIniciarComoRascunho()
        {
            var orcamento = CriarOrcamento();

            Assert.Equal(StatusOrcamento.Rascunho, orcamento.Status);
            Assert.Equal(1, orcamento.Versao);
            Assert.Empty(orcamento.Itens);
            Assert.Equal(0m, orcamento.Total);
        }

        [Fact]
        public void AdicionarItem_DeveCalcularTotalCorretamente()
        {
            var orcamento = CriarOrcamento();

            var item = CriarItem(quantidade: 2, precoUnitario: 100m);

            orcamento.AdicionarItem(item);

            Assert.Equal(200m, item.Subtotal);
            Assert.Equal(200m, item.Total);
            Assert.Equal(200m, orcamento.Total);
        }

        [Fact]
        public void ItemComDescontoPercentual_DeveCalcularTotalCorretamente()
        {
            var desconto = new Desconto(TipoDesconto.Percentual, 10m);

            var item = CriarItem(quantidade: 2, precoUnitario: 100m, desconto: desconto);

            Assert.Equal(200m, item.Subtotal);
            Assert.Equal(20m, item.ValorDesconto);
            Assert.Equal(180m, item.Total);
        }

        [Fact]
        public void DescontoAcimaDe15Porcento_SemAutorizador_DeveFalhar()
        {
            var desconto = new Desconto(TipoDesconto.Percentual, 20m);

            var exception = Assert.Throws<InvalidOperationException>(() => CriarItem(precoUnitario: 100m, desconto: desconto));

            Assert.Contains("usuário autorizador", exception.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void DescontoAcimaDe15Porcento_ComAutorizador_DeveSerPermitido()
        {
            var desconto = new Desconto(TipoDesconto.Percentual, 20m);

            var item = CriarItem(precoUnitario: 100m, desconto: desconto, autorizador: "gerente@oficina");

            Assert.Equal(20m, item.ValorDesconto);
            Assert.Equal(80m, item.Total);
            Assert.Equal("gerente@oficina", item.UsuarioAutorizadorDesconto);
        }

        [Fact]
        public void AdicionarItem_AposEnvio_DeveFalhar()
        {
            var agora = DateTimeOffset.UtcNow;

            var orcamento = CriarOrcamentoComItem(agora);

            orcamento.Enviar(
                "bruno",
                agora.AddMinutes(1));

            var exception = Assert.Throws<InvalidOperationException>(() =>
                orcamento.AdicionarItem(
                    CriarItem()));

            Assert.Contains(
                "rascunho",
                exception.Message,
                StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void AprovarApenasUmItem_DeveGerarAprovacaoParcial()
        {
            var orcamento = CriarOrcamento();

            var pastilha = new ItemOrcamento("Pastilha de freio", TipoItemOrcamento.Peca, 1, 200m);

            var amortecedor = new ItemOrcamento("Amortecedor", TipoItemOrcamento.Peca, 2, 500m);

            orcamento.AdicionarItem(pastilha);
            orcamento.AdicionarItem(amortecedor);

            orcamento.Enviar("recepcionista", Agora.AddHours(1));

            orcamento.AprovarItens([pastilha.Id], "cliente", Agora.AddHours(2), CanalAprovacao.WhatsApp);

            Assert.Equal(StatusOrcamento.AprovadoParcialmente, orcamento.Status);

            Assert.True(pastilha.Aprovado);
            Assert.False(amortecedor.Aprovado);

            Assert.Equal(CanalAprovacao.WhatsApp, pastilha.CanalAprovacao);

            Assert.Equal("cliente", pastilha.UsuarioAprovacao);
        }

        [Fact]
        public void AprovarItensRestantes_DeveTornarOrcamentoTotalmenteAprovado()
        {
            var orcamento = CriarOrcamento();

            var item1 = CriarItem();
            var item2 = CriarItem();

            orcamento.AdicionarItem(item1);
            orcamento.AdicionarItem(item2);

            orcamento.Enviar("recepcionista", Agora.AddHours(1));

            orcamento.AprovarItens([item1.Id], "cliente", Agora.AddHours(2), CanalAprovacao.WhatsApp);

            Assert.Equal(StatusOrcamento.AprovadoParcialmente, orcamento.Status);

            orcamento.AprovarItens([item2.Id], "cliente", Agora.AddHours(3), CanalAprovacao.WhatsApp);

            Assert.Equal(StatusOrcamento.Aprovado, orcamento.Status);

            Assert.All(orcamento.Itens, item => Assert.True(item.Aprovado));
        }

        [Fact]
        public void AprovarOrcamentoVencido_DeveFalhar()
        {
            var agora = DateTimeOffset.UtcNow;

            var orcamento = CriarOrcamentoComItem(agora);

            orcamento.Enviar(
                "bruno",
                agora.AddMinutes(1));

            var item = orcamento.Itens.Single();

            var exception = Assert.Throws<InvalidOperationException>(() =>
                orcamento.AprovarItens(
                    new[] { item.Id },
                    "bruno",
                    orcamento.ValidadeEm.AddMinutes(1),
                    CanalAprovacao.WhatsApp));

            Assert.Equal(
                StatusOrcamento.Expirado,
                orcamento.Status);

            Assert.False(item.Aprovado);

            Assert.Contains(
                "expirado",
                exception.Message,
                StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void CriarRevisao_DeveCriarNovaVersaoSemAlterarOriginal()
        {
            var original = CriarOrcamento();

            original.AdicionarItem(CriarItem(quantidade: 1, precoUnitario: 100m));

            original.Enviar("recepcionista", Agora.AddHours(1));

            var revisao = original.CriarRevisao(Agora.AddHours(2), Agora.AddDays(10));

            Assert.NotEqual(original.Id, revisao.Id);

            Assert.Equal(original.GrupoVersaoId, revisao.GrupoVersaoId);

            Assert.Equal(1, original.Versao);
            Assert.Equal(2, revisao.Versao);

            Assert.Equal(original.Id, revisao.OrcamentoOriginalId);

            Assert.Equal(StatusOrcamento.Enviado, original.Status);

            Assert.Equal(StatusOrcamento.Rascunho, revisao.Status);

            Assert.Single(original.Itens);
            Assert.Single(revisao.Itens);

            Assert.NotEqual(original.Itens.Single().Id, revisao.Itens.Single().Id);

            Assert.Equal(original.Total, revisao.Total);
        }

        [Fact]
        public void EnviarOrcamento_DeveRegistrarEventoDeAuditoria()
        {
            var orcamento = CriarOrcamento();

            orcamento.AdicionarItem(CriarItem());

            orcamento.Enviar("bruno", Agora.AddHours(1));

            var evento = Assert.Single(orcamento.Eventos);

            Assert.Equal("OrcamentoEnviado", evento.Tipo);

            Assert.Equal("bruno", evento.Usuario);

            Assert.Equal(Agora.AddHours(1), evento.OcorridoEm);
        }

        [Fact]
        public void AprovarItens_QuandoItemJaEstaAprovado_NaoDeveAlterarAprovacao()
        {
            var agora = DateTimeOffset.UtcNow;

            var orcamento = CriarOrcamentoComItem(agora);

            orcamento.Enviar("bruno", agora.AddMinutes(1));

            var item = orcamento.Itens.Single();

            orcamento.AprovarItens(
                new[] { item.Id },
                "bruno",
                agora.AddMinutes(2),
                CanalAprovacao.WhatsApp);

            var dataPrimeiraAprovacao = item.DataAprovacao;

            var evento = orcamento.AprovarItens(
                new[] { item.Id },
                "outro-usuario",
                agora.AddMinutes(10),
                CanalAprovacao.Link);

            Assert.Null(evento);
            Assert.True(item.Aprovado);
            Assert.Equal(dataPrimeiraAprovacao, item.DataAprovacao);
            Assert.Equal("bruno", item.UsuarioAprovacao);
            Assert.Equal(CanalAprovacao.WhatsApp, item.CanalAprovacao);
        }

        private static Orcamento CriarOrcamentoComItem(DateTimeOffset agora)
        {
            var orcamento = new Orcamento(Guid.NewGuid(), Guid.NewGuid(), agora, agora.AddDays(7));

            var item = new ItemOrcamento("Pastilha de freio", TipoItemOrcamento.Peca, 1, 100m, new Desconto(TipoDesconto.ValorFixo, 0), null, true);

            orcamento.AdicionarItem(item);

            return orcamento;
        }

        [Fact]
        public void CriarRevisao_DeveCriarNovaVersaoEmRascunho()
        {
            var agora = DateTimeOffset.UtcNow;

            var original = CriarOrcamentoComItem(agora);

            original.Enviar(
                "bruno",
                agora.AddMinutes(1));

            var revisao = original.CriarRevisao(
                agora.AddHours(1),
                agora.AddDays(7));

            Assert.NotEqual(original.Id, revisao.Id);

            Assert.Equal(1, original.Versao);
            Assert.Equal(2, revisao.Versao);

            Assert.Equal(
                original.GrupoVersaoId,
                revisao.GrupoVersaoId);

            Assert.Equal(
                original.Id,
                revisao.OrcamentoOriginalId);

            Assert.Equal(
                StatusOrcamento.Rascunho,
                revisao.Status);

            Assert.Single(revisao.Itens);

            // O original continua intacto.
            Assert.Equal(
                StatusOrcamento.Enviado,
                original.Status);
        }

        [Fact]
        public void CriarOrdemServico_QuandoNaoAprovado_DeveFalhar()
        {
            var agora = DateTimeOffset.UtcNow;

            var orcamento = CriarOrcamentoComItem(agora);

            var exception = Assert.Throws<InvalidOperationException>(() =>
                orcamento.CriarOrdemServico(
                    "bruno",
                    agora));

            Assert.Contains(
                "totalmente aprovados",
                exception.Message);
        }

        [Fact]
        public void CriarOrdemServico_QuandoAprovado_DeveCriarOSEEvento()
        {
            var agora = DateTimeOffset.UtcNow;

            var orcamento = CriarOrcamentoComItem(agora);

            orcamento.Enviar(
                "bruno",
                agora.AddMinutes(1));

            var item = orcamento.Itens.Single();

            orcamento.AprovarItens(
                new[] { item.Id },
                "bruno",
                agora.AddMinutes(2),
                CanalAprovacao.WhatsApp);

            var (ordemServico, evento) =
                orcamento.CriarOrdemServico(
                    "bruno",
                    agora.AddMinutes(3));

            Assert.Equal(
                StatusOrcamento.ConvertidoEmOrdemServico,
                orcamento.Status);

            Assert.Equal(
                orcamento.Id,
                ordemServico.OrcamentoId);

            Assert.Equal(
                "OrcamentoConvertidoEmOrdemServico",
                evento.Tipo);

            Assert.Equal(
                "bruno",
                evento.Usuario);
        }
    }
}
