namespace OrcamentoOficina.Application.Orcamentos.ReprovarOrcamento
{
    public sealed record ReprovarOrcamentoCommand(Guid OrcamentoId, string? Motivo, string? Usuario);
}
