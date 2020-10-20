using System.Collections.Generic;
using Model;

namespace BusinessLayer.Interfaces
{
    public interface IUsuarioRolBl
    {
        List<Rol> GetRolByUsuarioId(int idUsuario);
        void AgregarRolPorUsuario(Usuario usuario, int[] roles, int idUsuario);
        void UpdateRolByUsuario(UsuarioRol usuarioRol);
    }
}