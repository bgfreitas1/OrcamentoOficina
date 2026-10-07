using OrcamentoOficina.Domain.Entities;

namespace OrcamentoOficina.Application.Abstractions.Persistence
{
    public interface IOrdemServicoRepository
    {
        Task<OrdemServico?> ObterPorOrcamentoIdAsync(Guid orcamentoId, CancellationToken cancellationToken = default);

        Task AdicionarAsync(OrdemServico ordemServico, CancellationToken cancellationToken = default);
    }
}
