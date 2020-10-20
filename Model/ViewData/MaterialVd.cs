using System;

namespace Model.ViewData
{
    [Serializable]
    public class MaterialVd
    {

        public Material material { get; set; }   
        public String textoCmbMaterial { get; set; }
        public int idCmbMaterial { get; set; }

        public int cantidad { get; set; }

    }
}
