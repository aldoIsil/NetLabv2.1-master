using System;

namespace Model
{
    [Serializable]
    public class OrdenResultado
    {
        public string NroSolicitud { get; set; } 
        public string NroOficio { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public string Paciente { get; set; }
        public string nombrePaciente { get; set; }

        public DateTime fechaNacimiento { get; set; }

        public string EstablecimientoSolicitante { get; set; }

        public string DocIdentidad { get; set; }
        public string DocIdentidadSol { get; set; }

        public string codigoOrden { get; set; }


        public string DireccionActual { get; set; }
        public string MedicoTratante { get; set; }
        public string FechaNacimiento { get; set; }
        public string FechadeSolicitud { get; set; }
        public string Genero { get; set; }
        public string Establecimiento { get; set; }
        public UsuarioResultado Validador { get; set; }
        public UsuarioResultado Liberador { get; set; }
        public string Observaciones { get; set; }

        public string Edad { get; set; }
        public string Sexo { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }

        public string Solicitante { get; set; }

        public string Ubicacion { get; set; }

        public string EstablecimientoDestino { get; set; }

        public int NumeroInforme { get; set; }
        public string FechaIngresoInsRom { get; set; }

        public string TipoDocumento { get; set; }
        public int idLaboratorioDestino { get; set; }
        public Guid idOrden { get; set; }
        public int Inacal { get; set; }
    }
}