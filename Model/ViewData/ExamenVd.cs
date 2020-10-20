using System;
using System.Collections.Generic;

namespace Model.ViewData
{
    [Serializable]
    public class ExamenVd
    {

        public Examen examen { get; set; }
        public String idBtnEliminarExamen { get; set; }
        public String idCmbAnalito { get; set; }

        public List<TipoMuestra> tipoMuestraList { get; set; }


        public String[] analitosSeleccionados { get; set; }

        public String[] idTiposMuestraSeleccionados { get; set; }

        public String[] nombreTiposMuestraSeleccionados { get; set; }

    }
}
