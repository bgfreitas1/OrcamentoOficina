namespace OrcamentoOficina.Application.Orcamentos.Revisar
{
    public sealed record RevisarOrcamentoResult(Guid Id, int Versao, Guid GrupoVersaoId, Guid? OrcamentoOriginalId);
}
