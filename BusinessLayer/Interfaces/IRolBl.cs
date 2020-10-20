using System.Collections.Generic;
using Model;

namespace BusinessLayer.Interfaces
{
    public interface IRolBl
    {
        List<Rol> GetRoles(string nombre);
        List<Rol> GetRolesByUsuario(int idUsuario);
        void InsertRol(Rol rol);
        Rol GetRolById(int idRol);
        void UpdateRol(Rol rol);
    }
}