using System;

namespace Model
{
    [Serializable]
    public class DatoClinico
    {
        public int? IdCategoriaDatoPadre { get; set; }
        public int IdCategoriaDato { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool CategoriaVisible { get; set; }
        public int Orientacion { get; set; }
        public int Orden { get; set; }
        public int? IdDato { get; set; }
        public string Tipo { get; set; }
        public bool DatoVisible { get; set; }
        public bool Obligatorio { get; set; }
        public int? IdListaDato { get; set; }
        public ListaDato Lista { get; set; }
    }
    [Serializable]
    public class DatosAdicionales
    {
        public string codigo { get; set; }
        public string descripcion { get; set; }
        public string valor { get; set; }
        public string idDato { get; set; }
        public string noPrecisa { get; set; }
    }
}