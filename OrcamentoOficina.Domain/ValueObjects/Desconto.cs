using OrcamentoOficina.Domain.Enums;

namespace OrcamentoOficina.Domain.ValueObjects
{
    public sealed class Desconto
    {
        public TipoDesconto Tipo { get; private set; }

        public decimal Valor { get; private set; }

        private Desconto()
        {
        }

        public Desconto(TipoDesconto tipo, decimal valor)
        {
            if (!Enum.IsDefined(tipo))
                throw new ArgumentOutOfRangeException(nameof(tipo));

            if (valor < 0)
                throw new ArgumentOutOfRangeException(
                    nameof(valor), "O desconto não pode ser negativo.");

            if (tipo == TipoDesconto.Percentual && valor > 100)
                throw new ArgumentOutOfRangeException(
                    nameof(valor), "O percentual não pode exceder 100%.");

            Tipo = tipo;
            Valor = valor;
        }

        public decimal Calcular(decimal valorBase)
        {
            if (valorBase < 0)
                throw new ArgumentOutOfRangeException(nameof(valorBase));

            var desconto = Tipo switch
            {
                TipoDesconto.ValorFixo => Valor, TipoDesconto.Percentual => valorBase * Valor / 100m, _ => throw new InvalidOperationException("Tipo de desconto inválido.")
            };

            return Math.Round(Math.Min(desconto, valorBase), 2, MidpointRounding.AwayFromZero);
        }

        public decimal PercentualEquivalente(decimal valorBase)
        {
            if (valorBase <= 0)
                return 0;

            return Calcular(valorBase) / valorBase * 100m;
        }
    }
}
