using System;
using System.Collections.Generic;
using Model;

namespace NetLab.Models.Reportes
{
    [Serializable]
    public class ReporteResultadoViewModels
    {
        public OrdenResultado Orden { get; set; }

        public Laboratorio Laboratorio { get; set; }

        public List<MuestraResultado> Muestras { get; set; }

        public List<ExamenResultadoDetalle> Examenes { get; set; } 

        public List<ExamenResultadoInterpretacion> Interpretacion { get; set; }

        public CargoUsuarioEstablecimiento CargoUsuarioEstablecimiento { get; set; }
    }
}