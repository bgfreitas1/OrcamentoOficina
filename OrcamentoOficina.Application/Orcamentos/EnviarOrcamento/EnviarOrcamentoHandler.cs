using OrcamentoOficina.Application.Abstractions;
using OrcamentoOficina.Application.Abstractions.Persistence;
using OrcamentoOficina.Application.Common.Exceptions;
using OrcamentoOficina.Application.Orcamentos.EnviarOrcamento;

namespace OrcamentoOficina.Application.Orcamentos.Enviar;

public sealed class EnviarOrcamentoHandler(IOrcamentoRepository orcamentoRepository, IUnitOfWork unitOfWork, TimeProvider timeProvider)
{
    public async Task<EnviarOrcamentoResult> HandleAsync(EnviarOrcamentoCommand command, CancellationToken cancellationToken = default)
    {
        var orcamento = await orcamentoRepository.ObterPorIdAsync(command.OrcamentoId, cancellationToken) ?? throw new NotFoundException("Orçamento não encontrado.");

        var agora = timeProvider.GetUtcNow();

        var evento = orcamento.Enviar(command.Usuario, agora);

        orcamentoRepository.AdicionarEvento(evento);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new EnviarOrcamentoResult(orcamento.Id, orcamento.Status, orcamento.EnviadoEm!.Value);
    }
}
