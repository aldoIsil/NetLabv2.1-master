using DataLayer;
using System.Collections.Generic;
using BusinessLayer.Interfaces;
using Model;

namespace BusinessLayer
{
    public class CriterioRechazoBl : ICriterioRechazoBl
    {
        /// <summary>
        /// Descripción: Obtiene los criterios de rechazo ingresados por cada muestra.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idTipoMuestra"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public List<CriterioRechazo> GetCriteriosByTipoMuestra(int idTipoMuestra, int tipo)
        {
            using (var criterioDal = new CriterioRechazoDal())
            {
                return criterioDal.GetCriteriosByTipoMuestra(idTipoMuestra, tipo);
            }
        }
        /// <summary>
        /// Descripción: Obtiene los criterios de rechazo ingresados filtrado por el nombre.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        public List<CriterioRechazo> GetCriteriosActivos(string nombre)
        {
            using (var criterioDal = new CriterioRechazoDal())
            {
                return criterioDal.GetCriteriosActivos(nombre);
            }
        }

        /// <summary>
        /// Descripción: Obtiene los criterios de rechazo ingresados activos
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        public List<CriterioRechazo> GetCriterios(string nombre)
        {
            using (var criterioDal = new CriterioRechazoDal())
            {
                return criterioDal.GetCriterios(nombre);
            }
        }

        /// <summary>
        /// Descripción: Obtiene los criterios de rechazo ingresados filtrado por el Id.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idCriterioRechazo"></param>
        /// <returns></returns>
        public CriterioRechazo GetCriterioById(int idCriterioRechazo)
        {
            using (var criterioDal = new CriterioRechazoDal())
            {
                return criterioDal.GetCriterioById(idCriterioRechazo);
            }
        }
        /// <summary>
        /// Descripción: Registro de un criterio de rechazo.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="criterioRechazo"></param>
        public void InsertCriterio(CriterioRechazo criterioRechazo)
        {
            using (var criterioDal = new CriterioRechazoDal())
            {
                criterioDal.InsertCriterio(criterioRechazo);
            }
        }
        /// <summary>
        /// Descripción: Actualizacion de un criterio de rechazo.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="criterioRechazo"></param>       
        public void UpdateCriterio(CriterioRechazo criterioRechazo)
        {
            using (var criterioDal = new CriterioRechazoDal())
            {
                criterioDal.UpdateCriterio(criterioRechazo);
            }
        }
    }
}
