using System.Collections.Generic;
using DataLayer.DalConverter;
using Framework.DAL;
using Framework.DAL.Settings.Implementations;
using Model;

namespace DataLayer.Compartido
{
    public class ListaDetalleDal : DaoBase
    {
        public ListaDetalleDal(IDalSettings settings) : base(settings)
        {
        }

        public ListaDetalleDal() : this(new NetlabSettings())
        {
        }
        /// <summary>
        /// Descripción: Obtiene informacion del detalle las listas a mostrar .
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<ListaDetalle> GetListaDetalleById(int id)
        {
            var objCommand = GetSqlCommand("pNLS_ListaDetalleById");
            InputParameterAdd.Int(objCommand, "idLista", id);

            return ListaDetalleConvertTo.ListaDetalles(Execute(objCommand));
        }
    }
}