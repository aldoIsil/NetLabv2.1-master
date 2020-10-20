using BusinessLayer;
using BusinessLayer.Interfaces;
using Model;
using PagedList;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using NetLab.Helpers;
using Model.ViewData;
using BusinessLayer.Compartido.Enums;
using BusinessLayer.Compartido;

using System.ComponentModel;
using System.Data;
using NetLab.Extensions.ActionResults;
using ClosedXML.Excel;
using System.Net.Http;
using NetLab.App_Code;
using Rotativa.Options;
using System.Configuration;

namespace NetLab.Controllers
{
    public partial class OrdenMuestraController : ParentController
    {
        /// <summary>
        /// Juan Muga
        /// </summary>
        /// <param name="IdOrden"></param>
        /// <param name="CodigoOrden"></param>
        /// <returns></returns>
        public ActionResult DetalleOrden(string IdOrden, string CodigoOrden)
        {
            var idOrdenG = Guid.Parse(IdOrden);
            List<Model.ViewData.OrdenRecepcionDetalleVd> LstOrdenDetalle = new List<Model.ViewData.OrdenRecepcionDetalleVd>();
            if (this.Session["OrdenDetalle"] != null)
            {
                LstOrdenDetalle = (List<Model.ViewData.OrdenRecepcionDetalleVd>)this.Session["OrdenDetalle"];
            }

            return PartialView("_DetalleOrden", LstOrdenDetalle.Where(x => x.idOrden == idOrdenG).ToList());
        }
        /// <summary>
        /// Juan Muga
        /// </summary>
        /// <param name="IdOrden"></param>
        /// <param name="CodigoOrden"></param>
        /// <returns></returns>
        public ActionResult DetalleOrdenRechazo(string IdOrden)
        {
            var LstOrdenDetalle = new OrdenMuestraBl().GetOrdenMuestraRechazobyIdOrden(Guid.Parse(IdOrden));
            return PartialView("_DetalleOrdenRechazo", LstOrdenDetalle);
        }

        /// Descripción: Muestra el detalle de la Orden referenciada.
        /// Author: Marcos Mejia.
        /// Fecha Creacion: 27/06/2019
        public ActionResult DetalleOrdenReferencia(string IdOrden, string CodigoOrden)
        {
            var idOrdenG = Guid.Parse(IdOrden);
            List<Model.ViewData.OrdenRecepcionDetalleVd> LstOrdenDetalle = new List<Model.ViewData.OrdenRecepcionDetalleVd>();
            if (this.Session["OrdenDetalleRef"] != null)
            {
                LstOrdenDetalle = (List<Model.ViewData.OrdenRecepcionDetalleVd>)this.Session["OrdenDetalleRef"];
            }

            return PartialView("_DetalleOrdenReferencia", LstOrdenDetalle.Where(x => x.idOrden == idOrdenG).ToList());
        }
        /// <summary>
        /// Descripción: Controlador para realizar la busqueda de Muestras para recepcionar
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <returns></returns>
        public ActionResult BusquedaMuestraRecepcionar(string codigoOrden = null)
        {
            ViewBag.fechaDesde = DateTime.Now.ToDefaultDateFormatString();
            ViewBag.fechaHasta = DateTime.Now.ToDefaultDateFormatString();
            //return View();
            string nrodocumento = string.IsNullOrWhiteSpace(ViewBag.nroDocumento) ? "" : ViewBag.nroDocumento;
            string apellidopaterno = string.IsNullOrWhiteSpace(ViewBag.apellidopaterno) ? "" : ViewBag.apellidopaterno;
            string apellidomaterno = string.IsNullOrWhiteSpace(ViewBag.apellidomaterno) ? "" : ViewBag.apellidomaterno;
            string nombres = string.IsNullOrWhiteSpace(ViewBag.nombres) ? "" : ViewBag.nombres;
            //Se agregaron filtros de paciente(dni, ap.paterno, ap.materno, nombres)
            return BusquedaMuestraRecepcionar(1, "1", ViewBag.fechaDesde, ViewBag.fechaHasta, string.IsNullOrEmpty(codigoOrden) ? "" : codigoOrden, "", string.IsNullOrEmpty(codigoOrden) ? "1" : "0", null, "", nrodocumento, apellidopaterno, apellidomaterno, nombres);
        }
        /// <summary>
        /// Descripción: Controlador para realizar la busqueda de Muestras para recepcionar
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="esFechaRegistro"></param>
        /// <param name="fechaDesde"></param>
        /// <param name="fechaHasta"></param>
        /// <param name="codigoOrden"></param>
        /// <param name="nroOficio"></param>
        /// <param name="estadoOrden"></param>
        /// <param name="tipoOrden"></param>
        /// <param name="idMuestra"></param>
        /// <returns></returns>

