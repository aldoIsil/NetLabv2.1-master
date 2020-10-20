using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewData
{
    [Serializable]
    class OrdenResultadoDetalle
    {
        public Guid idOrden { get; set; }
        public string ConID_Muestra { get; set; }
        public string ConTipoMuestra { get; set; }
        public string ConEnfermedad { get; set; }
        public string ConnombreExamen { get; set; }

        public string ConnResultado { get; set; }

        public string ConFechaHoraColeccion { get; set; }
        public string ConFechaHoraRecepcionROM { get; set; }
        public string ConFechaHoraRecepcionLAB { get; set; }

        public string ConFechaHoravalidado { get; set; }

        public string ConEstatusResultado { get; set; }

    }
}
