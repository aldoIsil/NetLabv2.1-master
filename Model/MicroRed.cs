using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Serializable]
    public class MicroRed : General
    {

        public string IdMicroRed { get; set; }
        public string NombreMicroRed { get; set; }
        public string IdDISA { get; set; }
        public string IdRed { get; set; }
        //JRCR-REPORTE01
        [NotMapped]
        public int IdInstitucion { get; set; }

    }
}
