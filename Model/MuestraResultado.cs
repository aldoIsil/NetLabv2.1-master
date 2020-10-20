using System;

namespace Model
{
    [Serializable]
    public class MuestraResultado
    {
        public string TipoMuestra { get; set; }
        public string Remitente { get; set; }
        public DateTime FechaColeccion { get; set; }
        public string Receptor { get; set; }
        public DateTime FechaRecepcion { get; set; }

        public string codigomuestra { get; set; }

        public string EESS_LAB_Destino { get; set; }

        public string FechaHoraColeccion { get; set; }

        public string FechaHoraRecepcionROM { get; set; }

        public string FechaHoraRecepcionLAB { get; set; }

        public string tipodeMuestra { get; set; }

    }
}