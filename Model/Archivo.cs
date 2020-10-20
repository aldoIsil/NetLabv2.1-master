using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Serializable]
    public class Archivo : General
    {
        public int cCodArchivo { get; set; }
        public int Codigo { get; set; }
        public string dsNombre { get; set; }
        public byte[] dsData { get; set; }
        public DateTime FeRegistro { get; set; }
    }
}
