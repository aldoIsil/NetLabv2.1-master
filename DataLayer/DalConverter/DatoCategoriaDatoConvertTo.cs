using System.Data;
using Framework.DAL;
using Model;

namespace DataLayer.DalConverter
{
    public class DatoCategoriaDatoConvertTo : Converter
    {
        /// <summary>
        /// Descripción: Convierte un data table a una lista tipada con la entidad DatoCategoriaDato
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public static DatoCategoriaDato DatoCategoria(DataTable dataTable)
        {
            if (dataTable.Rows.Count == 0)
                return null;

            var row = dataTable.Rows[0];

            return new DatoCategoriaDato
            {
                IdDato = GetInt(row, "idDato"),
                IdCategoriaDato = GetInt(row, "idCategoriaDato"),
                Orden = GetInt(row, "orden"),
                Estado = GetInt(row, "estado")
            };
        }
    }
}