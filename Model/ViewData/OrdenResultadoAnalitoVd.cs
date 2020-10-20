using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewData
{
    [Serializable]
    public class OrdenResultadoAnalitoVd
    {
        public string unidad { get; set; }
        public OrdenResultadoAnalito OItem { get; set; }
        public Analito Item { get; set; }
        public AnalitoValorNormal VItem { get; set; }
        public List<ExamenMetodo> Metodos { get; set; }

        public List<AnalitoOpcionResultado> Opciones { get; set; }
    }
}
