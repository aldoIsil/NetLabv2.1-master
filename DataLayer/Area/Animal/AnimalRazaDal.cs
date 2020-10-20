using System.Collections.Generic;
using DataLayer.DalConverter;
using Framework.DAL;
using Framework.DAL.Settings.Implementations;
using Model;

namespace DataLayer.Area.Animal
{
    public class AnimalRazaDal :DaoBase
    {
        public AnimalRazaDal(IDalSettings settings) : base(settings)
        {
        }

        public AnimalRazaDal() : this(new NetlabSettings())
        {
        }
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
            var objCommand = GetSqlCommand("pNLS_RazaByEspecie");
            InputParameterAdd.Int(objCommand, "idEspecie", idEspecie);

            return RazaConvertTo.Razas(Execute(objCommand));
        }
    }
}