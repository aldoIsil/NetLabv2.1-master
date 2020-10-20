using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Serializable]
    public class TramaEnvioResultado
    {
        public string num_solicitud { get; set; }
        public string num_orden { get; set; }
        public string est_orden { get; set; }
        public int cod_enfermedad { get; set; }
        public int cod_tip_muestra { get; set; }
        public string cod_muestra { get; set; }
        public string est_muestra { get; set; }
        public string cod_tip_examen { get; set; }
        public string cod_examen { get; set; }
        public string est_examen { get; set; }
        public string des_resultado { get; set; }
        public string des_rechazo_muestra { get; set; }
        public string des_rechazo_examen { get; set; }
        public string fec_rechazo { get; set; }
        public string fec_resultados { get; set; }
        public string fec_rechazo_muestra { get; set; }
        public string fec_rechazo_examen { get; set; }
    }
}
