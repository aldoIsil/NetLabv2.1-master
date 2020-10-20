using System;
using System.Collections.Generic;
using Model;
namespace NetLab.Models.PEED
{
    [Serializable]
    public class EvaluacionPanelesViewModels
    {
        public ConfiguracionEvalControlCalidad Evaluacion { get; set; }
        public List<ConfiguracionPanelControlCalidad> Paneles { get; set; }
        public ConfiguracionPanelControlCalidad Panel { get; set; }
        public List<ConfiguracionMaterialControlCalidad> Materiales { get; set; }
        public ConfiguracionMaterialControlCalidad Material { get; set; }
    }
}