using Microsoft.AspNetCore.Mvc;
using OrcamentoOficina.Application.Common.Models;
using OrcamentoOficina.Application.Orcamentos.AdicionarItem;
using OrcamentoOficina.Application.Orcamentos.AlterarItem;
using OrcamentoOficina.Application.Orcamentos.Aprovar;
using OrcamentoOficina.Application.Orcamentos.Consultar;
using OrcamentoOficina.Application.Orcamentos.ConverterEmOrdemServico;
using OrcamentoOficina.Application.Orcamentos.Criar;
using OrcamentoOficina.Application.Orcamentos.Enviar;
using OrcamentoOficina.Application.Orcamentos.EnviarOrcamento;
using OrcamentoOficina.Application.Orcamentos.Historico;
using OrcamentoOficina.Application.Orcamentos.Listar;
using OrcamentoOficina.Application.Orcamentos.RemoverItem;
using OrcamentoOficina.Application.Orcamentos.ReprovarOrcamento;
using OrcamentoOficina.Application.Orcamentos.Revisar;
using OrcamentoOficina.Domain.Enums;

namespace OrcamentoOficina.API.Controllers;

[ApiController]
[Route("orcamentos")]
public sealed class OrcamentosController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType<CriarOrcamentoResult>(StatusCodes.Status201Created)]
    public async Task<IActionResult> Criar([FromBody] CriarOrcamentoRequest request, [FromServices] CriarOrcamentoHandler handler, CancellationToken cancellationToken)
    {
        var command = new CriarOrcamentoCommand(request.ClienteId, request.VeiculoId, request.ValidadeEm);

        var result = await handler.HandleAsync(command, cancellationToken);

        return CreatedAtAction(nameof(ObterPorId), new { id = result.Id }, result);
    }

    [HttpPost("{id:guid}/itens")]
    [ProducesResponseType<AdicionarItemResult>(StatusCodes.Status201Created)]
    public async Task<IActionResult> AdicionarItem(Guid id, [FromBody] AdicionarItemRequest request, [FromServices] AdicionarItemHandler handler, CancellationToken cancellationToken)
    {
        var command = new AdicionarItemCommand(id, request.ItemCatalogoId, request.Quantidade, request.TipoDesconto, request.Desconto, request.UsuarioAutorizadorDesconto);

        var result = await handler.HandleAsync(command, cancellationToken);

        return StatusCode(StatusCodes.Status201Created, result);
    }

    [HttpPut("{id:guid}/itens/{itemId:guid}")]
    public async Task<ActionResult<AlterarItemResult>> AlterarItem(Guid id, Guid itemId, AlterarItemRequest request, [FromServices] AlterarItemHandler handler, CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(new AlterarItemCommand(id, itemId, request.Quantidade, request.TipoDesconto, request.Desconto, request.UsuarioAutorizadorDesconto), cancellationToken);

        return Ok(result);
    }

    [HttpDelete("{id:guid}/itens/{itemId:guid}")]
    public async Task<IActionResult> RemoverItem(Guid id, Guid itemId, [FromServices] RemoverItemHandler handler, CancellationToken cancellationToken)
    {
        await handler.HandleAsync(new RemoverItemCommand(id, itemId), cancellationToken);

        return NoContent();
    }

    [HttpPost("{id:guid}/enviar")]
    public async Task<ActionResult<EnviarOrcamentoResult>> Enviar(Guid id, EnviarOrcamentoRequest request, [FromServices] EnviarOrcamentoHandler handler, CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(new EnviarOrcamentoCommand(id, request.Usuario), cancellationToken);

        return Ok(result);
    }

    [HttpPost("{id:guid}/aprovar")]
    public async Task<ActionResult<AprovarOrcamentoResult>> Aprovar(Guid id, AprovarOrcamentoRequest request, [FromServices] AprovarOrcamentoHandler handler, CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(new AprovarOrcamentoCommand(id, request.ItensIds, request.Usuario, request.Canal), cancellationToken);

        return Ok(result);
    }

    [HttpPost("{id:guid}/reprovar")]
    public async Task<ActionResult<ReprovarOrcamentoResult>> Reprovar(Guid id, ReprovarOrcamentoRequest request, [FromServices] ReprovarOrcamentoHandler handler, CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(new ReprovarOrcamentoCommand(id, request.Motivo, request.Usuario), cancellationToken);

        return Ok(result);
    }

    [HttpPost("{id:guid}/revisar")]
    public async Task<ActionResult<RevisarOrcamentoResult>> Revisar(Guid id, RevisarOrcamentoRequest request, [FromServices] RevisarOrcamentoHandler handler, CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(new RevisarOrcamentoCommand(id, request.ValidadeEm, request.Usuario),cancellationToken);

        return Ok(result);
    }

    [HttpPost("{id:guid}/ordem-servico")]
    public async Task<ActionResult<ConverterEmOrdemServicoResult>>
    ConverterEmOrdemServico(Guid id, ConverterEmOrdemServicoRequest request, [FromServices] ConverterEmOrdemServicoHandler handler, CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(new ConverterEmOrdemServicoCommand(id, request.Usuario),cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<OrcamentoDetalheResult>> ObterPorId(Guid id, [FromServices] ObterOrcamentoHandler handler, CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(new ObterOrcamentoQuery(id), cancellationToken);

        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<OrcamentoResumoResult>>> Listar(
    [FromQuery] StatusOrcamento? status,
    [FromQuery] Guid? clienteId,
    [FromQuery] string? placa,
    [FromQuery] DateTimeOffset? dataInicio,
    [FromQuery] DateTimeOffset? dataFim,
    [FromQuery] int page,
    [FromQuery] int pageSize,
    [FromServices] ListarOrcamentosHandler handler,
    CancellationToken cancellationToken)
    {
        page = page == 0 ? 1 : page;
        pageSize = pageSize == 0 ? 20 : pageSize;

        var query = new ListarOrcamentosQuery(status, clienteId, placa, dataInicio, dataFim, page, pageSize);

        var result = await handler.HandleAsync(query, cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:guid}/historico")]
    public async Task<ActionResult<IReadOnlyCollection<EventoOrcamentoResult>>> ObterHistorico(Guid id, [FromServices] ObterHistoricoOrcamentoHandler handler, CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(new ObterHistoricoOrcamentoQuery(id), cancellationToken);

        return Ok(result);
    }
}

public sealed record CriarOrcamentoRequest(Guid ClienteId, Guid VeiculoId, DateTimeOffset ValidadeEm);

public sealed record AdicionarItemRequest(Guid ItemCatalogoId, decimal Quantidade, TipoDesconto TipoDesconto, decimal Desconto, string? UsuarioAutorizadorDesconto);

public sealed record EnviarOrcamentoRequest(string Usuario);

public sealed record AprovarOrcamentoRequest(IReadOnlyCollection<Guid> ItensIds, string Usuario, CanalAprovacao Canal);

public sealed record ReprovarOrcamentoRequest(string Motivo, string Usuario);

public sealed record RevisarOrcamentoRequest(DateTimeOffset ValidadeEm, string Usuario);

public sealed record ConverterEmOrdemServicoRequest(string Usuario);