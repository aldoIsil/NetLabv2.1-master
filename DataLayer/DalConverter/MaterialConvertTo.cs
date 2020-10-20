using System.Collections.Generic;
using System.Data;
using System.Linq;
using Framework.DAL;
using Model;

namespace DataLayer.DalConverter
{
    public class MaterialConvertTo : Converter
    {
        /// <summary>
        /// Descripción: Convierte un data table a una lista tipada con la entidad Material
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<Material> Materiales(DataTable dataTable)
        {
            return dataTable.AsEnumerable().Select(GetMaterial).ToList();
        }

        public static Material Material(DataTable dataTable)
        {
            if (dataTable.Rows.Count == 0)
                return null;

            var row = dataTable.Rows[0];

            return GetMaterial(row);
        }

        private static Material GetMaterial(DataRow row)
        {
            return new Material
            {
                idMaterial = GetInt(row, "idMaterial"),
                volumen = GetDecimal(row, "volumen"),
                estado = GetInt(row, "estado"),
                IdPresentacion= GetInt(row, "idPresentacion"),
                IdTipoMuestra = GetInt(row, "idTipoMuestra"),
                IdReactivo = GetInt(row, "IdReactivo"),
                TipoMuestra = GetTipoMuestra(row),
                Presentacion = GetPresentacion(row),
                Reactivo = GetReactivo(row)

            };
        }

        private static TipoMuestra GetTipoMuestra(DataRow row)
        {
            return new TipoMuestra
            {
                nombre = GetString(row, "tipoMuestra")
            };
        }

        private static Presentacion GetPresentacion(DataRow row)
        {
            return new Presentacion
            {
                glosa = GetString(row, "presentacion")
            };
        }

        private static Reactivo GetReactivo(DataRow row)
        {
            return new Reactivo
            {
                glosa = GetString(row, "reactivo")
            };
        }
    }
}