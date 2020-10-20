using System;

namespace Model
{
    [Serializable]
    public class PlantillaEstablecimiento : General
    {
        public int IdPlantilla { get; set; }
        public int IdEstablecimiento { get; set; }
    }
}