using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Serializable]
    public class OrdenSolicitante
    {
        public int idSolicitante { get; set; }
        public string apellidoMaterno { get; set; }
        public string apellidoPaterno { get; set; }
        public string Nombres { get; set; }
        public int idProfesion { get; set; }
        public int codigoColegio { get; set; }
        public string Dni { get; set; }
        public string telefonoContacto { get; set; }
        public string correo { get; set; }
        public string CodigoUnico { get; set; }
        public int nroDocumento { get; set; }
        public int idEstablecimiento { get; set; }
        public string Establecimiento { get; set; }
        public int idUbigeoReniec { get; set; }
        public DateTime fechaNacimiento { get; set; }
        public string direccionReniec { get; set; }
        public int estado { get; set; }
        public DateTime fechaRegistro { get; set; }
        public int idUsuarioRegistro { get; set; }
        public DateTime fechaEdicion { get; set; }
        public int idUsuarioEdicion { get; set; }

        public Ubigeo UbigeoActual { get; set; }
        public Ubigeo UbigeoReniec { get; set; }

        public int estatus { get; set; }
    }
}
