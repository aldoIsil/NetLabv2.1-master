using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    [Serializable]
    public class Examen : General
    {
        public Guid idExamen { get; set; }

        [Display(Name = "LOINC")]
        [StringLength(8, ErrorMessage = "No debe ingresar mas de 8 caracteres")]
        public string Loinc { get; set; }

        [Display(Name = "CPT")]
        [Range(0, int.MaxValue, ErrorMessage = "Ingrese un valor entero válido")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Debe ingresar solo valores numéricos")]
        public int? Cpt { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Se requiere el nombre")]
        [StringLength(500, ErrorMessage = "No debe ingresar mas de 500 caracteres")]
        public string nombre { get; set; }

        [Display(Name = "Descripción")]
        [DataType(DataType.MultilineText)]
        public string descripcion { get; set; }
        //Autor: Juan Muga
        //Descripción: identificar si el examen es una prueba rapida.
        //Fecha Creación: 04/11/2017
        public int PruebaRapida { get; set; }
        public int Tipo { get; set; }//Identifica si es para consulta o procesamiento o ambas


        public int? IdGenero { get; set; }

        public List<Analito> analitoList { get; set; }

        public List<TipoMuestra> tipoMuestraList { get; set; }
        public string nombreEnfermedad { get; set; }
        public int idExamenAgrupado { get; set; }
    }

    public class ExamenPlataforma:General
    {
        public Guid idExamen { get; set; }
        public int idPlataforma { get; set; }
        public string Examen { get; set; }
        public string Plataforma { get; set; }
    }
}
