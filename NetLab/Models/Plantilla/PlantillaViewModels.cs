using System;
using System.ComponentModel.DataAnnotations;

namespace NetLab.Models.Plantilla
{
    [Serializable]
    public class PlantillaViewModels
    {
        public Model.Plantilla Plantilla { get; set; }

        [Display(Name = "Tipo de Muestra")]
        public TipoMuestraViewModels TipoMuestra { get; set; }
    }
}