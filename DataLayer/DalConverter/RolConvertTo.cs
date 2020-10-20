using System.Collections.Generic;
using System.Data;
using System.Linq;
using Framework.DAL;
using Model;

namespace DataLayer.DalConverter
{
    public class RolConvertTo : Converter
    {
        /// <summary>
        /// Descripción: Convierte un data table a una lista tipada con la entidad Rol
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<Rol> Roles(DataTable dataTable)
        {
            return dataTable.AsEnumerable().Select(GetRol).ToList();
        }

        public static Rol Rol(DataTable dataTable)
        {
            if (dataTable.Rows.Count == 0)
                return null;

            var row = dataTable.Rows[0];

            return GetRol(row);
        }

        private static Rol GetRol(DataRow row)
        {
            return new Rol
            {
                idRol = GetInt(row, "idRol"),
                nombre = GetString(row, "nombre"),
                descripcion = GetString(row, "descripcion"),
                Estado = GetInt(row, "estado")
            };
        }
    }
}