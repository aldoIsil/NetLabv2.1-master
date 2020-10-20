using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Serializable]
    public class DetalleCatalogo
    {

        public String Prueba { get; set; }

        public String Patogeno { get; set; }

        public String Muestra { get; set; }

        public String Caracteristica { get; set; }

        public String Transporte { get; set; }

        public String Laboratorio { get; set; }

        public int DiasEmision { get; set; }

        public int DiasEntrega { get; set; }
        public String CodigoUnico { get; set; }

        public List<DetalleCatalogo> detallecatalogoList { get; set; }
    }
}
