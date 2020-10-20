using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Model;

namespace NetLab.Models.Shared
{
    [Serializable]
    public class InstitucionesViewModels
    {
        public List<Institucion> Data { get; set; }

        public int CodigoInstitucion { get; set; }

        public IEnumerable<SelectListItem> Instituciones
        {
            get
            {
                return Data.Select(d => new SelectListItem
                {
                    Value = d.codigoInstitucion.ToString(),
                    Text = d.nombreInstitucion
                });
            }
        }
    }
}