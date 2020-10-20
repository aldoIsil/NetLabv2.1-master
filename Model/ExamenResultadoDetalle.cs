using System;
using System.Collections.Generic;

namespace Model
{
    [Serializable]
    public class ExamenResultadoDetalle
    {
        public string CodigoMuestra { get; set; }
        public string TipoMuestra { get; set; }
        public string Analito { get; set; } 
        public string Resultado { get; set; }
        public decimal ValorInferior { get; set; }
        public decimal ValorSuperior { get; set; }
        public string Observacion { get; set; }
        public DateTime FechaEmision { get; set; }
        public string Validador { get; set; }
        public string Liberador { get; set; }



        public string TipodeMuestra { get; set; }
        public string Enfermedad { get; set; }
        public string nombreExamen { get; set; }
        public string Componente { get; set; }
        public string tagResultado { get; set; }
        public string ValorReferencia { get; set; }
        public string Analista { get; set; }
        public string Verificador { get; set; }
        public string FechaHoraEmision { get; set; }
        public List<AnalitoOpcionResultado> ListJsResultados { get; set; }
        public int NroSecuencia { get; set; }
        public int estatusSol { get; set; }
        public int nroInforme { get; set; }
        public string interpretacion { get; set; }
        public string metodo { get; set; }
        public int iPruebaRapida { get; set; }
        public string EjecutorPr { get; set; }
        public string Plataforma { get; set; }
        public int Inacal { get; set; }
    }

    [Serializable]
    public class ExamenResultadoInterpretacion
    {
        public int idInterpretacion { get; set; }
        public string idAnalito { get; set; }
        public string Analito { get; set; }
        public string GlosaParent { get; set; }
        public string Glosa { get; set; }
        public string Interpretacion { get; set; }
        public int Estado { get; set; }
    }
}