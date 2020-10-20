using System.Collections.Generic;
using BusinessLayer.Interfaces;
using DataLayer;
using Model;

namespace BusinessLayer
{
    public class InstitucionBl : IInstitucionBl
    {
        /// <summary>
        /// Descripción: Obtiene Informacion de las instituciones, realiza busqueda filtrado por la descripcion de la institucion
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="textoBusqueda"></param>
        /// <returns></returns>
        public List<Institucion> GetInstitucionByTextoBusqueda(string textoBusqueda)
        {
            using (var institucionDal = new InstitucionDal())
            {
                return institucionDal.GetInstitucionByTextoBusqueda(textoBusqueda);
            }
        }
        /// <summary>
        /// Descripción: Obtiene Informacion de todas las instituciones.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <returns></returns>
        public List<Institucion> GetInstituciones()
        {
            using (var institucionDal = new InstitucionDal())
            {
                return institucionDal.GetInstituciones();
            }
        }

        public List<Institucion> ObtenerInstitucionPorTexto(string textoBusqueda)
        {
            using (var institucionDal = new InstitucionDal())
            {
                return institucionDal.ObtenerInstitucionPorTexto(textoBusqueda);
            }
        }

        public void IngresarInstitucion(string nombreInstitucion, int idUsuario, string descripcion)
        {
            using (var institucionDal = new InstitucionDal())
            {
                institucionDal.IngresarInstitucion(nombreInstitucion, idUsuario, descripcion);
            }
        }

        public void ActualizarInstitucion(int codigoInstitucion, string nombreInstitucion, int idUsuario, string descripcion, int estado)
        {
            using (var institucionDal = new InstitucionDal())
            {
                institucionDal.ActualizarInstitucion(codigoInstitucion,nombreInstitucion, idUsuario, descripcion, estado);
            }
        }

        public Institucion ObtenerInstitucionPorId(int Id)
        {
            using (var institucionDal = new InstitucionDal())
            {
                return institucionDal.ObtenerInstitucionPorId(Id);
            }
        }
    }
}
