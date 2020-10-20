using System.Collections.Generic;
using System.Data;
using System.Linq;
using Framework.DAL;
using Model;

namespace DataLayer.DalConverter
{
    public class ListaDatoConvertTo : Converter
    {
        /// <summary>
        /// Descripción: Convierte un data table a una lista tipada con la entidad ListaDato
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<ListaDato> ListaDatos(DataTable dataTable)
        {
            return dataTable.AsEnumerable().Select(GetListaDato).ToList();
        }
        /// <summary>
        /// Descripción: Convierte un data table a una lista tipada con la entidad ListaDato
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static ListaDato ListaDato(DataTable dataTable)
        {
            if (dataTable.Rows.Count == 0)
                return null;

            var row = dataTable.Rows[0];

            return GetListaDato(row);
        }
        /// <summary>
        /// Descripción: Convierte un data table a una lista tipada con la entidad ListaDato
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private static ListaDato GetListaDato(DataRow row)
        {
            return new ListaDato
            {
                IdListaDato = GetInt(row, "idListaDato"),
                Nombre = GetString(row, "nombre"),
                Descripcion = GetString(row, "descripcion"),
                Estado = GetInt(row, "estado")
            };
        }
        /// <summary>
        /// Descripción: Convierte un data table a una lista tipada con la entidad OpcionDato
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<OpcionDato> Opciones(DataTable dataTable)
        {
            return dataTable.AsEnumerable().Select(GetOpcion).ToList();
        }
        /// <summary>
        /// Descripción: Convierte un data table a una lista tipada con la entidad Opcion
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static OpcionDato Opcion(DataTable dataTable)
        {
            if (dataTable.Rows.Count == 0)
                return null;

            var row = dataTable.Rows[0];

            return GetOpcion(row);
        }
        /// <summary>
        /// Descripción: Convierte un data table a una lista tipada con la entidad OpcionDato
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private static OpcionDato GetOpcion(DataRow row)
        {
            return new OpcionDato
            {
                IdListaDato = GetInt(row, "idListaDato"),
                IdOpcionDato = GetInt(row, "idOpcionDato"),
                Valor = GetString(row, "valor"),
                Orden = GetInt(row, "orden")
            };
        }
    }
}