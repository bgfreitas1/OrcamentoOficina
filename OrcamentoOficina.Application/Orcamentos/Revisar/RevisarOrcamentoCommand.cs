namespace OrcamentoOficina.Application.Orcamentos.Revisar
{
    public sealed record RevisarOrcamentoCommand(Guid OrcamentoId, DateTimeOffset ValidadeEm, string Usuario);
}
