using Reniec.Entities;

namespace Reniec.Interfaces
{
    public interface IReniecConverter
    {
        Persona Convert(string[] personaArray);
        Persona ConvertNew(string[] personaArray);
    }
}
