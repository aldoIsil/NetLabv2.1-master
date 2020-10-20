using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NetLab.Models
{
    [Serializable]
    public class UsuarioEnfermedadExamenViewModels
    {
        public Model.Usuario Usuario { get; set; }
        public List<Model.Examen> Examenes { get; set; }
    }
}