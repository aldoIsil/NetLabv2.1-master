using Framework.DAL;
using Model;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DataLayer.DalConverter
{
    public class CategoriaDatoConvertTo : Converter
    {
        /// <summary>
        /// Descripción: Convierte un data table a una lista tipada con la entidad CategoriaDato
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public static List<CategoriaDato> Categorias(DataTable dataTable)
        {
            return dataTable.AsEnumerable().Select(GetCategoria).ToList();
        }
        /// <summary>
        /// Descripción: Convierte un data table a una lista tipada con la entidad CategoriaDato
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private static CategoriaDato GetCategoria(DataRow row)
        {
            return new CategoriaDato
            {
                IdCategoriaDato = GetInt(row, "idCategoriaDato"),
                Nombre = GetString(row, "nombre"),
                Descripcion = GetString(row, "descripcion"),
                IdCategoriaDatoPadre = GetIntNullable(row, "idCategoriaDatoPadre"),
                IdGenero = GetIntNullable(row, "idGenero"),
                Visible = GetBool(row, "visible"),
                Orientacion = GetInt(row, "orientacion"),
                Estado = GetInt(row, "estado")
            };
        }
        /// <summary>
        /// Descripción: Convierte un data table a una lista tipada con la entidad CategoriaDato
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public static CategoriaDato Categoria(DataTable dataTable)
        {
            if (dataTable.Rows.Count == 0)
                return null;

            var row = dataTable.Rows[0];

            return GetCategoria(row);
        }
    }
}
