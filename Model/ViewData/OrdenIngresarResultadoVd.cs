using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewData
{
    [Serializable]
    public class OrdenIngresarResultadoVd
    {
        public Guid idOrden { get; set; }
        public Guid idPaciente { get; set; }
        public Guid idAnimal { get; set; }
        public Guid idCBS { get; set; }
        public string nombreEstablecimiento { get; set; }
        public string nroOficio { get; set; }
        public string codigoOrden { get; set; }
        public string nroDocumento { get; set; }
        public string tipoOrden { get; set; }
        public int idEstablecimiento { get; set; }
        public int estadoOrden { get; set; }

        public DateTime fechaNacimiento { get; set; }
        public DateTime fechaRegistro { get; set; }
        public DateTime fechaSolicitud { get; set; }
        public string nombrePaciente { get; set; }
        public string edadPaciente { get; set; }
        public string edad { get; set; }
        public string nombreProyecto { get; set; }
        public string codigoPaciente { get; set; }
        public int EdadAnios { get; set; }
        public int Genero { get; set; }
        public string tipoDocumento { get; set; }
        public string nombreGenero { get; set; }

        public Guid idExamen { get; set; }
        public string nombreEnfermedad { get; set; }
        public string nombreExamen { get; set; }
        public int tipoExamen { get; set; }
        public string FechaSiembraCultivo { get; set; }

        public int idLaboratorioProcesamiento { get; set; }

        public string codigoMuestra { get; set; }
        public DateTime fechaRecepcion { get; set; }
        public DateTime fechaColeccion { get; set; }

        public string metodoExamen { get; set; }
        public string status { get; set; }
        public List<MuestraResultadoVd> muestrasValidar { get; set; }

        public List<MuestraResultadoVd> muestrasPendientesRecepcion { get; set; }

        public List<AreaProcesamiento> areas { get; set; }

        public int step { get; set; }

        public int ingresado { get; set; }
        public string validado { get; set; }
        public int liberado { get; set; }

        public int statusP { get; set; }

        public int conformeProceso { get; set; }

        public int estatusE { get; set; }
        public List<OrdenVd> ordenInfoListnew { get; set; }

        public List<Resultados> resultadosList { get; set; }
        public string MuestraConforme { get; set; }

        public int flagResultado { get; set; }
        public int? conformeP { get; set; }

        public Ubigeo UbigeoActual { get; set; }

        public Ubigeo UbigeoReniec { get; set; }


        public string ConFechaSolicitud { get; set; }
        public string ConcodigoOrden { get; set; }
        public string ConnroOficio { get; set; }
        public string ConDepartamentoOrigen { get; set; }
        public string ConProvinciaOrigen { get; set; }
        public string ConDistritoOrigen { get; set; }
        public string ConDisaOrigen { get; set; }
        public string ConRedOrigen { get; set; }
        public string ConMicroRedOrigen { get; set; }
        public string ConInstitucionOrigen { get; set; }
        public string ConEstablecimientoSolicitante { get; set; }
        public string ConEstablecimientoLatitud { get; set; }
        public string ConEstablecimientoLongitud { get; set; }
        public string ConDepartamentoDestino { get; set; }
        public string ConProvinciaDestino { get; set; }
        public string ConDistritoDestino { get; set; }
        public string ConDisaDestino { get; set; }
        public string ConRedDestino { get; set; }
        public string ConMicroRedDestino { get; set; }
        public string ConEESS_LAB_Destino { get; set; }
        public string ConDocIdentidad { get; set; }
        public string ConfechaNacimiento { get; set; }
        public string ConnombrePaciente { get; set; }
        public string ConID_Muestra { get; set; }
        public string ConTipoMuestra { get; set; }
        public string ConEnfermedad { get; set; }
        public string ConnombreExamen { get; set; }
        public string ConComponente { get; set; }
        public string ConnResultado { get; set; }
        public string ConMuestraConforme { get; set; }
        public string criteriosRechazo { get; set; }
        public string ObservacionRechazo { get; set; }
        public string ConFechaHoraColeccion { get; set; }
        public string ConHoraColeccion { get; set; }
        public string ConFechaHoraRecepcionInsROM { get; set; }
        public string ConFechaHoraRecepcionROM { get; set; }
        public string ConHoraRecepcionROM { get; set; }
        public string ConFechaHoraRecepcionLAB { get; set; }
        public string ConHoraRecepcionLAB { get; set; }
        public string ConFechaHoravalidado { get; set; }
        public string ConHoravalidado { get; set; }
        public string fechaRegistroOrden { get; set; }
        public string HoraRegistroOrden { get; set; }
        public string fechaRegistroRecepcionMuestra { get; set; }
        public string HoraRegistroRecepcionMuestra { get; set; }
        public string ConEstatusResultado { get; set; }
        public string ConEstatusE { get; set; }
        public string ConFechAddExamen { get; set; }
        public string ConMotivo { get; set; }

        /*agregado YRCA*/

        public string ConEdad { get; set; }
        public string ConSexo { get; set; }
        public string ConDireccionPaciente { get; set; }


        public string ConColor { get; set; }
        public string error { get; set; }
        public string dias { get; set; }
        public string catalogo { get; set; }

        public Guid idOrdenExamen { get; set; }
        public string Telefono { get; set; }
        public string FormatoSolicitud { get; set; }
    }

    [Serializable]
    public class EnfermedadResultados
    {
        public string ConFechaSolicitud { get; set; }
        public string codigoOrden { get; set; }
        public string codigoMuestra { get; set; }
        public string nroOficio { get; set; }
        public string ConDepartamentoOrigen { get; set; }
        public string ConProvinciaOrigen { get; set; }
        public string ConDistritoOrigen { get; set; }
        public string ConDisaOrigen { get; set; }
        public string ConRedOrigen { get; set; }
        public string ConMicroRedOrigen { get; set; }
        public string ConEstablecimientoSolicitante { get; set; }
        public string ConDepartamentoDestino { get; set; }
        public string ConProvinciaDestino { get; set; }
        public string ConDistritoDestino { get; set; }
        public string ConDisaDestino { get; set; }
        public string ConRedDestino { get; set; }
        public string ConMicroRedDestino { get; set; }
        public string nombreEstablecimiento { get; set; }
        public string nroDocumento { get; set; }
        public string ConfechaNacimiento { get; set; }
        public string nombrePaciente { get; set; }
        public int EdadAnios { get; set; }
        public string ConSexo { get; set; }
        public string ConTipoMuestra { get; set; }
        public string nombreEnfermedad { get; set; }
        public string nombreExamen { get; set; }
        public string ConComponente { get; set; }
        public DateTime fechaRegistro { get; set; }
        public string ConFechaHoraColeccion { get; set; }
        public string ConFechaHoraRecepcionROM { get; set; }
        public string ConFechaHoraRecepcionLAB { get; set; }
        public string ConFechaHoravalidado { get; set; }
        public string ConMuestraConforme { get; set; }
        public string criteriosRechazo { get; set; }
        public string ConnResultado { get; set; }
        public string ConEstatusE { get; set; }
    }
}
