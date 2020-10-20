using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Model
{
    [Serializable]
    public class ListaDetalle : General
    {
        public int IdDetalleLista { get; set; }
        public string Glosa { get; set; }
        public int OrdenLista { get; set; }

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