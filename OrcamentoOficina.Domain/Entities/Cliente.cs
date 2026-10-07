namespace OrcamentoOficina.Domain.Entities
{
    public sealed class Cliente
    {
        public Guid Id { get; private set; }

        public string Nome { get; private set; } = string.Empty;

        public string? Documento { get; private set; }

        public string? Telefone { get; private set; }

        public string? Email { get; private set; }

        private Cliente()
        {
        }

        public Cliente(string nome, string? documento = null, string? telefone = null, string? email = null)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("O nome do cliente é obrigatório.", nameof(nome));

            Id = Guid.NewGuid();
            Nome = nome.Trim();
            Documento = Normalizar(documento);
            Telefone = Normalizar(telefone);
            Email = Normalizar(email);
        }

        private static string? Normalizar(string? valor) => string.IsNullOrWhiteSpace(valor) ? null : valor.Trim();
    }
}
