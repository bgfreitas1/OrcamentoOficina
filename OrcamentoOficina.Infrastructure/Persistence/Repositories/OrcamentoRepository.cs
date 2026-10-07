using Microsoft.EntityFrameworkCore;
using OrcamentoOficina.Application.Abstractions.Persistence;
using OrcamentoOficina.Domain.Entities;

namespace OrcamentoOficina.Infrastructure.Persistence.Repositories
{
    internal sealed class OrcamentoRepository(AppDbContext context) : IOrcamentoRepository
    {
        public async Task AdicionarAsync(Orcamento orcamento, CancellationToken cancellationToken = default)
        {
            await context.Orcamentos.AddAsync(orcamento, cancellationToken);
        }

        public void AdicionarEvento(EventoOrcamento evento)
        {
            context.EventosOrcamento.Add(evento);
        }

        public void AdicionarItem(ItemOrcamento item)
        {
            context.ItensOrcamento.Add(item);
        }

        public Task<Orcamento?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return context.Orcamentos.Include(x => x.Itens).Include(x => x.Eventos).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
    }
}
