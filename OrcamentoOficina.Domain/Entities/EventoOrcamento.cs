namespace OrcamentoOficina.Domain.Entities
{
    public sealed class EventoOrcamento
    {
        public Guid Id { get; private set; }

        public Guid OrcamentoId { get; private set; }

        public string Tipo { get; private set; } = string.Empty;

        public string Usuario { get; private set; } = string.Empty;

        public DateTimeOffset OcorridoEm { get; private set; }

        public string? Dados { get; private set; }

        private EventoOrcamento()
        {
        }

        internal EventoOrcamento(Guid orcamentoId, string tipo, string usuario, DateTimeOffset ocorridoEm, string? dados = null)
        {
            if (orcamentoId == Guid.Empty)
                throw new ArgumentException("O orçamento é obrigatório.", nameof(orcamentoId));

            if (string.IsNullOrWhiteSpace(tipo))
                throw new ArgumentException("O tipo do evento é obrigatório.", nameof(tipo));

            if (string.IsNullOrWhiteSpace(usuario))
                throw new ArgumentException("O usuário é obrigatório.", nameof(usuario));

            Id = Guid.NewGuid();
            OrcamentoId = orcamentoId;
            Tipo = tipo.Trim();
            Usuario = usuario.Trim();
            OcorridoEm = ocorridoEm;
            Dados = dados;
        }
    }
}
