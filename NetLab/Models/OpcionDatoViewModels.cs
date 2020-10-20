using System;
using System.Collections.Generic;
using Model;

namespace NetLab.Models
{
    [Serializable]
    public class OpcionDatoViewModels
    {
        public ListaDato ListaDato { get; set; }
        public List<OpcionDato> Opciones { get; set; } 
    }
}