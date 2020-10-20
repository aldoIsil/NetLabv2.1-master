using System.Collections.Generic;
using System.Data;
using System.Linq;
using Framework.DAL;
using Model;

namespace DataLayer.DalConverter
{
    public class DatoConvertTo : Converter
    {
        /// <summary>
        /// Descripción: Convierte un data table a una lista tipada con la entidad GetDatos
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public static List<Dato> Datos(DataTable dataTable)
        {
            return dataTable.AsEnumerable().Select(GetDatos).ToList();
        }
        /// <summary>
        /// Descripción: Convierte un data table a una lista tipada con la entidad GetDatos
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public static Dato Dato(DataTable dataTable)
        {
            if (dataTable.Rows.Count == 0)
                return null;

            var row = dataTable.Rows[0];

            return GetDatos(row);
        }
        /// <summary>
        /// Descripción: Convierte un data table a una lista tipada con la entidad GetDatos
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private static Dato GetDatos(DataRow row)
        {
            return new Dato
            {
                IdDato = GetInt(row, "idDato"),
                Prefijo = GetString(row, "prefijo"),
                Sufijo = GetString(row, "sufijo"),
                IdTipo = GetInt(row, "idTipo"),
                IdDatoDependiente = GetInt(row, "idDatoDependiente"),
                idProyecto = GetInt(row, "idProyecto"),
                Proyecto = GetString(row, "Proyecto"),
                Visible = GetBool(row, "visible"),
                Obligatorio = GetBool(row, "obligatorio"),
                IdListaDato = GetIntNullable(row, "idListaDato"),
                IdGenero = GetIntNullable(row, "idGenero"),
                Estado = GetInt(row, "estado"),
                IdsExamen = GetString(row, "IdsExamen")
            };
        }
    }
}