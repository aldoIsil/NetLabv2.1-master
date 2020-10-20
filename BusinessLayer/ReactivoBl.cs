using System.Collections.Generic;
using BusinessLayer.Interfaces;
using DataLayer;
using Model;

namespace BusinessLayer
{
    public class ReactivoBl : IReactivoBl
    {
        /// <summary>
        /// Descripción: Obtiene informacion de los reactivos filtrados por la Glosa.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="glosa"></param>
        /// <returns></returns>
        public List<Reactivo> GetReactivos(string nombre)
        {
            using (var reactivoDal = new ReactivoDal())
            {
                return reactivoDal.GetReactivos(nombre); 
            }
        }
        /// <summary>
        /// Descripción: Obtiene informacion de los reactivos filtrados por el codigo del reactivo.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idReactivo"></param>
        /// <returns></returns>
        public Reactivo GetReactivoById(int idReactivo)
        {
            using (var reactivoDal = new ReactivoDal())
            {
                return reactivoDal.GetReactivoById(idReactivo); 
            }
        }
        /// <summary>
        /// Descripción: Insertar un tipo de reactivo 
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="reactivo"></param>
        public void InsertReactivo(Reactivo reactivo)
        {
            using (var reactivoDal = new ReactivoDal())
            {
                reactivoDal.InsertReactivo(reactivo);
            }
        }
        /// <summary>
        /// Descripción: Actualizar un tipo de reactivo.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="reactivo"></param>
        public void UpdateReactivo(Reactivo reactivo)
        {
            using (var reactivoDal = new ReactivoDal())
            {
                reactivoDal.UpdateReactivo(reactivo);
            }
        }
        /// <summary>
        /// Descripción: Obtiene una presentacion/reactivo activa
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios. 
        /// </summary>
        /// <param name="idPresentacion"></param> 
        /// <returns></returns>
        public List<Reactivo> GetReactivosByIdPresentacion(int? idPresentacion)
        {
            using (var reactivoDal = new ReactivoDal())
            {
                return reactivoDal.GetReactivosByIdPresentacion(idPresentacion) ;
            }
        }
        

    }
}