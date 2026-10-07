using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrcamentoOficina.Domain.Entities;

public sealed class OrdemServicoConfiguration : IEntityTypeConfiguration<OrdemServico>
{
    public void Configure(EntityTypeBuilder<OrdemServico> builder)
    {
        builder.ToTable("OrdensServico");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Usuario).HasMaxLength(150).IsRequired();

        builder.Property(x => x.CriadaEm).IsRequired();

        builder.HasIndex(x => x.OrcamentoId).IsUnique();

        builder.HasOne<Orcamento>().WithOne().HasForeignKey<OrdemServico>(x => x.OrcamentoId).OnDelete(DeleteBehavior.Restrict);
    }
}