using Model;
using NetLab.Models.Shared;
using System;
using System.Collections.Generic;

namespace NetLab.Models
{
    [Serializable]
    public class AnalitoValorViewModels
    {
        public Analito Analito { get; set; }
        public AnalitoValorNormal Valor { get; set; }
        public List<AnalitoOpcionResultado> Opciones { get; set; }
        public ListaDetalleViewModels Generos { get; set; }
    }
}