using System.Collections.Generic;
using DataLayer;
using BusinessLayer.Interfaces;
using Model;
using System;

namespace BusinessLayer
{
    public class EnfermedadBl : IEnfermedadBl
    {
        /// <summary>
        /// Descripción: Obtiene todas las enfermedades filtrado por el Nombre
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        public List<Enfermedad> GetEnfermedadesByNombre(string nombre)
        {
            using (var enfermedadDal = new EnfermedadDal())
            {
                return enfermedadDal.GetEnfermedadesByNombre(nombre);
            }

        }
        /// <summary>
        /// Descripción: Obtiene todas las enfermedades filtrado por el Nombre, Id Laboratorio y Id Usuario.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="idLaboratorio"></param>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public List<Enfermedad> GetEnfermedadesByNombre(string nombre, int idLaboratorio, int idUsuario)
        {
            using (var enfermedadDal = new EnfermedadDal())
            {
                return enfermedadDal.GetEnfermedadesByNombre(nombre, idLaboratorio, idUsuario);
            }
        }

        public List<Enfermedad> GetEnfermedades()
        {
            using (var enfermedadDAL = new EnfermedadDal())
            {
                return enfermedadDAL.GetEnfermedades();
            }
        }

        public List<Enfermedad> GetEnfermedadesByNombre(string nombre, string idOrden)
        {
            using (var enfermedadDAL = new EnfermedadDal())
            {
                return enfermedadDAL.GetEnfermedadesByNombre(nombre, idOrden);
            }
        }
    }
}
