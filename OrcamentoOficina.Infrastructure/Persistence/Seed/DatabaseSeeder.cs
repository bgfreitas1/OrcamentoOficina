using Microsoft.EntityFrameworkCore;
using OrcamentoOficina.Domain.Entities;
using OrcamentoOficina.Domain.Enums;

namespace OrcamentoOficina.Infrastructure.Persistence.Seed
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(AppDbContext context, CancellationToken cancellationToken = default)
        {
            var cliente = await context.Clientes.FirstOrDefaultAsync(x => x.Documento == "12345678901", cancellationToken);

            if (cliente is null)
            {
                cliente = new Cliente("Cliente Demonstração", "12345678901", "(11) 99999-9999", "cliente@exemplo.com");

                context.Clientes.Add(cliente);

                await context.SaveChangesAsync(cancellationToken);
            }

            var veiculoExiste = await context.Veiculos.AnyAsync(x => x.Placa == "ABC1D23", cancellationToken);

            if (!veiculoExiste)
            {
                var veiculo = new Veiculo(cliente.Id, "ABC1D23", "Toyota", "Corolla", 2022);

                context.Veiculos.Add(veiculo);

                await context.SaveChangesAsync(cancellationToken);
            }

            if (!await context.ItensCatalogo.AnyAsync(cancellationToken))
            {
                context.ItensCatalogo.AddRange(new ItemCatalogo("PEC-001", "Pastilha de freio dianteira", TipoItemOrcamento.Peca, 189.90m),

                    new ItemCatalogo("PEC-002", "Filtro de óleo", TipoItemOrcamento.Peca, 49.90m),

                    new ItemCatalogo("PEC-003", "Óleo do motor 5W30", TipoItemOrcamento.Peca, 59.90m),

                    new ItemCatalogo("SER-001", "Troca de óleo", TipoItemOrcamento.Servico, 120m),

                    new ItemCatalogo("SER-002", "Alinhamento", TipoItemOrcamento.Servico, 90m));

                await context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
