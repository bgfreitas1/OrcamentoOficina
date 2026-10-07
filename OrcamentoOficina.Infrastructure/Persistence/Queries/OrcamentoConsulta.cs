using Microsoft.EntityFrameworkCore;
using OrcamentoOficina.Application.Common.Interfaces;
using OrcamentoOficina.Application.Common.Models;
using OrcamentoOficina.Application.Orcamentos.Historico;
using OrcamentoOficina.Application.Orcamentos.Listar;

namespace OrcamentoOficina.Infrastructure.Persistence.Queries;

public sealed class OrcamentoConsulta(AppDbContext context) : IOrcamentoConsulta
{
    public Task<bool> ExisteAsync(Guid orcamentoId, CancellationToken cancellationToken = default)
    {
        return context.Orcamentos.AsNoTracking().AnyAsync(x => x.Id == orcamentoId, cancellationToken);
    }

    public async Task<PagedResult<OrcamentoResumoResult>> ListarAsync(ListarOrcamentosQuery filtro, CancellationToken cancellationToken = default)
    {
        var query = from orcamento in context.Orcamentos.AsNoTracking() join veiculo in context.Veiculos.AsNoTracking() on orcamento.VeiculoId equals veiculo.Id select new { Orcamento = orcamento, Veiculo = veiculo };

        if (filtro.Status.HasValue)
            query = query.Where(x => x.Orcamento.Status == filtro.Status.Value);
        

        if (filtro.ClienteId.HasValue)
            query = query.Where(x => x.Orcamento.ClienteId == filtro.ClienteId.Value);
        

        if (!string.IsNullOrWhiteSpace(filtro.Placa))
        {
            var placa = filtro.Placa.Trim().ToUpper();

            query = query.Where(x => x.Veiculo.Placa == placa);
        }

        if (filtro.DataInicio.HasValue)
        {
            query = query.Where(x => x.Orcamento.CriadoEm >= filtro.DataInicio.Value);
        }

        if (filtro.DataFim.HasValue)
        {
            query = query.Where(x => x.Orcamento.CriadoEm <= filtro.DataFim.Value);
        }

        var totalItems = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(x => x.Orcamento.CriadoEm)
            .ThenByDescending(x => x.Orcamento.Id)
            .Skip((filtro.Page - 1) * filtro.PageSize)
            .Take(filtro.PageSize)
            .Select(x => new OrcamentoResumoResult(
                x.Orcamento.Id,
                x.Orcamento.ClienteId,
                x.Orcamento.VeiculoId,
                x.Veiculo.Placa,
                x.Orcamento.Versao,
                x.Orcamento.Status,
                x.Orcamento.CriadoEm,
                x.Orcamento.ValidadeEm,
                x.Orcamento.Total))
            .ToListAsync(cancellationToken);

        var totalPages = totalItems == 0 ? 0 : (int)Math.Ceiling(totalItems / (double)filtro.PageSize);

        return new PagedResult<OrcamentoResumoResult>(items, filtro.Page, filtro.PageSize, totalItems, totalPages);
    }

    public async Task<IReadOnlyCollection<EventoOrcamentoResult>> ObterHistoricoAsync(Guid orcamentoId, CancellationToken cancellationToken = default)
    {
        return await context.EventosOrcamento
            .AsNoTracking()
            .Where(x => x.OrcamentoId == orcamentoId)
            .OrderBy(x => x.OcorridoEm)
            .ThenBy(x => x.Id)
            .Select(x => new EventoOrcamentoResult(
                x.Id,
                x.Tipo,
                x.Usuario,
                x.OcorridoEm,
                x.Dados))
            .ToListAsync(cancellationToken);
    }
}