using System;
using System.Collections.Generic;

namespace NetLab.Models.Plantilla
{
    [Serializable]
    public class PlantillaMuestraViewModels
    {
        public Model.Plantilla Plantilla { get; set; }
        public List<ExamenMuestraViewModels> Muestras { get; set; }
    }
}