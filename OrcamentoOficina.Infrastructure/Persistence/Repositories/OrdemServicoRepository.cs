using Microsoft.EntityFrameworkCore;
using OrcamentoOficina.Application.Abstractions.Persistence;
using OrcamentoOficina.Domain.Entities;
using OrcamentoOficina.Infrastructure.Persistence;

public sealed class OrdemServicoRepository(AppDbContext context) : IOrdemServicoRepository
{
    public Task<OrdemServico?> ObterPorOrcamentoIdAsync(Guid orcamentoId, CancellationToken cancellationToken = default)
    {
        return context.OrdensServico.FirstOrDefaultAsync(x => x.OrcamentoId == orcamentoId,cancellationToken);
    }

    public async Task AdicionarAsync(OrdemServico ordemServico, CancellationToken cancellationToken = default)
    {
        await context.OrdensServico.AddAsync(ordemServico, cancellationToken);
    }
}