using System;
using System.Collections.Generic;
using BusinessLayer.Interfaces;
using DataLayer.Area.Animal;

namespace BusinessLayer.Animal
{
    public class AnimalBl : IAnimalBl
    {
        /// <summary>
        /// Descripción: Obtiene informacion de muestras de animales.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <returns></returns>
        public List<Model.Animal> GetAnimales()
        {
            using (var animalDal = new AnimalDal())
            {
                return animalDal.GetAnimales();
            }
        }
        /// <summary>
        /// Descripción: Registra informacion de muestras de animales.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <param name="animal"></param>
        public void InsertAnimal(Model.Animal animal)
        {
            using (var animalDal = new AnimalDal())
            {
                animalDal.InsertAnimal(animal);
            }
        }
        /// <summary>
        /// Descripción: Actualiza informacion de muestras de animales.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <param name="animal"></param>
        public void UpdateAnimal(Model.Animal animal)
        {
            using (var animalDal = new AnimalDal())
            {
                animalDal.UpdateAnimal(animal);
            }
        }
        /// <summary>
        /// Descripción: Obtiene informacion de muestras de animales por Id
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <param name="idAnimal"></param>
        /// <returns></returns>
        public Model.Animal GetAnimalById(Guid idAnimal)
        {
            using (var animalDal = new AnimalDal())
            {
                return animalDal.GetAnimalById(idAnimal);
            }
        }
    }
}