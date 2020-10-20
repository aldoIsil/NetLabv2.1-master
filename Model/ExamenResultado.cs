using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    [Serializable]
    public class ExamenResultado
    {
        public Guid IdOrdenExamen { get; set; }
        public string Examen { get; set; } 
        public string Enfermedad { get; set; }
        public string Metodo { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FechaLiberacion { get; set; }

        public List<ExamenResultadoDetalle> Detalle { get; set; }
    }
}