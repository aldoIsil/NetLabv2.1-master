using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Serializable]
    public class MuestraPreRecepcion
    {
        [Display(Name = "Tipo Documento:")]
        public int tipoDocumento { get; set; }
        [Display(Name = "Nro Documento:")]
        public string nroDocumento { get; set; }
        [Display(Name = "Apellido Paterno:")]
        public string apePaterno { get; set; }
        [Display(Name = "Apellido Materno:")]
        public string apeMaterno { get; set; }
        [Display(Name = "Nombres:")]
        public string nombres { get; set; }
        public int idUsuarioRegistro { get; set; }
        [Display(Name = "Validación:")]
        public string tipoValidacion { get; set; }
    }
}
