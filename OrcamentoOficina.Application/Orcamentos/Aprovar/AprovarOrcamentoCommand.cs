using OrcamentoOficina.Domain.Enums;

namespace OrcamentoOficina.Application.Orcamentos.Aprovar
{
    public sealed record AprovarOrcamentoCommand(Guid OrcamentoId, IReadOnlyCollection<Guid> ItensIds, string Usuario, CanalAprovacao Canal);
}
