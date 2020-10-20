using System;
using System.ComponentModel.DataAnnotations;

namespace NetLab.Models.CriterioRechazo
{
    [Serializable]
    public class CriterioViewModels
    {
        public Model.CriterioRechazo Criterio { get; set; } 

        [Display(Name = "Tipo")]
        public TipoCriterioViewModels TipoCriterio { get; set; }
    }
}