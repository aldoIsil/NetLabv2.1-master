using System;
using System.Collections.Generic;

namespace Model
{
    [Serializable]
    public class OrdenMuestraRecepcion:General
    {
        public Guid idOrdenMuestraRecepcion {get;set;}//] [uniqueidentifier] NOT NULL,
	    public Guid idOrdenMuestra {get;set;}//] [uniqueidentifier] NULL,
	    public Guid idOrden {get;set;}//] [uniqueidentifier] NULL,
	    public int idMaterial {get;set;}//] [int] NULL,
	    public DateTime ? fechaRecepcion {get;set;}//] [date] NOT NULL,
	    public DateTime ? horaRecepcion {get;set;}//] [time](7) NOT NULL,
	    public int idEstablecimiento {get;set;}//] [int] NULL,
	    public int procesamiento { get; set; }//] [int] NULL,
        public string presentacionNombreNro  { get; set; }//] [int] NULL,
        public int estatus { get; set; }
        public DateTime ? fechaEnvio { get; set; }//] [date] NOT NULL,
        public DateTime ? horaEnvio { get; set; }//] [time](7) NOT NUL
        public int idLaboratorioDestino { get; set; }//] [time](7) NOT NUL
        public int idLaboratorioOrigen { get; set; }//] [time](7) NOT NUL
        public int conforme { get; set; }//] [int] NULL,
        public string examenNombre { get; set; }//] [int] NULL,
        public int Tipo { get; set; }//] [int] NULL,
        public string nombreLaboratorioDestino { get; set; }//] [int] NULL,
        public Guid idExamen { get; set; }//] [int] NULL,
        public Guid idOrdenMuestraRecepcionPadre { get; set; }
        public List<CriterioRechazo> criterioRechazoList { get; set; }
        public OrdenMaterial OrdenMaterial { get; set; }
        public string codigoMuestra { get; set; }
        public string rechazos { get; set; }
        public string horaRecepcionStr { get; set; }
        public string nombreLaboratorioEnvio { get; set; }
        public string fechaHoraColeccionStr { get; set; }
        public DateTime? fechaColeccion { get; set; }//] [date] NOT NULL,
        public DateTime? horaColeccion { get; set; }//] [time](7) NOT NUL

        public Int32 derivar { get; set; }// Juan Muga
        public Int32 idTipoMuestra { get; set; }// Juan Muga
        public Guid NewidOrdenMuestraRecepcion { get; set; }// Juan Muga
        //Juan Muga - inicio - SP pNLS_OrdenById
        public Guid idOrdenMaterial { get; set; } 
        public int idUsuarioRegistro { get; set; }
        public int estatusR { get; set; }
        //Juan Muga - fin
    }
}
