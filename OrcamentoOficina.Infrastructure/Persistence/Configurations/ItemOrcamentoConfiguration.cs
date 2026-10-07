using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrcamentoOficina.Domain.Entities;

namespace OrcamentoOficina.Infrastructure.Persistence.Configurations
{
    public sealed class ItemOrcamentoConfiguration : IEntityTypeConfiguration<ItemOrcamento>
    {
        public void Configure(EntityTypeBuilder<ItemOrcamento> builder)
        {
            builder.ToTable("ItensOrcamento");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Descricao).HasMaxLength(500).IsRequired();

            builder.Property(x => x.Tipo).HasConversion<int>().IsRequired();

            builder.Property(x => x.Quantidade).HasPrecision(18, 4).IsRequired();

            builder.Property(x => x.PrecoUnitario).HasPrecision(18, 2).IsRequired();

            builder.Property(x => x.UsuarioAutorizadorDesconto).HasMaxLength(200);

            builder.Property(x => x.UsuarioAprovacao).HasMaxLength(200);

            builder.Property(x => x.CanalAprovacao).HasConversion<int?>();

            builder.Ignore(x => x.Subtotal);

            builder.Ignore(x => x.ValorDesconto);

            builder.Ignore(x => x.Total);

            builder.OwnsOne(x => x.Desconto, desconto =>
            {
                desconto.Property(x => x.Tipo).HasColumnName("DescontoTipo").HasConversion<int>();

                desconto.Property(x => x.Valor).HasColumnName("DescontoValor").HasPrecision(18, 2);
            });

            builder.Property(x => x.DisponivelEmEstoque).IsRequired();
        }
    }
}
