namespace OrcamentoOficina.Domain.Entities;

public sealed class OrdemServico
{
    public Guid Id { get; private set; }

    public Guid OrcamentoId { get; private set; }

    public DateTimeOffset CriadaEm { get; private set; }

    public string Usuario { get; private set; } = null!;

    private OrdemServico()
    {
    }

    internal OrdemServico(Guid orcamentoId, DateTimeOffset criadaEm, string usuario)
    {
        if (orcamentoId == Guid.Empty)
            throw new ArgumentException("O orçamento é obrigatório.", nameof(orcamentoId));

        if (string.IsNullOrWhiteSpace(usuario))
            throw new ArgumentException("O usuário é obrigatório.", nameof(usuario));

        Id = Guid.NewGuid();
        OrcamentoId = orcamentoId;
        CriadaEm = criadaEm;
        Usuario = usuario.Trim();
    }
}