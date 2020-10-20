using System;
using System.Collections.Generic;

namespace Model
{
    [Serializable]
    public class OrdenExamen:General
    {
       	public Guid idOrdenExamen {get;set;}//] [uniqueidentifier] NOT NULL,
	    public Guid idOrden {get;set;}//] [uniqueidentifier] NOT NULL,
        public Examen Examen { get; set; }

        public Guid idExamen { get; set; }

        public int idEnfermedad { get; set; }
        public int idExamenMetodo { get; set; }
        
        public Enfermedad Enfermedad { get; set; }

        public List<OrdenMuestra> ordenMuestraList { get; set; }

        public int estatus { get; set; }

        public int estatusE { get; set; }

        public String idEnfermedadExamen { get { return Enfermedad.idEnfermedad + "_" + Examen.idExamen; } }

        public String descripcion { get { return Enfermedad.nombre + "_" + Examen.nombre; } }

        public int IdTipoMuestra { get; set; }
        //Juan Muga - inicio
        public OrdenExamen()
        {
            ordenMuestraList = new List<OrdenMuestra>();
        }
        //Juan Muga - fin

        //sotero - para nombre de muestra
        public string nombreTipoMuestra { get; set; }
        public int worked { get; set; }
        public string codigoExamen { get; set; }
    }
}
