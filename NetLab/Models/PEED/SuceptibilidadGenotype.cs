using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NetLab.Models.PEED
{
    [Serializable]
    public class SuceptibilidadGenotype
    {
        public int idOpcion { get; set; }
        public string NroPregunta { get; set; }
        public string IdentificacionMolecular { get; set; }
        public string Rifampicina { get; set; }
        public string Isoniacida { get; set; }
        public string KatG { get; set; }
        public string InhA { get; set; }
        public string KatGInhA { get; set; }

        public List<SuceptibilidadGenotype> LstSuceptibilidadGenotype { get; set; }
    }
}