using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BusinessLayer.Interfaces;
using DataLayer;
using Model;

namespace BusinessLayer
{
    public class TipoMuestraCriterioRechazoBl : ITipoMuestraCriterioRechazoBl
    {
        /// <summary>
        /// Descripción: Registra nuevos criterio de rechazo para un tipo de muestra 
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="tipoMuestra"></param>
        /// <param name="criterios"></param>
        /// <param name="idUsuario"></param>
        public void AgregarCriteriosPorMuestra(TipoMuestra tipoMuestra, int[] criterios, int idUsuario)
        {
            if (criterios == null || !criterios.Any()) return;

            var criteriosByTipoMuestra = GetCriteriosByTipoMuestraId(tipoMuestra.idTipoMuestra);

            criterios = criterios.Where(x => criteriosByTipoMuestra.All(y => y.IdCriterioRechazo != x)).ToArray();

            var listCriterios = criterios.Select(criterio => GetCriterioTipoMuestra(tipoMuestra, criterio, idUsuario)).ToList();

            InsertCriterioByTipoMuestra(listCriterios);
        }
        /// <summary>
        /// Descripción: Obtiene los criterios de rechazo por tipo de muestra 
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idTipoMuestra"></param>
        /// <returns></returns>
        public List<CriterioRechazo> GetCriteriosByTipoMuestraId(int idTipoMuestra)
        {
            using (var muestraCriterioRechazoDal = new TipoMuestraCriterioRechazoDal())
            {
                return muestraCriterioRechazoDal.GetCriteriosByTipoMuestraId(idTipoMuestra);
            }
        }
        /// <summary>
        /// Descripción: Retorna la entidad TipoMuestraCriterioRechazo cargada.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="tipoMuestra"></param>
        /// <param name="idCriterioRechazo"></param>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        private static TipoMuestraCriterioRechazo GetCriterioTipoMuestra(TipoMuestra tipoMuestra, int idCriterioRechazo, int idUsuario)
        {
            return new TipoMuestraCriterioRechazo
            {
                IdTipoMuestra = tipoMuestra.idTipoMuestra,
                IdCriterioRechazo = idCriterioRechazo,
                IdUsuarioRegistro = idUsuario
            };
        }
        /// <summary>
        /// Descripción: Registra Tipos de Criterios de Rechazo verificando que no exista.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="criterios"></param>
        private static void InsertCriterioByTipoMuestra(IEnumerable<TipoMuestraCriterioRechazo> criterios)
        {
            using (var muestraCriterioRechazoDal = new TipoMuestraCriterioRechazoDal())
            {
                muestraCriterioRechazoDal.BeginTransaction(IsolationLevel.ReadCommitted);
                try
                {
                    foreach (var item in criterios)
                    {
                        muestraCriterioRechazoDal.InsertCriterioByTipoMuestra(item);
                    }

                    muestraCriterioRechazoDal.Commit();
                }
                catch (Exception)
                {
                    muestraCriterioRechazoDal.Rollback();
                }
            }
        }
        /// <summary>
        /// Descripción: Actualiza Tipo de Criterios de Rechazo verificando que no exista.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="muestraCriterioRechazo"></param>
        public void UpdateCriterioByTipoMuestra(TipoMuestraCriterioRechazo muestraCriterioRechazo)
        {
            using (var muestraCriterioRechazoDal = new TipoMuestraCriterioRechazoDal())
            {
                muestraCriterioRechazoDal.UpdateCriterioByTipoMuestra(muestraCriterioRechazo);
            }
        }
    }
}