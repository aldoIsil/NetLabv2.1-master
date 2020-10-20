using Model;
using Model.ViewData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NetLab.Models.PEED
{
    [Serializable]
    public class ResultadoPEEDViewModels
    {
        public ResultadoControlCalidadVd ResultadoControlCalidad { get; set; }
        public List<ResultadoControlCalidadVd> ResultadosControlCalidad { get; set; }
        public ConfiguracionPanelControlCalidad Panel { get; set; }
        public List<ConfiguracionMaterialControlCalidad> Materiales { get; set; }
    }
}