using System.Collections.Generic;
using BusinessLayer.Animal.Interfaces;
using DataLayer.Area.Animal;
using Model;

namespace BusinessLayer.Animal
{
    public class EspecieBl : IEspecieBl
    {
        /// <summary>
        /// Descripción: Obtiene informacion de especies de animales.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <returns></returns>
        public List<AnimalEspecie> GetEspecies()
        {
            using (var especieDal = new AnimalEspecieDal())
            {
                return especieDal.GetEspecies();
            }
        }
    }
}