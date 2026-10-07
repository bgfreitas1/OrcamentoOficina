using OrcamentoOficina.Application.Abstractions.Estoque;

namespace OrcamentoOficina.Infrastructure.Estoque
{
    internal sealed class EstoqueServiceSimulado : IEstoqueService
    {
        public Task<bool> PossuiDisponibilidadeAsync(string codigo, decimal quantidade, CancellationToken cancellationToken = default)
        {
            // Simulação:
            // PEC-002 está indisponível.
            var disponivel = !codigo.Equals("PEC-002", StringComparison.OrdinalIgnoreCase);

            return Task.FromResult(disponivel);
        }
    }
}
