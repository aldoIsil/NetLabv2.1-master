using System;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    [Serializable]
    public class AnalitoValorNormal:General
    {
        public int idAnalitoValorNormal {get;set;}//] [int] NOT NULL,
	    public Guid idAnalito {get;set;}//] [int] NOT NULL,
	    public int idLista {get;set;}//] [int] NULL,


        [Display(Name = "Grupo Género")]
        public int grupoGenero {get;set;}//] [int] NULL,

        [Display(Name = "Glosa")]
        public string glosa { get; set; }//] [varbinary](500) NULL,

        [Display(Name = "Valor Inferior")]
        public decimal valorInferior {get;set;}//] [decimal](18, 0) NULL,

        [Display(Name = "Valor Superior")]
        public decimal valorSuperior {get;set;}//] [decimal](18, 0) NULL,
         
        [Display(Name = "Valor Alarma Inferior")]
        public decimal valorAlarmaInferior {get;set;}//] [decimal](18, 0) NULL,

        [Display(Name = "Valor Alarma Superior")]
        public decimal valorAlarmaSuperior {get;set;}//] [decimal](18, 0) NULL,


        [Display(Name = "Edad Inferior (Años)")]
        public int edadInferior {get;set;}//] [int] NULL,

        [Display(Name = "Edad Superior (Años)")]
        public int edadSuperior {get;set;}//] [int] NULL,

        [Display(Name = "Orden")]
        public int orden {get;set;}//] [int] NOT NULL,

        public ListaDetalle Genero { get; set; }
        public ListaDetalle Lista { get; set; }
    }
}
