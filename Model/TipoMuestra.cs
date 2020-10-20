using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Model.ViewData;

namespace Model
{
    [Serializable]
    public class TipoMuestra : General
    {
        public int idTipoMuestra { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Se requiere el nombre")]
        public string nombre { get; set; }

        [Display(Name = "Descripción")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Se requiere el nombre desc")]
        public string descripcion { get; set; }

        public Examen examen { get; set; }

        public Enfermedad enfermedad { get; set; }

        public string fechaColeccion { get; set; }

        public string horaColeccion { get; set; }

        public int posicion { get; set; }

        public List<Material> materialList { get; set; }

        public List<MaterialVd> materialVdList { get; set; }

        [Display(Name = "Nemotécnico")]
        public string nemo { get; set; }
    }

}
