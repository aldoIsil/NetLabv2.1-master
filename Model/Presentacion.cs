using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Model
{
    [Serializable]
    public class Presentacion:General
    {
        public int idPresentacion {get;set;}//] [int] NOT NULL,

        [Display(Name = "Glosa")]
        public string glosa { get; set; }//] [varchar](3000) NULL,

        public string Est { get; set; }

        public bool estadoBool { get; set; }
        
    }
}
