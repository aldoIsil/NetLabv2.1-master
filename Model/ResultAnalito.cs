using System;
using System.Collections.Generic;

namespace Model
{
    [Serializable]
    public class ResultAnalito
    {
        public Guid IdOrden { get; set; }

        public Guid IdExamen { get; set; }

        public string Examen { get; set; }
        public string Enfermedad { get; set; }

        public Guid IdAnalito { get; set; }

        public Guid IdOrdenResultadoAnalito { get; set; }

        public string Analito { get; set; }

        public string Resultado { get; set; }

        public string Unidad { get; set; }

        public string ValorReferencia { get; set; }

        public decimal ValorInferior { get; set; }

        public decimal ValorSuperior { get; set; }

        public string Observacion { get; set; }

        public List<AnalitoOpcionResultado> Metodos { get; set; }

        //si se combo o caja
        public int tipo { get; set; }


        //si se habilita o no el combo
        public int estatusE { get; set; }
        
        public string codigoOpcion { get; set; }

        public Guid IdOrdenExamen { get; set; }

        public List<AnalitoOpcionResultado> ListJsResultados { get; set; }
        public int idSecuencia { get; set; } //Nro de vez que ingresa un nuevo examen y un nuevo analito, será parte del key para identificar los mismos examenes ingresados mas de una vez

        public Paciente paciente { get; set; }
        public OrdenSolicitante solicitante { get; set; }

    }
    /// <summary>
    /// Clase para el trabajo del JSON del resultado.
    /// </summary>
    [Serializable]
    public class AnalitoValues
    {
        public Guid AnalitoId { get; set; }
        public string Id { get; set; }
        public string Value { get; set; }
        public Guid IdOrdenExamen { get; set; }
        public Guid IdOrdenResultadoAnalito { get; set; }
        public string Tipo { get; set; }
    }
    [Serializable]
    public class AnalitoResultValues
    {
        public Guid AnalitoId { get; set; }
        public string Id { get; set; }
        public string Value { get; set; }
        public Guid IdOrdenExamen { get; set; }
        public Guid IdOrdenResultadoAnalito { get; set; }
        public string Tipo { get; set; }
        public List<AnalitoOpcionResultado> ListAnalitoOpcionResultado { get; set; }
    }
}
