using System;

namespace Model
{
    [Serializable]
    public class Institucion : General
    {
        public int codigoInstitucion { get; set; }
        public string IdInstitucion { get; set; }
        public string nombreInstitucion { get; set; }
        public int estado { get; set; }
        public string descripcion { get; set; }
        public int idUsuarioRegistro { get; set; }
        public int idUsuarioEdicion { get; set; }
        public string fechaEdicion { get; set; }
        
    }
}
