using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Model;

namespace NetLab.Models.Shared
{
    [Serializable]
    public class EstablecimientoViewModels
    {
        public List<Establecimiento> Data { get; set; }

        public int IdEstablecimiento { get; set; }

        public IEnumerable<SelectListItem> Establecimientos
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
                    Value = d.IdEstablecimiento.ToString(),
                    Text = d.Nombre
                });
            }
        }
    }
}