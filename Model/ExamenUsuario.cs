using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Serializable]
    public class ExamenUsuario : General
    {
        public Guid IdExamen { get; set; }
        public int idUsuario { get; set; }
        public int idEnfermedad { get; set; }
        public int idUsuarioEnfermedadExamen { get; set; }
        public string Glosa { get; set; }
        public int Orden { get; set; }
        public int idTipo { get; set; }
    }
}
