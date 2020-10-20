using System.Collections.Generic;
using Model;

namespace BusinessLayer.Interfaces
{
    public interface IMenuBl
    {
        List<Menu> GetMenuByRol(int idRol);
        List<Menu> GetMenues(string nombre);
        //Menu GetMenuById(int idMenu);
        //void InsertMenu(Menu menu);
        //void UpdateMenu(Menu menu);
    }
}