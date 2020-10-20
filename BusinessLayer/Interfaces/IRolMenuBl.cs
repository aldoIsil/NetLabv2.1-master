using System.Collections.Generic;
using Model;

namespace BusinessLayer.Interfaces
{
    public interface IRolMenuBl
    {
        List<Menu> GetMenuByRolId(int idRol);
        void AgregarMenuPorRol(Rol rol, int[] menues, int idUsuario);
        void UpdateMenuByRol(RolMenu rolMenu);
    }
}