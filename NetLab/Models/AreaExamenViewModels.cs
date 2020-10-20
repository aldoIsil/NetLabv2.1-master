using Model;
using System;
using System.Collections.Generic;

namespace NetLab.Models
{
    [Serializable]
    public class AreaExamenViewModels
    {
        public AreaProcesamiento AreaProcesamiento { get; set; }
        public List<Examen> Examenes { get; set; }
    }
}