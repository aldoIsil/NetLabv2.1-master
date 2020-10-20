using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NetLab.Models
{
    [Serializable]
    public class PDFCodigoMuestraModel
    {
        public string codigo
        {
            get;
            set;
        }

        public string url
        {
            get;
            set;
        }
        public int cantidad
        {
            get;
            set;
        }
        //Sotero 03/07/2019 Generacion Codigo de Barra.
        public string Tipo
        {
            get;
            set;
        }
    }
}