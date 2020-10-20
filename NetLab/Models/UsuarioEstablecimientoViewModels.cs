using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Model;
using PagedList;

namespace NetLab.Models
{
    [Serializable]
    public class UsuarioEstablecimientoViewModels
    {
        public int? Page { get; set; }
        public Usuario Usuario { get; set; }
        public List<UsuarioEstablecimiento> Establecimientos { get; set; }
        public IPagedList PagingMetaData { get; set; }

        public List<Institucion> Instituciones { get; set; }
        public int CodigoInstitucion { get; set; }

        public IEnumerable<SelectListItem> SelectInstituciones
        {
            get
            {
                var instituciones = Instituciones.Select(d => new SelectListItem
                {
                    Value = d.codigoInstitucion.ToString(),
                    Text = d.nombreInstitucion
                }).ToList();

                instituciones.Insert(0, new SelectListItem {Value = "0", Text = "Seleccione un Sub Sector" });

                return instituciones;
            }
        }
    }
}