using System.Collections.Generic;
using System.Data;
using System.Linq;
using Framework.DAL;
using Model;

namespace DataLayer.DalConverter
{
    public class ExamenTipoMuestraConvertTo : Converter
    {
        /// <summary>
        /// Descripción: Convierte un data table a una lista con entidad ExamenTipoMuestra
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<ExamenTipoMuestra> TiposMuestra(DataTable dataTable)
        {
            return dataTable.AsEnumerable().Select(GetExamenTipoMuestra).ToList();
        }
        /// <summary>
        /// Descripción: Convierte un data table a una lista con entidad ExamenTipoMuestra
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        private static ExamenTipoMuestra GetExamenTipoMuestra(DataRow row)
        {
            return new ExamenTipoMuestra
            {
                IdTipoMuestra = GetInt(row, "idTipoMuestra"),
                TipoMuestra = GetTipoMuestra(row),
                Caracteristica = GetString(row, "caracteristicaMuestra"),
                Transporte = GetString(row, "transporteMuestra")
            };
        }
        /// <summary>
        /// Descripción: Convierte un data table a una lista con entidad ExamenTipoMuestra
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        private static TipoMuestra GetTipoMuestra(DataRow row)
        {
            return new TipoMuestra
            {
                idTipoMuestra = GetInt(row, "idTipoMuestra"),
                nombre = GetString(row, "nombre")
            };
        }
    }
}