using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Serializable]
    public class Correo
    {

        public OrdenSolicitante Solicitante { get; set; }
        public Paciente Paciente { get; set; }

        public Orden Orden { get; set; }
    }

    [Serializable]
    public class EnvioAlerta
    {
        public string Solicitante { get; set; }
        public string CelularSolicitante { get; set; }
        public string CorreoSolicitante { get; set; }
        public string Paciente { get; set; }
        public string CelularPaciente { get; set; }
        public string CorreoPaciente { get; set; }
        public string CodigoOrden { get; set; }
        public string Resultado { get; set; }
        public int Envio { get; set; }
    }
}
