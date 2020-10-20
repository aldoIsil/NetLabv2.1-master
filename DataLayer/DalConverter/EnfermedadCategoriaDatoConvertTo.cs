using System.Data;
using Framework.DAL;
using Model;

namespace DataLayer.DalConverter
{
    public class EnfermedadCategoriaDatoConvertTo : Converter
    {
        /// <summary>
        /// Descripción: Convierte un data table a una lista tipada con la entidad EnfermedadCategoriaDato
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static EnfermedadCategoriaDato EnfermedadCategoria(DataTable dataTable)
        {
            if (dataTable.Rows.Count == 0)
                return null;

            var row = dataTable.Rows[0];

            return new EnfermedadCategoriaDato
            {
                IdCategoriaDato = GetInt(row, "idCategoriaDato"),
                IdEnfermedad = GetInt(row, "idEnfermedad"),
                Orden = GetInt(row, "orden"),
                Estado = GetInt(row, "estado")
            };
        }
    }
}