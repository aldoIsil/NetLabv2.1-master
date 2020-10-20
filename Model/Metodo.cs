using System;

namespace Model
{
    [Serializable]
    public class Metodo:General
    {
        public int idMetodo { get; set; }//] [int] NOT NULL,
	    public string nombre {get;set;}//] [varchar](200) NOT NULL,
        public byte[] codigo { get; set; }//] [varbinary](100) NULL,
	    public string glosa1 {get;set;}//] [varchar](1000) NULL,
	    public string glosa2 {get;set;}//] [varchar](1000) NULL,
	    public int ordenMetodo {get;set;}//] [int] NOT NULL,
	    public int idAnalisis {get;set;}//] [int] NULL,
    }
}
