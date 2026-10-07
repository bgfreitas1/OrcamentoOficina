namespace OrcamentoOficina.Application.Abstractions.Persistence
{
    public interface IVeiculoRepository
    {
        Task<bool> PertenceAoClienteAsync(Guid veiculoId, Guid clienteId, CancellationToken cancellationToken = default);
    }
}
