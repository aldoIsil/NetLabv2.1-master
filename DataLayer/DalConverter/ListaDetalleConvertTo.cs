using System.Collections.Generic;
using System.Data;
using System.Linq;
using Framework.DAL;
using Model;

namespace DataLayer.DalConverter
{
    public class ListaDetalleConvertTo : Converter
    {
        /// <summary>
        /// Descripción: Obtiene informacion del detalle las listas a mostrar .
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<ListaDetalle> ListaDetalles(DataTable dataTable)
        {
            return dataTable.AsEnumerable().Select(GetListaDetalle).ToList();
        }
        /// <summary>
        /// Descripción: Obtiene informacion del detalle las listas a mostrar .
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private static ListaDetalle GetListaDetalle(DataRow row)
        {
            return new ListaDetalle
            {
                IdDetalleLista = GetInt(row, "idDetalleLista"),
                Glosa = GetString(row, "glosa"),
                OrdenLista = GetInt(row, "ordenLista")
            };
        }
    }
}