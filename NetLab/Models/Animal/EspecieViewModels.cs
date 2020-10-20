using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Model;

namespace NetLab.Models.Animal
{
    [Serializable]
    public class EspecieViewModels
    {
        public List<AnimalEspecie> Data { get; set; }

        public int IdEspecie { get; set; }

        public IEnumerable<SelectListItem> Especies
        {
            get
            {
                return Data.Select(d => new SelectListItem
                {
                    Value = d.IdEspecie.ToString(),
                    Text = d.Nombre
                });
            }
        }
    }
}