using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Model
{
    [Serializable]
    public class AnalitoOpcionResultado : General
    {
        public Guid IdAnalito { get; set; }
        public int IdAnalitoOpcionResultado { get; set; }
        public string Glosa { get; set; }
        public int Orden { get; set; }
        //sotero bustamante
        public string IdAnalitoOpcionParent { get; set; }

        [Display(Name = "Tipo")]
        public int Tipo { get; set; }

        public ListaDetalle TipoRespuesta { get; set; }

        public Guid? IdOrdenResultadoAnalito { get; set; }
        public string GlosaParent { get; set; }
        public Guid? IdOrdenExamen { get; set; }
        public int idAnalitoCabeceraVariable { get; set; }
        public List<SelectListItem> listPlataformas { get; set; }
        public string Plataforma { get; set; }
        public string[] idPlataforma { get; set; }
    }
}
