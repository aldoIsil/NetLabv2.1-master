using System;
using System.Collections.Generic;
using BusinessLayer.Interfaces;
using DataLayer;
using Model;

namespace BusinessLayer
{
    public class ExamenTipoMuestraBl : IExamenTipoMuestraBl
    {
        /// <summary>
        /// Descripción: Obtener los tipos de muestra por examen.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idExamen"></param>
        /// <returns></returns>
        public List<ExamenTipoMuestra> GetTipoMuestraByExamen(Guid idExamen)
        {
            using (var tipoMuestraDal = new ExamenTipoMuestraDal())
            {
                return tipoMuestraDal.GetTipoMuestraByExamen(idExamen);
            }
        }
        /// <summary>
        /// Descripción: Regista o Actualiza Tipo de Muestra/Examen. 
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="examenTipoMuestra"></param>
        public void InsertTipoMuestraByExamen(ExamenTipoMuestra examenTipoMuestra)
        {
            using (var tipoMuestraDal = new ExamenTipoMuestraDal())
            {
                tipoMuestraDal.InsertTipoMuestraByExamen(examenTipoMuestra);
            }
        }
        /// <summary>
        /// Descripción: Actualiza los tipos de muestra/examen
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="examenTipoMuestra"></param>
        public void UpdateTipoMuestraByExamen(ExamenTipoMuestra examenTipoMuestra)
        {
            using (var tipoMuestraDal = new ExamenTipoMuestraDal())
            {
                tipoMuestraDal.UpdateTipoMuestraByExamen(examenTipoMuestra);
            }
        }
    }
}