        [HttpPost]
        public ActionResult BusquedaMuestraRecepcionar(int? page, string esFechaRegistro, string fechaDesde, string fechaHasta, string codigoOrden,
                                                        string nroOficio, string estadoOrden, string tipoOrden, string idMuestra, string nroDocumento, string apellidopaterno, string apellidomaterno, string nombres)
        {
            var estadoOrdenF = 0;
            var tipoOrdenF = 0;

            if (estadoOrden != null)
                estadoOrdenF = int.Parse(estadoOrden);

            if (tipoOrden != null)
                tipoOrdenF = int.Parse(tipoOrden);

            var fechaDesdeA = new DateTime();
            var fechaHastaA = new DateTime();
            var fechaRegistro = 0;
            var pageNumber = page ?? 1;

            var fechaDesdeCriteria = fechaDesde ?? string.Empty;
            var fechaHastaCriteria = fechaHasta ?? string.Empty;

            if (!fechaDesdeCriteria.Equals(""))
                fechaDesdeA = DateTime.ParseExact(fechaDesdeCriteria, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            if (!fechaHastaCriteria.Equals(""))
                fechaHastaA = DateTime.ParseExact(fechaHastaCriteria, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            var fechaRegistroTmp = esFechaRegistro ?? string.Empty;
            if (!fechaRegistroTmp.Equals(""))
                fechaRegistro = int.Parse(fechaRegistroTmp);
            else if (fechaDesdeCriteria.Equals("") || fechaHastaCriteria.Equals(""))
                fechaRegistro = 0;
            else if (!fechaDesdeCriteria.Equals("") && !fechaHastaCriteria.Equals(""))
            {
                if (fechaDesdeA == fechaHastaA)
                {
                    fechaHastaA = fechaHastaA.AddDays(1);
                }
            }

            var codigoOrdenF = codigoOrden.Trim();
            var idMuestraF = idMuestra.Trim();
            var nroOficioA = nroOficio.Trim();
            var idLaboratorio = EstablecimientoSeleccionado?.IdEstablecimiento ?? 0;

            var ordenBl = new OrdenMuestraBl();
            nroDocumento = string.IsNullOrWhiteSpace(nroDocumento) ? "" : nroDocumento;
            apellidopaterno = string.IsNullOrWhiteSpace(apellidopaterno) ? "" : apellidopaterno;
            apellidomaterno = string.IsNullOrWhiteSpace(apellidomaterno) ? "" : apellidomaterno;
            nombres = string.IsNullOrWhiteSpace(nombres) ? "" : nombres;
            //Se agregaron filtros de paciente(dni, ap.paterno, ap.materno, nombres)
            var ordenesRecepcion = ordenBl.GetOrdenMuestraByEstablecimiento(estadoOrdenF, fechaRegistro, Logueado.idUsuario, codigoOrdenF, fechaDesdeA, fechaHastaA, nroOficioA, tipoOrdenF, idMuestraF, idLaboratorio, nroDocumento,apellidopaterno,apellidomaterno,nombres);

            if (ordenesRecepcion == null) return View();

            ViewBag.TotalRegistros = ordenesRecepcion.Count;
            int maximaCantidadFilas = string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["BandejasMaxRows"]) ? ordenesRecepcion.Count : int.Parse(ConfigurationManager.AppSettings["BandejasMaxRows"]);

            ViewBag.MostrarMensaje = ordenesRecepcion.Count > maximaCantidadFilas;
            var pageOfOrdenMuestra = ordenesRecepcion.ToPagedList(pageNumber, maximaCantidadFilas);

            ViewBag.recepcionado = estadoOrdenF;
            ViewBag.estadoOrden = string.IsNullOrEmpty(estadoOrden) ? "1" : "0";
            //var ordenMuestraBL = new OrdenMuestraBl();

            //List<OrdenRecepcionDetalleVd> OrdenDetalle = null;

            //if (ordenesRecepcion.Count() > 0)
            //{
            //    OrdenDetalle = ordenMuestraBL.MuestrasByOrdenDetalle(idLaboratorio);
            //}
           
            //this.Session["OrdenDetalle"] = OrdenDetalle;
            Session["ordenesRecepcion"] = null;
            Session["ordenesRecepcion"] = ordenesRecepcion;
            return View(pageOfOrdenMuestra);
        }
        /// <summary>
        /// Descripción: Controlador  para mostrar la informacion de las muestras a recepcionar.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="codigo"></param>
        /// <param name="genero"></param>
        /// <returns>, string CodigoOrdenMuestraRecepcion</returns>
        [HttpGet]
        public ActionResult RecepcionarMuestra(string id, string codigo)
        {
            var idOrdenG = Guid.Parse(id);

            var orden = new Orden
            {
                genero = 3,
                codigoOrden = codigo
            };

            var ordenMuestraBl = new OrdenMuestraBl();

            var muestraRecepcion = ordenMuestraBl.MuestraRecepcionadosbyOrden(idOrdenG, 1, Logueado.idUsuario, 1, 0, 0, EstablecimientoSeleccionado.IdEstablecimiento);//.Where(x => x.idOrdenMuestraRecepcion == Guid.Parse(CodigoOrdenMuestraRecepcion)).ToList();

            var listaCriterios = new Dictionary<int, List<SelectListItem>>();
            var criteriosBl = new CriterioRechazoBl();

            Laboratorio LaboratorioUbigeo = new Laboratorio();
            LaboratorioUbigeo.UbigeoLaboratorio = new Model.Ubigeo();
            LaboratorioUbigeo.UbigeoLaboratorio.Id = "      ";
            ViewBag.LabUbigeo = LaboratorioUbigeo;
            var muestrasOrdenesMaterial = ordenMuestraBl.MuestrasByOrden(idOrdenG);
            if (muestrasOrdenesMaterial != null && muestraRecepcion != null)
            {
                foreach (var item in muestrasOrdenesMaterial)
                {
                    if (!listaCriterios.ContainsKey(item.idTipoMuestra))
                    {
                        listaCriterios[item.idTipoMuestra] = new List<SelectListItem>();
                        foreach (var crItem in criteriosBl.GetCriteriosByTipoMuestra(item.idTipoMuestra, 2))
                        {
                            listaCriterios[item.idTipoMuestra].Add(new SelectListItem { Text = crItem.Glosa, Value = crItem.IdCriterioRechazo.ToString() });
                        }
                    }

                    item.ordenMuestraRecepcionList = muestraRecepcion.FindAll(p => p.idMaterial == item.idMaterial).ToList();
                    var cantidadMateriales = item.ordenMuestraRecepcionList.Count;
                    var nroMaterial = 1;

                    foreach (var itemA in item.ordenMuestraRecepcionList)
                    {
                        if (nroMaterial > cantidadMateriales) continue;
                        itemA.presentacionNombreNro = itemA.presentacionNombreNro + " #" + nroMaterial;
                        nroMaterial++;
                    }
                }
            }
            //Autor: Marcos Mejia
            //Descripción: Obtiene datos del usuario que registra la orden para enviarlo a la vista.
            //Fecha: 25-09-2018
            var usuario = ordenMuestraBl.ConsultaDatosUsuario(idOrdenG);
            if (usuario.idUsuario != Logueado.idUsuario)
            {
                orden.usuarioEnvio = new UsuarioEnvioSms();
                orden.usuarioEnvio = usuario;
            }
            orden.ordenMaterialList = muestrasOrdenesMaterial;
            ViewBag.listaCriterios = listaCriterios;

            ViewBag.nomLabUsuario = EstablecimientoSeleccionado.Nombre;
            ViewBag.idLabUsuario = EstablecimientoSeleccionado.IdEstablecimiento;


            return PartialView("RecepcionarMuestra", orden);
        }


        /// <summary>
        /// Descripción: Referencia a otro establecimiento las muestras seleccionadas
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idOrden"></param>
        /// <param name="genero"></param>
        /// <param name="idLab"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ReferenciarMuestra(string idOrden, string genero, string idLab)
        {
            var idOrdenG = Guid.Parse(idOrden);
            var ordenMuestraBl = new OrdenMuestraBl();

            var orden = ordenMuestraBl.GetInfoOrden(idOrdenG, EstablecimientoSeleccionado.IdEstablecimiento);

            var materialesOrdenes = ordenMuestraBl.MaterialesByOrden(idOrdenG, EstablecimientoSeleccionado.IdEstablecimiento);
            orden.ordenMaterialVdList = materialesOrdenes;

            var muestraRecepcion = ordenMuestraBl.MuestraRecepcionadosbyOrden(idOrdenG, 0, Logueado.idUsuario, 1, 0, 1, EstablecimientoSeleccionado.IdEstablecimiento);

            var muestrasOrdenesMaterial = ordenMuestraBl.MuestrasByOrden(idOrdenG);
            if (muestrasOrdenesMaterial != null && muestraRecepcion != null)
            {
                foreach (var item in muestrasOrdenesMaterial)
                {
                    item.ordenMuestraRecepcionList = muestraRecepcion.FindAll(p => p.idMaterial == item.idMaterial).ToList();
                    var cantidadMateriales = item.ordenMuestraRecepcionList.Count;
                    var nroMaterial = 1;
                    foreach (var itemA in item.ordenMuestraRecepcionList)
                    {
                        if (nroMaterial > cantidadMateriales) continue;
                        itemA.presentacionNombreNro = itemA.presentacionNombreNro + " #" + nroMaterial;
                        nroMaterial++;
                    }
                }
            }

            orden.ordenMaterialList = muestrasOrdenesMaterial;
            orden.idOrden = Guid.Parse(idOrden);
            orden.genero = int.Parse(genero);
            orden.idLaboratorioRecepcion = EstablecimientoSeleccionado.IdEstablecimiento;
            orden.ClasificacionEstablecimiento = EstablecimientoSeleccionado.clasificacion;
            
            return PartialView("ReferenciarMuestra", orden);
        }

        #region recepcionar
        /// <summary>
        /// Descripción: Registra la recepcion y/o rechazo de muestras
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        ///                     26/09/2017
        /// Modificación: Se agregaron comentarios.
        ///               Se agregó parametr NRO OFICIO
        /// </summary>
        /// <param name="page"></param>
        /// <param name="id"></param>
        /// <param name="idOrdenRecepcion"></param>
        /// <param name="fechaRecepcion"></param>
        /// <param name="horaRecepcion"></param>
        /// <param name="esConforme"></param>
        /// <param name="motivoRechazo"></param>
        /// <param name="esConforme2"></param>
        /// <param name="generoOrden"></param>
        /// <param name="orden"></param>
        /// <param name="idExamenMuestra"></param>
        /// <param name="cmbIdLabRecepcion"></param>
        /// <param name="nroOficio"></param>
        /// <returns></returns>
        [HttpPost]
        public string RecepcionarMuestra(int? page, string id, string[] idOrdenRecepcion, string[] fechaRecepcion, string[] horaRecepcion,
                                         string[] esConforme, string[] motivoRechazo, string[] esConforme2, string generoOrden, Orden orden,
                                         string[] idExamenMuestra, string cmbIdLabRecepcion, string nroOficio, string idestablecimientoEnvio, string hddDatoEESSEnvio, string fechaIngresoINS, string fechaReevaluacionFicha)
        {
            var idOrdenR = Guid.Parse(id);

            var idLaboratorioRecepcion = EstablecimientoSeleccionado.IdEstablecimiento;

            var totalMuestrasxRecepcion = 0;
            var totalMuestrasValidas = 0;
            var totalMuestrasInvalidas = 0;

            var listaMuestraRecepcion = new List<OrdenMuestraRecepcion>();
            var listaParaRecepcionar = new List<OrdenMuestraRecepcion>();

            var ordenMuestraBl = new OrdenMuestraBl();
            var tamanioFor = idOrdenRecepcion.Length;
            var totalMuestras = tamanioFor;
            var rechazos = new string[tamanioFor];
            var fechaRom = new DateTime?();
            var fechaReeval = new DateTime?();
            if (!string.IsNullOrEmpty(fechaIngresoINS))
            {
                fechaRom = DateTime.ParseExact(fechaIngresoINS, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            else
            {
                fechaRom = null;
            }
            if (!string.IsNullOrEmpty(fechaReevaluacionFicha))
            {
                fechaReeval = DateTime.ParseExact(fechaReevaluacionFicha, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            else
            {
                fechaReeval = null;
            }
            //Falta asingar fecha en nullll
            var listaRechazosCriterioTmp = new List<List<CriterioRechazo>>();
            var listaRechazosRegistrar = new List<List<CriterioRechazo>>();
            Orden _orden = new Orden() {
                idOrden = Guid.Parse(id),                
                fechaIngresoINS = fechaRom,
                fechaReevaluacionFicha = fechaReeval,
                nroOficio = nroOficio,
                idEstablecimientoEnvio = int.Parse(string.IsNullOrEmpty(hddDatoEESSEnvio)? "0": hddDatoEESSEnvio)
            };

            var listFr = new List<DateTime>();
            for (var i = 0; i < tamanioFor; i++)
            {
                var recepcionar = Request.Params["chkOm_" + (idOrdenRecepcion[i])] != null;

                var muestraRecepcion = new OrdenMuestraRecepcion
                {
                    idOrdenMuestraRecepcion = Guid.Parse(idOrdenRecepcion[i]),
                    fechaRecepcion = DateTime.ParseExact(fechaRecepcion[i], "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    horaRecepcion = DateTime.Parse(horaRecepcion[i]),
                    idExamen = Guid.Parse(idExamenMuestra[i]),
                    conforme = (recepcionar ? 1 : 0)
                };
                listFr.Add((DateTime)muestraRecepcion.fechaRecepcion);
                if (recepcionar) continue;

                rechazos[i] = Request.Params["motivoRechazo" + i];
                var listaRechazos = rechazos[i].Split(',');

                var criterioRechazoTmp = listaRechazos.Select(t => new CriterioRechazo
                {
                    IdOrdenMuestraRecepcion = muestraRecepcion.idOrdenMuestraRecepcion,
                    IdCriterioRechazo = int.Parse(t)
                }).ToList();

                listaRechazosCriterioTmp.Add(criterioRechazoTmp);

                //solo se recepcionaran en una primera instancia las ordenes muestra que hayan sido rechazadas, las demas pasaran a la sgte. pantalla la referencia.
                listaMuestraRecepcion.Add(muestraRecepcion);
            }
            Session["fechaRecepcion"] = listFr;
            //registro de recepciones
            if (listaMuestraRecepcion.Count > 0)
            {
                totalMuestrasxRecepcion = listaMuestraRecepcion.Count;

                var laboratorioExamenBl = new LaboratorioExamenBl();
                var listaExamenesValidos = laboratorioExamenBl.GetExamenesByLaboratorio(idLaboratorioRecepcion);

                if (listaExamenesValidos != null)
                {
                    foreach (var itemR in listaMuestraRecepcion)
                    {
                        var examenValido = listaExamenesValidos.FindAll(p => p.IdExamen == itemR.idExamen);

                        if (examenValido.Any())
                        {
                            listaParaRecepcionar.Add(itemR);
                            totalMuestrasValidas = totalMuestrasValidas + 1;
                        }
                        else
                        {
                            totalMuestrasInvalidas = totalMuestrasInvalidas + 1;
                        }
                    }
                }
                //Existe muestras a rececepcionar válidas
                if (listaParaRecepcionar.Any())
                {
                    ordenMuestraBl.RecepcionarMuestras(listaParaRecepcionar, idOrdenR, 0, Logueado.idUsuario);
                }
            }

            //registro de rechazos
            if (listaRechazosCriterioTmp.Count > 0)
            {
                foreach (var itemListCriterio in listaRechazosCriterioTmp)
                {
                    var idOrdenMuestraRecepcionTmp = itemListCriterio.First().IdOrdenMuestraRecepcion;

                    var idOrdenMuestraRValido = listaParaRecepcionar.FindAll(p => p.idOrdenMuestraRecepcion == idOrdenMuestraRecepcionTmp);
                    if (idOrdenMuestraRValido.Count > 0)
                    {
                        listaRechazosRegistrar.Add(itemListCriterio);
                    }
                }

                ordenMuestraBl.RegistrarCriteriosRechazos(listaRechazosRegistrar, Logueado.idUsuario);
                //Autor : Marcos Mejia 
                //Descricpion : Envio de correo a UTM informando rechazo.
                //Fecha: 25/09/2018
                if (listaRechazosRegistrar.Count > 0)
                {
                    //                   
                    try
                    {
                        new NetLab.Extensions.Request.SendRquest().EnviarResultados(idOrdenR.ToString());
                    }
                    catch (Exception)
                    {

                    }
                    var usuario = ordenMuestraBl.ConsultaDatosUsuario(idOrdenR);
                    var usuarioReceptor = usuario.idUsuarioReceptor;
                    if (usuario.idUsuario != Convert.ToInt16(usuarioReceptor) || Convert.ToInt16(usuario.idUsuarioReceptor) != 0)
                    {
                        orden.usuarioEnvio = new UsuarioEnvioSms();
                        orden.usuarioEnvio = usuario;
                        var correo = new EnvioCorreo();
                        string destinatario = orden.usuarioEnvio.Correo;
                        string asunto = "Rechazo de muestra Neltab v2";
                        string contenido = "Estimado(a) usuario: " + usuario.Nombre + "\n" + "Se rechazó la muestra con Código de Orden N° " + usuario.codigoOrden +
                                           ", para mayor detalle revisar la bandeja de Muestras Rechazadas en Netlabv2";
                        correo.EnviarCorreo(destinatario, asunto, contenido);

                        if (usuario.Celular != null || usuario.Celular != "")
                        {
                            string message = "Estimado(a) usuario, se rechazo un examen con Codigo de Orden " +  usuario.codigoOrden + " revise su bandeja de Muestras Rechazadas en el sistema Netlab v2.";
                            string send = correo.NewSendAlertaPaciente(usuario.Celular,message);
                        }
                    }
                }
            }

            if (totalMuestrasInvalidas > 0)
            {
                ViewBag.muestrasInvalidas = totalMuestrasInvalidas;
            }

            //Autor : Juan Muga 
            //Descricpion : Registro/Modificacion de campo Nro Oficio por el ROM.
            //Fecha: 26/09/2017
            new OrdenBl().UpdateNumeroOficio(_orden);

            return totalMuestrasInvalidas + "_" + id + "_" + generoOrden + "_" + idLaboratorioRecepcion + "_" + totalMuestrasxRecepcion + "_" + totalMuestrasValidas + "_" + totalMuestras;
        }

        #endregion

        #region referenciar
        /// <summary>
        /// Descripción: Registra la referencia a otro lab y/o
        ///              Registra la recepcion de las muestras.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="id"></param>
        /// <param name="codigoOrden"></param>
        /// <param name="idLab"></param>
        /// <param name="idOrdenRecepcion"></param>
        /// <param name="idOrdenReferencia"></param>
        /// <param name="checkConformePre"></param>
        /// <param name="idLaboratorioDestino"></param>
        /// <param name="fechaEnvio"></param>
        /// <param name="horaEnvio"></param>
        /// <param name="idExamen"></param>
        /// <param name="orden"></param>
        /// <param name="idExamenReferencia"></param>
        /// <param name="idTipoMuestra"> Juan Muga </param>
        /// <returns></returns>
        [HttpPost]
        public string ReferenciarMuestras(int? page, string id, string codigoOrden, string idLab, string[] idOrdenRecepcion, string[] idOrdenReferencia, string[] checkConformePre,
            string[] idLaboratorioDestino, string[] fechaEnvio, string[] horaEnvio, string[] idExamen, Orden orden, string[] idExamenReferencia, string[] idTipoMuestra)
        {
            var listaOrdenesMuestra = new List<OrdenMuestraRecepcion>();
            var listaOrdenesMuestraCrearReferenciar = new List<OrdenMuestraRecepcion>();
            var listaOrdenesMuestraCrearRecepcionar = new List<OrdenMuestraRecepcion>();
            var listaOrdenesMuestraRecepcionar = new List<OrdenMuestraRecepcion>();
            var listaParaReferenciarCrear = new List<OrdenMuestraRecepcion>();
            var listaParaRecepcionarCrear = new List<OrdenMuestraRecepcion>();
            var listaParaRecepcionarCrearSinRecepcion = new List<OrdenMuestraRecepcion>();
            var listaOrdenesDerivarMuestra = new List<OrdenMuestraRecepcion>();//juanmuga
            var listaFechasRecepcion = new List<DateTime>();
            listaFechasRecepcion = (List<DateTime>)Session["fechaRecepcion"];
            Session["fechaRecepcion"] = null;

            var totalMuestrasInvalidasR = 0;
            var totalMuestrasRecepcionadas = 0;
            var totalMuestras = 0;

            var idOrdenMuestraPadre = new Guid();
            var ordenMuestraBl = new OrdenMuestraBl();

            if (idOrdenRecepcion == null)
                return totalMuestrasInvalidasR + "_" + totalMuestrasRecepcionadas + "_" + totalMuestras;

            var tamanioFor = idOrdenRecepcion.Length;
            totalMuestras = tamanioFor;

            for (var i = 0; i < tamanioFor; i++)
            {
                var nameChekBox = "chkNom_" + (idOrdenRecepcion[i]);
                var nameLabDestino = "cmbLab_" + (idOrdenRecepcion[i]);
                var nameFechaEnvio = "txtFec_" + (idOrdenRecepcion[i]);
                var nameHoraEnvio = "txtHor_" + (idOrdenRecepcion[i]);
                var nameChekBoxDer = "chkDer_" + (idOrdenRecepcion[i]);
                var nameTipoMuestra = "lblTipoMuestra_" + (idOrdenRecepcion[i]);

                var chckReferenciar = Request.Params[nameChekBox];
                var cmbLabDestino = Request.Params[nameLabDestino];
                var txtFechaEnvio = Request.Params[nameFechaEnvio];
                var txtHoraEnvio = Request.Params[nameHoraEnvio];
                var chckDerivar = Request.Params[nameChekBoxDer];
                var txtTipoMuestra = Request.Params[nameTipoMuestra];

                var referenciar = chckReferenciar != null;
                var derivar = chckDerivar != null;

                if (referenciar)
                {
                    var muestraReferencia = new OrdenMuestraRecepcion();
                    if (idOrdenReferencia[i] != "")
                    {
                        muestraReferencia.idOrdenMuestraRecepcion = Guid.Parse(idOrdenRecepcion[i]);
                        muestraReferencia.fechaEnvio = DateTime.ParseExact(txtFechaEnvio, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        muestraReferencia.horaEnvio = DateTime.Parse(txtHoraEnvio);
                        muestraReferencia.fechaRecepcion = listaFechasRecepcion[i];
                        muestraReferencia.idLaboratorioDestino = int.Parse(cmbLabDestino);
                        muestraReferencia.idLaboratorioOrigen = EstablecimientoSeleccionado.IdEstablecimiento;
                        idOrdenMuestraPadre = muestraReferencia.idOrdenMuestraRecepcion;
                        muestraReferencia.idExamen = Guid.Parse(idExamen[i]); // Jmuga Dato para la validacion
                        listaOrdenesMuestra.Add(muestraReferencia);
                    }
                    else
                    {
                        muestraReferencia.fechaEnvio = DateTime.ParseExact(txtFechaEnvio, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        muestraReferencia.horaEnvio = DateTime.Parse(txtHoraEnvio);
                        muestraReferencia.idLaboratorioDestino = int.Parse(cmbLabDestino);
                        muestraReferencia.idLaboratorioOrigen = EstablecimientoSeleccionado.IdEstablecimiento;
                        muestraReferencia.idExamen = Guid.Parse(idExamen[i]);
                        muestraReferencia.idOrden = Guid.Parse(id);
                        muestraReferencia.idTipoMuestra = ObtieneTipoMuestraOrdenMuestra(Guid.Parse(idExamen[i]), idTipoMuestra);
                        muestraReferencia.idOrdenMuestraRecepcionPadre = Guid.Parse(idOrdenReferencia[0]);
                        listaOrdenesMuestraCrearReferenciar.Add(muestraReferencia);
                    }
                }
                else
                {
                    //juan muga inicio
                    if (derivar)
                    {
                        //recepcionar y derivar
                        var muestraRecepcionDerivar = new OrdenMuestraRecepcion();
                        muestraRecepcionDerivar.derivar = Convert.ToInt32(derivar);
                        muestraRecepcionDerivar.idTipoMuestra = ObtieneTipoMuestraOrdenMuestra(Guid.Parse(idExamen[i]), idTipoMuestra);
                        if (idOrdenReferencia[i] != "")
                        {
                            muestraRecepcionDerivar.idOrdenMuestraRecepcion = Guid.Parse(idOrdenRecepcion[i]);
                            muestraRecepcionDerivar.fechaEnvio = DateTime.ParseExact(txtFechaEnvio, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            muestraRecepcionDerivar.horaEnvio = DateTime.Parse(txtHoraEnvio);
                            muestraRecepcionDerivar.idLaboratorioDestino = int.Parse(cmbLabDestino);
                            muestraRecepcionDerivar.idLaboratorioOrigen = EstablecimientoSeleccionado.IdEstablecimiento;
                            idOrdenMuestraPadre = muestraRecepcionDerivar.idOrdenMuestraRecepcion;

                            listaOrdenesDerivarMuestra.Add(muestraRecepcionDerivar);

                            muestraRecepcionDerivar.idOrdenMuestraRecepcion = Guid.Parse(idOrdenRecepcion[i]);
                            muestraRecepcionDerivar.fechaRecepcion = listaFechasRecepcion[i];//DateTime.Now;
                            muestraRecepcionDerivar.horaRecepcion = DateTime.Now;
                            muestraRecepcionDerivar.idExamen = Guid.Parse(idExamen[i]);
                            muestraRecepcionDerivar.conforme = 1;
                            listaOrdenesMuestraRecepcionar.Add(muestraRecepcionDerivar);
                        }
                        else
                        {
                            muestraRecepcionDerivar.fechaEnvio = DateTime.ParseExact(txtFechaEnvio, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            muestraRecepcionDerivar.horaEnvio = DateTime.Parse(txtHoraEnvio);
                            muestraRecepcionDerivar.idLaboratorioDestino = int.Parse(cmbLabDestino);
                            muestraRecepcionDerivar.idLaboratorioOrigen = EstablecimientoSeleccionado.IdEstablecimiento;
                            muestraRecepcionDerivar.idExamen = Guid.Parse(idExamen[i]);
                            muestraRecepcionDerivar.idOrden = Guid.Parse(id);
                            muestraRecepcionDerivar.idOrdenMuestraRecepcionPadre = idOrdenMuestraPadre;
                            listaOrdenesMuestraCrearReferenciar.Add(muestraRecepcionDerivar);
                        }
                    }
                    //juan muga fin
                    else //Recepcionar
                    {
                        var muestraRecepcion = new OrdenMuestraRecepcion();
                        if (idOrdenReferencia[i] != "")
                        {
                            muestraRecepcion.idOrdenMuestraRecepcion = Guid.Parse(idOrdenRecepcion[i]);
                            muestraRecepcion.fechaRecepcion = listaFechasRecepcion[i];//DateTime.Now;
                            muestraRecepcion.horaRecepcion = DateTime.Now;
                            muestraRecepcion.idExamen = Guid.Parse(idExamen[i]);
                            muestraRecepcion.conforme = 1;
                            muestraRecepcion.idTipoMuestra = ObtieneTipoMuestraOrdenMuestra(Guid.Parse(idExamen[i]), idTipoMuestra);
                            listaOrdenesMuestraRecepcionar.Add(muestraRecepcion);
                        }
                        else
                        {
                            //cuando es nueva alicuota y se recepcionará
                            muestraRecepcion.fechaRecepcion = listaFechasRecepcion[i];//DateTime.Now;
                            muestraRecepcion.horaRecepcion = DateTime.Now;
                            muestraRecepcion.idExamen = Guid.Parse(idExamen[i]);
                            muestraRecepcion.idOrden = Guid.Parse(id);
                            muestraRecepcion.idOrdenMuestraRecepcionPadre = idOrdenMuestraPadre;
                            muestraRecepcion.idTipoMuestra = ObtieneTipoMuestraOrdenMuestra(Guid.Parse(idExamen[i]), idTipoMuestra);
                            listaOrdenesMuestraCrearRecepcionar.Add(muestraRecepcion);
                        }
                    }
                }
            }
            var xOrdenmuestraRom = new OrdenMuestraRom()
            {
                listaOrdenesMuestra = listaOrdenesMuestra,
                listaOrdenesMuestraCrearReferenciar = listaOrdenesMuestraCrearReferenciar,
                listaOrdenesMuestraCrearRecepcionar = listaOrdenesMuestraCrearRecepcionar,
                listaOrdenesMuestraRecepcionar = listaOrdenesMuestraRecepcionar,
                listaParaReferenciarCrear = listaParaReferenciarCrear,
                listaParaRecepcionarCrear = listaParaRecepcionarCrear,
                listaParaRecepcionarCrearSinRecepcion = listaParaRecepcionarCrearSinRecepcion,
                listaOrdenesDerivarMuestra = listaOrdenesDerivarMuestra
            };
            return ordenMuestraBl.OrdenMuestraProcesoRom(xOrdenmuestraRom, Guid.Parse(id), Logueado.idUsuario, EstablecimientoSeleccionado.IdEstablecimiento);
        }
        public int ObtieneTipoMuestraOrdenMuestra(Guid idexamen, string[] idTipoMuestra)
        {
            var rTipoMuestra = new ExamenTipoMuestra();
            int i = 0;
            foreach (string item in idTipoMuestra)
            {
                rTipoMuestra = new ExamenTipoMuestraBl().GetTipoMuestraByExamen(idexamen).FindAll(p => p.IdTipoMuestra == int.Parse(item)).FirstOrDefault();
                if (rTipoMuestra != null)
                {
                    i++;
                    break;
                }
            }
            return i > 0 ? rTipoMuestra.IdTipoMuestra : -1;
        }
        #endregion
        /// <summary>
        ///  Descripción: Registra la referencia a otro laboratorio y/o
        ///               Registra la recepcion de las muestras.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="codigo"></param>
        /// <returns>, string CodigoOrdenMuestraRecepcion</returns>
        [HttpGet]
        public ActionResult OrdenMuestraRecepcionada(string id, string codigo)
        {
            var idOrdenG = Guid.Parse(id);
            ViewBag.codigoOrden = codigo;

            var ordenMuestraBl = new OrdenMuestraBl();

            var orden = ordenMuestraBl.GetInfoOrden(idOrdenG, EstablecimientoSeleccionado.IdEstablecimiento);

            var materialesOrdenes = ordenMuestraBl.MaterialesByOrden(idOrdenG, EstablecimientoSeleccionado.IdEstablecimiento);
            orden.ordenMaterialVdList = materialesOrdenes;

            var muestraRecepcion = ordenMuestraBl.MuestraRecepcionadosbyOrden(idOrdenG, 0, Logueado.idUsuario, 3, 1, 0, EstablecimientoSeleccionado.IdEstablecimiento);//.Where(x => x.idOrdenMuestraRecepcion == Guid.Parse(CodigoOrdenMuestraRecepcion)).ToList();

            var muestrasOrdenesMaterial = ordenMuestraBl.MuestrasByOrdenRecepcionadas(idOrdenG);


            if (muestrasOrdenesMaterial != null && muestraRecepcion != null)
            {
                foreach (var item in muestrasOrdenesMaterial)
                {
                    item.ordenMuestraRecepcionList = muestraRecepcion.FindAll(p => p.idMaterial == item.idMaterial).ToList();
                    var cantidadMateriales = item.ordenMuestraRecepcionList.Count;
                    var nroMaterial = 1;
                    foreach (var itemA in item.ordenMuestraRecepcionList)
                    {
                        if (nroMaterial > cantidadMateriales) continue;
                        itemA.presentacionNombreNro = itemA.presentacionNombreNro + " #" + nroMaterial;
                        nroMaterial++;
                    }
                }
            }

            orden.ordenMaterialList = muestrasOrdenesMaterial;
            orden.codigoOrden = codigo;
            orden.idOrden = Guid.Parse(id);

            return View(orden);

        }

        /// <summary>
        /// Descripción: obtiene el o los EESS/Laboratorios
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <returns></returns>
        public string GetAllLaboratorios()
        {
            var data = Request.Params["data[q]"];
            //ILaboratorioBl laboratorioBl = new LaboratorioBl();
            //var laboratorioList = laboratorioBl.GetLaboratoriosByTextoBusqueda(data, ((Usuario)Session["UsuarioLogin"]).idUsuario);
            var laboratorioList = StaticCache.ObtenerLaboratorios();
            laboratorioList = string.IsNullOrWhiteSpace(data) ? laboratorioList : laboratorioList.Where(x => x.NombreEstablecimiento.ToLower().Contains(data.Trim().ToLower()) || x.CodigoUnico.ToLower().Contains(data.Trim().ToLower())).ToList();

            var resultado = "{\"q\":\"" + data + "\",\"results\":[";
            var existeLaboratorio = false;

            if (Request.Params["derivar"] == "on")
            {
                var laboratorioList2 = (from x in laboratorioList where x.clasificacion.Contains("LAB INS") select x).ToList();
                Session["laboratorioList"] = laboratorioList2;

                foreach (var laboratorio in laboratorioList2)
                {
                    var nombre = laboratorio.Nombre.Replace("\"", "");

                    resultado += "{\"id\":\"" + laboratorio.IdEstablecimiento + "\",\"text\":\"" + nombre + "\"},";
                    existeLaboratorio = true;
                }
            }
            else
            {
                Session["laboratorioList"] = laboratorioList;
                string dato = Convert.ToString(Session["tipoRegistro"]);

                var tipoRegistro = String.IsNullOrEmpty(dato) ? Enums.TipoRegistroOrden.ORDEN_RECEPCION : (Enums.TipoRegistroOrden)Session["tipoRegistro"];
                if ((tipoRegistro == Enums.TipoRegistroOrden.ORDEN_RECEPCION || tipoRegistro == Enums.TipoRegistroOrden.EDITAR_ORDEN_DATOCLINICO) && EstablecimientoSeleccionado.IdEstablecimiento == 991)
                {
                    var laboratorioList2 = (from x in laboratorioList where x.clasificacion.Contains("LAB INS") select x).ToList();
                    Session["laboratorioList"] = laboratorioList2;
                    foreach (var laboratorio in laboratorioList2)
                    {
                        var nombre = laboratorio.Nombre.Replace("\"", "");

                        resultado += "{\"id\":\"" + laboratorio.IdEstablecimiento + "\",\"text\":\"" + nombre + "\"},";
                        existeLaboratorio = true;
                    }
                }
                else
                {
                    var laboratorioList3 = (from x in laboratorioList where !x.clasificacion.Contains("LAB INS") select x).ToList();
                    Session["laboratorioList"] = laboratorioList3;
                    foreach (var laboratorio in laboratorioList3)
                    {
                        var nombre = laboratorio.Nombre.Replace("\"", "");

                        resultado += "{\"id\":\"" + laboratorio.IdEstablecimiento + "\",\"text\":\"" + nombre + "\"},";
                        existeLaboratorio = true;
                    }
                }
            }

            if (existeLaboratorio)
                resultado = resultado.Substring(0, resultado.Length - 1) + "]}";
            else
                resultado = resultado.Substring(0, resultado.Length) + "]}";

            return resultado;
        }

        /// <summary>
        /// Descripción: Metodo que retorna examenes concatenado con el codigo y la descripcion.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="genero"></param>
        /// <param name="idlab"></param>
        /// <returns></returns>
        public String GetExamenes()
        {
            String data = this.Request.Params["data[q]"];
            int idlab = int.Parse(Request.Params["idLaboratorio"]);
            int genero = int.Parse(Request.Params["genero"]);
            var Derivar = Request.Params["derivar"];
            IExamenBl examenBl = new ExamenBl();

            List<Examen> examenList = examenBl.GetExamenesByIdLaboratorioRecepcion(idlab, genero, data);
            //List<Examen> examenList = examenBl.GetExamenesByGenero(genero, data);

            String resultado = "{\"q\":\"" + data + "\",\"results\":[";


            Boolean existeEnfermedad = false;
            foreach (Examen examen in examenList)
            {
                resultado += "{\"id\":\"" + examen.idExamen + "\",\"text\":\"" + examen.nombre.TrimEnd() + "\"},";
                existeEnfermedad = true;
            }

            if (existeEnfermedad)
                resultado = resultado.Substring(0, resultado.Length - 1) + "]}";
            else
                resultado = resultado.Substring(0, resultado.Length) + "]}";

            this.Session["examenesSeleccionados"] = new String[0];
            this.Session["tiposMuestraSeleccionados"] = new String[0];
            this.Session["materialesSeleccionados"] = new String[0];

            /*Se limpian las variables de Session de listas*/
            this.Session["tipoMuestraList"] = new List<TipoMuestra>();
            //this.Session["materialList"] = new List<MaterialVd>();

            /*Se agrega la nueva lista de examenes de la enfermedad seleccionada*/
            this.Session["examenList"] = examenList;
            var model = examenList;
            return resultado;
        }
        #region  BUSQUEDA MUESTRA REFERENCIAR
        /// <summary>
        /// Descripción: obtiene el o los EESS/Laboratorios
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <returns></returns>
        public ActionResult BusquedaMuestraReferenciar()
        {
            ViewBag.fechaDesde = DateTime.Now.ToDefaultDateFormatString();
            ViewBag.fechaHasta = DateTime.Now.ToDefaultDateFormatString();

            return BusquedaMuestraReferenciar(1, "1", ViewBag.fechaDesde, ViewBag.fechaHasta, "", "", "1", null, "");
        }

        /// <summary>
        /// Descripción: Realiza busqueda de muestras-ordenes para referenciar.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="esFechaRegistro"></param>
        /// <param name="fechaDesde"></param>
        /// <param name="fechaHasta"></param>
        /// <param name="codigoOrden"></param>
        /// <param name="nroOficio"></param>
        /// <param name="estadoOrden"></param>
        /// <param name="tipoOrden"></param>
        /// <param name="idMuestra"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult BusquedaMuestraReferenciar(int? page, string esFechaRegistro, string fechaDesde, string fechaHasta, string codigoOrden,
                                                        string nroOficio, string estadoOrden, string tipoOrden, string idMuestra)
        {
            var estadoOrdenF = 0;
            var tipoOrdenF = 0;

            if (estadoOrden != null)
                estadoOrdenF = int.Parse(estadoOrden);

            if (tipoOrden != null)
                tipoOrdenF = int.Parse(tipoOrden);

            var fechaDesdeA = new DateTime();
            var fechaHastaA = new DateTime();
            var fechaRegistro = 0;
            var pageNumber = page ?? 1;

            var fechaDesdeCriteria = fechaDesde ?? string.Empty;
            var fechaHastaCriteria = fechaHasta ?? string.Empty;

            if (!fechaDesdeCriteria.Equals(""))
                fechaDesdeA = DateTime.ParseExact(fechaDesdeCriteria, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            if (!fechaHastaCriteria.Equals(""))
                fechaHastaA = DateTime.ParseExact(fechaHastaCriteria, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            var fechaRegistroTmp = esFechaRegistro ?? string.Empty;

            if (!fechaRegistroTmp.Equals(""))
                fechaRegistro = int.Parse(fechaRegistroTmp);
            else if (fechaDesdeCriteria.Equals("") || fechaHastaCriteria.Equals(""))
                fechaRegistro = 0;
            else if (!fechaDesdeCriteria.Equals("") && !fechaHastaCriteria.Equals(""))
            {
                if (fechaDesdeA == fechaHastaA)
                {
                    fechaHastaA = fechaHastaA.AddDays(1);
                }
            }

            var codigoOrdenF = codigoOrden.Trim();
            var idMuestraF = idMuestra.Trim();
            var nroOficioA = nroOficio.Trim();
            var idLaboratorio = EstablecimientoSeleccionado?.IdEstablecimiento ?? 0;

            var ordenBl = new OrdenMuestraBl();
            var ordenesRecepcion = ordenBl.GetOrdenMuestraByEstablecimiento(3, fechaRegistro, Logueado.idUsuario, codigoOrdenF, fechaDesdeA, fechaHastaA, nroOficioA, tipoOrdenF, idMuestraF, idLaboratorio, string.Empty, string.Empty, string.Empty, string.Empty);

            if (ordenesRecepcion == null) return View();

            var pageOfOrdenMuestra = ordenesRecepcion.ToPagedList(pageNumber, 60);

            ViewBag.recepcionado = estadoOrdenF;

            var OrdenDetalleRef = ordenBl.DetalleOrdenReferencia(fechaRegistro, codigoOrden, fechaDesdeA, fechaHastaA, nroOficio, idMuestra, idLaboratorio);
            this.Session["OrdenDetalleRef"] = OrdenDetalleRef;

            return View(pageOfOrdenMuestra);
        }

        /// <summary>
        /// Descripción: Obtiene los parametros para realizar la edicion de muestras a referenciar.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="codigo"></param>
        /// <param name="genero"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditarReferencias(string id, string codigo, string genero)
        {
            var idOrdenG = Guid.Parse(id);
            var ordenMuestraBl = new OrdenMuestraBl();

            var orden = ordenMuestraBl.GetInfoOrden(idOrdenG, EstablecimientoSeleccionado.IdEstablecimiento);

            var materialesOrdenes = ordenMuestraBl.MaterialesRefenciadosByOrden(idOrdenG, EstablecimientoSeleccionado.IdEstablecimiento);
            orden.ordenMaterialVdList = materialesOrdenes;

            //obtener muestras referenciadas
            var muestraRecepcion = ordenMuestraBl.MuestraReferenciadosEditar(idOrdenG, Logueado.idUsuario, 2, EstablecimientoSeleccionado.IdEstablecimiento);

            //lista de materiales para agrupar las muestras por tipo de muestra
            var muestrasOrdenesMaterial = ordenMuestraBl.MuestrasByOrden(idOrdenG);
            if (muestrasOrdenesMaterial != null && muestraRecepcion != null)
            {
                foreach (var item in muestrasOrdenesMaterial)
                {
                    item.ordenMuestraRecepcionList = muestraRecepcion.FindAll(p => p.idMaterial == item.idMaterial).ToList();
                    var cantidadMateriales = item.ordenMuestraRecepcionList.Count;
                    var nroMaterial = 1;
                    foreach (var itemA in item.ordenMuestraRecepcionList)
                    {
                        if (nroMaterial > cantidadMateriales) continue;
                        itemA.presentacionNombreNro = itemA.presentacionNombreNro + " #" + nroMaterial;
                        nroMaterial++;
                    }
                }
            }

            orden.ordenMaterialList = muestrasOrdenesMaterial;
            orden.genero = int.Parse(genero);

            return PartialView("EditarReferencias", orden);
        }
        /// <summary>
        /// Descripción: Luego del Postback ejecuta la edicion de datos en la referenciación.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="codigoOrden"></param>
        /// <param name="idOrdenRecepcion"></param>
        /// <param name="idLaboratorioDestino"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditarReferencias(string id, string codigoOrden, string[] idOrdenRecepcion, string[] idLaboratorioDestino)
        {
            var listaOrdenesEditar = new List<OrdenMuestraRecepcion>();
            var tamanioFor = idOrdenRecepcion.Length;

            for (var i = 0; i < tamanioFor; i++)
            {
                var chckEditar = Request.Params["chkNom_" + (idOrdenRecepcion[i])];
                var editar = chckEditar != null;

                if (!editar) continue;

                var muestraEditar = new OrdenMuestraRecepcion
                {
                    idOrdenMuestraRecepcion = Guid.Parse(idOrdenRecepcion[i]),
                    idLaboratorioDestino = int.Parse(idLaboratorioDestino[i])
                };

                listaOrdenesEditar.Add(muestraEditar);
            }

            if (listaOrdenesEditar.Count <= 0)
                return RedirectToAction("BusquedaMuestraRecepcionar");

            var ordenMuestraBl = new OrdenMuestraBl();
            ordenMuestraBl.EditarLaboratoriosMuestras(listaOrdenesEditar);

            return RedirectToAction("BusquedaMuestraRecepcionar");
        }
        /// <summary>
        /// Descripción: Recepcion masiva de muestras.
        /// Author: Terceros.
        /// Fecha Creacion: 26/11/2017
        /// Fecha Modificación: 26/11/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="datos"></param>
        /// <returns></returns>
        public string RecepcionMuestrasMasivo(string datos)
        {
            string rpta = "";
            var ordenMuestraBl = new OrdenMuestraBl();
            int idUsuario = Logueado.idUsuario;
            if (datos != null)
            {
                if (ordenMuestraBl.RecepcionarMuestrasMasivo(datos, idUsuario))
                {

                    rpta = "ok";
                }
            }
            return rpta;
        }

        #endregion


        /// Descripción: Metodo para obtener Muestras Rechazadas por ROM.
        /// Author: Marcos Mejia
        /// Fecha Creacion: 01/12/2017


        public ActionResult BusquedaMuestraRechazar()
        {
            ViewBag.fechaDesde = DateTime.Now.ToDefaultDateFormatString();
            ViewBag.fechaHasta = DateTime.Now.ToDefaultDateFormatString();
            //return View();

            return BusquedaMuestraRechazar(1, "1", ViewBag.fechaDesde, ViewBag.fechaHasta, "", "", "1", null, "");
        }

        [HttpPost]
        public ActionResult BusquedaMuestraRechazar(int? page, string esFechaRegistro, string fechaDesde, string fechaHasta, string codigoOrden,
                                                        string nroOficio, string estadoOrden, string tipoOrden, string idMuestra)
        {
            Session["CER_Ordenes"] = null;
            var estadoOrdenF = 0;
            var tipoOrdenF = 0;

            if (estadoOrden != null)
                estadoOrdenF = int.Parse(estadoOrden);

            if (tipoOrden != null)
                tipoOrdenF = int.Parse(tipoOrden);

            var fechaDesdeA = new DateTime();

            var fechaHastaA = new DateTime();
            var fechaRegistro = 0;
            var pageNumber = page ?? 1;

            var fechaDesdeCriteria = fechaDesde ?? string.Empty;
            var fechaHastaCriteria = fechaHasta ?? string.Empty;

            if (!fechaDesdeCriteria.Equals(""))
                fechaDesdeA = DateTime.ParseExact(fechaDesdeCriteria, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            if (!fechaHastaCriteria.Equals(""))
                fechaHastaA = DateTime.ParseExact(fechaHastaCriteria, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            var fechaRegistroTmp = esFechaRegistro ?? string.Empty;
            if (!fechaRegistroTmp.Equals(""))
                fechaRegistro = int.Parse(fechaRegistroTmp);
            else if (fechaDesdeCriteria.Equals("") || fechaHastaCriteria.Equals(""))
                fechaRegistro = 0;
            else if (!fechaDesdeCriteria.Equals("") && !fechaHastaCriteria.Equals(""))
            {
                if (fechaDesdeA == fechaHastaA)
                {
                    fechaHastaA = fechaHastaA.AddDays(1);
                }
            }

            var codigoOrdenF = codigoOrden.Trim();
            var idMuestraF = idMuestra.Trim();
            var nroOficioA = nroOficio.Trim();
            var idLaboratorio = EstablecimientoSeleccionado?.IdEstablecimiento ?? 0;

            var ordenBl = new OrdenMuestraBl();
            var ordenesRecepcion = ordenBl.GetOrdenMuestraByEstablecimientoRechazado(estadoOrdenF, fechaRegistro, Logueado.idUsuario, codigoOrdenF, fechaDesdeA, fechaHastaA, nroOficioA, tipoOrdenF, idMuestraF, idLaboratorio);
            Session["CER_Ordenes"] = ordenesRecepcion;
            if (ordenesRecepcion == null) return View();

            var pageOfOrdenMuestra = ordenesRecepcion.ToPagedList(pageNumber, 60);

            ViewBag.recepcionado = estadoOrdenF;

            var ordenMuestraBL = new OrdenMuestraBl();
            //var OrdenDetalle = ordenMuestraBL.MuestrasByOrdenDetalle(idLaboratorio);
            //this.Session["OrdenDetalle"] = OrdenDetalle;
            return View(pageOfOrdenMuestra);
        }


        public ActionResult VerRechazo(string id, string codigo)
        {
            var ordenBl = new IngresoResultadosBl();
            var idEstablecimientoLogin = EstablecimientoSeleccionado.IdEstablecimiento;

            var ordenMuestraBl = new OrdenMuestraBl();
            var ordenExamen = ordenMuestraBl.GetInfoOrden(Guid.Parse(id), EstablecimientoSeleccionado.IdEstablecimiento);


            var orden = ordenBl.GetMuestrasValidadas(Guid.Parse(id), Logueado.idUsuario, idEstablecimientoLogin);

            orden.ordenInfoListnew = ordenExamen.ordenInfoList.ToList();

            var listaCriterios = new Dictionary<int, List<SelectListItem>>();
            var criteriosBl = new CriterioRechazoBl();

            var fechaColeccion = DateTime.MinValue;

            foreach (var item in orden.muestrasValidar)
            {

                if (fechaColeccion < item.fechaColeccion)
                    fechaColeccion = item.fechaColeccion;

                if (listaCriterios.ContainsKey(item.idTipoMuestra)) continue;

                listaCriterios[item.idTipoMuestra] = new List<SelectListItem>();

                foreach (var crItem in criteriosBl.GetCriteriosByTipoMuestra(item.idTipoMuestra, 3))
                {
                    listaCriterios[item.idTipoMuestra].Add(new SelectListItem
                    {
                        Text = crItem.Glosa,
                        Value = crItem.IdCriterioRechazo.ToString()
                    });
                }
            }

            ViewBag.listaCriterios = listaCriterios;
            return PartialView("MuestrasValidadas", orden);
        }
        [HttpPost]
        public ActionResult ShowLevantarObservaciones(string idorden, string tipoMuestra, string idOrdenMuestraRecepcion, string idOrdenMaterial)
        {
            var ordenMuestraBl = new OrdenMuestraBl();
            OrdenBl ordenBl = new OrdenBl();
            Orden orden = ordenBl.GetOrdenById(Guid.Parse(idorden));
            var listaBl = new ListaBl();
            var tipoDocumentoList = new List<SelectListItem>();

            foreach (var itemDetalle in listaBl.GetListaByOpcion(OpcionLista.TipoDocumento).ListaDetalle)
                tipoDocumentoList.Add(new SelectListItem
                {
                    Text = itemDetalle.Glosa,
                    Value = Convert.ToString(itemDetalle.IdDetalleLista)
                });
            var ordenCriterioRechazo = ordenMuestraBl.GetOrdenCriterioRechazo(Guid.Parse(idorden));
            ViewBag.TipoRechazoAlta = ordenCriterioRechazo;
            ViewBag.tipoDocumentoList = tipoDocumentoList;
            Session["ordenCriterioRechazo"] = ordenCriterioRechazo;
            //Session["tipoDocumentoList"] = tipoDocumentoList;
            orden.ordenMuestraList = orden.ordenMuestraList.Where(p => p.TipoMuestra.nombre == tipoMuestra).ToList();

            var x = new OrdenMuestraRecepcion();
            x.idOrdenMuestraRecepcion = Guid.Parse(idOrdenMuestraRecepcion);
            var list = new List<OrdenMuestraRecepcion>();
            list.Add(x);
            orden.ordenMuestraRecepcionadaList = list;

            //var a = new OrdenMaterial();
            //var list2 = new List<OrdenMaterial>();
            //a.idOrdenMaterial = Guid.Parse(idOrdenMaterial);
            //list2.Add(a);
            orden.ordenMaterialList = orden.ordenMaterialList.Where(p => p.idOrdenMaterial == Guid.Parse(idOrdenMaterial)).ToList();


            //LoadSolicitantes();
            LoadNombreSolicitante(orden.solicitante);

            Session["sOrden"] = orden;
            Session["tipoRegistro"] = Enums.TipoRegistroOrden.EDITAR_ORDEN_DATOCLINICO;
            return PartialView("_LevantarObservaciones", orden);
        }


        [HttpPost]
        public ActionResult LevantarObservacionesLab(string hddRECHAZOFECHAINGRESOROM, string hddRECHAZOPACIENTE, string hddRECHAZODATOCLINICO, string hddRECHAZOCODIFICACION, string hddRECHAZOFECHAEVALFICHA, string hddRECHAZOFECHASOL, string hddRECHAZOFECHAOBTENCIONMUESTRA, string hddRECHAZOSOLICITANTE, string hddRECHAZONROOFICIO, string hddRECHAZOEESSDESTINO, string fechaIngresoINS, string fechaReevaluacionFicha, string nroOficio, string fechaSolicitud, string hddFechaObtencion, string hddHoraObtencion,
          string tipoDocumento, string nroDocumento, string idOrden, string Solicitante, Orden orden)
        {
            var bDatoClinico = Convert.ToBoolean(hddRECHAZODATOCLINICO);
            var bpaciente = Convert.ToBoolean(hddRECHAZOPACIENTE);
            var bFechaObtencion = Convert.ToBoolean(hddRECHAZOFECHAOBTENCIONMUESTRA);
            var bEESSDestino = Convert.ToBoolean(hddRECHAZOEESSDESTINO);
            var error = "";
            //string idPaciente = "";
            var sessionOrden = (Orden)Session["sOrden"];
            orden.IdUsuarioEdicion = Logueado.idUsuario;
            orden.ordenMuestraRecepcionadaList = sessionOrden.ordenMuestraRecepcionadaList;
            if (bpaciente)
            {
                orden.Paciente = new Paciente { IdPaciente = new PacienteBl().GetPacientesByTipoNroDocumento(int.Parse(tipoDocumento), nroDocumento).FirstOrDefault().IdPaciente };
            }
            else
            {
                orden.Paciente = new Paciente { IdPaciente = Guid.Empty };
            }
            if (bFechaObtencion)
            {
                var ordenMuestraListAgregados = new List<OrdenMuestra>();
                ordenMuestraListAgregados = sessionOrden.ordenMuestraList;
                foreach (var ordenMuestra in ordenMuestraListAgregados)
                {
                    var idMuestra = ordenMuestra.idOrdenMuestra;

                    var fechatmp = Request.Params["fechaMuestra" + idMuestra];
                    var fecha = fechatmp.Split('/');
                    ordenMuestra.fechaColeccion = new DateTime(Convert.ToInt32(fecha[2]), Convert.ToInt32(fecha[1]), Convert.ToInt32(fecha[0]));

                    var horatmp = Request.Params["horaMuestra" + idMuestra];
                    var hora = horatmp.Split(':');
                    ordenMuestra.horaColeccion = new DateTime(Convert.ToInt32(fecha[2]), Convert.ToInt32(fecha[1]),
                        Convert.ToInt32(fecha[0]), Convert.ToInt32(hora[0]), Convert.ToInt32(hora[1]), 0);

                }
                orden.ordenMuestraList = ordenMuestraListAgregados;
            }
            //if (bEESSDestino)
            //{
            //    List<OrdenMaterial> ordenMaterialListAgregados = new List<OrdenMaterial>();

            //    ordenMaterialListAgregados = sessionOrden.ordenMaterialList;
            //    for (int i = 0; i < ordenMaterialListAgregados.Count; i++)
            //    {
            //        var idRow = ordenMaterialListAgregados[i].idOrdenMaterial;
            //        OrdenMaterial ordenMaterial = ordenMaterialListAgregados[i];
            //        ordenMaterial.cantidad = 1;

            //        var valor = Request.Params["laboratorio" + idRow];

            //        ordenMaterial.Laboratorio = new Laboratorio();
            //        ordenMaterial.Laboratorio.IdLaboratorio = Convert.ToInt32(Request.Params["laboratorio" + idRow]);
            //    }
            //    orden.ordenMaterialList = ordenMaterialListAgregados;
            //}
            if (bDatoClinico)
            {
                List<Enfermedad> enfermedadListAgregados = new List<Enfermedad>();

                enfermedadListAgregados = sessionOrden.enfermedadList;
                List<OrdenDatoClinico> ordenDatoClinicoList = new List<OrdenDatoClinico>();
                var datosClinicos = enfermedadListAgregados.SelectMany(e => e.categoriaDatoList).SelectMany(c => c.OrdenDatoClinicoList);

                foreach (var ordenDatoClinico in datosClinicos)
                {
                    var id = ordenDatoClinico.Enfermedad.idEnfermedad.ToString() + ordenDatoClinico.Dato.IdDato.ToString();

                    OrdenDatoClinico ordenDatoClinicoFormulario =
                        ordenDatoClinicoList.FirstOrDefault(x => x.Dato.IdDato == ordenDatoClinico.Dato.IdDato);

                    //Si el dato clinico existe previamente en el formulario entonces se copia los valores del existente
                    if (ordenDatoClinicoFormulario != null)
                    {
                        ordenDatoClinico.noPrecisa = ordenDatoClinicoFormulario.noPrecisa;
                        ordenDatoClinico.ValorString = ordenDatoClinicoFormulario.ValorString;
                    }
                    else
                    {
                        //Si el dato clinico no existe se crea con los valores obtenidos
                        ordenDatoClinicoFormulario = new OrdenDatoClinico();
                        Dato datoClinico = new Dato();
                        datoClinico.IdDato = ordenDatoClinico.Dato.IdDato;
                        ordenDatoClinicoFormulario.Dato = datoClinico;
                        ordenDatoClinicoList.Add(ordenDatoClinicoFormulario);


                        var formValue = Request.Form["valueDato" + id];
                        var checkNoPrecisaBoolean = Convert.ToBoolean(Request.Form["checkNoPrecisa" + id]);

                        if ((int)Enums.TipoCampo.CHECKBOX == ordenDatoClinico.Dato.IdTipo
                            || (int)Enums.TipoCampo.COMBO == ordenDatoClinico.Dato.IdTipo)
                        {
                            ordenDatoClinico.noPrecisa = formValue == null || formValue.Equals("0");
                            ordenDatoClinico.ValorString = formValue == null || formValue.Equals("0")
                                ? ""
                                : formValue;
                        }
                        else
                        {
                            ordenDatoClinico.noPrecisa = checkNoPrecisaBoolean;
                            ordenDatoClinico.ValorString = !checkNoPrecisaBoolean ? formValue : string.Empty;
                        }

                        ordenDatoClinicoFormulario.noPrecisa = ordenDatoClinico.noPrecisa;
                        ordenDatoClinicoFormulario.ValorString = ordenDatoClinico.ValorString;
                    }
                }

                orden.enfermedadList = enfermedadListAgregados;
            }
            //Registrar y/o actualizar!!!
            try
            {
                var ordenMuestraBl = new OrdenMuestraBl();
                orden.IdUsuarioEdicion = Logueado.idUsuario;
                ordenMuestraBl.RegistrarLevantarObservacion(hddRECHAZOFECHAINGRESOROM, hddRECHAZOPACIENTE, hddRECHAZODATOCLINICO, hddRECHAZOCODIFICACION, hddRECHAZOFECHAEVALFICHA, hddRECHAZOFECHASOL, hddRECHAZOFECHAOBTENCIONMUESTRA, hddRECHAZOSOLICITANTE, hddRECHAZONROOFICIO, orden);
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return RedirectToAction("BusquedaMuestrasIngresadas");
        }

        public string ValidarRegistro(string hddRECHAZOFECHAINGRESOROM, string hddRECHAZOPACIENTE, string hddRECHAZODATOCLINICO, string hddRECHAZOCODIFICACION,
                                    string hddRECHAZOFECHAEVALFICHA, string hddRECHAZOFECHASOL, string hddRECHAZOFECHAOBTENCIONMUESTRA, string hddRECHAZOSOLICITANTE,
                                    string hddRECHAZONROOFICIO, string hddRECHAZOEESSDESTINO,
                                    string fechaIngresoINS, string fechaReevaluacionFicha, string nroOficio, string fechaSolicitud,
                                    string tipoDocumento, string nroDocumento, string idOrden, string Solicitante, string fechaObtencion, string horaObtencion)
        {
            var bFechaRom = Convert.ToBoolean(hddRECHAZOFECHAINGRESOROM);
            var bPaciente = Convert.ToBoolean(hddRECHAZOPACIENTE);
            var bDatoClinico = Convert.ToBoolean(hddRECHAZODATOCLINICO);
            var bFechaEval = Convert.ToBoolean(hddRECHAZOFECHAEVALFICHA);
            var bFechaSol = Convert.ToBoolean(hddRECHAZOFECHASOL);
            var bFechaObtencion = Convert.ToBoolean(hddRECHAZOFECHAOBTENCIONMUESTRA);
            var bSolicitante = Convert.ToBoolean(hddRECHAZOSOLICITANTE);
            var bNroOficio = Convert.ToBoolean(hddRECHAZONROOFICIO);
            var bEESSDestino = Convert.ToBoolean(hddRECHAZOEESSDESTINO);
            var sessionOrden = (Orden)Session["sOrden"];
            var error = "";
            string idPaciente = "";

            if (bFechaRom)
            {
                if (String.IsNullOrEmpty(fechaIngresoINS))
                {
                    error = error + "- La fecha de ingreso al INS, no puede estar vacía." + "\n\r";
                }
            }

            if (bFechaEval)
            {
                if (String.IsNullOrEmpty(fechaReevaluacionFicha))
                {
                    error = error + "- La fecha de evaluación de Ficha, no puede estar vacía." + "\n\r";
                }
            }

            if (bPaciente)
            {
                if (String.IsNullOrEmpty(tipoDocumento))
                {
                    error = error + "- Debe seleccionar un tipo de documento." + "\n\r";
                }
                else if (String.IsNullOrEmpty(nroDocumento))
                {
                    error = error + "- El nro de documento no puede estar vacío." + "\n\r";
                }
                else
                {
                    var existePaciente = new PacienteBl().GetPacientesByTipoNroDocumento(int.Parse(tipoDocumento), nroDocumento);
                    if (existePaciente.Count() == 0)
                    {
                        error = error + "- El paciente no existe, por favor registrarlo antes de utilizar esta opción." + "\n\r";
                    }
                    else
                    {
                        idPaciente = existePaciente.FirstOrDefault().IdPaciente.ToString();
                    }
                }
            }
            if (bFechaSol)
            {
                if (string.IsNullOrEmpty(fechaSolicitud))
                {
                    error = error + "- La fecha de solicitud no puede estar vacio." + "\n\r";
                }
            }
            if (bSolicitante)
            {
                if (string.IsNullOrEmpty(Solicitante))
                {
                    error = error + "- El solicitante no puede estar vacio." + "\n\r";
                }
            }
            if (bNroOficio)
            {
                if (string.IsNullOrEmpty(nroOficio))
                {
                    error = error + "- El Nro Oficio no puede estar vacio." + "\n\r";
                }
            }

            return error;
        }
        [HttpPost]
        public ActionResult BusquedaMuestrasIngresadas(string codigoOrden, string nroOficio, string codigoMuestra)
        {
            if (string.IsNullOrEmpty(codigoOrden) && string.IsNullOrEmpty(nroOficio) && string.IsNullOrEmpty(codigoMuestra))
            {
                return View();
            }
            var ordenMuestraBl = new OrdenMuestraBl();
            var ordenesRecepcion = ordenMuestraBl.GetOrdenMuestraIngresada(codigoOrden, nroOficio, codigoMuestra);

            Session["tipoRegistro"] = Enums.TipoRegistroOrden.EDITAR_ORDEN_DATOCLINICO;
            return View(ordenesRecepcion);
        }
        public ActionResult BusquedaMuestrasIngresadas()
        {
            return BusquedaMuestrasIngresadas("", "", "");
        }
        //public PartialViewResult LoadSolicitante()
        //{
        //    //LoadSolicitantes();
        //    return PartialView("_Solicitante");
        //}
        //private void LoadSolicitantes()
        //{
        //    List<OrdenSolicitante> solicitantes = new List<OrdenSolicitante>();
        //    SolicitanteBl solicitanteBL = new SolicitanteBl();
        //    ViewBag.Solicitantes = solicitanteBL.GetListaSolicitante().Select(s => new SelectListItem
        //    {
        //        Text = string.Format("{0} - {1} {2} {3}", s.codigoColegio, s.apellidoPaterno, s.apellidoMaterno, s.Nombres),
        //        Value = s.idSolicitante.ToString()
        //    });

        //    Session["solicitantes"] = solicitantes;
        //}
        private void LoadNombreSolicitante(string solicitante)
        {
            SolicitanteBl solicitanteBL = new SolicitanteBl();
            ViewBag.SolicitanteNombre = solicitanteBL.GetSolicitanteById(solicitante);
        }

        public ActionResult ShowPopupNuevoEnfermedadExamen(Guid idOrden)
        {
            var ordenExamenListAgregados = new List<OrdenExamen>();

            ViewBag.idOrdenAnalista = idOrden;

            ViewBag.valueLaboratorioPreSeleccionada = EstablecimientoSeleccionado.IdEstablecimiento;
            ViewBag.textoLaboratorioPreSeleccionada = EstablecimientoSeleccionado.Nombre;

            ViewBag.tipoRegistro = 3;

            ViewBag.examenList = new List<Examen>();
            ViewBag.tipoMuestraList = new List<TipoMuestra>();

            return PartialView("_FormPopupEnfermedadExamen");
        }





        public ActionResult ExportarExcel()
        {
            List<OrdenRecepcionVd> lista = new List<OrdenRecepcionVd>();
            if (this.Session["CER_Ordenes"] != null)
            {
                lista = (List<OrdenRecepcionVd>)this.Session["CER_Ordenes"];
            }

            DataTable dtlista = new DataTable("dtlista");
            dtlista = ConvertToDataTable(lista);
            dtlista.Columns.Remove("idOrden");
            dtlista.Columns.Remove("tipoOrden");
            dtlista.Columns.Remove("nroDocumento");
            dtlista.Columns.Remove("nroDocCepaBanco");
            dtlista.Columns.Remove("nroDocAnimal");
            dtlista.Columns.Remove("estadoOrden");
            dtlista.Columns.Remove("fechaRegistro");
            dtlista.Columns.Remove("EXISTE_PENDIENTE");
            dtlista.Columns.Remove("EXISTE_REFERENCIADO");
            dtlista.Columns.Remove("EXISTE_RECIBIDO");
            dtlista.Columns.Remove("EXISTE_ORDENRECHAZO");
            dtlista.Columns.Remove("EXISTE_RECHAZOLAB");
            dtlista.Columns.Remove("genero");
            dtlista.Columns.Remove("fechaSolicitud");
            dtlista.Columns.Remove("nombreGenero");
            dtlista.Columns.Remove("tipoDocumento");
            dtlista.Columns.Remove("fechaNacimiento");

            dtlista.Columns.Remove("idOrdenExamen");
            dtlista.Columns.Remove("idEstablecimiento");
            dtlista.Columns.Remove("oPaciente");
            dtlista.Columns.Remove("codigoOrden");
            dtlista.Columns.Remove("muestra");
            dtlista.Columns.Remove("tipoMuestra");
            dtlista.Columns.Remove("enfermedad");
            dtlista.Columns.Remove("resultado");
            dtlista.Columns.Remove("fechaHoraColeccion");
            dtlista.Columns.Remove("fechaHoraRecepcion");
            dtlista.Columns.Remove("fechaHoraRececpionLab");
            dtlista.Columns.Remove("fechaHoraValidado");
            dtlista.Columns.Remove("observacionRechazoLab");
            dtlista.Columns.Remove("fechaRegistroOrden");
            dtlista.Columns.Remove("ingresado");
            dtlista.Columns.Remove("validado");
            dtlista.Columns.Remove("estatusE");
            dtlista.Columns.Remove("estadoOrdenResultado");
            dtlista.Columns.Remove("idOrdenMuestraRecepcion");
            dtlista.Columns.Remove("idOrdenMaterial");

            dtlista.TableName = "Rechazo Muestras";
            dtlista.Columns[0].ColumnName = "CODIGO ORDEN";
            dtlista.Columns[1].ColumnName = "OFICIO";
            dtlista.Columns[2].ColumnName = "DOCUMENTO IDENTIDAD";
            dtlista.Columns[3].ColumnName = "FECHA RECHAZO";
            dtlista.Columns[4].ColumnName = "EESS ORIGEN";
            dtlista.Columns[5].ColumnName = "FECHA SOLICITUD";
            dtlista.Columns[6].ColumnName = "PACIENTE";
            dtlista.Columns[7].ColumnName = "EESS DESTINO";
            dtlista.Columns[8].ColumnName = "FECHA OBTENCION";
            dtlista.Columns[9].ColumnName = "EXAMEN";
            dtlista.Columns[10].ColumnName = "CRITERIO RECHAZO";
            dtlista.Columns[11].ColumnName = "CODIGO MUESTRA";
            dtlista.Columns[12].ColumnName = "ESTADO RECHAZO";
            dtlista.Columns[13].ColumnName = "USUARIO RECHAZO";

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dtlista);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;

                return new ExcelResult(wb, "RechazoMuestrasROMLab");
            }

        }

        public ActionResult ExportarResultadosExcel()
        {
            List<OrdenRecepcionVd> lista = new List<OrdenRecepcionVd>();
            if (this.Session["ordenesRecepcion"] != null)
            {
                lista = (List<OrdenRecepcionVd>)this.Session["ordenesRecepcion"];
            }

            DataTable dtlista = new DataTable("dtlista");
            dtlista = ConvertToDataTable(lista);
            dtlista.Columns.Remove("idOrden");
            dtlista.Columns.Remove("nroDocumento");
            dtlista.Columns.Remove("estadoOrden");
            dtlista.Columns.Remove("fechaRegistro");
            dtlista.Columns.Remove("fechaRegistroStr");
            dtlista.Columns.Remove("EXISTE_PENDIENTE");
            dtlista.Columns.Remove("EXISTE_REFERENCIADO");
            dtlista.Columns.Remove("EXISTE_RECIBIDO");
            dtlista.Columns.Remove("EXISTE_ORDENRECHAZO");
            dtlista.Columns.Remove("EXISTE_RECHAZOLAB");
            dtlista.Columns.Remove("genero");
            
            dtlista.Columns.Remove("fechaSolicitud");
            dtlista.Columns.Remove("paciente");
            dtlista.Columns.Remove("nombreEstablecimientoDestino");
            dtlista.Columns.Remove("fechaColeccion");
            dtlista.Columns.Remove("examen");
            dtlista.Columns.Remove("criterioRechazo");
            dtlista.Columns.Remove("codigoMuestra");
            dtlista.Columns.Remove("EstadoRechazo");
            dtlista.Columns.Remove("usuario");

            dtlista.TableName = "Exportación Recepción";
            dtlista.Columns[0].ColumnName = "Codigo Orden";
            dtlista.Columns[1].ColumnName = "Documento Referencia";
            dtlista.Columns[2].ColumnName = "Tipo de Orden";
            dtlista.Columns[3].ColumnName = "Nro. Documento";
            dtlista.Columns[4].ColumnName = "Nro. Documento Cepa";
            dtlista.Columns[5].ColumnName = "Nro. Documento Animal";
            dtlista.Columns[6].ColumnName = "Establecimiento";
            dtlista.Columns[7].ColumnName = "Fecha Solicitud";
            dtlista.Columns[8].ColumnName = "Genero";
            dtlista.Columns[9].ColumnName = "Tipo de Documento";
            dtlista.Columns[10].ColumnName = "Fecha Nacimiento";

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dtlista);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;

                return new ExcelResult(wb, "Recepcion Muestras");
            }
        }


        public DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties =
               TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }

        public string EnvioMuestraRom(string codigoMuestra, string nroOficio)
        {
            DataLayer.OrdenMuestraDal dal = new DataLayer.OrdenMuestraDal();
            string muestra = dal.EnvioMuestraRom(codigoMuestra, nroOficio, Logueado.idUsuario, EstablecimientoSeleccionado.IdEstablecimiento);
            return muestra;
        }

        public string GeneraSolicitudSalidaMuestrasRom(string codigoMuestra)
        {
            DataLayer.OrdenMuestraDal dal = new DataLayer.OrdenMuestraDal();
            string solicitud = dal.GeneraSolicitudSalidaMuestrasRom(codigoMuestra, Logueado.idUsuario);
            return solicitud;
        }

        public ActionResult SolicitudSalidaMuestraRom()
        {
            return View();
        }

        public string ListarSolicitudSalidaMuestraRom(string fechaDesde, string fechaHasta, string codigoMuestra, string formulario)
        {
            DateTime _fechaDesde = Convert.ToDateTime(fechaDesde);
            DateTime _fechaHasta = Convert.ToDateTime(fechaHasta);
            DataLayer.OrdenMuestraDal dal = new DataLayer.OrdenMuestraDal();
            string LisMx = dal.ListarSolicitudSalidaMuestraRom(_fechaDesde, _fechaHasta, codigoMuestra, formulario, EstablecimientoSeleccionado.IdEstablecimiento);
            return LisMx;
        }


        public DataTable ConvertStringToDatatable(string datos)
        {
            DataTable dtExportData = new DataTable("dtExportData");
            dtExportData.TableName = "Solicitud Salida Muestras";
            dtExportData.Columns.Add("NroFormulario");
            dtExportData.Columns.Add("NroOficio");
            dtExportData.Columns.Add("CodigoLineal");
            dtExportData.Columns.Add("CodigoMuestra");
            dtExportData.Columns.Add("FechaGeneracion");
            dtExportData.Columns.Add("EstablecimientoOrigen");
            dtExportData.Columns.Add("Paciente");
            dtExportData.Columns.Add("DocumentoIdentidad");

            string[] fila = datos.Split('|');
            string[] columna;

            for (int i = 0; i < fila.Length; i++)
            {
                System.Data.DataRow row = dtExportData.NewRow();
                columna = fila[i].Split(',');
                for (int j = 0; j < columna.Length; j++)
                {
                    row[j] = columna[j].ToString();
                }
                dtExportData.Rows.Add(row);
            }
            return dtExportData;
        }

        public ActionResult ExportarSolicitudExcel(string formato)
        {
            DataLayer.OrdenMuestraDal dal = new DataLayer.OrdenMuestraDal();
            string _formato = dal.ListarSolicitudSalidaMuestraRomPorFormato(formato,1,Logueado.idUsuario);

            DataTable DTexcelDatos = ConvertStringToDatatable(_formato);

            if (DTexcelDatos.Rows.Count > 0)
            {
                //DTexcelDatosClinicos.TableName = "Datos Clínicos";

                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(DTexcelDatos);
                    wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    wb.Style.Font.Bold = true;
                    return new ExcelResult(wb, formato);
                }
            }
            return null;
        }

        public ActionResult ExportarSolicitudPDF(string formato)
        {
            DataLayer.OrdenMuestraDal dal = new DataLayer.OrdenMuestraDal();
            string _formato = dal.ListarSolicitudSalidaMuestraRomPorFormato(formato,2,Logueado.idUsuario);
            List<OrdenSolicitudSalidaMuestra> ListOrden = new List<OrdenSolicitudSalidaMuestra>();

            if (_formato.Length > 0)
            {
                string[] lista = _formato.Split('|');
                string[] campo;
                for (int i = 0; i < lista.Length; i++)
                {
                    OrdenSolicitudSalidaMuestra orden = new OrdenSolicitudSalidaMuestra();
                    campo = lista[i].Split(',');
                    orden.formato = campo[0].ToString();
                    orden.nroOficio = campo[1].ToString();
                    orden.codigoLineal = campo[2].ToString();
                    orden.codigoMuestra = campo[3].ToString();
                    orden.fechaEnvioMuestra = campo[4].ToString();
                    orden.Establecimiento = campo[5].ToString();
                    orden.Paciente = campo[6].ToString();
                    orden.DocumentoIdentidad = campo[7].ToString();
                    ListOrden.Add(orden);
                }
            }

            string footer = Server.MapPath("~/Views/ReporteResultados/PrintFooter.html");

            string customSwitches = string.Format("--print-media-type " +
                                                       "--header-spacing \"0\" " +
                                                       "--footer-html \"{0}\" " +
                                                       "--footer-spacing \"10\" " +
                                                       "--footer-line  " +
                                                       "--footer-font-size \"10\" ", footer);
            return new Rotativa.ViewAsPdf("ExportarSolicitudPDF", ListOrden)
            {
                FileName = formato + ".pdf",
                PageSize = Size.A4,
                PageOrientation = Orientation.Landscape,
                PageMargins = { Top = 15, Bottom = 25 },
                CustomSwitches = customSwitches
            };
        }

    }
}

