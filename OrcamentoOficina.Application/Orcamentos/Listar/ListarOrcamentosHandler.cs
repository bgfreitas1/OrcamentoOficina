using OrcamentoOficina.Application.Common.Interfaces;
using OrcamentoOficina.Application.Common.Models;

namespace OrcamentoOficina.Application.Orcamentos.Listar;

public sealed class ListarOrcamentosHandler(
    IOrcamentoConsulta consulta)
{
    public Task<PagedResult<OrcamentoResumoResult>> HandleAsync(ListarOrcamentosQuery query,CancellationToken cancellationToken = default)
    {
        if (query.Page < 1)
            throw new ArgumentException("A página deve ser maior ou igual a 1.");
        
        if (query.PageSize is < 1 or > 100)
            throw new ArgumentException("O tamanho da página deve estar entre 1 e 100.");
        
        if (query.DataInicio.HasValue && query.DataFim.HasValue && query.DataInicio > query.DataFim)
            throw new ArgumentException("A data inicial não pode ser posterior à data final.");
        
        return consulta.ListarAsync(query, cancellationToken);
    }
}