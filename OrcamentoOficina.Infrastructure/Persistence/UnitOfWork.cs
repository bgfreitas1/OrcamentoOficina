using OrcamentoOficina.Application.Abstractions.Persistence;
using OrcamentoOficina.Application.Common.Exceptions;
using System.Data.Entity.Infrastructure;

namespace OrcamentoOficina.Infrastructure.Persistence
{
    internal sealed class UnitOfWork(AppDbContext context) : IUnitOfWork
    {
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new ConcurrencyException("O orçamento foi alterado por outra operação. Recarregue os dados e tente novamente.", ex);
            }
        }

        //public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        //{            
        //    context.ChangeTracker.DetectChanges();

        //    var debugView = context.ChangeTracker.DebugView.LongView;

        //    System.Diagnostics.Debug.WriteLine("===== CHANGE TRACKER =====");

        //    System.Diagnostics.Debug.WriteLine(debugView);

        //    System.Diagnostics.Debug.WriteLine("===== FIM CHANGE TRACKER =====");

        //    return context.SaveChangesAsync(cancellationToken);
        //}
    }
}
