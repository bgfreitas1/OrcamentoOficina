using Microsoft.Extensions.DependencyInjection;
using OrcamentoOficina.Application.Common.Interfaces;
using OrcamentoOficina.Application.Orcamentos.AdicionarItem;
using OrcamentoOficina.Application.Orcamentos.AlterarItem;
using OrcamentoOficina.Application.Orcamentos.Aprovar;
using OrcamentoOficina.Application.Orcamentos.Consultar;
using OrcamentoOficina.Application.Orcamentos.ConverterEmOrdemServico;
using OrcamentoOficina.Application.Orcamentos.Criar;
using OrcamentoOficina.Application.Orcamentos.Enviar;
using OrcamentoOficina.Application.Orcamentos.EnviarOrcamento;
using OrcamentoOficina.Application.Orcamentos.Historico;
using OrcamentoOficina.Application.Orcamentos.Listar;
using OrcamentoOficina.Application.Orcamentos.RemoverItem;
using OrcamentoOficina.Application.Orcamentos.ReprovarOrcamento;
using OrcamentoOficina.Application.Orcamentos.Revisar;

namespace OrcamentoOficina.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton(TimeProvider.System);

        services.AddScoped<CriarOrcamentoHandler>();

        services.AddScoped<AdicionarItemHandler>();

        services.AddScoped<AlterarItemHandler>();

        services.AddScoped<RemoverItemHandler>();

        services.AddScoped<EnviarOrcamentoHandler>();

        services.AddScoped<AprovarOrcamentoHandler>();

        services.AddScoped<ReprovarOrcamentoHandler>();

        services.AddScoped<RevisarOrcamentoHandler>();

        services.AddScoped<ConverterEmOrdemServicoHandler>();

        services.AddScoped<ObterOrcamentoHandler>();

        services.AddScoped<ListarOrcamentosHandler>();

        services.AddScoped<ObterHistoricoOrcamentoHandler>();

        return services;
    }
}