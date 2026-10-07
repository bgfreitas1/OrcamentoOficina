using OrcamentoOficina.Application.Orcamentos.Historico;
using OrcamentoOficina.Domain.Entities;

namespace OrcamentoOficina.Application.Abstractions.Persistence
{
    public interface IOrcamentoRepository
    {
        Task<Orcamento?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task AdicionarAsync(Orcamento orcamento, CancellationToken cancellationToken = default);

        void AdicionarItem(ItemOrcamento item);

        void AdicionarEvento(EventoOrcamento evento);       
    }
}
