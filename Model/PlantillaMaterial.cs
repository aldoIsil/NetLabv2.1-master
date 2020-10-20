using System;

namespace Model
{
    [Serializable]
    public class PlantillaMaterial : General
    {
        public int IdPlantilla { get; set; }
        public int IdMaterial { get; set; }
        public int Cantidad { get; set; }
        public int Orden { get; set; }
    }
}
