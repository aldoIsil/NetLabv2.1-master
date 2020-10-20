using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Model;

namespace NetLab.Models.DatoClinico
{
    [Serializable]
    public class ListaDatoViewModels
    {
        public List<ListaDato> Data { get; set; }

        public int IdListaDato { get; set; }

        public IEnumerable<SelectListItem> Listas
        {
            get
            {
                return Data.Select(d => new SelectListItem
                {
                    Value = d.IdListaDato.ToString(),
                    Text = d.Nombre
                });
            }
        }
    }
}