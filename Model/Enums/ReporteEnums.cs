using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enums
{
    [Serializable]
    public enum TipoReporte
    {
        OportunidadObtencionMuestras = 1,
        OportunidadEnvioMuestras = 2,
        OportunidadAnalisisMuestras = 3,
        PorcentajeResultadosEmitidos = 4,
        //JRCR - 2doProducto
        MotivoRechazoROM = 5,//Radar
        MotivoRechazoLaboratorio = 6,//Radar
        NroPorcentajeMuestrasRechazadasEnROM = 7, //reporte iii y iv, nivel regional y nacional //pie
        NroPorcentajeResultadosRechazadosPorVerificador = 8, //pie
        NroResultadosCorregidosDespuesPublicacion = 9, //bar
        NroMuestrasProcesadasBaciloscopia = 10,
        CantidadReporteResultadoConsultado = 11,
        CantidadReporteResultadoConsultadoPorSolicitante = 12,
        CantidadReporteResultadoInformados = 13,
        /*yrca*/
        ReportedeProductividad =14

    }

    [Serializable]
    public enum Colores
    {
        Red = 1,
        Blue = 2,
        Green = 3,
        Peru = 4,
        Maroon = 5,
        Yellow = 6,
        Pink = 7,
    }
}
