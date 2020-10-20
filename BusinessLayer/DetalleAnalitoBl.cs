using DataLayer;
using System.Collections.Generic;
using System.Linq;
using Model;
using System;

namespace BusinessLayer
{
    public class DetalleAnalitoBl 
    {
        /// <summary>
        /// Descripción: Obtiene informacion para la validacion de los componenetes de un a prueba.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idOrdenExamen"></param>
        /// <returns></returns>
        public List<ResultAnalito> GetAnalitoByOrdenExamen(string[] idOrdenExamen)
        {
            if (idOrdenExamen == null || !idOrdenExamen.Any())
                return new List<ResultAnalito>();

            using (var detalleanalitoDal = new DetalleAnalitoDal())
            {
                return detalleanalitoDal.GetAnalitoByOrdenExamen(string.Join(",", idOrdenExamen)); 
            }
        }
        /// <summary>
        /// Descripción: Obtiene informacion de los componenetes por examen.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idOrdenExamen"></param>
        /// <param name="genero"></param>
        /// <returns></returns>
        public List<ResultAnalito> GetAnalitoByOrdenExamenAndGenero(string[] idOrdenExamen,int genero, int? idPlataforma = null)
        {
            if (idOrdenExamen == null || !idOrdenExamen.Any())
                return new List<ResultAnalito>();

            using (var detalleanalitoDal = new DetalleAnalitoDal())
            {
                return detalleanalitoDal.GetAnalitoByOrdenExamenAndGenero(string.Join(",", idOrdenExamen),genero,idPlataforma);
            }
        }
        /// <summary>
        /// Descripción: Obtiene las opciones ingresados por cada analito.  
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idAnalito"></param>
        /// <returns></returns>
        public List<AnalitoOpcionResultado> GetAnalitosbyIdAnalito(Guid idAnalito)
        {
            using (var detalleanalitoDal = new DetalleAnalitoDal())
            {
                return detalleanalitoDal.GetAnalitosbyIdAnalito(idAnalito,Guid.Parse(""));
            }
        }
    }
}
