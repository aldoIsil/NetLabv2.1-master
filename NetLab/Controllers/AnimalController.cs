using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.WebPages;
using BusinessLayer;
using BusinessLayer.Compartido;
using BusinessLayer.Compartido.Enums;
using BusinessLayer.Compartido.Interfaces;
using BusinessLayer.Interfaces;
using BusinessLayer.Animal;
using Model;
using NetLab.Controllers.FormConverter;
using NetLab.Controllers.FormConverter.Entities;
using NetLab.Controllers.FormConverter.Interfaces;
using NetLab.Models.Shared;
using NetLab.Models.Animal;
using NetLab.Helpers;
using PagedList;
using Ubigeo = BusinessLayer.Compartido.Enums.Ubigeo;

namespace NetLab.Controllers
{
    public class AnimalController : ParentController
    {
        private readonly IAnimalConverter _animalConverter;
        private readonly IAnimalBl _animalBl;
        private readonly IListaBl _listaBl;

        public AnimalController(IAnimalConverter animalCollectionConverter,
            IAnimalBl animalBl,
            IListaBl listaBl)
        {
            _animalConverter = animalCollectionConverter;
            _animalBl = animalBl;
            _listaBl = listaBl;
        }

        public AnimalController() : this(new AnimalConverter(), new AnimalBl(), new ListaBl())
        {
        }

        /// <summary>
        /// Descripción: Muestra el formulario de búsqueda de Animales, también realiza la acción de búsqueda
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <param name="page">página actual del resultado</param>
        /// <param name="search">término de búsqueda</param>
        /// <returns></returns>        
        [HttpGet]
        public ActionResult Index(int? page, string search)
        {
            var pageNumber = page ?? 1;

            if (search == null) return View();

            ViewBag.search = search;

            if (search.IsEmpty())
                return View();

            var animales = _animalBl.GetAnimales();

            var filtered = animales.Where(
                a => a.Nombre.Contains(search, StringComparison.OrdinalIgnoreCase) || 
                a.Propietario.ApellidoMaterno.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                a.Propietario.ApellidoPaterno.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                a.Propietario.Nombres.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                a.Refugio.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                a.Codificacion.ToString().Equals(search)
            );

            var pageOfAnimales = filtered.ToPagedList(pageNumber, GetSetting<int>(PageSize));

            return View(pageOfAnimales);
        }

        /// <summary>
        /// Descripción: Muestra el formulario para agregar un nuevo Animal.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Agregar()
        {
            var especieViewModels = GetEspecieViewModels();
            var razaViewModels = GetRazaViewModelsByEspecie(especieViewModels.Data.First().IdEspecie);
            var sexoViewModels = GetSexoViewModels();
            var origenViewModels = GetOrigenViewModels();
            var generoViewModels = GetGeneroViewModels();
            var tipoDocumentoViewModels = GetTipoDocumentoViewModels();

            var model = new AnimalViewModels
            {
                Especie = especieViewModels,
                Raza = razaViewModels,
                Sexo = sexoViewModels,
                Origen = origenViewModels,
                Genero = generoViewModels,
                TipoDocumento = tipoDocumentoViewModels
            };

            return View(model);
        }

        /// <summary>
        /// Descripción: Muestra el formulario para editar un Animal existente.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <param name="idAnimal">Identificador del animal a editar</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Editar(Guid idAnimal)
        {
            var animal = _animalBl.GetAnimalById(idAnimal);

            if (animal == null) return View("Error");

            var idUbigeoAnimal = animal.IdUbigeo;
            var idUbigeoPropietario = animal.Propietario?.UbigeoActual?.Id;

            var model = new AnimalViewModels
            {
                Animal = animal,
                Sexo = GetSexoViewModels(animal.Genero),
                Origen = GetOrigenViewModels(animal.Origen),
                Especie = GetEspecieViewModels(animal.Raza.Especie.IdEspecie),
                Raza = GetRazaViewModelsByEspecie(animal.Raza.Especie.IdEspecie, animal.Raza.IdRaza),
                IdDepartamento = idUbigeoAnimal.ExtractUbigeo(Ubigeo.Departamento),
                IdProvincia = idUbigeoAnimal.ExtractUbigeo(Ubigeo.Provincia),
                IdDistrito = idUbigeoAnimal.ExtractUbigeo(Ubigeo.Distrito),
                DepartamentoProp = idUbigeoPropietario.ExtractUbigeo(Ubigeo.Departamento),
                ProvinciaProp = idUbigeoPropietario.ExtractUbigeo(Ubigeo.Provincia),
                DistritoProp = idUbigeoPropietario.ExtractUbigeo(Ubigeo.Distrito),
                TipoDocumento = GetTipoDocumentoViewModels(animal.Propietario),
                Genero = GetGeneroViewModels(animal.Propietario)
            };

            return View(model);
        }

        /// <summary>
        /// Descripción: Obtiene la lista de Razas por especie, para animales.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <param name="codigoEspecie">Identificador de la especie</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetRazaPorEspecie(string codigoEspecie)
        {
            var razaViewModels = GetRazaViewModelsByEspecie(Convert.ToInt32(codigoEspecie));
            return PartialView("_Raza", razaViewModels);
        }

