using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model
{
    [Serializable]
    public class Paciente : General
    {
        [Display(Name = "ID Paterno")]
        public Guid IdPaciente { get; set; }

        [Required(ErrorMessage = "Se requiere el Apellido Paterno")]
        [Display(Name = "Apellido Paterno:")]
        [StringLength(500)]
        public string ApellidoPaterno { get; set; }

        [Display(Name = "Apellido Materno:")]
        [StringLength(500)]
        public string ApellidoMaterno { get; set; }

        [Display(Name = "Nombres:")]
        [Required(ErrorMessage = "Se requiere los Nombres")]
        [StringLength(500)]
        public string Nombres { get; set; }

        [Display(Name = "Fecha de Nacimiento:")]
        //[Required(ErrorMessage = "Se requiere la Fecha de Nacimiento")]
        [DataType(DataType.Date, ErrorMessage = "Debe ingresar un valor de tipo fecha")]
        public DateTime? FechaNacimiento { get; set; }

        [Display(Name = "Tipo de Documento:")]
        public int? TipoDocumento { get; set; }

        public string tipoDocumen { get; set; }

        [Display(Name = "Número de Documento:")]
        [StringLength(20)]
        [Required(ErrorMessage = "Se requiere el Número de Documento")]
        public string NroDocumento { get; set; }

        [Display(Name = "Sexo:")]
        public int? Genero { get; set; }

        public Ubigeo UbigeoActual { get; set; }

        public Ubigeo UbigeoReniec { get; set; }

        [Display(Name = "Dirección:")]
        [StringLength(2000)]
        public string DireccionReniec { get; set; }

        [Display(Name = "Dirección:")]
        [StringLength(2000)]
        [Required(ErrorMessage = "Se requiere la Dirección Actual")]
        public string DireccionActual { get; set; }

        [Display(Name = "Teléfono Fijo:")]
        [StringLength(20)]
        [RegularExpression("([0-9]+)", ErrorMessage = "Debe ingresar un valor numérico")]
        public string TelefonoFijo { get; set; }

        [Display(Name = "Celular 1:")]
        [StringLength(9)]
        [Range(0, 999999999, ErrorMessage = "Debe ingresar un valor numérico")]
        public string Celular1 { get; set; }

        [Display(Name = "Celular 2:")]
        [StringLength(9)]
        [Range(0, 999999999, ErrorMessage = "Debe ingresar un valor numérico")]
        public string Celular2 { get; set; }

        public int estatus { get; set; }


        [Display(Name = "Correo Electrónico:")]
        [EmailAddress(ErrorMessage = "La dirección de correo electrónico es inválida")]
        public String correoElectronico { get; set; }

        [Display(Name = "Ocupación:")]
        public String ocupacion { get; set; }

        [Display(Name = "Tipo Seguro de Salud:")]
        [Required(ErrorMessage = "Se requiere Tipo de Seguro de Salud")]
        public int? tipoSeguroSalud { get; set; }

        public string tipoSeguro { get; set; }



        [Display(Name = "otroSeguroSalud:")]
        public String otroSeguroSalud { get; set; }

        public ListaDetalle listaDetalleTipoDocumento { get; set; }

        public ListaDetalle listaDetalleGenero { get; set; }

        public int edadAnios { get; set; }

        public int edadMeses { get; set; }

        public string edadPaciente { get; set; }

        public int codificacion { get; set; }

        public string sexo { get; set; }

        public String iniciales { get {
                if (this.ApellidoPaterno == null || this.ApellidoMaterno == null)
                {
                    return this.Nombres.Substring(0, 1);
                }
                else {
                    return this.Nombres.Substring(0, 1) + this.ApellidoPaterno.Substring(0, 1) + this.ApellidoMaterno.Substring(0, 1);
                }
                
            } }

        public String nombreCompleto
        {
            get
            {
                    return this.Nombres + " " +this.ApellidoPaterno + " " + this.ApellidoMaterno;
            }
        }

        public string tipoDocumentoNombre { get; set; }
        public string generoNombre { get; set; }
        public int EstatusE { get; set; }
        public Guid idOrden { get; set; }
        public int idEstablecimiento { get; set; }
        public int mcaDatoTutor { get; set; }

        public string comentario { get; set; }

        public string EstablecimientoOrigen { get; set; }
        public string EstablecimientoDestino { get; set; }
        public string UsuarioReferencia { get; set; }
        public DateTime FechaReferencia { get; set; }

        public virtual List<Paciente> ListPaciente { get; set; }

        #region HistorialLaboratorio
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FechaObtencion { get; set; }
        public string Enfermedad { get; set; }
        public string idExamen { get; set; }
        public string Examen { get; set; }
        public string Componente { get; set; }
        public string Resultado { get; set; }
        public string EESSOrigen{ get; set; }
        public string EstadoResultado { get; set; }
        public string FechaValidacion { get; set; }
        public string CriteriosRechazo { get; set; }
        public string CodigoOrden { get; set; }
        public string NroOficio { get; set; }
        #endregion

        public DateTime FechaRecepcionRom { get; set; }

        #region InformacionPacienteNetLab1
        public string sApellidoMaterno { get; set; }
        public string sApellidoPaterno { get; set; }
        public string sNombres { get; set; }
        public string sNumeroDocumento { get; set; }
        public string cCodMuestra { get; set; }
        public DateTime dFechaObtencionMuestra { get; set; }
        public string Renipress { get; set; }
        public string NombreEESS { get; set; }
        public string sEnfermedad { get; set; }
        public string Prueba { get; set; }
        public string NombreVariable { get; set; }
        public string sResultado { get; set; }
        public int cCodPrueba { get; set; }
        #endregion

        [NotMapped]
        public int TienePruebaRapidaSiscovid { get; set; }
        public Boolean EstadoReniec { get; set; }
    }

    [Serializable]
    public class Ciudad : General
    {
        public int idPais { get; set; }
        public string NombrePais { get; set; }
        public int idCiudad { get; set; }
        public string NombreCiudad { get; set; }
    }
}
