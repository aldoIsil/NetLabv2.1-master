using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Model;

namespace NetLab.Models.Shared
{
    [Serializable]
    public class ProyectoViewModels
    {
        public List<Proyecto> Data { get; set; }

        public int IdProyecto { get; set; }

        public IEnumerable<SelectListItem> Proyectos
        {
            get
            {
                return Data.Select(d => new SelectListItem
                {
                    Value = d.IdProyecto.ToString(),
                    Text = d.Nombre
                });
            }
        }
    }
}