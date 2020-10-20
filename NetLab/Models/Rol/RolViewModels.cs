using System;
using System.ComponentModel.DataAnnotations;

namespace NetLab.Models.Rol
{
    [Serializable]
    public class RolViewModels
    {
        public Model.Rol Rol { get; set; } 

        //[Display(Name = "Tipo")]
        //public TipoCriterioViewModels TipoCriterio { get; set; }
    }
}