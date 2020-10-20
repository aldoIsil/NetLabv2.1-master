using System;
using System.Collections.Generic;

namespace Model
{
    [Serializable]
    public class OrdenMaterial : General
    {
        public Guid idOrdenMaterial  {get;set;}// [uniqueidentifier] NOT NULL,
	    public Guid idOrden {get;set;}//] [uniqueidentifier] NULL,
	    public int idMaterial {get;set;}//] [int] NULL,
	    public int cantidad {get;set;}//] [int] NULL,
	    public Guid codigo {get;set;}//] [uniqueidentifier] NULL,
	  //  public Establecimiento Establecimiento {get;set;}//] [int] NULL,
        public int idProyecto { get; set; }//] [int] NULL,
        public List<OrdenMuestraRecepcion> ordenMuestraRecepcionList { get; set; }
        public string tipoMuestraNombre { get; set; }

        public Material Material { get; set; }

        public int estatus { get; set; }

        public decimal volumenMuestraColectada { get; set; }


        public OrdenMuestra ordenMuestra { get; set;  }

        public int idTipoMuestra { get; set; }

        public int noPrecisaVolumen { get; set; }

        public DateTime fechaEnvio { get; set; }//] [date] NULL,
        public string fechaEnvioToString => fechaEnvio.ToString("dd/MM/yyyy");


        public DateTime horaEnvio { get; set; }//] [time](7) NULL,

        public string horaEnvioToString => horaEnvio.ToString("HH:mm");


        public int Tipo { get; set; }

        public String tipoToString { get { return Tipo == 1 ? "Muestra Primaria" : "Alícuota"; } }


        public Laboratorio Laboratorio { get; set; }

        public OrdenExamen OrdenExamen { get; set; }
        public string ExamenNombreCompleto { get; set; }

        //SOTERO BUSTMANTE AGREGADO PARA NRO DE MUESTRA DE PACIENTE
        public int nroMuestra { get; set; }

        public Guid idMuestraCod { get; set; }
    }
}
