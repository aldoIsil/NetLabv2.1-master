using System;
using System.Collections.Generic;
namespace BusinessLayer.Interfaces
{
    public interface IAnimalBl
    {
        List<Model.Animal> GetAnimales();
        void InsertAnimal(Model.Animal animal);
        void UpdateAnimal(Model.Animal animal);
        Model.Animal GetAnimalById(Guid idAnimal);
    }
}