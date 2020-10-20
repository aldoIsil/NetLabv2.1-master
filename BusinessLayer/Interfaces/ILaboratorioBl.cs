using System.Collections.Generic;
using Model;
using System;

namespace BusinessLayer.Interfaces
{
    public interface ILaboratorioBl
    {
        List<LaboratorioVMSelect> GetLaboratoriosByTextoBusqueda(string textoBusqueda, int idUsuario);
        List<LaboratorioVMSelect> GetLaboratoriosByTextoBusqueda(string textoBusqueda, int idUsuario, Guid idExamen);
        List<Laboratorio> GetLaboratoriosByNombre(string nombre);
        Laboratorio GetLaboratorioById(int id);
        List<Laboratorio> GetLaboratoriosAllByTextoBusqueda(string textoBusqueda);
        void UpdateLaboratorio(Laboratorio laboratorio);

        void InsertLaboratorio(Laboratorio laboratorio);

        List<Laboratorio> GetLaboratorioByMicroredByTextoBusqueda(string textoBusqueda, int idDisa, int idInstitucion, int idRed, int idMicrored);
        Laboratorio GetLaboratorioByUserId(int idUsuario);
        List<Laboratorio> GetListaLaboratorioByUserId(int idUsuario);
        List<UsuarioLaboratorio> GetUsuarioLaboratorioByUser(int idUsuario);

        List<LaboratorioVMSelect> GetLaboratoriossStaticCache();
    }
}