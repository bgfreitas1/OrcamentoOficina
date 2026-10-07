using OrcamentoOficina.Application.Abstractions.Persistence;
using OrcamentoOficina.Application.Common.Exceptions;

namespace OrcamentoOficina.Application.Orcamentos.ConverterEmOrdemServico
{
    public sealed class ConverterEmOrdemServicoHandler(IOrcamentoRepository orcamentoRepository, IOrdemServicoRepository ordemServicoRepository, IUnitOfWork unitOfWork, TimeProvider timeProvider)
    {
        public async Task<ConverterEmOrdemServicoResult> HandleAsync(ConverterEmOrdemServicoCommand command, CancellationToken cancellationToken = default)
        {            
            var existente = await ordemServicoRepository.ObterPorOrcamentoIdAsync(command.OrcamentoId, cancellationToken);

            if (existente is not null)
                return new ConverterEmOrdemServicoResult(existente.Id, existente.OrcamentoId, existente.CriadaEm);
            
            var orcamento = await orcamentoRepository.ObterPorIdAsync(command.OrcamentoId, cancellationToken) ?? throw new NotFoundException("Orçamento não encontrado.");

            var agora = timeProvider.GetUtcNow();

            var (ordemServico, evento) = orcamento.CriarOrdemServico(command.Usuario, agora);

            await ordemServicoRepository.AdicionarAsync(ordemServico, cancellationToken);

            orcamentoRepository.AdicionarEvento(evento);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return new ConverterEmOrdemServicoResult(ordemServico.Id, ordemServico.OrcamentoId, ordemServico.CriadaEm);
        }
    }
}
