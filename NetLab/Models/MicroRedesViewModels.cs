using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Model;

namespace NetLab.Models
{
    [Serializable]
    public class MicroRedesViewModels
    {
        
        public List<MicroRed> Data { get; set; }

    }
}