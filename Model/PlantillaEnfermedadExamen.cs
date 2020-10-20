using System;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    [Serializable]
    public class PlantillaEnfermedadExamen : General
    {
        public int IdPlantilla { get; set; }

        [Required(ErrorMessage = "Debe seleccionar la enfermedad")]
        public int? IdEnfermedad { get; set; }

        public string Enfermedad { get; set; }

        [Required(ErrorMessage = "Debe seleccionar el examen")]
        public Guid? IdExamen { get; set; }

        public string Examen { get; set; }

        [Required(ErrorMessage = "Debe seleccionar el tipo de muestra")]
        public int? IdTipoMuestra { get; set; }

        public string Muestra { get; set; }

        [Required(ErrorMessage = "Debe seleccionar el material")]
        public int? IdMaterial { get; set; }

        public string Material { get; set; }

        public string Presentacion { get; set; }

        public string Reactivo { get; set; }

        public decimal? Volumen { get; set; }

        [Required(ErrorMessage = "Debe ingresar la cantidad")]
        [Range(1, 9999, ErrorMessage = "Ingrese una cantidad valida")]
        public int? Cantidad { get; set; }
    }
}