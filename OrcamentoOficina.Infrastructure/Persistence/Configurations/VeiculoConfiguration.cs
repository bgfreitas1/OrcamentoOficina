using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrcamentoOficina.Domain.Entities;

namespace OrcamentoOficina.Infrastructure.Persistence.Configurations
{
    public sealed class VeiculoConfiguration
     : IEntityTypeConfiguration<Veiculo>
    {
        public void Configure(EntityTypeBuilder<Veiculo> builder)
        {
            builder.ToTable("Veiculos");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Placa).HasMaxLength(10).IsRequired();

            builder.Property(x => x.Marca).HasMaxLength(100).IsRequired();

            builder.Property(x => x.Modelo).HasMaxLength(150).IsRequired();

            builder.HasIndex(x => x.Placa).IsUnique();

            builder.HasIndex(x => x.ClienteId);

            builder.HasOne<Cliente>().WithMany().HasForeignKey(x => x.ClienteId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
