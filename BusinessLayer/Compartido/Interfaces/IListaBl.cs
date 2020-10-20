using BusinessLayer.Compartido.Enums;
using Model;

namespace BusinessLayer.Compartido.Interfaces
{
    public interface IListaBl
    {
        Lista GetListaByOpcion(OpcionLista opcion);
    }
}