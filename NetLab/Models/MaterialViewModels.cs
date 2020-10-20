using System;
using System.ComponentModel.DataAnnotations;
using Model;
using NetLab.Models.Shared;

namespace NetLab.Models
{
    [Serializable]
    public class MaterialViewModels
    {
        public Material Material { get; set; } 

        [Display(Name = "Tipo de Muestra")]
        public TipoMuestraViewModels TipoMuestra { get; set; }

        [Display(Name = "Presentación")]
        public PresentacionViewModels Presentacion { get; set; }

        public ReactivoViewModels Reactivo { get; set; }
    }
}