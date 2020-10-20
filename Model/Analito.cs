using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    [Serializable]
    public class Analito : General
    {
        public string Examen { get; set; }

        public Guid IdAnalito { get; set; }

        [Display(Name = "Unidad")]
        public int IdListaUnidad { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Se requiere el nombre")]
        public string Nombre { get; set; }

        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "Se requiere la descripcion")]
        public string Descripcion { get; set; }

        [Display(Name = "Tipo")]
        public int Tipo { get; set; }

        public ListaDetalle TipoRespuesta { get; set; }
        public ListaDetalle Unidad { get; set; }


        //sotero bustamante para configuracion de resultados
        public int idOpcionParent { get; set; }
    }
}
