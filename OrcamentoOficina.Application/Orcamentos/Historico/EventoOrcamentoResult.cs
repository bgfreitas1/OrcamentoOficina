namespace OrcamentoOficina.Application.Orcamentos.Historico
{
    public sealed record EventoOrcamentoResult(Guid Id, string Tipo, string Usuario, DateTimeOffset OcorridoEm, string? Dados);
}
