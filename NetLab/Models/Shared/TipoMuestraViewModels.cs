using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Model;

namespace NetLab.Models.Shared
{
    [Serializable]
    public class TipoMuestraViewModels
    {
        public List<TipoMuestra> Data { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un tipo de muestra")]
        public int IdTipoMuestra { get; set; }

        public IEnumerable<SelectListItem> Muestras
        {
            get
            {
                return Data.Select(d => new SelectListItem
                {
                    Value = d.idTipoMuestra.ToString(),
                    Text = d.nombre
                });
            }
        }
    }
}