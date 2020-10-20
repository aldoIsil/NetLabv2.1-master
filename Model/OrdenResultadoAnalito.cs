using System;

namespace Model
{
    [Serializable]
    public class OrdenResultadoAnalito:General
    {
       	public Guid idOrdenResultadoAnalisis {get;set;}//] [uniqueidentifier] NOT NULL,

        public Guid idOrdenExamen {get;set;}//] [uniqueidentifier] NULL,

        public int idAnalisis {get;set;}//] [int] NULL,

        public int idArea {get;set;}//] [int] NULL,

        public decimal valor {get;set;}//] [decimal](18, 6) NULL,

        public string glosa {get;set;}//] [nchar](1000) NULL,

        public int orden {get;set;}//] [int] NULL,

        public string codigoOpcion { get; set; }//] [nchar](10) NULL,

        public int ingresado { get; set; }

        public int validado { get; set; }

        public int liberado { get; set; }

        public string observacion { get; set; }

        public string resultado { get; set; }

        public int idExamenMetodo { get; set; }
        public string convResultado { get; set; }
        public string interpretacion { get; set; }
        public int idSecuencia { get; set; }
        public int RegistroyFinalizacion { get; set; }
        public int Plataforma { get; set; }

    }
}
