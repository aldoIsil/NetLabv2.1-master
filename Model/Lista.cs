using System;
using System.Collections.Generic;

namespace Model
{
    [Serializable]
    public class Lista : General
    {
        public int IdLista { get; set; }
        public string Glosa { get; set; }
        public List<ListaDetalle> ListaDetalle { get; set; }
    }
}