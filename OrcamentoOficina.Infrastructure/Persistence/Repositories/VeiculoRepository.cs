using Microsoft.EntityFrameworkCore;
using OrcamentoOficina.Application.Abstractions.Persistence;

namespace OrcamentoOficina.Infrastructure.Persistence.Repositories;

internal sealed class VeiculoRepository(AppDbContext context) : IVeiculoRepository
{
    public Task<bool> PertenceAoClienteAsync(Guid veiculoId, Guid clienteId, CancellationToken cancellationToken = default)
    {
        return context.Veiculos.AnyAsync(x => x.Id == veiculoId && x.ClienteId == clienteId, cancellationToken);
    }
}