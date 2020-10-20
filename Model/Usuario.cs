using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace Model
{
    [Serializable]
    public class Usuario:General
    {
        public int idUsuario {get;set;}//] [int] NOT NULL,

        [Display(Name = "Login:")]
        public string login {get;set;}//] [varchar](50) NULL,

        [Required(ErrorMessage = "Se requiere el Apellido Paterno")]
        [Display(Name = "Apellido Paterno:")]
        [DataType(DataType.Text)]
        public string apellidoPaterno {get;set;}//] [varchar](100) NULL,

        [Required(ErrorMessage = "Se requiere el Apellido Materno")]
        [Display(Name = "Apellido Materno:")]
        [DataType(DataType.Text)]
        public string apellidoMaterno {get;set;}//] [varchar](100) NULL,

        [Required(ErrorMessage = "Se requiere los Nombres")]
        [Display(Name = "Nombres:")]
        [DataType(DataType.Text)]
        public string nombres {get;set;}//] [varchar](100) NULL,

        [Display(Name = "Iniciales:")]
        [DataType(DataType.Text)]
        public string iniciales {get;set;}//] [varchar](100) NULL,

        public string codigoColegio {get;set;}//] [varchar](15) NULL,

        [Display(Name = "RNE:")]
        public string RNE {get;set;}//] [varchar](10) NULL,

        [EmailAddress(ErrorMessage = "La dirección de correo electrónico es inválida")]
        [Display(Name = "Correo:")]
        [DataType(DataType.EmailAddress)]
        public string correo {get;set;}//] [varchar](300) NULL,

        [Display(Name = "Contraseña:")]
        public byte[] contrasenia { get; set; }//] [varbinary](256) NULL,

        [Display(Name = "Fecha Ingreso:")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime fechaIngreso {get;set;}//] [datetime] NULL,

        [Display(Name = "Fecha Ultima Acceso:")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime fechaUltimoAcceso {get;set;}//] [datetime] NULL,

        [Display(Name = "Fecha Caducidad:")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime fechaCaducidad {get;set;}//] [datetime] NULL,

        [Display(Name = "Firma Digital:")]
        public byte[] firmaDigital { get; set; }//] [varbinary] NULL,

        public int estatus { get; set; }//] [int] NOT NULL,

        public int tipo { get; set; }//1: reniec | 2: base de datos

        [Required(ErrorMessage = "Se requiere Cargo")]
        [Display(Name = "Cargo:")]
        public string cargo { get; set; }//] [varchar](15) NULL,

        [Required(ErrorMessage = "Se requiere el Número de Documento")]
        [Display(Name = "DNI:")]
        [MaxLength(8)]
        public string documentoIdentidad { get; set; }

        [Required(ErrorMessage = "Se requiere el Número de Telefono")]
        [Display(Name = "Telefono:")]
        [DataType(DataType.PhoneNumber)]
        public string telefono { get; set; }
        public string tipoDoc { get; set; }

        [Display(Name = "Tiempo Caducidad:")]
        public int tiempoCaducidad { get; set; }

        public int estado { get; set; }
        [Display(Name = "Profesion:")]
        public int profesion { get; set; }

        public int Establecimiento { get; set; }

        public int Renovacion { get; set; }
        public int idTipoUsuario { get; set; }

        [Required(ErrorMessage = "Se requiere los Condicion Laboral")]
        public string condicionLaboral { get; set; }
        public int tipoDocumento { get; set; }
        public int componente { get; set; }
        public int nAprobacion { get; set; }
        public string _profesion { get; set; }
        public string TipoUsuario { get; set; }
        public string componenteUsuario { get; set; }
    }

    [Serializable]
    public class UsuarioEnvioSms:General
    {
        public string codigoOrden { get; set; }
        /*Usuario Registrador de Orden*/
        public int idUsuario { get; set; }
        public string Nombre { get; set; }
        public string Celular { get; set; }
        public string Correo { get; set; }
        public string codigoMuestra { get; set; }
        public string Establecimiento { get; set; }

        /*Usuario Receptor de Orden*/
        public string idUsuarioReceptor { get; set; }
        public string mensaje { get; set; }

    }

    [Serializable]
    public class SolicitudUsuario :General
    {
        public int idSolicitudUsuario { get; set; }
        public int tipoSolicitud { get; set; }
        public Usuario usuario { get; set; }
        public Rol _rol { get; set; }
        public List<Rol> rol { get; set; }
        public List<Enfermedad> enfermedad { get; set; }
        public Examen _examen { get; set; }
        public List<Examen> examen { get; set; }
        public List<SelectListItem> profesion { get; set; }
        public List<SelectListItem> tipoDocumento { get; set; }
        public string fechaEnvio { get; set; }
        public string Renipress { get; set; }
        public string Establecimiento { get; set; }
        public string nombreInstitucion { get; set; }
        public string nombreDisa { get; set; }
        public string nombreRed { get; set; }
        public string nombreMicroRed { get; set; }
        public int estado { get; set; }
        public int idUsuarioVal1 { get; set; }
        public int idUsuarioVal2 { get; set; }
        public string validador1 { get; set; }
        public string validador2 { get; set; }
        public string fechaVal1 { get; set; }
        public string fechaVal2 { get; set; }
        public string comentario1 { get; set; }
        public string comentario2 { get; set; }
        public bool firmante { get; set; }
        public string JefeEESS { get; set; }
        public string cargoJf { get; set; }
        public Archivo file { get; set; }
       
    }
}


