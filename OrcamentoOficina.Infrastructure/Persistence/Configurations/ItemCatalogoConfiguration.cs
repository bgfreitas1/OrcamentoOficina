using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrcamentoOficina.Domain.Entities;

namespace OrcamentoOficina.Infrastructure.Persistence.Configurations;

internal sealed class ItemCatalogoConfiguration : IEntityTypeConfiguration<ItemCatalogo>
{
    public void Configure(EntityTypeBuilder<ItemCatalogo> builder)
    {
        builder.ToTable("ItensCatalogo");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Codigo).HasMaxLength(50).IsRequired();

        builder.Property(x => x.Descricao).HasMaxLength(500).IsRequired();

        builder.Property(x => x.Tipo).HasConversion<int>().IsRequired();

        builder.Property(x => x.Preco).HasPrecision(18, 2).IsRequired();

        builder.Property(x => x.Ativo).IsRequired();

        builder.HasIndex(x => x.Codigo).IsUnique();

        builder.HasIndex(x => new { x.Tipo, x.Ativo });
    }
}