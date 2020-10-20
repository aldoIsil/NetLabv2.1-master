using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Serializable]
    public class OrdenMuestraRom : General
    {
        public List<OrdenMuestraRecepcion> listaOrdenesMuestra { get; set; }
        public List<OrdenMuestraRecepcion> listaOrdenesMuestraCrearReferenciar { get; set; }
        public List<OrdenMuestraRecepcion> listaOrdenesMuestraCrearRecepcionar { get; set; }
        public List<OrdenMuestraRecepcion> listaOrdenesMuestraRecepcionar { get; set; }
        public List<OrdenMuestraRecepcion> listaParaReferenciarCrear { get; set; }
        public List<OrdenMuestraRecepcion> listaParaRecepcionarCrear { get; set; }
        public List<OrdenMuestraRecepcion> listaParaRecepcionarCrearSinRecepcion { get; set; }
        public List<OrdenMuestraRecepcion> listaOrdenesDerivarMuestra { get; set; }
    }
}
