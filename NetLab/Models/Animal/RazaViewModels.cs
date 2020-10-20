using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Model;

namespace NetLab.Models.Animal
{
    [Serializable]
    public class RazaViewModels
    {
        public List<AnimalRaza> Data { get; set; }

        public int IdRaza { get; set; }

        public IEnumerable<SelectListItem> Razas
        {
            get
            {
                return Data.Select(d => new SelectListItem
                {
                    Value = d.IdRaza.ToString(),
                    Text = d.Nombre
                });
            }
        }
    }
}