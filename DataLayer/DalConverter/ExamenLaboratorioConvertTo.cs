using System.Collections.Generic;
using System.Data;
using System.Linq;
using Framework.DAL;
using Model;

namespace DataLayer.DalConverter
{
    public class ExamenLaboratorioConvertTo : Converter
    {
        /// <summary>
        /// Descripción: Convierte un data table a una lista tipada con la entidad ExamenLaboratorio
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<ExamenLaboratorio> Examenes(DataTable dataTable)
        {
            return dataTable.AsEnumerable().Select(GetExamenLaboratorio).ToList();
        }
        /// <summary>
        /// Descripción: Convierte un data table a una lista tipada con la entidad ExamenLaboratorio
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static ExamenLaboratorio ExamenLaboratorio(DataTable dataTable)
        {
            if (dataTable.Rows.Count == 0)
                return null;

            var row = dataTable.Rows[0];

            return GetExamenLaboratorio(row);
        }
        /// <summary>
        /// Descripción: Convierte un data table a una lista tipada con la entidad ExamenLaboratorio
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private static ExamenLaboratorio GetExamenLaboratorio(DataRow row)
        {
            return new ExamenLaboratorio
            {
                IdExamen = GetGuid(row, "idExamen"),
                Examen = GetExamen(row),
                DiasEmision = GetIntNullable(row, "diasEmisionResultado"),
                DiasEntrega = GetIntNullable(row, "diasEntregaResultado"),
                Estado = GetInt(row, "estado")
            };
        }

        private static Examen GetExamen(DataRow row)
        {
            return new Examen
            {
                idExamen = GetGuid(row, "idExamen"),
                nombre = GetString(row, "nombre")
            };
        }
    }
}