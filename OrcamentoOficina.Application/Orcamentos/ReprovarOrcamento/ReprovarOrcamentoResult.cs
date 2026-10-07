using OrcamentoOficina.Domain.Enums;

namespace OrcamentoOficina.Application.Orcamentos.ReprovarOrcamento
{
    public sealed record ReprovarOrcamentoResult(Guid Id, StatusOrcamento Status, string Motivo);
}
