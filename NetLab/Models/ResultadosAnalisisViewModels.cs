using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NetLab.Models
{
    [Serializable]
    public class ResultadosAnalisisViewModels
    {

    }
    /// Descripción: Clase para el formato del JSON
    /// Author: Juan Muga.
    /// Fecha Creacion: 21/05/2018
    [Serializable]
    public class JsonParsedValues
    {
        //public string AnalitoId { get; set; }
        public List<AnalitoValues> Combovalues { get; set; }
        public List<AnalitoValues> Checkvalues { get; set; }
    }
    /// <summary>
    /// Descripción: Clase para 
    /// Author: Juan Muga.
    /// Fecha Creacion: 21/05/2018
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
    /// <summary>
    /// Descripción: Clase para setear las observaciones por analito
    /// Author: Juan Muga.
    /// Fecha Creacion: 21/05/2018
    /// </summary>
    [Serializable]
    public class ObservacionesValues
    {
        public Guid IdOrdenResultadoAnalito { get; set; }
        public string ObservacionContent { get; set; }
    }
}