using System;

namespace Model
{
    [Serializable]
    public class PlantillaCategoriaDato : General
    {
        public int IdPlantilla { get; set; }
        public int IdCategoriaDato { get; set; }
        public int Orden { get; set; }
    }
}
