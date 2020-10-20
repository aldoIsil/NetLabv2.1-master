using System.Collections.Generic;
using System;
using Model;

namespace BusinessLayer.Interfaces
{
    public interface IAnalitoBl
    {
        List<Analito> GetAnalitos();

        List<Analito> GetAnalitosByNombre(string nombre);

        Analito GetAnalitoById(Guid idAnalito);

        List<Analito> GetAnalitosByIdExamen(Guid idExamen);

        List<Analito> SearchAnalitos(string search);

        void RegistrarAnalito(Analito analito);

        void ActualizarAnalito(Analito analito);


        List<ExamenMetodo> GetMetodosByAnalito(Guid idAnalito);
        
        void RegistrarMetodo(ExamenMetodo metodo);

        void ActualizarMetodo(ExamenMetodo metodo);


        List<AnalitoOpcionResultado> GetOpcionesByAnalito(Guid idAnalito);

        AnalitoOpcionResultado GetOpcionById(Guid idAnalito, int idOpcion);

        void RegistrarOpcion(AnalitoOpcionResultado opcion);

        void ActualizarOpcion(AnalitoOpcionResultado opcion);


        List<AnalitoValorNormal> GetValoresByAnalito(Guid idAnalito);

        AnalitoValorNormal GetValorById(Guid idAnalito, int idValor);

        void RegistrarValor(AnalitoValorNormal opcion);

        void ActualizarValor(AnalitoValorNormal opcion);
    }
}
