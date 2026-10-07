using OrcamentoOficina.Domain.Enums;

namespace OrcamentoOficina.Application.Orcamentos.AdicionarItem
{
    public sealed record AdicionarItemCommand(Guid OrcamentoId, Guid ItemCatalogoId, decimal Quantidade, TipoDesconto TipoDesconto, decimal Desconto, string? UsuarioAutorizadorDesconto);
}
