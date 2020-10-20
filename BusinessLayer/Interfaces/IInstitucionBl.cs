using System.Collections.Generic;
using Model;
using System;

namespace BusinessLayer.Interfaces
{
    public interface IInstitucionBl
    {
        List<Institucion> GetInstitucionByTextoBusqueda(string textoBusqueda);
        List<Institucion> GetInstituciones();
    }
   
}