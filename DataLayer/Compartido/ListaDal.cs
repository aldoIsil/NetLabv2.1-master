using DataLayer.DalConverter;
using Framework.DAL;
using Framework.DAL.Settings.Implementations;
using Model;

namespace DataLayer.Compartido
{
    public class ListaDal : DaoBase
    {
        public ListaDal(IDalSettings settings) : base(settings)
        {
        }

        public ListaDal() : this(new NetlabSettings())
        {
        }
        /// <summary>
        /// Descripción: Obtiene informacion de las listas a mostrar .
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="opcion"></param>
        /// <returns></returns>
        public Lista GetListaById(int id)
        {
            var objCommand = GetSqlCommand("pNLS_ListaById");
            InputParameterAdd.Int(objCommand, "idLista", id);

            return ListaConvertTo.Lista(Execute(objCommand));
        }
    }
}