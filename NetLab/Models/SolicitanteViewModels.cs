using Model;
using NetLab.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NetLab.Models
{
    [Serializable]
    public class SolicitanteViewModels
    {
        public OrdenSolicitante Solicitante { get; set; }
        public int idSolicitante { get; set; }
        public string apellidoMaterno { get; set; }
        public string apellidoPaterno { get; set; }
        public string Nombres { get; set; }
        public ListaDetalleViewModels Profesion { get; set; }
        public int idProfesion { get; set; }
        public int codigoColegio { get; set; }
        public int telefonoContacto { get; set; }
        public string correo { get; set; }
        public int idEstablecimiento { get; set; }
        public int nroDocumento { get; set; }
        public int idUbigeoReniec { get; set; }
        public DateTime fechaNacimiento { get; set; }
        public string direccionReniec { get; set; }
        public int estado { get; set; }
        public DateTime fechaRegistro { get; set; }
        public int idUsuarioRegistro { get; set; }
        public DateTime fechaEdicion { get; set; }
        public int idUsuarioEdicion { get; set; }
    }
}