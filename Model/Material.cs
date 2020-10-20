using System;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    [Serializable]
    public class Material : General
    {
        public int idMaterial { get; set; }

        [Display(Name = "Volumen")]
        [Required(ErrorMessage = "Debe ingresar el volumen")]
        public decimal volumen { get; set; }

        public int IdTipoMuestra { get; set; }

        public TipoMuestra TipoMuestra { get; set; }

        public int IdPresentacion { get; set; }

        public Presentacion Presentacion { get; set; }

        public int IdReactivo { get; set; }

        public Reactivo Reactivo { get; set; }

        [Display(Name = "Descripción")]
        public string descripcion => Presentacion.glosa + " " + Reactivo.glosa + " " + volumen + "ml";

        public int estado { get; set; }
    }
}
