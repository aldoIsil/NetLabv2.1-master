using System;
using System.Collections.Generic;

namespace Model
{
    [Serializable]
    public class OrdenMuestra:General
    {
        public Guid idOrdenMuestra {get;set;}//] [uniqueidentifier] NOT NULL,
	    public Guid idOrden {get;set;}// [uniqueidentifier] NOT NULL,
	//    public int idTipoMuestra {get;set;}//] [int] NULL,
	    public int idProyecto {get;set;}//] [int] NULL,
	    public Guid codigo {get;set;}//] [uniqueidentifier] NULL,
	    public DateTime fechaColeccion {get;set;}//] [date] NULL,
        public string fechaColeccionToString => fechaColeccion.ToString("dd/MM/yyyy");


        public DateTime horaColeccion {get;set;}//] [time](7) NULL,

        public string horaColeccionToString => horaColeccion.ToString("HH:mm");

        public int numero {get;set;}//] [int] NOT NULL,
	    public int seriado {get;set;}//] [int] NOT NULL,

        public List<OrdenMaterial> ordenMaterialList { get; set; }

        public List<OrdenExamen> ordenExamenList { get; set; }

        public TipoMuestra TipoMuestra { get; set; }

        public Guid idOrdenExamen { get; set; }

        public Guid idMuestraCod { get; set; }

        public MuestraCodificacion MuestraCodificacion { get; set; }

        public int estatus { get; set; }

        public Establecimiento Establecimiento { get; set; }

        public List<CriterioRechazo> criterioRechazoList { get; set; }

        public int idTipoMuestra { get; set; }
        //Juan Muga - inicio
        public OrdenMuestra()
        {
            ordenExamenList = new List<OrdenExamen>();
            MuestraCodificacion = new MuestraCodificacion();
            ordenMaterialList = new List<OrdenMaterial>();
            TipoMuestra = new TipoMuestra();
        }
        //Juan Muga - fin
    }

    public class OrdenSolicitudSalidaMuestra
    {
        public Guid idOrdenMuestra { get; set; }
        public string formato { get; set; }
        public string nroOficio { get; set; }
        public string codigoLineal { get; set; }
        public string codigoMuestra { get; set; }
        public string fechaEnvioMuestra { get; set; }
        public string Establecimiento { get; set; }
        public string Paciente { get; set; }
        public string DocumentoIdentidad { get; set; }

    }
}
