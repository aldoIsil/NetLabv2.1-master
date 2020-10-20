using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Serializable]
    public class CargoUsuarioEstablecimiento
    {
        public string nombresUsuario { get; set; }
        public int idEstablecimiento { get; set; }
        public string cargo { get; set; }
        public DateTime periodoIni { get; set; }
        public DateTime periodoFin { get; set; }
        public int idUsuarioRegistro { get; set; }
        public DateTime fechaRegistro { get; set; }

    }
}
