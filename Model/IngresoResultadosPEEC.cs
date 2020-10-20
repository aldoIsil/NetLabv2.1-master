using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Serializable]
    public class IngresoResultadosPEEC
    {
        public Guid idConfiguracionPanel { get; set; }
        public Guid idConfigEvaluacion { get; set; }
        [DisplayName("Lab Evaluador")]
        public string EESSEvaluador { get; set; }
        public string nroLote { get; set; }
        public string nombre { get; set; }
        [DisplayName("Descripción")]
        public string descripcion { get; set; }
        [DisplayName("Tipo")]
        public int idTipo { get; set; }
        public int idEESSEvaluador { get; set; }
        public int idEESSEvaluado { get; set; }
        public string DescTipo { get; set; }
        [DisplayName("Evaluación")]
        public string DescEvaluacion { get; set; }
        [DisplayName("Resultado")]
        public string Resultado { get; set; }
        
    }
}
