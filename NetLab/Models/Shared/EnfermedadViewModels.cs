using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace NetLab.Models.Shared
{
    [Serializable]
    public class EnfermedadViewModels
    {
        public List<Enfermedad> Data { get; set; }

        public string IdEnfermedad { get; set; }

        public IEnumerable<SelectListItem> Enfermedades
        {
            get
            {
                if (!Data.Any()) return new List<SelectListItem>
                {
                    new SelectListItem
                    {
                        Value = string.Empty,
                        Text = string.Empty
                    }
                };

                return Data.Select(d => new SelectListItem
                {
                    Value = d.idEnfermedad.ToString(),
                    Text = d.nombre
                });
            }
        }
    }
}