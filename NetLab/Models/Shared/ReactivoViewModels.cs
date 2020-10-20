using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Model;
using System.ComponentModel.DataAnnotations;
using System;

namespace NetLab.Models.Shared
{
    [Serializable]
    public class ReactivoViewModels
    {
        public List<Reactivo> Data { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un reactivo")]
        public int IdReactivo { get; set; }

        public IEnumerable<SelectListItem> Reactivos
        {
            get
            {
                return Data.Select(d => new SelectListItem
                {
                    Value = d.idReactivo.ToString(),
                    Text = d.glosa
                });
            }
        }
    }
}