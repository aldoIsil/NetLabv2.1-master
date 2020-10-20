using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Serializable]
    public class ExcelResultadoTbGenotype
    {
        public string codigoOrden { get; set; }
        public string codigoMuestra { get; set; }
        public string tipoMuestra { get; set; }
        public string resultado { get; set; }
        public string Rifampicina { get; set; }
        public string GenRPOAltaResistente { get; set; }
        public string Isoniacida { get; set; }
        public string GenINHaAltaResistencia { get; set; }
        public string GenKatGBajaResistencia { get; set; }
        public string GenINHaAltaResistencia_GenKatGBajaResistencia { get; set; }
    }
    [Serializable]
    public class CitometriaResultadoMasivo
    {
        public string codigoMuestra { get; set; }
        public string CD4 { get; set; }
        public string CD8 { get; set; }
        public string CD3 { get; set; }
        public string CD4_CD8 { get; set; }
        public string CD4_CD3 { get; set; }
        public string CD8_CD3 { get; set; }
        public DateTime fechaAnalisis { get; set; }
    }
    [Serializable]
    public class CitometriaResultadoMasivoProcesado
    {
        public string CodigoMuestra { get; set; }
        public string Observacion { get; set; }
        public string Estado { get; set; }
    }
    [Serializable]
    public class CodigoLineal
    {
        public string CodigoMuestraLineal { get; set; }
    }
}
