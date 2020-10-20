using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Model;

namespace NetLab.Models.Shared
{
    [Serializable]
    public class PresentacionViewModels
    {
        public List<Presentacion> Data { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una presentación")]
        public int IdPresentacion { get; set; }

        public IEnumerable<SelectListItem> Presentaciones
        {
            get
            {
                return Data.Select(d => new SelectListItem
                {
                    Value = d.idPresentacion.ToString(),
                    Text = d.glosa
                });
            }
        }
    }
}