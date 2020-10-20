using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewData
{
    [Serializable]
    public class OrdenRecepcionDetalleVd
    {
        //Detalle
        public Guid idOrden { get; set; }
        public string FechaSolicitud { get; set; }
        public string FechaObtencion { get; set; }
        public string HoraObtencion { get; set; }
        public string codigoOrden { get; set; }
        public string EstablecimientoOrigen { get; set; }
        public string codigoMuestra { get; set; }
        public string Enfermedad { get; set; }
        public string Examen { get; set; }
        public string TipoMuestra { get; set; }
        public string Tipo { get; set; }
        public string EstablecimientoDestino { get; set; }
        public int ExistePendiente { get; set; }
        public int ExisteRecibido { get; set; }
        public int ExisteRechazo { get; set; }
        public string estatus { get; set; }
        public string ConformeR { get; set; }
        public int IdLaboratorioDestino { get; set; }
        public string FechaRecepcion { get; set; }
        public string HoraRecepcion { get; set; }
        public string IdMuestraCod { get; set; }
        //Jose Chavez
        public int EstatusP { get; set; }
        public string FechaRecepcionLab { get; set; }
        public string HoraRecepcionLab { get; set; }
        public Guid IdOrdenMuestra { get; set; }
        public Guid CodigoOrdenMuestraRecepcion { get; set; }
        public int flagResultado { get; set; }
        public string validado { get; set; }
    }
}
