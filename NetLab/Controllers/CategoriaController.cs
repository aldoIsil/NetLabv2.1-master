using BusinessLayer.DatoClinico;
using System.Web.Mvc;
using BusinessLayer.Compartido;
using BusinessLayer.Compartido.Enums;
using BusinessLayer.Compartido.Interfaces;
using BusinessLayer.DatoClinico.Interfaces;
using Model;
using NetLab.Controllers.FormConverter;
using NetLab.Controllers.FormConverter.Entities;
using NetLab.Controllers.FormConverter.Interfaces;
using NetLab.Models.DatoClinico;
using NetLab.Models.Shared;

namespace NetLab.Controllers
{
    public class CategoriaController : ParentController
    {
        private readonly ICategoriaConverter _categoriaConverter;
        private readonly ICategoriaDatoBl _categoriaDatoBl;
        private readonly IListaBl _listaBl;

        public CategoriaController(ICategoriaConverter categoriaConverter, ICategoriaDatoBl categoriaDatoBl, IListaBl listaBl)
        {
            _categoriaConverter = categoriaConverter;
            _categoriaDatoBl = categoriaDatoBl;
            _listaBl = listaBl;
        }

        public CategoriaController() : this(new CategoriaConverter(), new CategoriaDatoBl(), new ListaBl())
        {
        }
        /// <summary>
        /// Descripción: Controlador para mostrar la pantalla de de busqueda y mantenimiento a las Categorias(Datos Variables)
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            var model = new CategoriaEditViewModels();

            return View(model);
        }
        /// <summary>
        /// Descripción: Controlador para mostrar la pantalla de edicion de datos.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit(CategoriaEditViewModels model)
        {
            return View("Index", model);
        }
        /// <summary>
        /// Descripción: Controlador para mostrar informacion de las categorias.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idEnfermedad"></param>
        /// <param name="nombreEnfermedad"></param>
        /// <param name="idCategoriaPadre"></param>
        /// <param name="nombrePadre"></param>
        /// <param name="fromUrl"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetCategorias(string idEnfermedad, string nombreEnfermedad, int? idCategoriaPadre, string nombrePadre, string fromUrl)
        {
            var categorias = _categoriaDatoBl.GetCategoriaByEnfermedad(idEnfermedad, idCategoriaPadre);

            var model = new CategoriaListaViewModels
            {
                IdEnfermedad = idEnfermedad,
                NombreEnfermedad = nombreEnfermedad,
                IdCategoriaPadre = idCategoriaPadre,
                NombrePadre = nombrePadre,
                Categoria = categorias,
                FromUrl = fromUrl
            };

            return PartialView("_Categorias", model);
        }
        /// <summary>
        /// Descripción: Redirecciona a la pagina principal y muestra las categorias.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="fromUrl"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetCategoriaActual(string fromUrl)
        {
            return Redirect(fromUrl);
        }
        /// <summary>
        /// Descripción: Controlador para mostrar la pantalla de ingreso de datos.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idEnfermedad"></param>
        /// <param name="idCategoriaPadre"></param>
        /// <param name="nombrePadre"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult NuevaCategoria(string idEnfermedad, int? idCategoriaPadre, string nombrePadre)
        {
            var categoriaDato = new CategoriaDato
            {
                Visible = true,
                IdGenero = 3
            };

            var model = new CategoriaViewModels
            {
                IdEnfermedad = idEnfermedad,
                IdCategoriaPadre = idCategoriaPadre,
                NombrePadre = nombrePadre,
                Clase = GetClaseGeneroViewModels(categoriaDato),
                Categoria = categoriaDato
            };

            return PartialView("_NuevaCategoria", model);
        }
        /// <summary>
        /// Descripción: Controlador para mostrar las Categprisd a esitar.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="idEnfermedad"></param>
        /// <param name="idCategoriaPadre"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditarCategoria(int id, string idEnfermedad, int? idCategoriaPadre)
        {
            var categoria = _categoriaDatoBl.GetCategoriaById(id);

            if (categoria == null) return View("Error");

            categoria.IdGenero = categoria.IdGenero ?? 3;

            var model = new CategoriaViewModels
            {
                Categoria = categoria,
                IdEnfermedad = idEnfermedad,
                IdCategoriaPadre = idCategoriaPadre,
                Clase = GetClaseGeneroViewModels(categoria)
            };

            return PartialView("_EditarCategoria", model);
        }
        /// <summary>
        /// Descripción: Controlador para realiar un delete  soft de una categoria.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.

