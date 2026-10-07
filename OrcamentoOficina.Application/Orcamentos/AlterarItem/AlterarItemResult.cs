namespace OrcamentoOficina.Application.Orcamentos.AlterarItem
{
    public sealed record AlterarItemResult(Guid Id, string Descricao, decimal Quantidade, decimal PrecoUnitario, decimal Total, bool DisponivelEmEstoque);
}
