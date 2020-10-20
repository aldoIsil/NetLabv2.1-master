using System.Collections.Generic;
using Model;

namespace BusinessLayer.Interfaces
{
    public interface IEnfermedadBl
    {
        List<Enfermedad> GetEnfermedadesByNombre(string nombre);
        List<Enfermedad> GetEnfermedadesByNombre(string nombre, int idLaboratorio, int idUsuario);

        List<Enfermedad> GetEnfermedadesByNombre(string nombre, string idOrden);
    }
}
