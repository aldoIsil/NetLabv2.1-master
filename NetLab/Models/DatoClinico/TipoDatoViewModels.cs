using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Model;

namespace NetLab.Models.DatoClinico
{
    [Serializable]
    public class TipoDatoViewModels
    {
        public List<TipoDato> Data { get; set; }

        public int IdTipoDato { get; set; }

        public IEnumerable<SelectListItem> Tipos
        {
            get
            {
                return Data.Select(d => new SelectListItem
                {
                    Value = d.IdTipo.ToString(),
                    Text = d.Nombre
                });
            }
        }
    }
}