using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewData
{
    [Serializable]
    public class OrdenVd
    {
        public string nombreEnfermedad { get; set; }
        public string nombreTipoMuestra { get; set; }
        public string nombreExamen { get; set; }
        public int ConformeR { get; set; }
        public Guid idExamen { get; set; }
    }
}
