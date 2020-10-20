using System.Collections.Generic;
using BusinessLayer.Interfaces;
using DataLayer;
using Model;

namespace BusinessLayer
{
    public class PresentacionBl : IPresentacionBl
    {
        /// <summary>
        /// Descripción: Obtiene una presentacion filtrada por la Glosa
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="glosa"></param>
        /// <returns></returns>
        public List<Presentacion> GetPresentaciones(string nombre)
        {
            using (var presentacionDal = new PresentacionDal())
            {
                return presentacionDal.GetPresentaciones(nombre);
            }
        }
        /// <summary>
        /// Descripción: Obtiene una presentacion
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idPresentacion"></param>
        /// <returns></returns>
        public Presentacion GetPresentacionById(int idPresentacion)
        {
            using (var presentacionDal = new PresentacionDal())
            {
                return presentacionDal.GetPresentacionById(idPresentacion);
            }
        }
        /// <summary>
        /// Descripción:insertar una nueva Presentacion
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="presenta"></param>
        public void InsertPresentacion(Presentacion pres)
        {
            using (var presentacionDal = new PresentacionDal())
            {
                presentacionDal.InsertPresentacion(pres);
            }
        }
        /// <summary>
        /// Descripción: Actualiza una presentacion
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="presenta"></param>
        public void UpdatePresentacion(Presentacion pres)
        {
            using (var presentacionDal = new PresentacionDal())
            {
                presentacionDal.UpdatePresentacion(pres);
            }
        }
        /// <summary>
        /// Descripción: Obtiene las presentaciones por tipo de muestra
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idTipoMuestra"></param>
        /// <returns></returns>
        public List<Presentacion> GetPresentacionesByIdTipoMuestra(int? idTipoMuestra)
        {
            using (var presentacionDal = new PresentacionDal())
            {
                return presentacionDal.GetPresentacionesByIdTipoMuestra(idTipoMuestra);
            }
        }
    }
}


