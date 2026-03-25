using EspacioCadete;

namespace EspacioCadeteria
{
    // Un Record es como una clase, pero optimizada para ser solo de lectura.
    // Ideal para informes o DTOs.
    public record Informe
    {
        public int TotalPedidos { get; init; }
        public int PedidosEntregados { get; init; }
        public double MontoTotalRecaudadoCadeteria { get; init; }
        public List<string> PromediosDeEntregaPorCadete { get; init; }
        public List<string> RecaudadoPorCadete { get; init; }
        public double TotalRecaudado { get; init; }
    }
}