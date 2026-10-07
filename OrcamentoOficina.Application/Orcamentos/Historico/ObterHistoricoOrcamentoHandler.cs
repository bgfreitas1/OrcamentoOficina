using OrcamentoOficina.Application.Common.Exceptions;
using OrcamentoOficina.Application.Common.Interfaces;

namespace OrcamentoOficina.Application.Orcamentos.Historico;

public sealed class ObterHistoricoOrcamentoHandler(IOrcamentoConsulta consulta)
{
    public async Task<IReadOnlyCollection<EventoOrcamentoResult>> HandleAsync(ObterHistoricoOrcamentoQuery query, CancellationToken cancellationToken = default)
    {
        var existe = await consulta.ExisteAsync(query.OrcamentoId, cancellationToken);

        if (!existe)
            throw new NotFoundException("Orçamento não encontrado.");        

        return await consulta.ObterHistoricoAsync(query.OrcamentoId, cancellationToken);
    }
}