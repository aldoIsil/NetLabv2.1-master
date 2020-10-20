using System;
using System.Collections.Generic;
using Model;

namespace NetLab.Models.Plantilla
{
    [Serializable]
    public class EstablecimientoPlantillaViewModels
    {
        public Model.Plantilla Plantilla { get; set; }
        public List<Establecimiento> Establecimientos { get; set; } 
    }
}