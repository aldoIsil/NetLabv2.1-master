using Model;
using NetLab.Models;
using System;

namespace NetLab.Controllers.FormConverter.Entities
{
    [Serializable]
    public class UsuarioConverterRequest
    {
        public Usuario usuario { get; set; }
        public int IdUsuarioLogueado { get; set; }
        public UsuarioViewModels UsuarioModel { get; set; }
    }
}