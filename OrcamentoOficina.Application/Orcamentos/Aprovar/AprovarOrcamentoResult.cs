using OrcamentoOficina.Domain.Enums;

namespace OrcamentoOficina.Application.Orcamentos.Aprovar
{
    public sealed record AprovarOrcamentoResult(Guid Id, StatusOrcamento Status);
}
