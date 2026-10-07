namespace OrcamentoOficina.Application.Orcamentos.ConverterEmOrdemServico
{
    public sealed record ConverterEmOrdemServicoResult(Guid OrdemServicoId, Guid OrcamentoId, DateTimeOffset CriadaEm);
}
