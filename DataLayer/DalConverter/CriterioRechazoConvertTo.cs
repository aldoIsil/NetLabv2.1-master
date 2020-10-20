using System.Collections.Generic;
using System.Data;
using System.Linq;
using Framework.DAL;
using Model;

namespace DataLayer.DalConverter
{
    public class CriterioRechazoConvertTo : Converter
    {
        /// <summary>
        /// Descripción: Convierte data table a una lista tipada de CriterioRechazo
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<CriterioRechazo> Criterios(DataTable dataTable)
        {
            return dataTable.AsEnumerable().Select(GetCriterio).ToList();
        }
        /// <summary>
        /// Descripción: Convierte data table a una lista tipada de CriterioRechazo
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static CriterioRechazo Criterio(DataTable dataTable)
        {
            if (dataTable.Rows.Count == 0)
                return null;

            var row = dataTable.Rows[0];

            return GetCriterio(row);
        }
        /// <summary>
        /// Descripción: Convierte data table a una lista tipada de CriterioRechazo
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private static CriterioRechazo GetCriterio(DataRow row)
        {
            return new CriterioRechazo
            {
                IdCriterioRechazo = GetInt(row, "idCriterioRechazo"),
                Glosa = GetString(row, "glosa"),
                Tipo = GetInt(row, "tipo"),
                DescripcionTipo = GetString(row, "descripcionTipo"),
                Estado = GetInt(row, "Estado")
            };
        }
    }
}