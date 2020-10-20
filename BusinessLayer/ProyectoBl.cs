using BusinessLayer.Interfaces;
using System.Collections.Generic;
using DataLayer;
using Model;

namespace BusinessLayer
{
    public class ProyectoBl : IProyectoBl
    {
        /// <summary>
        /// Descripción: Obtiene el Modulo activo a ingresar.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <returns></returns>
        public List<Proyecto> GetProyectos()
        {
            using (var proyectoDal = new ProyectoDal())
            {
                return proyectoDal.GetProyectos();
            }
        }
        /// <summary>
        /// Descripción: Obtiene el Modulo activo a ingresar para Banco de Sangre.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <returns></returns>
        public List<Proyecto> GetProyectosBS()
        {
            using (var proyectoDal = new ProyectoDal())
            {
                return proyectoDal.GetProyectosBS();
            }
        }
    }
}