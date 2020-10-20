using System.Collections.Generic;
using Model;

namespace BusinessLayer.Interfaces
{
    public interface IUsuarioBl
    {
        List<Usuario> GetUsuarios(string login, string apellidosMaterno, string apellidosPaterno, string nombres, string dni, int region);
        void InsertUsuario(Usuario usuario);
        void UpdateUsuario(Usuario usuario);
        int ExisteLogin(string login, int accion);
        bool ExisteDni(string dni);
        Usuario GetUsuarioById(int idUsuario);
        int GetTotalCountEstablecimientoByUsuario(int idUsuario);
        IEnumerable<UsuarioEstablecimiento> GetEstablecimientoByUsuario(int idUsuario, int startIndex, int pageSize);
        void InsertUsuarioEstablecimiento(UsuarioEstablecimiento usuarioEstablecimiento);
        string ResetearClave(int id);
    }
}
