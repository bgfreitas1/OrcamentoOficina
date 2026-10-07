using OrcamentoOficina.Domain.Enums;

namespace OrcamentoOficina.Application.Orcamentos.AlterarItem
{
    public sealed record AlterarItemCommand(Guid OrcamentoId, Guid ItemId, decimal Quantidade, TipoDesconto TipoDesconto, decimal Desconto, string? UsuarioAutorizadorDesconto);
}
