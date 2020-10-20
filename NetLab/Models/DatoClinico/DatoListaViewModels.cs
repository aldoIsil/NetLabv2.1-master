using System;
using System.Collections.Generic;
using Model;

namespace NetLab.Models.DatoClinico
{
    [Serializable]
    public class DatoListaViewModels
    {
        public int IdCategoria { get; set; }
        public string NombreCategoria { get; set; }
        public string IdEnfermedad { get; set; }
        public string NombreEnfermedad { get; set; }
        public List<Dato> Datos { get; set; }
    }
}