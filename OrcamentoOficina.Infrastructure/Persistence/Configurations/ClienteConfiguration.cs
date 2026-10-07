using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrcamentoOficina.Domain.Entities;

namespace OrcamentoOficina.Infrastructure.Persistence.Configurations
{
    public sealed class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.ToTable("Clientes");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Nome).HasMaxLength(200).IsRequired();

            builder.Property(x => x.Documento).HasMaxLength(20);

            builder.Property(x => x.Telefone).HasMaxLength(30);

            builder.Property(x => x.Email).HasMaxLength(254);

            builder.HasIndex(x => x.Documento);

            builder.HasIndex(x => x.Nome);
        }
    }
}
