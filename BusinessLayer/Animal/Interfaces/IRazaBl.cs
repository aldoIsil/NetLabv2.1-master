using System.Collections.Generic;
using Model;

namespace BusinessLayer.Animal.Interfaces
{
    public interface IRazaBl
    {
        List<AnimalRaza> GetRazaByEspecie(int idEspecie);
    }
}