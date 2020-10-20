using System;
using System.Collections.Generic;

namespace Model.ViewData
{
    [Serializable]
    public class AnalitoVd
    {

        public Analito analito { get; set; }

        public String posicionAnalito { get; set; }       

        public List<Analito> analitoList { get; set; }

    }
}
