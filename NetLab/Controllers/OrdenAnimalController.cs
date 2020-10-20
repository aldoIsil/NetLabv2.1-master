using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BusinessLayer;
using Model;
using NetLab.Models.Orden;
using NetLab.Models.Shared;
using BusinessLayer.Interfaces;
using BusinessLayer.Animal;

namespace NetLab.Controllers
{
    public class OrdenAnimalController : ParentController
    {
        private readonly IAnimalBl _animalBl;

        public OrdenAnimalController(IAnimalBl animalBl)
        {
            _animalBl = animalBl;
        }

        public OrdenAnimalController() : this(new AnimalBl())
        {
        }
        /// <summary>
        /// Descripción: Controlador para mostrar la carga de adatos y configuracion de controles.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, no se utiliza esta opcion.
        /// </summary>
        /// <param name="idAnimal"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index(Guid idAnimal)
        {
            var proyectoViewModels = GetProyectoViewModels();
            var animal = GetAnimalModel(idAnimal);

            var ordenExamenListAgregados = new List<OrdenExamen>();
            if (Session["ordenExamenListAgregados"] != null)
            {
                ordenExamenListAgregados = (List<OrdenExamen>)Session["ordenExamenListAgregados"];
            }
            
            var ordenMuestraListAgregados = new List<OrdenMuestra>();
            if (Session["ordenMuestraListAgregados"] != null)
            {
                ordenMuestraListAgregados = (List<OrdenMuestra>)Session["ordenMuestraListAgregados"];
            }


            var ordenMaterialListAgregados = new List<OrdenMaterial>();
            if (Session["ordenMaterialListAgregados"] != null)
            {
                ordenMaterialListAgregados = (List<OrdenMaterial>)Session["ordenMaterialListAgregados"];
            }

            var ordenAnimal = new OrdenAnimalViewModels
            {
                Orden = new Orden { idAnimal = idAnimal },
                Animal = animal,
                Proyecto = proyectoViewModels,
                Establecimiento = new EstablecimientoViewModels { Data = new List<Establecimiento>() },
                OrdenExamen = ordenExamenListAgregados,
                OrdenMuestra = ordenMuestraListAgregados,
                OrdenMaterial = ordenMaterialListAgregados
            };

            return View("Agregar", ordenAnimal);
        }
        /// <summary>
        /// Descripción: Controlador para el obtener informacion de un animal en especial
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, no se utiliza esta opcion.
        /// </summary>
        /// <param name="idAnimal"></param>
        /// <returns></returns>
        private Animal GetAnimalModel(Guid idAnimal)
        {
            return _animalBl.GetAnimalById(idAnimal);
        }
        /// <summary>
        /// Descripción: Controlador para obtener los proyectos(Modulos)
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, no se utiliza esta opcion.
        /// </summary>
        /// <returns></returns>
        private static ProyectoViewModels GetProyectoViewModels()
        {
            var proyectoBl = new ProyectoBl();
            var proyectos = proyectoBl.GetProyectos().OrderBy(p => p.Nombre).ToList();

            return new ProyectoViewModels
            {
                Data = proyectos
            };
        }
        /// <summary>
        /// Descripción: Controlador para el registro de una orden de tipo animal
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, no se utiliza esta opcion.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult NuevaOrden(OrdenAnimalViewModels model)
        {
            try
            {
                //var animal = _animalCollectionConverter.AnimalConverter(collection, this);

                //_animalBl.InsertAnimal(animal);

                TempData["UserMessage"] = "La orden se registró correctamente.";

                return RedirectToAction("Index", "Animal");
            }
            catch
            {
                return View("Error");
            }
        }
    }
}