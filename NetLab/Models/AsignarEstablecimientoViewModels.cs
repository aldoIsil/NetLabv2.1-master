using System;

namespace NetLab.Models
{
    [Serializable]
    public class AsignarEstablecimientoViewModels
    {
        public int IdUsuario { get; set; } 
        public int CodigoInstitucion { get; set; }
        public int CodigoDisa { get; set; }
        public int CodigoRed { get; set; }
        public int CodigoMicroRed { get; set; }
        public int CodigoEstablecimiento { get; set; }
        public int[] ChkEliminar { get; set; }
        public string Submit { get; set; }
    }
}