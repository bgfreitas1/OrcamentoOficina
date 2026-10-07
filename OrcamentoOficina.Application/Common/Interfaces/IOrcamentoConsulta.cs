using OrcamentoOficina.Application.Common.Models;
using OrcamentoOficina.Application.Orcamentos.Historico;
using OrcamentoOficina.Application.Orcamentos.Listar;

namespace OrcamentoOficina.Application.Common.Interfaces;

public interface IOrcamentoConsulta
{
    Task<PagedResult<OrcamentoResumoResult>> ListarAsync(ListarOrcamentosQuery query, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<EventoOrcamentoResult>> ObterHistoricoAsync(Guid orcamentoId, CancellationToken cancellationToken = default);

    Task<bool> ExisteAsync(Guid orcamentoId, CancellationToken cancellationToken = default);
}