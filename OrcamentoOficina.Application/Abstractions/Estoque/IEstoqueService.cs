namespace OrcamentoOficina.Application.Abstractions.Estoque
{
    public interface IEstoqueService
    {
        Task<bool> PossuiDisponibilidadeAsync(string codigo, decimal quantidade, CancellationToken cancellationToken = default);
    }
}
