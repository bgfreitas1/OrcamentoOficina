namespace OrcamentoOficina.Application.Orcamentos.Criar
{
    public sealed record CriarOrcamentoCommand(Guid ClienteId, Guid VeiculoId, DateTimeOffset ValidadeEm);
}
