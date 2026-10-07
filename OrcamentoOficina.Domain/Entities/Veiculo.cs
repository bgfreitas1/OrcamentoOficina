namespace OrcamentoOficina.Domain.Entities
{
    public sealed class Veiculo
    {
        public Guid Id { get; private set; }

        public Guid ClienteId { get; private set; }

        public string Placa { get; private set; } = string.Empty;

        public string Marca { get; private set; } = string.Empty;

        public string Modelo { get; private set; } = string.Empty;

        public int? Ano { get; private set; }

        private Veiculo()
        {
        }

        public Veiculo(Guid clienteId, string placa, string marca, string modelo, int? ano = null)
        {
            if (clienteId == Guid.Empty)
                throw new ArgumentException("O cliente é obrigatório.", nameof(clienteId));

            if (string.IsNullOrWhiteSpace(placa))
                throw new ArgumentException("A placa é obrigatória.", nameof(placa));

            if (string.IsNullOrWhiteSpace(marca))
                throw new ArgumentException("A marca é obrigatória.", nameof(marca));

            if (string.IsNullOrWhiteSpace(modelo))
                throw new ArgumentException("O modelo é obrigatório.", nameof(modelo));

            Id = Guid.NewGuid();
            ClienteId = clienteId;
            Placa = placa.Trim().ToUpperInvariant();
            Marca = marca.Trim();
            Modelo = modelo.Trim();
            Ano = ano;
        }
    }
}
