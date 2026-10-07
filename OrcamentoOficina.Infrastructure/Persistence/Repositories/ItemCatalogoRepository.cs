using Microsoft.EntityFrameworkCore;
using OrcamentoOficina.Application.Abstractions.Persistence;
using OrcamentoOficina.Domain.Entities;

namespace OrcamentoOficina.Infrastructure.Persistence.Repositories
{
    internal sealed class ItemCatalogoRepository(AppDbContext context) : IItemCatalogoRepository
    {
        public Task<ItemCatalogo?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return context.ItensCatalogo.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id && x.Ativo, cancellationToken);
        }
    }
}
