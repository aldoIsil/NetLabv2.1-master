using System.Collections.Generic;
using Model;

namespace BusinessLayer.DatoClinico.Interfaces
{
    public interface IDatoBl
    {
        List<Dato> GetDatos();
        List<Dato> GetDatosByIdEnfermedad(string enfermedad);
        List<Dato> GetDatoByCategoria(int idCategoria);
        List<TipoDato> GetTipos();
        void InsertDato(Dato dato, int idCategoria);
        Dato GetDatoById(int id);
        DatoCategoriaDato GetDatoCategoriaById(int id, int idCategoria);
        void UpdateDato(Dato dato, DatoCategoriaDato datoCategoria);
        List<ListaDato> GetListaDato();
        void InsertListaDato(ListaDato listaDato);
        ListaDato GetListaDatoById(int id);
        void UpdateListaDato(ListaDato listaDato);
        List<OpcionDato> GetOpcionDatoByLista(int id);
        void UpdateOpcionDato(OpcionDato opcionDato);
        void InsertOpcionDato(OpcionDato opcionDato);
        OpcionDato GetOpcionDatoById(int id);
    }
    
}
