using OrcamentoOficina.Domain.Enums;

namespace OrcamentoOficina.Application.Orcamentos.AlterarItem
{
    public sealed record AlterarItemRequest(decimal Quantidade, TipoDesconto TipoDesconto, decimal Desconto, string? UsuarioAutorizadorDesconto);
}
