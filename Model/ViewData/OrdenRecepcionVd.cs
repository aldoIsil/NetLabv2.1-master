using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewData
{
    [Serializable]
    public class OrdenRecepcionVd
    {
        public Guid idOrden { get; set; }

        public string codigo { get; set; }

        public string nroOficio { get; set; }

        public string tipoOrden { get; set; }

        public string nroDocumento { get; set; }

        public string nroDocPaciente { get; set; }

        public string nroDocCepaBanco { get; set; }

        public string nroDocAnimal { get; set; }

        public int estadoOrden { get; set; }

        public DateTime fechaRegistro { get; set; }

        public string fechaRegistroStr { get; set; }

        public string nombreEstablecimiento { get; set; }

        public int EXISTE_PENDIENTE { get; set; }

        public int EXISTE_REFERENCIADO { get; set; }

        public int EXISTE_RECIBIDO { get; set; }
        public int EXISTE_ORDENRECHAZO { get; set; }

        public int EXISTE_RECHAZOLAB { get; set; }

        public int genero { get; set; }

        public DateTime fechaSolicitud { get; set; }

        public string fechaSolicitudStr { get; set; }

        public string nombreGenero { get; set; }

        public string tipoDocumento { get; set; }

        public DateTime ? fechaNacimiento { get; set; }


        public string paciente { get; set; }
        public string nombreEstablecimientoDestino { get; set; }
        public string fechaColeccion { get; set; }
        public string examen { get; set; }
        public string criterioRechazo { get; set; }
        public string codigoMuestra { get; set; }
        public string EstadoRechazo { get; set; }
        public string usuario { get; set; }

        public string idOrdenExamen { get; set; }
        public int idEstablecimiento { get; set; }
        public Paciente oPaciente { get; set; }
        public string codigoOrden { get; set; }
        public string muestra { get; set; }
        public string tipoMuestra { get; set; }
        public string enfermedad { get; set; }
        public string resultado { get; set; }
        public string fechaHoraColeccion { get; set; }
        public string fechaHoraRecepcion { get; set; }
        public string fechaHoraRececpionLab { get; set; }
        public string fechaHoraValidado { get; set; }
        public string observacionRechazoLab { get; set; }
        public string fechaRegistroOrden { get; set; }
        public string ingresado { get; set; }
        public string validado { get; set; }
        public int estatusE { get; set; }
        public string estadoOrdenResultado { get; set; }
        public string idOrdenMuestraRecepcion { get; set; }
        public string idOrdenMaterial { get; set; }
    }
}
