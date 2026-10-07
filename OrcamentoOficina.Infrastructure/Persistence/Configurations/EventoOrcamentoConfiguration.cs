using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrcamentoOficina.Domain.Entities;

namespace OrcamentoOficina.Infrastructure.Persistence.Configurations
{
    public sealed class EventoOrcamentoConfiguration : IEntityTypeConfiguration<EventoOrcamento>
    {
        public void Configure(EntityTypeBuilder<EventoOrcamento> builder)
        {
            builder.ToTable("EventosOrcamento");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Tipo).HasMaxLength(100).IsRequired();

            builder.Property(x => x.Usuario).HasMaxLength(200).IsRequired();

            builder.Property(x => x.OcorridoEm).IsRequired();

            builder.Property(x => x.Dados).HasColumnType("nvarchar(max)");

            builder.HasIndex(x => new { x.OrcamentoId, x.OcorridoEm });
        }
    }
}
