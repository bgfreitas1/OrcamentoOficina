using OrcamentoOficina.Application.Abstractions.Persistence;
using OrcamentoOficina.Application.Common.Exceptions;
using OrcamentoOficina.Domain.ValueObjects;

namespace OrcamentoOficina.Application.Orcamentos.AlterarItem
{
    public sealed class AlterarItemHandler(IOrcamentoRepository orcamentoRepository, IUnitOfWork unitOfWork)
    {
        public async Task<AlterarItemResult> HandleAsync(AlterarItemCommand command, CancellationToken cancellationToken = default)
        {
            var orcamento = await orcamentoRepository.ObterPorIdAsync(command.OrcamentoId, cancellationToken) ?? throw new NotFoundException("Orçamento não encontrado.");

            var item = orcamento.Itens.SingleOrDefault(x => x.Id == command.ItemId) ?? throw new NotFoundException("Item não encontrado no orçamento.");

            var desconto = new Desconto(command.TipoDesconto, command.Desconto);

            orcamento.AlterarItem(command.ItemId, command.Quantidade, item.PrecoUnitario, desconto, command.UsuarioAutorizadorDesconto);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return new AlterarItemResult(item.Id, item.Descricao, item.Quantidade, item.PrecoUnitario, item.Total, item.DisponivelEmEstoque);
        }
    }
}
