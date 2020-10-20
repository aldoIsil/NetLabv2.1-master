using System;

namespace Model.ViewData
{
    [Serializable]
    public class MuestraResultadoVd
    {
        public Guid idOrdenMaterial { get; set; }
        public Guid idOrdenMuestraRecepcion { get; set; }
        public int idTipoMuestra { get; set;  }
        public string presentacion { get; set; }
        public string tipoMuestra { get; set; }
        public string reactivo { get; set; }
        public decimal volumen { get; set; }
        public string codificacion { get; set; }
        public DateTime fechaColeccion { get; set; }
        public DateTime fechaRecepcion { get; set; }
        public DateTime fechaRecepcionP { get; set; }
        public string MotivoNoConforme { get; set; }
        public string fechaHoraColeccion { get; set; }

        public string fechaHoraRecepcionLAB { get; set; }

        public string fechaHoraRecepcionROM { get; set; }
        
        public string muestraConforme { get; set; }

        public string criteriosRechazo { get; set; }

        public string observacionrechazo { get; set; }


        //AGREGADO SOTERO BUSTAMANTE ORDEN LLEGADA MUESTRA RECPCION 28/10/2017
        public int secuenObtencion { get; set; }
        public int idUsuario { get; set; }
        public string nombreExamen { get; set; }
        public string ExamenComun { get; set; }
        public Guid idExamen { get; set; }
    }
}
