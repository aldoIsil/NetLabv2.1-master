using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;
using BusinessLayer;
using Model;
using PagedList;
using BusinessLayer.Compartido;
using System.Linq;
using System.Web.WebPages;
using BusinessLayer.Compartido.Enums;
using Ubigeo = Model.Ubigeo;
using DataLayer;
using System.Configuration;

namespace NetLab.Controllers
{
    public class PacienteController : ParentController
    {
        /// <summary>
        /// Descripción: Controlador para la busqueda de pacientes.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.   
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="tipoDocumento"></param>
        /// <param name="nroDocumento"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SearchRecepcion(int? page, int? tipoDocumento, string nroDocumento, string apellidoPaterno, string apellidoMaterno, string mensajeregistro = "")
        {
            ViewBag.RegistroOrdenRecepcion = 1;
            var actionResult = Busqueda(page, tipoDocumento, nroDocumento, apellidoPaterno, apellidoMaterno);

            if (TempData["shortMessage"] != null)
            {
                ViewBag.textoRegistro = TempData["shortMessage"].ToString();
            }

            if (!string.IsNullOrWhiteSpace(mensajeregistro)) ViewBag.textoRegistro = mensajeregistro;

            //PRUEBAS - Mostrar nueva creacion orden
            var prod = GetSetting<bool>("useConnectionProd");
            var permitirTodos = GetSetting<bool>("permitirTodos");
            var ids = prod ? GetSetting<string>("usuariosProd") : GetSetting<string>("usuariosDev");
            //List<int> crearNuevaOrden = new List<int>(new int[] { 72, 190, 1143 });
            List<int> crearNuevaOrden = ids.Split(',').Select(Int32.Parse).ToList();
            ViewBag.CrearNuevaOrden = permitirTodos || (!permitirTodos && crearNuevaOrden.Any(x => x == Logueado.idUsuario));

            return actionResult;
        }
        /// <summary>
        /// Descripción: Controlador para la presentacion de la busqueda de pacientes.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="tipoDocumento"></param>
        /// <param name="nroDocumento"></param> 
        /// <param name="tipoDocumento"></param>
        /// <param name="nroDocumento"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index(int? page, int? tipoDocumento, string nroDocumento, string apellidoPaterno, string apellidoMaterno)
        {
            if (Session["RegistroOrdenRecepcion"] != null)
            {
                ViewBag.RegistroOrdenRecepcion = Session["RegistroOrdenRecepcion"];
            }
            else
            {
                ViewBag.RegistroOrdenRecepcion = 0;
            }

            //PRUEBAS - Mostrar nueva creacion orden
            var prod = GetSetting<bool>("useConnectionProd");
            var permitirTodos = GetSetting<bool>("permitirTodos");
            var ids = prod ? GetSetting<string>("usuariosProd") : GetSetting<string>("usuariosDev");
            //List<int> crearNuevaOrden = new List<int>(new int[] { 72, 190, 1143 });
            List<int> crearNuevaOrden = ids.Split(',').Select(Int32.Parse).ToList();
            ViewBag.CrearNuevaOrden = permitirTodos || (!permitirTodos && crearNuevaOrden.Any(x => x == Logueado.idUsuario));

            Session.Remove("RegistroOrdenRecepcion");
            var actionResult = Busqueda(page, tipoDocumento, nroDocumento, apellidoPaterno, apellidoMaterno);

            if (TempData["shortMessage"] != null)
            {
                ViewBag.textoRegistro = TempData["shortMessage"].ToString();
            }
            return actionResult;
        }
        /// <summary>
        /// Descripción: Controlador para la presentacion de la busqueda del historial clinico de un paciente.
        /// Author: Juan Muga.
        /// Fecha Creacion: 11/10/2017
        /// Fecha Modificación:
        /// Modificación: 
        /// </summary>
        /// <param name="NroDocumento"></param>
        /// <returns></returns>
        public ActionResult ShowHistoriaClinicaPaciente(int? page, string NroDocumento, int? tipo)
        {
            PacienteBl pacienteBL = new PacienteBl();
            List<Paciente> pacienteList = new List<Paciente>();
            pacienteList = pacienteBL.GetDatosPacienteByNroDocumento(NroDocumento,Logueado.idUsuario);
            var pageNumber = page ?? 1;
            var tipoPage = (tipo != 1) ? 0 : tipo;
            const int numRegPorPagima = 1000;
            var pageOfPacientes = pacienteList.ToPagedList(pageNumber, numRegPorPagima);
            ViewBag.notFound = "No se encontraron resultados";
            ViewBag.NroDocumento = NroDocumento;
            ViewBag.tipo = tipoPage;
            return PartialView("_Busqueda", pageOfPacientes);
        }
        /// <summary>
        /// Descripción: Controlador para la realizacion de la busqueda de los
        /// pacientes en la bd local y por medio del servicio web de la reniec.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="tipoDocumento"></param>
        /// <param name="nroDocumento"></param>
        /// <returns></returns>
        private ActionResult Busqueda(int? page, int? tipoDocumento, string nroDocumento, string apellidoPaterno, string apellidoMaterno)
        {
            var listaBl = new ListaBl();
            var tipoDocumentoList = new List<SelectListItem>();

            foreach (var itemDetalle in listaBl.GetListaByOpcion(OpcionLista.TipoDocumento).ListaDetalle)
                tipoDocumentoList.Add(new SelectListItem
                {
                    Text = itemDetalle.Glosa,
                    Value = Convert.ToString(itemDetalle.IdDetalleLista)
                });

            ViewBag.tipoDocumentoList = tipoDocumentoList;
            ViewBag.nroDocumento = nroDocumento;

            //if (nroDocumento.IsEmpty()) return View("Index");

            var pageNumber = page ?? 1;
            var tipoDocumentoCriteria = tipoDocumento ?? -1;
            var nroDocumentoCriteria = nroDocumento ?? string.Empty;
            //Juan Muga
            var apePaternoCriteria = apellidoPaterno;
            var apeMaternoCriteria = apellidoMaterno;
            //Fin
            const int numRegPorPagima = 10;
            bool bReniec = false;

            var pacienteBl = new PacienteBl();
            var paciente = new Paciente();
            var pacienteDal = new PacienteDal();
            Boolean reniec = pacienteDal.EstadoReniec();
            paciente.EstadoReniec = reniec;

            //Juan Muga
            //var pacienteList = pacienteBl.getPacientes(pageNumber, numRegPorPagima, tipoDocumentoCriteria, nroDocumentoCriteria);
            var pacienteList = pacienteBl.getPacientes(pageNumber, numRegPorPagima, tipoDocumentoCriteria, nroDocumentoCriteria, apePaternoCriteria, apeMaternoCriteria);
            //Fin

            var pageOfPacientes = pacienteList.ToPagedList(pageNumber, numRegPorPagima);

            //if (pacienteList.Count == 0 && !nroDocumentoCriteria.Equals(string.Empty) )
            if (pacienteList.Count == 0)
            {
                if (reniec)
                {
                    var seguroList = new List<SelectListItem>();

                    foreach (var itemDetalle in listaBl.GetListaByOpcion(OpcionLista.TipoSeguroSalud).ListaDetalle)
                        seguroList.Add(new SelectListItem
                        {
                            Text = itemDetalle.Glosa,
                            Value = Convert.ToString(itemDetalle.IdDetalleLista)
                        });

                    ViewBag.seguroList = seguroList;

                    //Si no se obtienen pacientes se busca en Reniec siempre y cuando sea DNI
                    if (tipoDocumentoCriteria == 1 && nroDocumentoCriteria.Trim().Length == 8)
                    {
                        paciente.NroDocumento = nroDocumentoCriteria;
                        paciente.IdUsuarioRegistro = Logueado.idUsuario;
                        paciente = pacienteBl.getReniec(paciente);

                        if (paciente != null)
                        {
                            //Si paciente es distinto de null entonces se encontro en RENIEC
                            //Si se encontro en RENIEC le ponemos estatus 1 y no se pueden editar los datos de RENIEC
                            ViewBag.TipoDocumento = listaBl.GetListaByOpcion(OpcionLista.TipoDocumento).ListaDetalle.Where(x => x.IdDetalleLista == tipoDocumentoCriteria).First().Glosa;
                            ViewBag.Genero = listaBl.GetListaByOpcion(OpcionLista.GeneroPersona).ListaDetalle.Where(x => x.IdDetalleLista == paciente.Genero).First().Glosa;

                            paciente.UbigeoActual = new Ubigeo { Id = "      " };
                            paciente.estatus = 1;
                            paciente.TipoDocumento = Convert.ToInt32(tipoDocumentoCriteria);
                            paciente.Estado = 0;
                            bReniec = true;
                            return View("New", paciente);
                        }
                    }
                }   
            }

            //Verifica si no es validado con reniec                
            if (!bReniec)
            {
                //paciente.estatus = 0;
                //paciente.Estado = 1;
            }
            pacienteList.ToPagedList(pageNumber, numRegPorPagima);
            ViewBag.tipoDocumento = tipoDocumento;
            ViewBag.nroDocumento = nroDocumento;
            ViewBag.notFound = "";

            //if (pacienteList.Count != 0 || nroDocumentoCriteria.IsEmpty()) return View("Index", pageOfPacientes);
            if (pacienteList.Count != 0) return View("Index", pageOfPacientes);

            ViewBag.notFound = "No se encontraron resultados";

            if (tipoDocumento == 1)
                ViewBag.notFound =
                    "No se encontraron resultados y el servicio de RENIEC no responde en estos momentos, por favor revise el número de documento, si es válido puede crear un nuevo Paciente haciendo clic en el botón Nuevo.";

            ViewBag.reniec = reniec;
            return View("Index", pageOfPacientes);
        }

