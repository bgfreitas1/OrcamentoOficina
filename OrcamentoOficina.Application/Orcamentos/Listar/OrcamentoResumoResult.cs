using OrcamentoOficina.Domain.Enums;

namespace OrcamentoOficina.Application.Orcamentos.Listar
{
    public sealed record OrcamentoResumoResult(Guid Id, Guid ClienteId, Guid VeiculoId, string Placa, int Versao, StatusOrcamento Status, DateTimeOffset CriadoEm, DateTimeOffset ValidadeEm, decimal Total);
}
