using OrcamentoOficina.Application.Abstractions.Persistence;
using OrcamentoOficina.Application.Common.Exceptions;

namespace OrcamentoOficina.Application.Orcamentos.Revisar
{
    public sealed class RevisarOrcamentoHandler(IOrcamentoRepository orcamentoRepository, IUnitOfWork unitOfWork, TimeProvider timeProvider)
    {
        public async Task<RevisarOrcamentoResult> HandleAsync(RevisarOrcamentoCommand command, CancellationToken cancellationToken = default)
        {
            var orcamento = await orcamentoRepository.ObterPorIdAsync(command.OrcamentoId, cancellationToken) ?? throw new NotFoundException("Orçamento não encontrado.");

            var agora = timeProvider.GetUtcNow();

            var revisao = orcamento.CriarRevisao(agora, command.ValidadeEm);

            revisao.RegistrarCriacaoRevisao(command.Usuario, agora);

            await orcamentoRepository.AdicionarAsync(revisao, cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return new RevisarOrcamentoResult(revisao.Id, revisao.Versao, revisao.GrupoVersaoId, revisao.OrcamentoOriginalId);
        }
    }
}
