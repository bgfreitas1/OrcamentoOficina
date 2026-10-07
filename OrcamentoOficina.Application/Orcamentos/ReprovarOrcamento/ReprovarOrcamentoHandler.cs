using OrcamentoOficina.Application.Abstractions.Persistence;
using OrcamentoOficina.Application.Common.Exceptions;

namespace OrcamentoOficina.Application.Orcamentos.ReprovarOrcamento
{
    public sealed class ReprovarOrcamentoHandler(IOrcamentoRepository orcamentoRepository, IUnitOfWork unitOfWork, TimeProvider timeProvider)
    {
        public async Task<ReprovarOrcamentoResult> HandleAsync(ReprovarOrcamentoCommand command, CancellationToken cancellationToken = default)
        {
            var orcamento = await orcamentoRepository.ObterPorIdAsync(command.OrcamentoId, cancellationToken) ?? throw new NotFoundException("Orçamento não encontrado.");

            var agora = timeProvider.GetUtcNow();

            var evento = orcamento.Reprovar(command.Motivo, command.Usuario, agora);

            orcamentoRepository.AdicionarEvento(evento);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return new ReprovarOrcamentoResult(orcamento.Id, orcamento.Status, orcamento.MotivoReprovacao!);
        }
    }
}
