using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NetLab.Models.PEED
{
    [Serializable]
    public class Baciloscopia
    {
        public int idOpcion { get; set; }
        public string NroPregunta { get; set; }
        public string Resultados { get; set; }
        public List<Baciloscopia> LstBaciloscopia { get; set; }
    }
}