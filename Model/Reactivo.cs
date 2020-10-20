using System;

namespace Model
{
    [Serializable]
    public class Reactivo:General
    {
        public int idReactivo {get;set;}//] [int] NOT NULL,
	    public string glosa {get;set;}//] [varchar](3000) NULL,
    }
}
