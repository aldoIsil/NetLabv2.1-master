using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Model;

namespace NetLab.Models.Shared
{
    [Serializable]
    public class ListaDetalleViewModels
    {
        public List<ListaDetalle> Data { get; set; }

        public IEnumerable<SelectListItem> SelectList
        {
            get
            {
                return Data.Select(d => new SelectListItem
                {
                    Value = d.IdDetalleLista.ToString(),
                    Text = d.Glosa
                });
            }
        }
    }
}