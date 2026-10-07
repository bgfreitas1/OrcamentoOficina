namespace OrcamentoOficina.Application.Orcamentos.AdicionarItem
{
    public sealed record AdicionarItemResult(Guid Id, string Descricao, decimal Quantidade, decimal PrecoUnitario, decimal Total, bool DisponivelEmEstoque);
}
