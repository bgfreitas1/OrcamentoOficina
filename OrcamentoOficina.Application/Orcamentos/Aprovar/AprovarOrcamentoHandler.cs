using OrcamentoOficina.Application.Abstractions.Persistence;
using OrcamentoOficina.Application.Common.Exceptions;

namespace OrcamentoOficina.Application.Orcamentos.Aprovar
{
    public sealed class AprovarOrcamentoHandler(IOrcamentoRepository orcamentoRepository, IUnitOfWork unitOfWork, TimeProvider timeProvider)
    {
        public async Task<AprovarOrcamentoResult> HandleAsync(AprovarOrcamentoCommand command, CancellationToken cancellationToken = default)
        {
            var orcamento = await orcamentoRepository.ObterPorIdAsync(command.OrcamentoId, cancellationToken) ?? throw new NotFoundException("Orçamento não encontrado.");

            var agora = timeProvider.GetUtcNow();

            var evento = orcamento.AprovarItens(command.ItensIds, command.Usuario, agora, command.Canal);

            if (evento is not null)
                orcamentoRepository.AdicionarEvento(evento);            

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return new AprovarOrcamentoResult(orcamento.Id, orcamento.Status);
        }
    }
}
