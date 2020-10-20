using Model;
using NetLab.Models.Shared;
using System;

namespace NetLab.Models.DatoClinico
{
    [Serializable]
    public class DatoViewModels
    {
        public string IdEnfermedad { get; set; }
        public string NombreEnfermedad { get; set; }
        public int IdCategoria { get; set; }
        public Dato Dato { get; set; } 
        public ListaDatoViewModels Lista { get; set; }
        public TipoDatoViewModels Tipo { get; set; }
        public ClaseGeneroViewModels Clase { get; set; }
        public string[] IdsExamen { get; set; }
        public bool TipoSeleccionExamen { get; set; }
    }
}