        /// </summary>
        /// <param name="id"></param>
        /// <param name="idEnfermedad"></param>
        /// <param name="fromUrl"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EliminarCategoria(int id, string idEnfermedad, string fromUrl)
        {
            try
            {
                var categoria = _categoriaDatoBl.GetCategoriaById(id);
                categoria.IdUsuarioEdicion = Logueado.idUsuario;
                categoria.Estado = 0;

                var enfermedadCategoria = _categoriaDatoBl.GetEnfermedadCategoriaById(id, idEnfermedad);
                enfermedadCategoria.IdUsuarioEdicion = Logueado.idUsuario;
                enfermedadCategoria.Estado = 0;

                _categoriaDatoBl.UpdateCategoria(categoria, enfermedadCategoria);

                var nombrePadre = GetNombrePadre(categoria);

                return RedirectToAction("GetCategorias", new {idEnfermedad, idCategoriaPadre = categoria.IdCategoriaDatoPadre, nombrePadre, fromUrl });
            }
            catch
            {
                return View("Error");
            }
        }
        /// <summary>
        /// Descripción: Controlador para realizar el registro de ua categoría.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult NuevaCategoria(CategoriaViewModels model)
        {
            try
            {
                var categoria = GetObjectRequest(model);

                _categoriaDatoBl.InsertCategoria(categoria, model.IdEnfermedad);

                TempData["UserMessage"] = "La categoría se registró correctamente.";

                return RedirectToAction("GetCategorias", new { idEnfermedad = model.IdEnfermedad, idCategoriaPadre = model.IdCategoriaPadre, nombrePadre = model.NombrePadre });
            }
            catch 
            {
                return View("Error");
            }
        }
        /// <summary>
        /// Descripción: Controlador para la ejecutar la edicion de datos de la categoria
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditarCategoria(CategoriaViewModels model)
        {
            try
            {
                var categoria = GetObjectRequest(model);

                _categoriaDatoBl.UpdateCategoria(categoria);

                TempData["UserMessage"] = "La categoría se actualizó correctamente.";

                return RedirectToAction("GetCategorias", new { idEnfermedad = model.IdEnfermedad, idCategoriaPadre = model.IdCategoriaPadre, nombrePadre = model.NombrePadre });
            }
            catch
            {
                return View("Error");
            }
        }
        /// <summary>
        /// Descripción: Controlador para la presentacion del sistema.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private CategoriaDato GetObjectRequest(CategoriaViewModels model)
        {
            var categoriaConverterRequest = new CategoriaConverterRequest
            {
                CategoriaViewModels = model,
                IdUsuarioLogueado = Logueado.idUsuario
            };

            var categoria = _categoriaConverter.ConvertFrom(categoriaConverterRequest);
            return categoria;
        }
        /// <summary>
        /// Descripción: Metodo que devuelve la descripcion de la Categoria Padre.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="categoria"></param>
        /// <returns></returns>
        private string GetNombrePadre(CategoriaDato categoria)
        {
            if (!categoria.IdCategoriaDatoPadre.HasValue) return null;

            var categoriaPadre = _categoriaDatoBl.GetCategoriaById(categoria.IdCategoriaDatoPadre.Value);

            return categoriaPadre?.Nombre;
        }
        /// <summary>
        /// Descripción: Metodo que devuelve la entidad claseGeneroViewModels con informacion
        /// de las clases.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="categoriaDato"></param>
        /// <returns></returns>
        private ClaseGeneroViewModels GetClaseGeneroViewModels(CategoriaDato categoriaDato = null)
        {
            var clases = _listaBl.GetListaByOpcion(OpcionLista.ClaseGenero);

            return new ClaseGeneroViewModels
            {
                Data = clases.ListaDetalle,
                IdClase = categoriaDato?.IdGenero ?? 0
            };
        }
    }
}