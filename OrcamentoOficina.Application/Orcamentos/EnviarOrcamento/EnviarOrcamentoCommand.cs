namespace OrcamentoOficina.Application.Orcamentos.EnviarOrcamento
{
    public sealed record EnviarOrcamentoCommand(Guid OrcamentoId, string Usuario);
}
