using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Serializable]
    public class OrdenMuestrasRechazo
    {
        public Guid idOrden { get; set; }
        public string nombreTipoMuestra { get; set; }
        public string nombrePaciente { get; set; }
        public string nombreEstablecimiento { get; set; }
        public DateTime fechaRechazo { get; set; }
        public string nombreUsuario { get; set; }
        public string CriterioRechazo { get; set; }
        public string Codificacion { get; set; }
        public string nombreExamen { get; set; }
        public int idCriterioRechazo { get; set; }
        public int tipoRechaazoAlta { get; set; }        
    }
}
