using OrcamentoOficina.Domain.Entities;

namespace OrcamentoOficina.Application.Abstractions.Persistence
{
    public interface IItemCatalogoRepository
    {
        Task<ItemCatalogo?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
