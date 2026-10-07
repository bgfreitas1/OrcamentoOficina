using OrcamentoOficina.Domain.Enums;

namespace OrcamentoOficina.Application.Orcamentos.EnviarOrcamento
{
    public sealed record EnviarOrcamentoResult(Guid Id, StatusOrcamento Status, DateTimeOffset EnviadoEm);
}
