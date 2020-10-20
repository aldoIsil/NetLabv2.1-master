using Model;
using Reniec.Entities;

namespace BusinessLayer
{
    public interface IReniecUpdater
    {
        Paciente GetPaciente(Persona persona);
    }
}
