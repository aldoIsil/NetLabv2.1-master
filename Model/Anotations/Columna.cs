using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Model.Anotations
{ 
    [System.AttributeUsage(System.AttributeTargets.Property |
                           System.AttributeTargets.Struct)
    ]
    [Serializable]
    public class Columna : System.Attribute
    {
        private string Nombre;

        public Columna(string Nombre)
        {
            this.Nombre = Nombre;
        }
    }
}