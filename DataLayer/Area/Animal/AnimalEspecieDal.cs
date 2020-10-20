using System.Collections.Generic;
using DataLayer.DalConverter;
using Framework.DAL;
using Framework.DAL.Settings.Implementations;
using Model;

namespace DataLayer.Area.Animal
{
    public class AnimalEspecieDal :DaoBase
    {
        public AnimalEspecieDal(IDalSettings settings) : base(settings)
        {
        }

        public AnimalEspecieDal() : this(new NetlabSettings())
        {
        }
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
            var objCommand = GetSqlCommand("pNLS_Especie");

            return EspecieConvertTo.Especies(Execute(objCommand));
        }
    }
}