using System;
using System.Collections.Generic;
using Model;

namespace NetLab.Models
{
    [Serializable]
    public class ReactivoPresentacionViewModels
    {
        public Reactivo Reactivo { get; set; }

        public List<Model.Presentacion> Presentaciones { get; set; }

    }

}