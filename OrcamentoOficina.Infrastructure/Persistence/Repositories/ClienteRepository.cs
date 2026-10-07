using Microsoft.EntityFrameworkCore;
using OrcamentoOficina.Application.Abstractions.Persistence;

namespace OrcamentoOficina.Infrastructure.Persistence.Repositories;

internal sealed class ClienteRepository(AppDbContext context) : IClienteRepository
{
    public Task<bool> ExisteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return context.Clientes.AnyAsync(x => x.Id == id, cancellationToken);
    }
}