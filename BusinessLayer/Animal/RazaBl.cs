using System.Collections.Generic;
using BusinessLayer.Animal.Interfaces;
using DataLayer.Area.Animal;
using Model;

namespace BusinessLayer.Animal
{
    public class RazaBl : IRazaBl
    {
        /// <summary>
        /// Descripción: Obtiene informacion de razas por especie.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <param name="idEspecie"></param>
        /// <returns></returns>
        public List<AnimalRaza> GetRazaByEspecie(int idEspecie)
        {
            using (var razaDal = new AnimalRazaDal())
            {
                return razaDal.GetRazaByEspecie(idEspecie);
            }
        }
    }
}