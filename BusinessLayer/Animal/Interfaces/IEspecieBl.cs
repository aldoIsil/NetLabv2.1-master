using System.Collections.Generic;
using Model;

namespace BusinessLayer.Animal.Interfaces
{
    public interface IEspecieBl
    {
        List<AnimalEspecie> GetEspecies();
    }
}