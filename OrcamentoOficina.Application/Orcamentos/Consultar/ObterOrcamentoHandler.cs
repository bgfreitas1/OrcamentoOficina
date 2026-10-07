using OrcamentoOficina.Application.Abstractions.Persistence;
using OrcamentoOficina.Application.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrcamentoOficina.Application.Orcamentos.Consultar
{
    public sealed class ObterOrcamentoHandler(IOrcamentoRepository orcamentoRepository)
    {
        public async Task<OrcamentoDetalheResult> HandleAsync(ObterOrcamentoQuery query, CancellationToken cancellationToken = default)
        {
            var orcamento = await orcamentoRepository.ObterPorIdAsync(query.OrcamentoId, cancellationToken) ?? throw new NotFoundException("Orçamento não encontrado.");

            return new OrcamentoDetalheResult(
                orcamento.Id,
                orcamento.ClienteId,
                orcamento.VeiculoId,
                orcamento.Versao,
                orcamento.GrupoVersaoId,
                orcamento.OrcamentoOriginalId,
                orcamento.Status,
                orcamento.CriadoEm,
                orcamento.ValidadeEm,
                orcamento.EnviadoEm,
                orcamento.Subtotal,
                orcamento.ValorDescontoGeral,
                orcamento.Total,
                orcamento.MotivoReprovacao,
                orcamento.Itens
                    .Select(item => new ItemOrcamentoResult(
                        item.Id,
                        item.Descricao,
                        item.Tipo,
                        item.Quantidade,
                        item.PrecoUnitario,
                        item.Desconto.Tipo,
                        item.Desconto.Valor,
                        item.ValorDesconto,
                        item.Total,
                        item.DisponivelEmEstoque,
                        item.Aprovado,
                        item.UsuarioAprovacao,
                        item.DataAprovacao,
                        item.CanalAprovacao))
                    .ToList());
        }
    }
}