        /// <summary>
        /// Descripción: Devuelve los datos de una persona por documento de identidad, 
        ///              buscando primero en la base de datos del NetLab y si no lo encuentra 
        ///              en la base de datos de la RENIEC.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <param name="nroDocumento">El documento de identidad de la persona</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ValidarPersona(string nroDocumento)
        {
            if (string.IsNullOrEmpty(nroDocumento)) return new EmptyResult();

            var pacienteBl = new PacienteBl();
            var paciente = pacienteBl.GetPacienteByDocumento(nroDocumento);

            if (paciente != null)
                return Json(paciente, JsonRequestBehavior.AllowGet);

            var reniecConsumer = new ReniecConsumer();
            var persona = reniecConsumer.getReniec(nroDocumento);

            var reniecUpdater = new ReniecUpdater();
            paciente = reniecUpdater.GetPaciente(persona);

            return Json(paciente, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Descripción: Envía el formulario para agregar un nuevo Animal.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <param name="model">Los datos del animal que se van a agregar</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult NuevoAnimal(AnimalViewModels model)
        {
            try
            {
                var animalConverterRequest = new AnimalConverterRequest
                {
                    AnimalViewModels = model,
                    IdUsuarioLogueado = Logueado.idUsuario
                };

                var animal = _animalConverter.ConvertFrom(animalConverterRequest);

                _animalBl.InsertAnimal(animal);

                TempData["UserMessage"] = "El animal se registró correctamente.";

                return RedirectToAction("Index","OrdenAnimal", new { animal.IdAnimal });
            }
            catch
            {
                return View("Error");
            }
        }

        /// <summary>
        /// Descripción: Envía el formulario para editar los datos de un Animal existente.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <param name="model">Los datos del animal que se van a editar</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditarAnimal(AnimalViewModels model)
        {
            try
            {
                var animalConverterRequest = new AnimalConverterRequest
                {
                    AnimalViewModels = model,
                    IdUsuarioLogueado = Logueado.idUsuario
                };

                var animal = _animalConverter.ConvertFrom(animalConverterRequest);

                _animalBl.UpdateAnimal(animal);

                TempData["UserMessage"] = "El animal se actualizó correctamente.";

                return RedirectToAction("Index");
            }
            catch
            {
                return View("Error");
            }
        }
        /// <summary>
        /// Descripción: Controlador para obtener las especies.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <param name="idEspecie"></param>
        /// <returns></returns>
        private static EspecieViewModels GetEspecieViewModels(int? idEspecie = null)
        {
            var especieBl = new EspecieBl();
            var especies = especieBl.GetEspecies();
            
            return new EspecieViewModels
            {
                Data = especies,
                IdEspecie = idEspecie ?? 0
            };
        }
        /// <summary>
        /// Descripción: Envía el formulario para editar los datos de un Animal existente.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017.
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <param name="idEspecie"></param>
        /// <param name="idRaza"></param>
        /// <returns></returns>
        private static RazaViewModels GetRazaViewModelsByEspecie(int idEspecie, int? idRaza = null)
        {
            var razaBl = new RazaBl();
            var razas = razaBl.GetRazaByEspecie(idEspecie);

            return new RazaViewModels
            {
                Data = razas,
                IdRaza = idRaza ?? 0
            };
        }
        /// <summary>
        /// Descripción: Controlador para obtener la lista de Opciones de Sexo por animal.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <param name="idsexo"></param>
        /// <returns></returns>
        private SexoViewModels GetSexoViewModels(int? idsexo = null)
        {
            var generos = _listaBl.GetListaByOpcion(OpcionLista.GeneroAnimal);

            return new SexoViewModels
            {
                Data = generos.ListaDetalle,
                IdSexo = idsexo ?? 0
            };
        }
        /// <summary>
        /// Descripción: Controlador para obtener la lista de Opciones de Origenes por animal.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <param name="idOrigen"></param>
        /// <returns></returns>
        private OrigenViewModels GetOrigenViewModels(int? idOrigen = null)
        {
            var generos = _listaBl.GetListaByOpcion(OpcionLista.OrigenAnimal);

            return new OrigenViewModels
            {
                Data = generos.ListaDetalle,
                IdOrigen = idOrigen ?? 0
            };
        }
        /// <summary>
        /// Descripción: Controlador para obtener la lista de Opciones de Genero.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <param name="propietario"></param>
        /// <returns></returns>
        private GeneroViewModels GetGeneroViewModels(Paciente propietario = null)
        {
            var generos = _listaBl.GetListaByOpcion(OpcionLista.GeneroPersona);

            return new GeneroViewModels
            {
                Data = generos.ListaDetalle,
                IdGenero = propietario?.Genero ?? 0
            };
        }
        /// <summary>
        /// Descripción: Controlador para obtener la lista de Opciones de Tipo de Documento.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <param name="propietario"></param>
        /// <returns></returns>
        private TipoDocumentoViewModels GetTipoDocumentoViewModels(Paciente propietario = null)
        {
            var tipoDocumentos = _listaBl.GetListaByOpcion(OpcionLista.TipoDocumento);

            return new TipoDocumentoViewModels
            {
                Data = tipoDocumentos.ListaDetalle,
                IdTipoDocumento = propietario?.TipoDocumento ?? 0
            };
        }
    }
}
