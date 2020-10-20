using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace Model
{
    [Serializable]
    public class Establecimiento : General
    {
        [Display(Name = "Establecimiento:")]
        public int IdEstablecimiento { get; set; }
        public string CodigoInstitucion { get; set; }
        public string CodigoUnico { get; set; }

        public string clasificacion { get; set; }
        public string Nombre { get; set; }
        public string Ubigeo { get; set; }
        public string Direccion { get; set; }
        public string CodigoDisa { get; set; }
        public string CodigoRed { get; set; }
        public string CodigoMicroRed { get; set; }
        public int CodigoCategoria { get; set; }
        public string Latitud { get; set; }
        public string Longitud { get; set; }

        public string correoelectronico { get; set; }
        public Ubigeo eUbigeo { get; set; }

        public string website { get; set; }
        //[NotMapped]
        //public HttpPostedFileBase Logo { get; set; }
        //[NotMapped]
        //public HttpPostedFileBase Sello { get; set; }

        public int IdLabIns { get; set; }
        public int idCategoria { get; set; }
    }
}
