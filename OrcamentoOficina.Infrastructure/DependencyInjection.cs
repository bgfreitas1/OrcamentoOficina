using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrcamentoOficina.Application.Abstractions.Estoque;
using OrcamentoOficina.Application.Abstractions.Persistence;
using OrcamentoOficina.Application.Common.Interfaces;
using OrcamentoOficina.Infrastructure.Estoque;
using OrcamentoOficina.Infrastructure.Persistence;
using OrcamentoOficina.Infrastructure.Persistence.Queries;
using OrcamentoOficina.Infrastructure.Persistence.Repositories;

namespace OrcamentoOficina.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Database") ?? throw new InvalidOperationException("Connection string 'Database' não configurada.");

            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

            services.AddScoped<IOrcamentoRepository, OrcamentoRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IOrcamentoRepository, OrcamentoRepository>();
            services.AddScoped<IClienteRepository, ClienteRepository>();
            services.AddScoped<IVeiculoRepository, VeiculoRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IItemCatalogoRepository, ItemCatalogoRepository>();

            services.AddScoped<IEstoqueService, EstoqueServiceSimulado>();

            services.AddScoped<IOrdemServicoRepository, OrdemServicoRepository>();

            services.AddScoped<IOrcamentoConsulta, OrcamentoConsulta>();

            return services;
        }
    }
}
