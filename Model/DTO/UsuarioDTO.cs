using System;

namespace Model.DTO
{
    [Serializable]
    public class UsuarioDTO
    {
        public Usuario usuario { get; set; }
        public int idUsuario { get; set; }
        public string login { get; set; }
        public string apellidoPaterno { get; set; }
        public string apellidoMaterno { get; set; }
        public string nombres { get; set; }
        public string iniciales { get; set; }
        public string codigoColegio { get; set; }
        public string RNE { get; set; }
        public string correo { get; set; }
        public byte[] contrasenia { get; set; }
        public DateTime fechaIngreso { get; set; }
        public DateTime fechaUltimoAcceso { get; set; }
        public DateTime fechaCaducidad { get; set; }
        public byte[] firmaDigital { get; set; }

    }
}
