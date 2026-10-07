using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrcamentoOficina.Domain.Entities;

namespace OrcamentoOficina.Infrastructure.Persistence.Configurations
{
    public sealed class OrcamentoConfiguration : IEntityTypeConfiguration<Orcamento>
    {
        public void Configure(EntityTypeBuilder<Orcamento> builder)
        {
            builder.ToTable("Orcamentos");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Status).HasConversion<int>().IsRequired();

            builder.Property(x => x.Versao).IsRequired();

            builder.Property(x => x.CriadoEm).IsRequired();

            builder.Property(x => x.ValidadeEm).IsRequired();

            builder.Property(x => x.MotivoReprovacao).HasMaxLength(1000);

            builder.Property(x => x.UsuarioAutorizadorDescontoGeral).HasMaxLength(200);

            builder.Property(x => x.RowVersion).IsRowVersion();

            builder.Ignore(x => x.Subtotal);

            builder.Ignore(x => x.ValorDescontoItens);

            builder.Ignore(x => x.TotalAntesDescontoGeral);

            builder.Ignore(x => x.ValorDescontoGeral);

            builder.Ignore(x => x.Total);

            builder.OwnsOne(x => x.DescontoGeral, desconto =>
            {
                desconto.Property(x => x.Tipo).HasColumnName("DescontoGeralTipo").HasConversion<int>();

                desconto.Property(x => x.Valor).HasColumnName("DescontoGeralValor").HasPrecision(18, 2);
            });              

            
            var itensNavigation = builder.HasMany(x => x.Itens).WithOne().HasForeignKey("OrcamentoId").OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(x => x.Itens).UsePropertyAccessMode(PropertyAccessMode.Field);


            builder.HasMany(x => x.Eventos).WithOne().HasForeignKey(x => x.OrcamentoId).OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => x.ClienteId);

            builder.HasIndex(x => x.VeiculoId);

            builder.HasIndex(x => x.Status);

            builder.HasIndex(x => x.CriadoEm);

            builder.HasIndex(x => new { x.GrupoVersaoId, x.Versao }).IsUnique();


            builder.Navigation(x => x.Eventos).UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.HasMany(x => x.Itens).WithOne().HasForeignKey("OrcamentoId").OnDelete(DeleteBehavior.Cascade);


            builder.Navigation(x => x.Itens).HasField("_itens").UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.Navigation(x => x.Eventos).HasField("_eventos").UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
