using System;

namespace Model
{
    [Serializable]
    public class MuestraCodificacion:General
    {
      	public Guid idMuestraCod { get;set;}//] [int] NULL,
        public int idEstablecimiento { get; set; }//] [int] NULL,
        public string codificacion { get;set;}//] [varchar](50) NULL,
        public string codificacionLineal { get; set; }//] [varchar](50) NULL,
        public string idCodificacion {get;set; }//] [int] NULL,
        public string estado { get; set; }
        public string fecha { get; set; }
        public string usuario { get; set; }

    }
}
