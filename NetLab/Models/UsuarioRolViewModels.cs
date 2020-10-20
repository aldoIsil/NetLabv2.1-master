using System;
using System.Collections.Generic;
using Model;

namespace NetLab.Models
{
    [Serializable]
    public class UsuarioRolViewModels
    {
        public Model.Usuario Usuario { get; set; }
        public List<Model.Rol> Roles { get; set; }
    }
}