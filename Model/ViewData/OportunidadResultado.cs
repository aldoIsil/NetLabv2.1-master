using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewData
{
    [Serializable]
    public class OportunidadResultado
    {
        public int idIndicador { get; set; }
        public int total { get; set; }
        public int anio { get; set; }
        public string mes { get; set; }
        public int nroOportuno { get; set; }
        public string porcOportunidad { get; set; }
        public int tiempoCatalogo { get; set; }
    }
}
