using System;
using System.Collections.Generic;
using Model;

namespace NetLab.Models
{
    [Serializable]
    public class UsuarioAreaProcesamientoViewModels
    {
        public Model.Usuario Usuario { get; set; }
        public List<Model.AreaProcesamiento> Areas { get; set; }
    }
}