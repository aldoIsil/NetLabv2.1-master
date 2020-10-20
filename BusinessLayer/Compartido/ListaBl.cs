using BusinessLayer.Compartido.Enums;
using BusinessLayer.Compartido.Interfaces;
using DataLayer.Compartido;
using Model;

namespace BusinessLayer.Compartido
{
    public class ListaBl : IListaBl
    {
        /// <summary>
        /// Descripción: Obtiene informacion de las listas a mostrar .
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="opcion"></param>
        /// <returns></returns>
        public Lista GetListaByOpcion(OpcionLista opcion)
        {
            Lista lista;
            var id = (int) opcion;

            using (var listaDal = new ListaDal())
            {
                lista = listaDal.GetListaById(id);
            }

            using (var listaDetalleDal = new ListaDetalleDal())
            {
                lista.ListaDetalle = listaDetalleDal.GetListaDetalleById(id);
            }

            return lista;
        } 
    }
}