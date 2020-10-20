using System.Collections.Generic;
using Model;

namespace BusinessLayer.Interfaces
{
    public interface IProyectoBl
    {
        List<Proyecto> GetProyectos();
        List<Proyecto> GetProyectosBS();
    }
    
}