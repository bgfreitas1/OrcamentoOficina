namespace OrcamentoOficina.Application.Orcamentos.RemoverItem
{
    public sealed record RemoverItemCommand(Guid OrcamentoId, Guid ItemId);
}
