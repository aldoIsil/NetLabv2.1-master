using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewData
{
    [Serializable]
    public class OrdenMaterialVd
    {
        public string tipoMuestraNombre { get; set; }
        public string presentacionNombre { get; set; }
        public string cantidad { get; set; }
        public string reactivoNombre { get; set; }

    }
}
