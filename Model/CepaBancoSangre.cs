using System;

namespace Model
{
    [Serializable]
    public class CepaBancoSangre:General
    {
        public Guid idOrden { get; set; }
        public Guid idCepaBancoSangre {get;set;} // [uniqueidentifier] NOT NULL,
	    public int tipo {get;set;}
	    public string codificacion {get;set;}
        public DateTime fechaColeccion { get; set; }//] [date] NULL,
        public DateTime horaColeccion { get; set; }//] [time](7) NULL,
        public Guid idMuestraCod { get; set; }

        public int idMaterial { get; set; }

        public MuestraCodificacion muestra { get; set; }
    }
}
