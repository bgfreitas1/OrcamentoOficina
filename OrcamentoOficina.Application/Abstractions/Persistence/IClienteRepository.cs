namespace OrcamentoOficina.Application.Abstractions.Persistence
{
    public interface IClienteRepository
    {
        Task<bool> ExisteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
