using System;
using BusinessLayer.Interfaces;
using DataLayer;
using System.Collections.Generic;
using Model;

namespace BusinessLayer
{
    public class CatalogoBl
    {
        /// <summary>
        /// Descripción: Clase para aplicar las reglas del negocio
        ///              y obtener los datos de la clase CatalogoDal  
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        public List<Enfermedad> GetEnfermedades (string nombre = null)
        {
            using (var catalogoDal = new CatalogoDal())
            {
                return catalogoDal.GetEnfermedades(nombre);
            }
        }



    }
}
