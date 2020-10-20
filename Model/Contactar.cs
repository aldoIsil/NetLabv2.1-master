using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Serializable]
    public class Contactar : General
    {
        public string nombres { get; set; }
        public string apellidos { get; set; }
        public string cargo { get; set; }
        public int idEstablecimiento { get; set; }
        public string celular { get; set; }
        public string email { get; set; }
        public string mensaje { get; set; }
    }
}
