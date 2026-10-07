using OrcamentoOficina.Domain.Enums;

namespace OrcamentoOficina.Application.Orcamentos.Listar
{
    public sealed record ListarOrcamentosQuery(StatusOrcamento? Status, Guid? ClienteId, string? Placa, DateTimeOffset? DataInicio, DateTimeOffset? DataFim, int Page = 1, int PageSize = 20);
}
