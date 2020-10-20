using System.Collections.Generic;
using Model;

namespace BusinessLayer.Interfaces
{
    public interface IEstablecimientoBl
    {
        List<Establecimiento> GetEstablecimientosByTextoBusqueda(string textoBusqueda, int idUsuario);//, List<Establecimiento> listaEstablecimientos);
        List<Establecimiento> GetEstablecimientosByNombre(string nombre);
        Establecimiento GetEstablecimientoById(int id);
        List<Establecimiento> GetEstablecimientosFrecuentesByIdUsuario(int idUsuario);
        List<Establecimiento> GetDisaByInstitucionByTextoBusqueda(string textoBusqueda, int idInstitucion);
        List<Establecimiento> GetDisaByInstitucionLabByTextoBusqueda(string textoBusqueda, int idInstitucion);
        List<Establecimiento> GetDisaRegionByTextoBusqueda(string textoBusqueda, int idUsuario);
        List<Establecimiento> GetDisaRegionLabByTextoBusqueda(string textoBusqueda, int idUsuario);
        List<Establecimiento> GetRedByDisaByTextoBusqueda(string textoBusqueda, int idDisa, int idInstitucion);
        List<Establecimiento> GetMicroredByRedByTextoBusqueda(string textoBusqueda, int idDisa, int idInstitucion, int idRed);
        List<Establecimiento> GetEstablecimientoByRedByTextoBusqueda(string textoBusqueda, int idDisa, int idInstitucion, int idRed, int idMicrored);
        List<Establecimiento> GetEstablecimientosAllByTextoBusqueda(string textoBusqueda);
        List<UsuarioEstablecimiento> GetUsuarioEstablecimientoByUser(int idUsuario);
        void EliminarUsuarioEstablecimiento(int idUsuarioEdicion, int idUsuario);
        void EliminarEstablecimientos(int idUsuarioEdicion, int idUsuario, int[] establecimientos);
        void InsertarUsuarioEstablecimientoByMicroRed(int idUsuarioRegistro, int idUsuario, int idInstitucion, string idDisa, string idRed, string idMicroRed);
        void InsertarUsuarioEstablecimientoByRed(int idUsuarioRegistro, int idUsuario, int idInstitucion, string idDisa, string idRed);
        void InsertarUsuarioEstablecimientoByDisa(int idUsuarioRegistro, int idUsuario, int idInstitucion, string idDisa);
        void InsertarUsuarioEstablecimientoByInstitucion(int idUsuarioRegistro, int idUsuario, int idInstitucion);
    }
}
