using Microsoft.EntityFrameworkCore;
using OrcamentoOficina.Domain.Entities;

namespace OrcamentoOficina.Infrastructure.Persistence
{
    public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Cliente> Clientes => Set<Cliente>();

        public DbSet<Veiculo> Veiculos => Set<Veiculo>();

        public DbSet<Orcamento> Orcamentos => Set<Orcamento>();

        public DbSet<ItemOrcamento> ItensOrcamento => Set<ItemOrcamento>();

        public DbSet<EventoOrcamento> EventosOrcamento => Set<EventoOrcamento>();

        public DbSet<ItemCatalogo> ItensCatalogo => Set<ItemCatalogo>();

        public DbSet<OrdemServico> OrdensServico => Set<OrdemServico>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
