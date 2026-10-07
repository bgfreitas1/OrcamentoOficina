using OrcamentoOficina.Domain.Enums;

namespace OrcamentoOficina.Application.Orcamentos.Consultar
{
    public sealed record OrcamentoDetalheResult(
    Guid Id,
    Guid ClienteId,
    Guid VeiculoId,
    int Versao,
    Guid GrupoVersaoId,
    Guid? OrcamentoOriginalId,
    StatusOrcamento Status,
    DateTimeOffset CriadoEm,
    DateTimeOffset ValidadeEm,
    DateTimeOffset? EnviadoEm,
    decimal Subtotal,
    decimal ValorDescontoGeral,
    decimal Total,
    string? MotivoReprovacao,
    IReadOnlyCollection<ItemOrcamentoResult> Itens);

    public sealed record ItemOrcamentoResult(
        Guid Id,
        string Descricao,
        TipoItemOrcamento Tipo,
        decimal Quantidade,
        decimal PrecoUnitario,
        TipoDesconto TipoDesconto,
        decimal Desconto,
        decimal ValorDesconto,
        decimal Total,
        bool DisponivelEmEstoque,
        bool Aprovado,
        string? UsuarioAprovacao,
        DateTimeOffset? DataAprovacao,
        CanalAprovacao? CanalAprovacao);
}
