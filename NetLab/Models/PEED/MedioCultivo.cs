using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NetLab.Models.PEED
{
    [Serializable]
    public class MedioCultivo
    {
        public int idOpcion { get; set; }
        public string NroPregunta { get; set; }
        public string Resultados { get; set; }
        public List<MedioCultivo> LstMedioCultivo { get; set; }
    }
}