using System;
using System.Collections.Generic;
using Model;

namespace NetLab.Models
{
    [Serializable]
    public class RolMenuViewModels
    {
        public Model.Rol Rol { get; set; }
        public List<Model.Menu> Menues { get; set; }
    }
}