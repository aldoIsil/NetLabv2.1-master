using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Model
{
    [Serializable]
    public class ConfiguracionEvalControlCalidad
    {
        [DisplayName("Codigo Evaluación")]
        public Guid idConfigEvaluacion { get; set; }
        public Establecimiento EstablecimientoEvaluador { get; set; }
        public Establecimiento EstablecimientoEvaluado { get; set; }
        public Enfermedad Enfermedad { get; set; }
        [DisplayName("Nombre Evaluación")]
        public string Nombre { get; set; }
        [DisplayName("Descripción Evaluación")]
        public string Descripcion { get; set; }
        public int idusuarioregistro { get; set; }
        [DisplayName("Usuario Registro")]
        public string UsuarioRegistro { get; set; }
        public string UsuarioEdicion { get; set; }
        public string fecharegistro { get; set; }
        public int idusuarioedicion { get; set; }
        public string fechaedicion { get; set; }
        public int estado { get; set; }
        public string DescEstado { get; set; }
    }
}
