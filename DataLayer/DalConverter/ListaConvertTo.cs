using System.Data;
using Framework.DAL;
using Model;

namespace DataLayer.DalConverter
{
    public class ListaConvertTo : Converter
    {
        /// <summary>
        /// Descripción: Convierte un data table a una lista tipada con la entidad Lista
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017. 
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static Lista Lista(DataTable dataTable)
        {
            if (dataTable.Rows.Count == 0)
                return null;

            var row = dataTable.Rows[0];

            return new Lista
            {
                IdLista = GetInt(row, "idLista"),
                Glosa = row["glosa"].ToString()
            };
        }
    }
}