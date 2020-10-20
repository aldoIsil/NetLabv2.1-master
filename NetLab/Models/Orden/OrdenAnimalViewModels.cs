using NetLab.Models.Shared;
using System;
using System.Collections.Generic;

namespace NetLab.Models.Orden
{
    [Serializable]
    public class OrdenAnimalViewModels
    {
        public Model.Orden Orden { get; set; }

        public Model.Animal Animal { get; set; }

        public ProyectoViewModels Proyecto { get; set; }

        public EstablecimientoViewModels Establecimiento { get; set; }

        public string Observacion { get; set; }

        public List<Model.OrdenExamen> OrdenExamen { get; set; }

        public List<Model.OrdenMuestra> OrdenMuestra { get; set; }

        public List<Model.OrdenMaterial> OrdenMaterial { get; set; }
    }
}