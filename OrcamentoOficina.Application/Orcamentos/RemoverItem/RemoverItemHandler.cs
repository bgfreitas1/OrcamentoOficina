using OrcamentoOficina.Application.Abstractions.Persistence;
using OrcamentoOficina.Application.Common.Exceptions;

namespace OrcamentoOficina.Application.Orcamentos.RemoverItem
{
    public sealed class RemoverItemHandler(IOrcamentoRepository orcamentoRepository, IUnitOfWork unitOfWork)
    {
        public async Task HandleAsync(RemoverItemCommand command, CancellationToken cancellationToken = default)
        {
            var orcamento = await orcamentoRepository.ObterPorIdAsync(command.OrcamentoId, cancellationToken) ?? throw new NotFoundException("Orçamento não encontrado.");

            var itemExiste = orcamento.Itens.Any(x => x.Id == command.ItemId);

            if (!itemExiste)
                throw new NotFoundException("Item não encontrado no orçamento.");

            orcamento.RemoverItem(command.ItemId);

            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
