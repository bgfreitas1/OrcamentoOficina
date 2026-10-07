using OrcamentoOficina.Application.Abstractions.Estoque;
using OrcamentoOficina.Application.Abstractions.Persistence;
using OrcamentoOficina.Application.Common.Exceptions;
using OrcamentoOficina.Domain.Entities;
using OrcamentoOficina.Domain.Enums;
using OrcamentoOficina.Domain.ValueObjects;

namespace OrcamentoOficina.Application.Orcamentos.AdicionarItem
{
    public sealed class AdicionarItemHandler(IOrcamentoRepository orcamentoRepository, IItemCatalogoRepository catalogoRepository, IEstoqueService estoqueService, IUnitOfWork unitOfWork)
    {
        public async Task<AdicionarItemResult> HandleAsync(AdicionarItemCommand command, CancellationToken cancellationToken = default)
        {
            var orcamento = await orcamentoRepository.ObterPorIdAsync(command.OrcamentoId, cancellationToken) ?? throw new NotFoundException("Orçamento não encontrado.");

            var catalogo = await catalogoRepository.ObterPorIdAsync(command.ItemCatalogoId, cancellationToken) ?? throw new NotFoundException("Item de catálogo não encontrado.");

            var disponivel = true;

            if (catalogo.Tipo == TipoItemOrcamento.Peca)
                disponivel = await estoqueService.PossuiDisponibilidadeAsync(catalogo.Codigo, command.Quantidade, cancellationToken);
            
            var desconto = new Desconto(command.TipoDesconto, command.Desconto);

            var item = new ItemOrcamento(catalogo.Descricao, catalogo.Tipo, command.Quantidade, catalogo.Preco, desconto, command.UsuarioAutorizadorDesconto, disponivel);

            orcamento.AdicionarItem(item);
            orcamentoRepository.AdicionarItem(item);                                                                                         

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return new AdicionarItemResult(item.Id, item.Descricao, item.Quantidade, item.PrecoUnitario, item.Total, item.DisponivelEmEstoque);
        }
    }
}
