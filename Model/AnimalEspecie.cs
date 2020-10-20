using System;

namespace Model
{
    [Serializable]
    public class AnimalEspecie : General
    {
        public int IdEspecie { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
    }
}
