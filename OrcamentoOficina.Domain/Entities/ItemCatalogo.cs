using OrcamentoOficina.Domain.Enums;

namespace OrcamentoOficina.Domain.Entities
{
    public sealed class ItemCatalogo
    {
        public Guid Id { get; private set; }

        public string Codigo { get; private set; } = string.Empty;

        public string Descricao { get; private set; } = string.Empty;

        public TipoItemOrcamento Tipo { get; private set; }

        public decimal Preco { get; private set; }

        public bool Ativo { get; private set; }

        private ItemCatalogo()
        {
        }

        public ItemCatalogo(string codigo, string descricao, TipoItemOrcamento tipo, decimal preco)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                throw new ArgumentException("Código obrigatório.");

            if (string.IsNullOrWhiteSpace(descricao))
                throw new ArgumentException("Descrição obrigatória.");

            if (!Enum.IsDefined(tipo))
                throw new ArgumentOutOfRangeException(nameof(tipo));

            if (preco < 0)
                throw new ArgumentOutOfRangeException(nameof(preco));

            Id = Guid.NewGuid();
            Codigo = codigo.Trim().ToUpperInvariant();
            Descricao = descricao.Trim();
            Tipo = tipo;
            Preco = preco;
            Ativo = true;
        }
    }
}
