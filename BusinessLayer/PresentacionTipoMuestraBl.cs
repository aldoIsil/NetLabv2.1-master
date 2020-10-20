using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BusinessLayer.Interfaces;
using DataLayer;
using Model;

namespace BusinessLayer
{
    public class PresentacionTipoMuestraBl : IPresentacionTipoMuestraBl
    {
        /// <summary>
        /// Descripción: Realiza la transaccion para obtener informacion de los tipos de muestras 
        /// a registrar y registralas para una presentacion
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="presentacion"></param>
        /// <param name="tipoMuestras"></param>
        /// <param name="idUsuario"></param>
        public void AgregarTipoMuestrasPorPresentacion(Presentacion presentacion, int[] tipoMuestras, int idUsuario)
        {
            if (tipoMuestras == null || !tipoMuestras.Any()) return;

            var tipoMuestrasByPresentacion = GetTipoMuestrasByPresentacionId (presentacion.idPresentacion);

            tipoMuestras = tipoMuestras.Where(x => tipoMuestrasByPresentacion.All(y => y.idTipoMuestra != x)).ToArray();

            var listTipoMuestras = tipoMuestras.Select(tipomuestra => GetTipoMuestraPresentacion(presentacion, tipomuestra, idUsuario)).ToList();

            InsertTipoMuestraByPresentacion(listTipoMuestras);
        }
        /// <summary>
        /// Descripción: Obtiene la presentacion de muestras ACTIVOS por codigo de presentacion
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idPresentacion"></param>
        /// <returns></returns>
        public List<TipoMuestra> GetTipoMuestrasByPresentacionId(int idPresentacion)
        {
            using (var presentaTipoMuestraDal = new PresentacionTipoMuestraDal())
            {
                return presentaTipoMuestraDal.GetTipoMuestrasByPresentacionId(idPresentacion) ;
            }
            
        }
        /// <summary>
        /// Descripción: Devuelve la entidad PresentacionTipoMuestra.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="presentacion"></param>
        /// <param name="idTipoMuestra"></param>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        private static PresentacionTipoMuestra GetTipoMuestraPresentacion (Presentacion presentacion, int idTipoMuestra, int idUsuario)
        {
            return new PresentacionTipoMuestra 
            {
                IdPresentacion = presentacion.idPresentacion,
                IdTipoMuestra = idTipoMuestra,
                IdUsuarioRegistro = idUsuario
            };
        }
        /// <summary>
        /// Descripción: Registra o Actualiza la Presentacion de la Muestra
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="tipomuestra"></param>
        private static void InsertTipoMuestraByPresentacion(IEnumerable<PresentacionTipoMuestra> tipomuestras)
        {
            using (var presentacionTipoMuestraDal  = new PresentacionTipoMuestraDal())
            {
                presentacionTipoMuestraDal.BeginTransaction(IsolationLevel.ReadCommitted);

                try
                {
                    foreach (var item in tipomuestras)
                    {
                        presentacionTipoMuestraDal.InsertTipoMuestraByPresentacion(item);
                    }

                    presentacionTipoMuestraDal.Commit();
                }
                catch (Exception)
                {
                    presentacionTipoMuestraDal.Rollback();
                }
            }
        }
        /// <summary>
        /// Descripción: Actualiza un tipo de muestra de una presentacion
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="presentaTipoMuestra"></param>
        public void UpdateTipoMuestraByPresentacion(PresentacionTipoMuestra presentaTipoMuestra)
        {
            using (var presentaTipoMuestraDal = new PresentacionTipoMuestraDal())
            {
                presentaTipoMuestraDal.UpdateTipoMuestraByPresentacion(presentaTipoMuestra);
            }

        }


    }
}
