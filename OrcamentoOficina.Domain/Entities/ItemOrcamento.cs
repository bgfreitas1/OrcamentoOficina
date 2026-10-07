using OrcamentoOficina.Domain.Enums;
using OrcamentoOficina.Domain.ValueObjects;

namespace OrcamentoOficina.Domain.Entities
{
    public sealed class ItemOrcamento
    {
        public Guid Id { get; private set; }

        public string Descricao { get; private set; } = string.Empty;

        public TipoItemOrcamento Tipo { get; private set; }

        public decimal Quantidade { get; private set; }

        public decimal PrecoUnitario { get; private set; }

        public Desconto Desconto { get; private set; } = null!;

        public bool Aprovado { get; private set; }

        public string? UsuarioAutorizadorDesconto { get; private set; }

        public decimal Subtotal => Math.Round(Quantidade * PrecoUnitario, 2, MidpointRounding.AwayFromZero);

        public decimal ValorDesconto => Desconto.Calcular(Subtotal);

        public decimal Total => Subtotal - ValorDesconto;

        public string? UsuarioAprovacao { get; private set; }

        public DateTimeOffset? DataAprovacao { get; private set; }

        public CanalAprovacao? CanalAprovacao { get; private set; }

        public bool DisponivelEmEstoque { get; private set; } = true;

        private ItemOrcamento()
        {
        }

        public ItemOrcamento(string descricao, TipoItemOrcamento tipo, decimal quantidade, decimal precoUnitario, Desconto? desconto = null, string? usuarioAutorizadorDesconto = null, bool disponivelEmEstoque = true)
        {
            if (string.IsNullOrWhiteSpace(descricao))
                throw new ArgumentException("A descrição é obrigatória.", nameof(descricao));

            if (!Enum.IsDefined(tipo))
                throw new ArgumentOutOfRangeException(nameof(tipo));

            ValidarValores(quantidade, precoUnitario);

            descricao = descricao.Trim();

            var descontoAplicado = desconto ?? new Desconto(TipoDesconto.ValorFixo, 0);

            ValidarAutorizacao(quantidade * precoUnitario, descontoAplicado, usuarioAutorizadorDesconto);

            Id = Guid.NewGuid();
            Descricao = descricao;
            Tipo = tipo;
            Quantidade = quantidade;
            PrecoUnitario = precoUnitario;
            Desconto = descontoAplicado;

            UsuarioAutorizadorDesconto = NormalizarUsuario(usuarioAutorizadorDesconto);

            DisponivelEmEstoque = disponivelEmEstoque;
        }

        internal void Alterar(decimal quantidade, decimal precoUnitario, Desconto desconto, string? usuarioAutorizadorDesconto)
        {
            if (quantidade <= 0)
                throw new ArgumentOutOfRangeException(nameof(quantidade), "A quantidade deve ser maior que zero.");

            if (precoUnitario < 0)
                throw new ArgumentOutOfRangeException(nameof(precoUnitario), "O preço unitário não pode ser negativo.");

            ArgumentNullException.ThrowIfNull(desconto);

            if (desconto.PercentualEquivalente(quantidade * precoUnitario) > 15m && string.IsNullOrWhiteSpace(usuarioAutorizadorDesconto))
                throw new InvalidOperationException("Descontos superiores a 15% exigem um usuário autorizador.");
            
            Quantidade = quantidade;
            PrecoUnitario = precoUnitario;
            Desconto = desconto;
            UsuarioAutorizadorDesconto = string.IsNullOrWhiteSpace(usuarioAutorizadorDesconto) ? null : usuarioAutorizadorDesconto.Trim();
        }

        internal void Aprovar(string usuario, DateTimeOffset agora, CanalAprovacao canal)
        {
            if (Aprovado)
                return;

            if (string.IsNullOrWhiteSpace(usuario))
                throw new ArgumentException("O usuário da aprovação é obrigatório.",nameof(usuario));

            if (!Enum.IsDefined(canal))
                throw new ArgumentOutOfRangeException(nameof(canal), "Canal de aprovação inválido.");

            Aprovado = true;
            UsuarioAprovacao = usuario.Trim();
            DataAprovacao = agora;
            CanalAprovacao = canal;
        }

        private static void ValidarValores(decimal quantidade, decimal precoUnitario)
        {
            if (quantidade <= 0)
                throw new ArgumentOutOfRangeException(nameof(quantidade), "A quantidade deve ser maior que zero.");

            if (precoUnitario < 0)
                throw new ArgumentOutOfRangeException(nameof(precoUnitario), "O preço unitário não pode ser negativo.");
        }

        private static void ValidarAutorizacao(decimal valorBase, Desconto desconto, string? usuarioAutorizador)
        {
            if (desconto.PercentualEquivalente(valorBase) > 15m && string.IsNullOrWhiteSpace(usuarioAutorizador))
                throw new InvalidOperationException("Descontos acima de 15% exigem um usuário autorizador.");            
        }

        private static string? NormalizarUsuario(string? usuario) => string.IsNullOrWhiteSpace(usuario) ? null : usuario.Trim();
    }
}
