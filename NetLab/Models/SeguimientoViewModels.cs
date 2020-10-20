using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Model;

namespace NetLab.Models
{
    [Serializable]
    public class SeguimientoViewModels
    {
        public CCSeguimientoCab Seguimientocab { get; set; }
        public CCSEguimientoDetalle SeguimientoDetalle { get; set; }
    }

}
