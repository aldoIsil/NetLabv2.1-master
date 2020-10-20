
using System;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    [Serializable]
    public class Dato : General
    {
        public int IdDato { get; set; }
        public string Prefijo { get; set; }
        public string Sufijo { get; set; }
        public int IdTipo { get; set; }
        public TipoDato Tipo { get; set; }
        public int IdDatoDependiente { get; set; }
        public bool Visible { get; set; }
        public bool Obligatorio { get; set; }
        public int? IdGenero { get; set; }
        public int? IdListaDato { get; set; }
        public ListaDato ListaDato { get; set; }
        [Display(Name = "Motivo")]
        public int idProyecto { get; set; }
        public string Proyecto { get; set; }
        public string IdsExamen { get; set; }
    }
}
