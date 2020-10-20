using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NetLab.Models
{
    [Serializable]
    public class SolicitudExamenTropismoViewModels
    {
        public Paciente paciente { get; set; }
        public Establecimiento establecimiento { get; set; }
        public SolicitanteViewModels solicitante { get; set; }
        public string criterioTropismo { get; set; }
        public ExamenResultadoDetalle resultado { get; set; }
        public DateTime fechaTropismo { get; set; }
        public DateTime fechaMuestra { get; set; }
    }
}