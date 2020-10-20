using Model;
using NetLab.Models.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace NetLab.Models
{
    [Serializable()]
    public class UsuarioViewModels
    {
        public Usuario Usuario { get; set; }
        //public ClaseGeneroViewModels Clase { get; set; } 
        //[NotMapped]
        //public HttpPostedFileBase Firma { get; set; }

        public ListaDetalleViewModels TiposUsuario { get; set; }

        public ListaDetalleViewModels Profesion { get; set; }
        public ListaDetalleViewModels Componente { get; set; }
        public ListaDetalleViewModels Acceso { get; set; }
        public ListaDetalleViewModels NivelAprobacion { get; set; }

        public List<UsuarioEstablecimiento> listaEstablecimientos { get; set; }

        public List<UsuarioLaboratorio> listaLaboratorios { get; set; }

        public InstitucionesViewModels Instituciones { get; set; }

        public int cantEstablecimientos { get; set; }
        public int cantLaboratorios { get; set; }
        public int tipoUsuarioLogeado { get; set; }
    }
}