        /// <summary>
        /// Descripción: Controlador para validar los datos del paciente con el web service de la reniec.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="fechaDesde"></param>
        /// <param name="fechaHasta"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ListaValidar(int? page, string fechaDesde, string fechaHasta)
        {
            ListaBl listaBl = new ListaBl();
            //Obtiene lista de documentos
            //Inicio
            //Se comentó codigo no necesario
            //List<SelectListItem> tipoDocumentoList = new List<SelectListItem>();
            //foreach (ListaDetalle itemDetalle in listaBl.GetListaByOpcion(BusinessLayer.Compartido.Enums.OpcionLista.TipoDocumento).ListaDetalle)
            //    tipoDocumentoList.Add(new SelectListItem { Text = itemDetalle.Glosa, Value = Convert.ToString(itemDetalle.IdDetalleLista) });
            //ViewBag.tipoDocumentoList = tipoDocumentoList;
            //Fin

            var pageNumber = page ?? 1;

            DateTime dateFrom = DateTime.Now.AddDays(-1);
            DateTime dateTo = DateTime.Now;

            if (fechaDesde == null || fechaHasta == null || fechaDesde.Equals("") || fechaHasta.Equals(""))
            {
                fechaDesde = dateFrom.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                fechaHasta = dateTo.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            }

            dateFrom = DateTime.ParseExact(fechaDesde, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            dateTo = DateTime.ParseExact(fechaHasta, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            var numRegPorPagima = 10;

            var pacienteBl = new PacienteBl();

            var pacienteList = new List<Paciente>();
            pacienteList = pacienteBl.getPacientesSinValidar(pageNumber, numRegPorPagima, dateFrom, dateTo);

            var pageOfExamenes = pacienteList.ToPagedList(pageNumber, GetSetting<int>(PageSize));
            ViewBag.fechaDesde = fechaDesde;
            ViewBag.fechaHasta = fechaHasta;
            ViewBag.RegistroOrdenRecepcion = 0;
            ViewBag.SourceIsListaValidar = 1;
            return View(pageOfExamenes);
        }

        /// <summary>
        /// Descripción: Controlador para validar los input de cada campo por base de datos en el mantenedor del paciente.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idPaciente"></param>
        /// <param name="NroDocumento"></param>
        /// <returns></returns>
        public ActionResult Validar(Guid idPaciente, string NroDocumento)
        {
            Paciente paciente = new Paciente();
            paciente.IdPaciente = idPaciente;
            paciente.NroDocumento = NroDocumento;
            paciente.IdUsuarioEdicion = ((Usuario)Session["UsuarioLogin"]).idUsuario;
            PacienteBl pacienteBl = new PacienteBl();
            paciente = pacienteBl.ValidarDatosPaciente(paciente);
            return PartialView("Validar", paciente);
        }

        /// <summary>
        /// Descripción: Controlador para incializar los input de cada campo en el mantenedor del paciente.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="RegistroOrdenRecepcion"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult New(int RegistroOrdenRecepcion,Boolean reniec)
        {
            ListaBl listaBl = new ListaBl();
            //Obtiene lista de documentos
            List<SelectListItem> tipoDocumentoList = new List<SelectListItem>();
            foreach (ListaDetalle itemDetalle in listaBl.GetListaByOpcion(BusinessLayer.Compartido.Enums.OpcionLista.TipoDocumento).ListaDetalle)
                tipoDocumentoList.Add(new SelectListItem { Text = itemDetalle.Glosa, Value = Convert.ToString(itemDetalle.IdDetalleLista) });
            ViewBag.tipoDocumentoList = tipoDocumentoList;

            List<SelectListItem> generoList = new List<SelectListItem>();
            foreach (ListaDetalle itemDetalle in listaBl.GetListaByOpcion(BusinessLayer.Compartido.Enums.OpcionLista.GeneroPersona).ListaDetalle)
                generoList.Add(new SelectListItem { Text = itemDetalle.Glosa, Value = Convert.ToString(itemDetalle.IdDetalleLista) });
            ViewBag.generoList = generoList;

            List<SelectListItem> seguroList = new List<SelectListItem>();
            foreach (ListaDetalle itemDetalle in listaBl.GetListaByOpcion(BusinessLayer.Compartido.Enums.OpcionLista.TipoSeguroSalud).ListaDetalle)
                seguroList.Add(new SelectListItem { Text = itemDetalle.Glosa, Value = Convert.ToString(itemDetalle.IdDetalleLista) });
            ViewBag.seguroList = seguroList;

            Paciente paciente = new Paciente();
            paciente.UbigeoActual = new Ubigeo();
            paciente.UbigeoActual.Id = "      ";
            paciente.tipoSeguroSalud = null;
            paciente.EstadoReniec = reniec;
            ViewBag.RegistroOrdenRecepcion = RegistroOrdenRecepcion;

            ViewBag.notFound = "";
            return View(paciente);
        }

        /// <summary>
        /// Descripción: Controlador para cargar los input de cada campo y editar la informacion del paciente.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idPaciente"></param>
        /// <param name="RegistroOrdenRecepcion"></param>
        /// <param name="SourceIsListaValidar"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit(String idPaciente, int RegistroOrdenRecepcion, int SourceIsListaValidar = 0)
        {
            ViewBag.RegistroOrdenRecepcion = RegistroOrdenRecepcion;

            PacienteBl pacienteBl = new PacienteBl();
            Paciente paciente = pacienteBl.getPacienteById(Guid.Parse(idPaciente));

            ListaBl listaBl = new ListaBl();
            //Obtiene lista de documentos
            List<SelectListItem> tipoDocumentoList = new List<SelectListItem>();
            foreach (ListaDetalle itemDetalle in listaBl.GetListaByOpcion(BusinessLayer.Compartido.Enums.OpcionLista.TipoDocumento).ListaDetalle)
                tipoDocumentoList.Add(new SelectListItem { Text = itemDetalle.Glosa, Value = Convert.ToString(itemDetalle.IdDetalleLista) });
            ViewBag.tipoDocumentoList = tipoDocumentoList;


            List<SelectListItem> generoList = new List<SelectListItem>();
            foreach (ListaDetalle itemDetalle in listaBl.GetListaByOpcion(BusinessLayer.Compartido.Enums.OpcionLista.GeneroPersona).ListaDetalle)
                generoList.Add(new SelectListItem { Text = itemDetalle.Glosa, Value = Convert.ToString(itemDetalle.IdDetalleLista) });
            ViewBag.generoList = generoList;


            List<SelectListItem> seguroList = new List<SelectListItem>();
            foreach (ListaDetalle itemDetalle in listaBl.GetListaByOpcion(BusinessLayer.Compartido.Enums.OpcionLista.TipoSeguroSalud).ListaDetalle)
                seguroList.Add(new SelectListItem { Text = itemDetalle.Glosa, Value = Convert.ToString(itemDetalle.IdDetalleLista) });
            ViewBag.seguroList = seguroList;

            //estatus 0 SIN VALIDAR RENIEC, 1 VALIDADO CON RENIEC
            if (paciente.estatus != 0)
            {
                ViewBag.TipoDocumento = listaBl.GetListaByOpcion(BusinessLayer.Compartido.Enums.OpcionLista.TipoDocumento).ListaDetalle.Where(x => x.IdDetalleLista == 1).First().Glosa;
                ViewBag.Genero = listaBl.GetListaByOpcion(BusinessLayer.Compartido.Enums.OpcionLista.GeneroPersona).ListaDetalle.Where(x => x.IdDetalleLista == paciente.Genero).First().Glosa;
            }

            ViewBag.SourceIsListaValidar = SourceIsListaValidar;

            return View("New", paciente);
        }

        ///Descripción: Recibe los datos del paciente del jquery New.js para utilizarlo en el ActionResult New.
        /// Author: Marcos Mejia.
        /// Fecha Creacion: 08/06/2018
        public void GetApellidos(String ApellidoPaterno, String ApellidoMaterno)
        {
            var t = new Paciente()
            {
                ApellidoPaterno = ApellidoPaterno,
                ApellidoMaterno = ApellidoMaterno
            };
            Session["GetApellidos"] = t;
        }

        /// <summary>
        /// Descripción: Controlador para Registrar los input de cada campo en el mantenedor del paciente.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="paciente"></param>
        /// <param name="ActualDepartamento"></param>
        /// <param name="ActualProvincia"></param>
        /// <param name="ActualDistrito"></param>
        /// <param name="chckDireccionActual"></param>
        /// <param name="RegistroOrdenRecepcion"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult New(Paciente paciente, String ActualDepartamento, String ActualProvincia, String ActualDistrito, String chckDireccionActual, int RegistroOrdenRecepcion, String chkSinDatosTutor)
        {
            //if (paciente.ApellidoPaterno == null && paciente.ApellidoMaterno == null)
            //{
            //    paciente.ApellidoPaterno = ((Paciente)Session["GetApellidos"]).ApellidoPaterno;
            //    paciente.ApellidoMaterno = ((Paciente)Session["GetApellidos"]).ApellidoMaterno;
            //}
            //Session.Remove("GetApellidos");
            ViewBag.RegistroOrdenRecepcion = RegistroOrdenRecepcion;

            PacienteBl pacienteBl = new PacienteBl();
            List<Paciente> pacienteList = pacienteBl.GetPacientesByTipoNroDocumento(paciente.TipoDocumento.Value, paciente.NroDocumento);
            if (chkSinDatosTutor != null) paciente.mcaDatoTutor = 1;
            //Si Reniec es distinto de null entonces se valida si es necesario 
            //los datos de direccion actual
            if (paciente.UbigeoReniec != null)
            {
                if (chckDireccionActual != null)
                {
                    ActualDepartamento = paciente.UbigeoReniec.Id.Substring(0, 2);
                    ActualProvincia = paciente.UbigeoReniec.Id.Substring(2, 2);
                    ActualDistrito = paciente.UbigeoReniec.Id.Substring(4, 2);
                    paciente.DireccionActual = paciente.DireccionReniec;
                }

            }
            paciente.UbigeoActual = new Ubigeo();
            paciente.UbigeoActual.Id = "      ";
            bool valido = validarPaciente(paciente, pacienteList, ActualDepartamento, ActualProvincia, ActualDistrito);

            if (!valido)
            {
                return View("New", paciente);
            }

            Ubigeo ubigeoActual = new Ubigeo();
            ubigeoActual.Id = ActualDepartamento + ActualProvincia + ActualDistrito;

            //if (paciente.estatus == 0)
            if (paciente.UbigeoReniec == null)
            {
                paciente.UbigeoReniec = ubigeoActual;
            }
            paciente.UbigeoActual = ubigeoActual;
            paciente.IdUsuarioRegistro = ((Usuario)Session["UsuarioLogin"]).idUsuario;

            if (paciente.TipoDocumento == 1 && paciente.EstadoReniec)
            {
                paciente = pacienteBl.getReniec(paciente);
            }

            
            paciente.IdPaciente = pacienteBl.InsertPaciente(paciente);

            Session["RegistroOrdenRecepcion"] = RegistroOrdenRecepcion;
            return RedirectToAction("Index", new { nroDocumento = paciente.NroDocumento, tipoDocumento = paciente.TipoDocumento });

            //return RedirectToAction("New", "Orden", new { idPaciente = paciente.IdPaciente, NroDocumento = paciente.NroDocumento, Nombre = paciente.Nombres + " " + paciente.ApellidoPaterno + " " + paciente.ApellidoMaterno, tipoRegistro = RegistroOrdenRecepcion });

        }

        /// <summary>
        /// Descripción: Controlador para ejecutar los cambios ingresado del paciente.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="paciente"></param>
        /// <param name="ActualDepartamento"></param>
        /// <param name="ActualProvincia"></param>
        /// <param name="ActualDistrito"></param>
        /// <param name="RegistroOrdenRecepcion"></param>
        /// <param name="sourceIsListaValidar"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(Paciente paciente, string ActualDepartamento, string ActualProvincia, 
            string ActualDistrito, int RegistroOrdenRecepcion, int sourceIsListaValidar = 0)
        {
            ViewBag.RegistroOrdenRecepcion = RegistroOrdenRecepcion;

            PacienteBl pacienteBl = new PacienteBl();
            List<Paciente> pacienteList = pacienteBl.GetPacientesByTipoNroDocumento(paciente.TipoDocumento.Value, paciente.NroDocumento).Where(x => x.IdPaciente != paciente.IdPaciente).ToList();

            ViewBag.SourceIsListaValidar = sourceIsListaValidar;
            var ubigeoActual = new Ubigeo
            {
                Id = ActualDepartamento + ActualProvincia + ActualDistrito
            };

            paciente.UbigeoActual = ubigeoActual;
            bool valido = validarPaciente(paciente, pacienteList, ActualDepartamento, ActualProvincia, ActualDistrito);

            if (!valido)
            {
                return View("New", paciente);
            }

            //var pacienteBl = new PacienteBl();
            paciente.IdUsuarioEdicion = ((Usuario)Session["UsuarioLogin"]).idUsuario;
            pacienteBl.ActualizarPaciente(paciente);

            if (sourceIsListaValidar == 1)
                return RedirectToAction("ListaValidar", new { page = 1, fechaDesde = (string)null, fechaHasta = (string)null });

            Session["RegistroOrdenRecepcion"] = RegistroOrdenRecepcion;
            return RedirectToAction("Index", new { nroDocumento = paciente.NroDocumento, tipoDocumento = paciente.TipoDocumento });
        }

        /// <summary>
        /// Descripción: Metodo para validar la informacion del paciente.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="paciente"></param>
        /// <param name="pacienteList"></param>
        /// <param name="ActualDepartamento"></param>
        /// <param name="ActualProvincia"></param>
        /// <param name="ActualDistrito"></param>
        /// <returns></returns>
        private bool validarPaciente(Paciente paciente, List<Paciente> pacienteList, String ActualDepartamento, String ActualProvincia, String ActualDistrito)
        {
            bool valido = false;



            if (pacienteList.Count > 0)
            {
                ModelState.AddModelError(String.Empty, "El número de documento ya se encuentra registrado");
            }
            else if (paciente.TipoDocumento == 1 && paciente.NroDocumento.Length != 8)
            {
                ModelState.AddModelError(String.Empty, "DNI debe ser de 8 dígitos");
            }
            else if ((paciente.TipoDocumento == 2 || paciente.TipoDocumento == 3) && paciente.NroDocumento.Length > 12)
            {
                ModelState.AddModelError(String.Empty, "Nro de documento debe ser menor o igual a 12 dígitos");
            }
            else if (string.IsNullOrEmpty(paciente.mcaDatoTutor.ToString()))
            {
                if (paciente.TipoDocumento == 4 && paciente.NroDocumento.Length != 11)
                {
                    ModelState.AddModelError(String.Empty, "Nro de documento debe ser de 11 dígitos");
                }
                else if (paciente.TipoDocumento == 4 && (Convert.ToInt32(paciente.NroDocumento.Substring(0, 1)) < 1 || Convert.ToInt32(paciente.NroDocumento.Substring(0, 1)) > 4))
                {
                    ModelState.AddModelError(String.Empty, "Primer dígito del Nro de documento deber ser 1, 2, 3 o 4");
                }
                else if (paciente.TipoDocumento == 4 && Convert.ToInt32(paciente.NroDocumento.Substring(9, 2)) < 1)
                {
                    ModelState.AddModelError(String.Empty, "Últimos dos dígitos del Nro de documento debe ser un valor mayor a 0");
                }
            }

            else if (ActualDepartamento.Equals(""))
            {
                ModelState.AddModelError(String.Empty, "Seleccione el Departamento");
                paciente.UbigeoActual.Id = "      ";
            }
            else if (ActualProvincia.Equals(""))
            {
                ModelState.AddModelError(String.Empty, "Seleccione la Provincia");
                paciente.UbigeoActual.Id = ActualDepartamento + "    ";
            }
            else if (ActualDistrito.Equals(""))
            {
                ModelState.AddModelError(String.Empty, "Seleccione el Distrito");
                paciente.UbigeoActual.Id = ActualDepartamento + ActualProvincia + "  ";
            }
            else if (paciente.tipoSeguroSalud == 9 && (paciente.otroSeguroSalud == null || paciente.otroSeguroSalud.Trim().Length == 0))
            {
                ModelState.AddModelError(String.Empty, "Ingrese el Tipo de Seguro Salud");
            }
            else if (!FechaNacimientoValida(paciente.FechaNacimiento))
            {
                ModelState.AddModelError(String.Empty, "La fecha de nacimiento no puede ser mayor o igual a la fecha actual.");
            }
            else
            {
                valido = true;
            }

            if (!valido)
            {
                ListaBl listaBl = new ListaBl();
                List<SelectListItem> tipoDocumentoList = new List<SelectListItem>();
                foreach (ListaDetalle itemDetalle in listaBl.GetListaByOpcion(BusinessLayer.Compartido.Enums.OpcionLista.TipoDocumento).ListaDetalle)
                    tipoDocumentoList.Add(new SelectListItem { Text = itemDetalle.Glosa, Value = Convert.ToString(itemDetalle.IdDetalleLista) });
                ViewBag.tipoDocumentoList = tipoDocumentoList;

                List<SelectListItem> generoList = new List<SelectListItem>();
                foreach (ListaDetalle itemDetalle in listaBl.GetListaByOpcion(BusinessLayer.Compartido.Enums.OpcionLista.GeneroPersona).ListaDetalle)
                    generoList.Add(new SelectListItem { Text = itemDetalle.Glosa, Value = Convert.ToString(itemDetalle.IdDetalleLista) });
                ViewBag.generoList = generoList;

                List<SelectListItem> seguroList = new List<SelectListItem>();
                foreach (ListaDetalle itemDetalle in listaBl.GetListaByOpcion(BusinessLayer.Compartido.Enums.OpcionLista.TipoSeguroSalud).ListaDetalle)
                    seguroList.Add(new SelectListItem { Text = itemDetalle.Glosa, Value = Convert.ToString(itemDetalle.IdDetalleLista) });
                ViewBag.seguroList = seguroList;
            }


            return valido;
        }
        /// <summary>
        /// Descripción: Metodo para validar el campo Fecha de Nacimiento.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool FechaNacimientoValida(DateTime? value)
        {
            DateTime fechaElegida;

            try
            {
                fechaElegida = Convert.ToDateTime(value);
            }
            catch (Exception)
            {
                fechaElegida = DateTime.Now;
            }

            if (DateTime.Compare(DateTime.Now.Date, fechaElegida.Date) < 0)
                return false;

            return true;
        }
        /// <summary>
        /// Descripción: Metodo para Mostrar informacion del paciente desde el netlab v1.0.
        /// Author: Juan Muga Rivera.
        /// Fecha Creacion: 08/20/2018
        /// </summary>
        /// <param name="page"></param>
        /// <param name="NroDocumento"></param>
        /// <returns></returns>
        public ActionResult ShowInformacionNetLabv1(int? page, string NroDocumento, string CodigoMuestra)
        {
            List<Paciente> pacienteList = new List<Paciente>();
            if (String.IsNullOrEmpty(NroDocumento) && String.IsNullOrEmpty(CodigoMuestra)) return View();
            pacienteList = GetDataNetLab1(NroDocumento, CodigoMuestra);
            var pageNumber = page ?? 1;
            var pageOfPacientes = pacienteList.ToPagedList(1, 100);
            ViewBag.notFound = "No se encontraron resultados";
            ViewBag.NroDocumento = NroDocumento;
            Session["PacienteNetLab1"] = pacienteList;
            return PartialView("_PacienteNetLabv1", pageOfPacientes);
        }
        /// <summary>
        /// Descripción: Muestra detalle del paciente en NetLab v1.0.
        /// Author: Juan Muga.
        /// Fecha Creacion: 20/08/2018
        /// </summary>
        /// <param name="Prueba"></param>
        /// <returns></returns>
        public ActionResult DetallePacienteNetLab1(string Prueba, string Muestra)
        {
            var LstPaciente = new List<Paciente>();
            if (this.Session["PacienteNetLab1"] != null)
            {
                LstPaciente = (List<Paciente>)this.Session["PacienteNetLab1"];
            }
            return PartialView("_DetallePacienteNetLab1", LstPaciente.Where(x => x.Prueba == Prueba && x.cCodMuestra == Muestra).ToList());
        }
        [HttpGet]
        public ActionResult HistorialPacienteNetLab1(int? page, string nroDocumento)
        {
            return View();
        }
        /// <summary>
        /// Descripción: Lee datos del paciente en NetLab v1.0.
        /// Author: Juan Muga.
        /// Fecha Creacion: 20/08/2018
        /// </summary>
        /// <param name="nroDocumento"></param>
        /// <param name="CodigoMuestra"></param>
        /// <returns></returns>
        public List<Model.Paciente> GetDataNetLab1(string nroDocumento, string CodigoMuestra)
        {
            return new PacienteBl().GetDatosPacienteNetLab1(nroDocumento, CodigoMuestra).Distinct().ToList();
        }

        /// Descripción: Busca historial de los pacientes NetLab v2.0.
        /// Author: Yonatan Clemente
        /// Fecha Creacion: 08/05/2018
        public ActionResult ConsultaHistPaciente(int? page, int? tipoDocumento, string nroDocumento, string apellidoPaterno, string apellidoMaterno, string nombre)
        {
            var listaBl = new ListaBl();
            var tipoDocumentoList = new List<SelectListItem>();
            var tipoDocumentoCriteria = tipoDocumento ?? -1;
            foreach (var itemDetalle in listaBl.GetListaByOpcion(OpcionLista.TipoDocumento).ListaDetalle)
                tipoDocumentoList.Add(new SelectListItem
                {
                    Text = itemDetalle.Glosa,
                    Value = Convert.ToString(itemDetalle.IdDetalleLista)
                });

            ViewBag.tipoDocumentoList = tipoDocumentoList;
            List<Paciente> LisPaciente = new List<Paciente>();
            PacienteBl paciente = new PacienteBl();
            LisPaciente = paciente.ObtenerHistorialPacientePorUsuario(tipoDocumentoCriteria, nroDocumento, apellidoPaterno, apellidoMaterno, nombre, Logueado.idUsuario);
            //LisPaciente = paciente.ObtenerHistorialPaciente(tipoDocumentoCriteria, nroDocumento, apellidoPaterno, apellidoMaterno, nombre);
            var pacienteDal = new PacienteDal();
            var reniec = pacienteDal.EstadoReniec();
            if (!string.IsNullOrWhiteSpace(nroDocumento) && !LisPaciente.Any() && reniec)
            {
                //Buscar si existe en tabla kobos
                if (paciente.BuscarPacientePorNroDocumentoEnPRK(nroDocumento))
                {
                    if (!string.IsNullOrWhiteSpace(nroDocumento) && nroDocumento.Length == 8)
                    {
                        //buscar paciente por reniec
                        var reneicpaciente = paciente.getReniec(new Paciente { NroDocumento = nroDocumento});
                        if (reneicpaciente != null)
                        {
                            //registrar paciente
                            reneicpaciente.TipoDocumento = 1;//DNI
                            reneicpaciente.tipoSeguroSalud = 0;
                            reneicpaciente.estatus = 1;
                            reneicpaciente.IdUsuarioRegistro = Logueado.idUsuario;
                            reneicpaciente.UbigeoActual = reneicpaciente.UbigeoReniec;
                            var nuevoPaciente = paciente.InsertPaciente(reneicpaciente);
                            if (nuevoPaciente != Guid.Empty)
                            {
                                //realizar nueva busqueda
                                LisPaciente = paciente.ObtenerHistorialPacientePorUsuario(tipoDocumentoCriteria, nroDocumento, apellidoPaterno, apellidoMaterno, nombre, Logueado.idUsuario);
                            }
                        }
                    }
                }
            }

            return View(LisPaciente);
        }
        /// <summary>
        /// Descripción: Lee datos del paciente y muetsra formulario para referencia
        /// Author: Jose Chavez.
        /// Fecha Creacion: 15/07/2019
        /// </summary>
        /// <param name="idPaciente"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ReferenciarPaciente(string idPaciente)
        {
            return PartialView("_FormAddEstablecimientoReferencia", new PacienteBl().GetPacienteReferencia(idPaciente));
        }
        /// <summary>
        /// Descripción: Registra datos del paciente para la referencia
        /// Author: Jose Chavez.
        /// Fecha Creacion: 15/07/2019
        /// </summary>
        /// <param name="observaciones"></param>
        /// <param name="idPaciente"></param>
        /// <param name="idEstablecimiento"></param>
        /// <returns></returns>
        [HttpPost]
        public string ReferenciarPaciente2(string observaciones, string idPaciente, int idEstablecimiento)
        {
            return new PacienteBl().InsertPacienteReferencia(new Paciente() { IdPaciente = Guid.Parse(idPaciente), idEstablecimiento = idEstablecimiento, comentario = observaciones, IdUsuarioRegistro = Logueado.idUsuario }, EstablecimientoSeleccionado.IdEstablecimiento);
        }
        public ActionResult PacienteCoronavirus(int? page, int? tipoDocumento, string NroDocumento, string nombre, string apellidoPaterno, string apellidoMaterno)
        {
            var tipoDocumentoList = new List<SelectListItem>();
            tipoDocumentoList = ObtenerListaTipoDocumento();
            ViewBag.tipoDocumentoList = tipoDocumentoList;
            if (string.IsNullOrWhiteSpace(NroDocumento) && string.IsNullOrWhiteSpace(nombre) && string.IsNullOrWhiteSpace(apellidoPaterno) && string.IsNullOrWhiteSpace(apellidoMaterno))
            {
                return View("PacienteCoronavirus");
            }

            ViewBag.nombre = nombre;
            ViewBag.NroDocumento = NroDocumento;
            ViewBag.apellidoPaterno = apellidoPaterno;
            ViewBag.apellidoMaterno = apellidoMaterno;
            PacienteBl pacienteBL = new PacienteBl();
            List<Paciente> pacienteList = new List<Paciente>();
            
            pacienteList = pacienteBL.GetDatosPacienteByNroDocumentoCoronavirus(NroDocumento, nombre, apellidoPaterno, apellidoMaterno, Logueado.idUsuario.ToString());
            var pageNumber = page ?? 1;
            var tipoPage = 1;
            const int numRegPorPagima = 1000;
            var pageOfPacientes = pacienteList.ToPagedList(pageNumber, numRegPorPagima);
            ViewBag.notFound = "No se encontraron resultados";
            ViewBag.NroDocumento = NroDocumento;
            ViewBag.tipo = tipoPage;
            return View("PacienteCoronavirus", pageOfPacientes);
        }
        //Paciente/PacienteTbc - seguimiento de pacientes solo para enfermedad TBC
        public ActionResult PacienteTbc(int? page, int? tipoDocumento, string NroDocumento, string nombre, string apellidoPaterno, string apellidoMaterno)
        {
            var tipoDocumentoList = new List<SelectListItem>();
            tipoDocumentoList = ObtenerListaTipoDocumento();
            ViewBag.tipoDocumentoList = tipoDocumentoList;
            if (string.IsNullOrWhiteSpace(NroDocumento) && string.IsNullOrWhiteSpace(nombre) && string.IsNullOrWhiteSpace(apellidoPaterno) && string.IsNullOrWhiteSpace(apellidoMaterno))
            {
                return View("PacienteTbc");
            }

            ViewBag.nombre = nombre;
            ViewBag.NroDocumento = NroDocumento;
            ViewBag.apellidoPaterno = apellidoPaterno;
            ViewBag.apellidoMaterno = apellidoMaterno;
            PacienteBl pacienteBL = new PacienteBl();
            List<Paciente> pacienteList = new List<Paciente>();

            pacienteList = pacienteBL.GetDatosPacienteByNroDocumentoTbc(NroDocumento, nombre, apellidoPaterno, apellidoMaterno, Logueado.idUsuario.ToString());
            var pageNumber = page ?? 1;
            var tipoPage = 1;
            const int numRegPorPagima = 1000;
            var pageOfPacientes = pacienteList.ToPagedList(pageNumber, numRegPorPagima);
            ViewBag.notFound = "No se encontraron resultados";
            ViewBag.NroDocumento = NroDocumento;
            ViewBag.tipo = tipoPage;
            return View("PacienteTbc", pageOfPacientes);
        }
        //Paciente/PacienteVIH - seguimiento de pacientes solo para enfermedad VIH
        public ActionResult PacienteVIH(int? page, int? tipoDocumento, string NroDocumento, string nombre, string apellidoPaterno, string apellidoMaterno)
        {
            var tipoDocumentoList = new List<SelectListItem>();
            tipoDocumentoList = ObtenerListaTipoDocumento();
            ViewBag.tipoDocumentoList = tipoDocumentoList;
            if (string.IsNullOrWhiteSpace(NroDocumento) && string.IsNullOrWhiteSpace(nombre) && string.IsNullOrWhiteSpace(apellidoPaterno) && string.IsNullOrWhiteSpace(apellidoMaterno))
            {
                return View("PacienteVIH");
            }

            ViewBag.nombre = nombre;
            ViewBag.NroDocumento = NroDocumento;
            ViewBag.apellidoPaterno = apellidoPaterno;
            ViewBag.apellidoMaterno = apellidoMaterno;
            PacienteBl pacienteBL = new PacienteBl();
            List<Paciente> pacienteList = new List<Paciente>();

            pacienteList = pacienteBL.GetDatosPacienteByNroDocumentoVIH(NroDocumento, nombre, apellidoPaterno, apellidoMaterno, Logueado.idUsuario.ToString());
            var pageNumber = page ?? 1;
            var tipoPage = 1;
            const int numRegPorPagima = 1000;
            var pageOfPacientes = pacienteList.ToPagedList(pageNumber, numRegPorPagima);
            ViewBag.notFound = "No se encontraron resultados";
            ViewBag.NroDocumento = NroDocumento;
            ViewBag.tipo = tipoPage;
            return View("PacienteVIH", pageOfPacientes);
        }
        [HttpPost]
        public string BusquedaPacienteCovid(string TipoDocumento, string nroDocumento)
        {
            var pacienteBl = new PacienteBl();
            var paciente = new Paciente();
            var res = "";
            paciente.TipoDocumento = int.Parse(TipoDocumento);
            paciente.NroDocumento = nroDocumento;
            List<Paciente> pacienteList = pacienteBl.getPacientes2(1,10,paciente.TipoDocumento.Value, paciente.NroDocumento);
            if (!pacienteList.Any())
            {
                if (paciente.TipoDocumento == 1)
                {
                    var reniecPaciente = pacienteBl.getReniec(paciente);
                    if (reniecPaciente != null)//si existe en reniec
                    {
                        reniecPaciente.IdUsuarioRegistro = Logueado.idUsuario;
                        reniecPaciente.TipoDocumento = 1;
                        reniecPaciente.UbigeoActual = new Ubigeo { Id = reniecPaciente.UbigeoReniec.Id };
                        reniecPaciente.IdPaciente = pacienteBl.InsertPaciente(reniecPaciente);
                        paciente = pacienteBl.getPacienteById(reniecPaciente.IdPaciente);
                        res = paciente.Nombres + " " + paciente.ApellidoPaterno + " " + paciente.ApellidoMaterno;
                    }
                }
                //else
                //{
                //    var ubigeo = new Ubigeo { Id = "" };
                //    paciente.IdUsuarioRegistro = idUsuarioRegistro;
                //    paciente.TipoDocumento = 2;
                //    paciente.UbigeoActual = ubigeo;
                //    paciente.UbigeoReniec = ubigeo;
                //    paciente.IdPaciente = pacienteBl.InsertPaciente(paciente);
                //}
            }
            else
            {
                paciente = pacienteList.FirstOrDefault();
                res = paciente.Nombres + " " + paciente.ApellidoPaterno + " " + paciente.ApellidoMaterno;
            }
            return res;
        }
        public ActionResult UpdateReniec()
        {
            try
            {
                PacienteBl p = new PacienteBl();
                var listaPaciente = p.xLstPaciente();
                PacienteDal pac = new PacienteDal();
                foreach (var item in listaPaciente)
                {
                    pac.UpdateReniec(p.getReniec(item));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View();
        }

        public ActionResult ConsultaOrdenResultadoPaciente()
        {
            return View();
        }

        public string BuscarPacienteLaboratorio(string nroDocumento, string apellidoPaterno, string apellidoMaterno, string nombre)
        {
            PacienteDal dal = new PacienteDal();
            string paciente = "";
            paciente = dal.BuscarPaciente(nroDocumento, apellidoPaterno, apellidoMaterno, nombre, Logueado.idUsuario);
            return paciente;
        }

        public string ConsultaExternaPaciente(Guid idPaciente)
        {
            PacienteDal dal = new PacienteDal();
            string resultado = "";
            resultado = dal.ConsultaExternaPaciente(idPaciente,EstablecimientoSeleccionado.IdEstablecimiento);
            return resultado;
        }

        public ActionResult PacienteCoronavirusExterno(int? page, int? tipoDocumento, string NroDocumento, string nombre, string apellidoPaterno, string apellidoMaterno)
        {
            if (string.IsNullOrWhiteSpace(NroDocumento) && string.IsNullOrWhiteSpace(nombre) && string.IsNullOrWhiteSpace(apellidoPaterno) && string.IsNullOrWhiteSpace(apellidoMaterno))
            {
                return View("PacienteCoronavirusExterno");
            }
            var listaBl = new ListaBl();
            var tipoDocumentoList = new List<SelectListItem>();
            var tipoDocumentoCriteria = tipoDocumento ?? -1;
            foreach (var itemDetalle in listaBl.GetListaByOpcion(OpcionLista.TipoDocumento).ListaDetalle)
                tipoDocumentoList.Add(new SelectListItem
                {
                    Text = itemDetalle.Glosa,
                    Value = Convert.ToString(itemDetalle.IdDetalleLista)
                });

            ViewBag.tipoDocumentoList = tipoDocumentoList;
            ViewBag.nombre = nombre;
            ViewBag.NroDocumento = NroDocumento;
            ViewBag.apellidoPaterno = apellidoPaterno;
            ViewBag.apellidoMaterno = apellidoMaterno;
            PacienteBl pacienteBL = new PacienteBl();
            List<Paciente> pacienteList = new List<Paciente>();
            
            pacienteList = pacienteBL.GetDatosPacienteByNroDocumentoCoronavirusExterno(NroDocumento, nombre, apellidoPaterno, apellidoMaterno, Logueado.idUsuario.ToString());
            var pageNumber = page ?? 1;
            var tipoPage = 1;
            const int numRegPorPagima = 1000;
            var pageOfPacientes = pacienteList.ToPagedList(pageNumber, numRegPorPagima);
            ViewBag.notFound = "No se encontraron resultados";
            ViewBag.NroDocumento = NroDocumento;
            ViewBag.tipo = tipoPage;
            return View("PacienteCoronavirusExterno", pageOfPacientes);
        }


        public ActionResult MostrarPruebaRapidaSiscovidPaciente(string nroDocumento)
        {
            PacienteBl pacienteBL = new PacienteBl();
            List<Paciente> pacienteList = new List<Paciente>();
            pacienteList = pacienteBL.ObtenerSiscovidPruebaRapidaPorNroDocumento(nroDocumento);
            var pageNumber = 1;
            var tipoPage = 1;
            const int numRegPorPagima = 1000;
            var pageOfPacientes = pacienteList.ToPagedList(pageNumber, numRegPorPagima);
            ViewBag.notFound = "No se encontraron resultados";
            ViewBag.NroDocumento = nroDocumento;
            ViewBag.tipo = tipoPage;
            return PartialView("_ResultadosPRSiscovidPorNrodocumento", pageOfPacientes);
        }

        public ActionResult ConsultaExternaResultadoPaciente()
        {
            return View();
        }

        public string ConsultaDatoPaciente(string nroDocumento, string apellidoPaterno, string apellidoMaterno, string nombre)
        {
            PacienteDal dal = new PacienteDal();
            string paciente = "";
            paciente = dal.ConsultaDatoPaciente(nroDocumento, apellidoPaterno, apellidoMaterno, nombre, Logueado.idUsuario);
            return paciente;
        }

        public string ResultadosConsultaExternaPaciente(Guid idPaciente)
        {
            PacienteDal dal = new PacienteDal();
            string resultado = "";
            resultado = dal.ResultadosConsultaExternaPaciente(idPaciente);
            return resultado;
        }

        public List<SelectListItem> ObtenerListaTipoDocumento()
        {
            List<ListaDetalle> opciones = new List<ListaDetalle>();
            if (Session["ListaTipoDocumento"] != null)
            {
                if (!((List<ListaDetalle>)Session["ListaTipoDocumento"]).Any())
                {
                    opciones = CargarListaTipoDocumento();
                }
                else
                {
                    opciones = (List<ListaDetalle>)Session["ListaTipoDocumento"];
                }
            }
            else
            {
                opciones = CargarListaTipoDocumento();
            }

            var tipoDocumentoList = new List<SelectListItem>();
            foreach (var itemDetalle in opciones)
            {
                tipoDocumentoList.Add(new SelectListItem
                {
                    Text = itemDetalle.Glosa,
                    Value = Convert.ToString(itemDetalle.IdDetalleLista)
                });
            }

            Session["ListaTipoDocumento"] = opciones;
            return tipoDocumentoList;
        }

        public List<ListaDetalle> CargarListaTipoDocumento()
        {
            var listaBl = new ListaBl();
            var resultado = listaBl.GetListaByOpcion(OpcionLista.TipoDocumento).ListaDetalle;
            return resultado;
        }
    }
}