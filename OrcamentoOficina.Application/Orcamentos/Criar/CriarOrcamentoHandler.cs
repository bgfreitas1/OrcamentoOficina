using OrcamentoOficina.Application.Abstractions.Persistence;
using OrcamentoOficina.Application.Common.Exceptions;
using OrcamentoOficina.Domain.Entities;

namespace OrcamentoOficina.Application.Orcamentos.Criar
{
    public sealed class CriarOrcamentoHandler(IOrcamentoRepository orcamentoRepository, IClienteRepository clienteRepository, IVeiculoRepository veiculoRepository, IUnitOfWork unitOfWork, TimeProvider timeProvider)
    {
        public async Task<CriarOrcamentoResult> HandleAsync(CriarOrcamentoCommand command, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(command);

            if (!await clienteRepository.ExisteAsync(command.ClienteId, cancellationToken))
                throw new NotFoundException($"Cliente '{command.ClienteId}' não encontrado.");
            
            if (!await veiculoRepository.PertenceAoClienteAsync(command.VeiculoId, command.ClienteId, cancellationToken))
                throw new NotFoundException("Veículo não encontrado para o cliente informado.");
            
            var agora = timeProvider.GetUtcNow();

            var orcamento = new Orcamento(command.ClienteId, command.VeiculoId, agora, command.ValidadeEm);

            await orcamentoRepository.AdicionarAsync(orcamento, cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return new CriarOrcamentoResult(orcamento.Id, orcamento.Versao);
        }
    }
}
