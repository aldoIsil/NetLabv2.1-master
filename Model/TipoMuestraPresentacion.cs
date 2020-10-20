using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Serializable]
    public class TipoMuestraPresentacion
    {
        public TipoMuestra TipoMuestra { get; set; }
        
        public Presentacion Presentacion { get; set; }
        
      
        
    }
}
