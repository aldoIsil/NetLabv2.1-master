using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using BusinessLayer;
using BusinessLayer.Interfaces;
using System.Threading.Tasks;
using Model.DTO;
using Model.ViewData;
using System.Globalization;
using BusinessLayer.DatoClinico;
using BusinessLayer.DatoClinico.Interfaces;
using NetLab.App_Code;

namespace NetLab.Controllers
{
    public class BancoSangreController : ParentController
    {
        protected int TipoMuestraSangreId = 1;
        protected int TipoPresentacionBolsa = 9;

        List<MaterialVd> materialList;
        List<Proyecto> proyectosBS;
        /**
            Array de elementos seleccionados:
                examenesSeleccionados
                tiposMuestraSeleccionados
                materialesSeleccionados

            Lista de objetos para seleccionar:
                examenList
                tipoMuestraList
                materialList
                enfermedadList

        */

        #region busqueda

        /// <summary>
        /// Descripción: Obtiene los establecimientos por texto ingresado.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017.
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <returns></returns>
        public string GetEstablecimientos()
        {

            String data = this.Request.Params["data[q]"];

            IEstablecimientoBl establecimientoBl = new EstablecimientoBl();
            //var listaEstablecimientos = Session["loginEstablecimientoList"] as List<Establecimiento>;
            List<Establecimiento> establecimientoList = establecimientoBl.GetEstablecimientosByTextoBusqueda(data, ((Usuario)this.Session["UsuarioLogin"]).idUsuario);


            var resultado = "{\"q\":\"" + data + "\",\"results\":[";

            foreach (var establecimiento in establecimientoList)
            {
                //TODO: YRVING LIMPIAR ESTO
                //establecimiento.nombre = establecimiento.nombre + " - Departamento: " + establecimiento.departamento +
                //    " - Provincia: " + establecimiento.provincia + " - Distrito: " + establecimiento.distrito;

                resultado += "{\"id\":\"" + establecimiento.IdEstablecimiento + "\",\"text\":\"" + establecimiento.Nombre + "\"},";
            }

            resultado = resultado.Substring(0, resultado.Length - 1) + "]}";

            return resultado;
        }
        /// <summary>
        /// Descripción: Obtiene la lista de enfermedades por el texto ingresado.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017.
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <returns></returns>
        public String GetEnfermedades()
        {

            String data = this.Request.Params["data[q]"];

            IEnfermedadBl enfermedadBl = new EnfermedadBl();
            List<Enfermedad> enfermedadList = enfermedadBl.GetEnfermedadesByNombre(data);

            String resultado = "{\"q\":\"" + data + "\",\"results\":[";

            Boolean existeEnfermedad = false;
            foreach (Enfermedad enfermedad in enfermedadList)
            {
                resultado += "{\"id\":\"" + enfermedad.idEnfermedad + "\",\"text\":\"" + enfermedad.nombre + "\"},";
                existeEnfermedad = true;
            }

            if (existeEnfermedad)
                resultado = resultado.Substring(0, resultado.Length - 1) + "]}";
            else
                resultado = resultado.Substring(0, resultado.Length) + "]}";

            Session["enfermedadList"] = enfermedadList;
            return resultado;
        }

        #endregion


        #region Seleccion de Enfermedad
        /// <summary>
        /// Descripción: Obtiene la lista de enfermedades por el texto ingresado.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017.
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <param name="idEnfermedad"></param>
        /// <returns></returns>
        public ActionResult GetExamenes(int idEnfermedad)
        {
            IExamenBl examenBl = new ExamenBl();
            List<Examen> examenList = examenBl.GetExamenesByIdEnfermedad(idEnfermedad, 1,""); //TODO: CAMBIAR

            List<Examen> examenListTmp = new List<Examen>();
            examenListTmp.Add(new Examen { nombre = "" });
            foreach (Examen examen in examenList)
            {
                examenListTmp.Add(examen);
            }
            examenList = examenListTmp;

            /*Se limpian las variables de Session de objetos seleccionados*/
            this.Session["examenesSeleccionados"] = new String[0];
            this.Session["tiposMuestraSeleccionados"] = new String[0];
            this.Session["materialesSeleccionados"] = new String[0];

            /*Se limpian las variables de Session de listas*/
            this.Session["tipoMuestraList"] = new List<TipoMuestra>();
            this.Session["materialList"] = new List<MaterialVd>();

            /*Se agrega la nueva lista de examenes de la enfermedad seleccionada*/
            this.Session["examenList"] = examenList;
            var model = examenList;
            return PartialView("_AgregarCmbExamen", model);
        }
        /// <summary>
        /// Descripción: Obtiene los datos clinicos por enfermedad
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017.
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <param name="idEnfermedad"></param>
        /// <returns></returns>
        public ActionResult GetDatosClinicos(string idEnfermedad)
        {
            IDatoBl datoBl = new DatoBl();
            List<Dato> datoList = datoBl.GetDatosByIdEnfermedad(idEnfermedad);
            /*Se agrega la nueva lista de datos clinicos de la enfermedad seleccionada*/
            this.Session["datoList"] = datoList;
            var model = datoList;
            return PartialView("_AgregarTblDatoClinico", model);
        }

        #endregion

        #region Seleccion de Examen
        /// <summary>
        /// Descripción: Obtiene la lista de tipos de muestra por enfermedad.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017.
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <param name="idExamen"></param>
        /// <returns></returns>
        public ActionResult GetTiposMuestraByIdExamen(string[] idExamen)
        {
            List<TipoMuestra> tipoMuestraList = new List<TipoMuestra>();
            List<Guid> idExamenList = new List<Guid>();
            if (idExamen != null)
            {
                ITipoMuestraBl tipoMuestraBl = new TipoMuestraBl();
                for (int i = 0; i < idExamen.Count(); i++)
                {
                    idExamenList.Add(Guid.Parse(idExamen[i]));
                }
                tipoMuestraList = tipoMuestraBl.GetTiposMuestraByIdExamen(idExamenList);
            }


            List<TipoMuestra> tipoMuestraListTmp = new List<TipoMuestra>();
            tipoMuestraListTmp.Add(new TipoMuestra { nombre = "" });
            foreach (TipoMuestra tipoMuestra in tipoMuestraList)
            {
                tipoMuestraListTmp.Add(tipoMuestra);
            }
            var model = tipoMuestraListTmp;


            //Se agrega los examenes seleccionados a la Session
            this.Session["examenesSeleccionados"] = idExamen;

            //Se agrega la lista de tipos de muestra a la Session
            this.Session["tipoMuestraList"] = model;

            return PartialView("_AgregarCmbTipoMuestra", model);
        }

        /// <summary>
        /// Descripción: Muestra la informacion de los examenes agregados.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017.
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <param name="idEnfermedad"></param>
        /// <param name="idExamen"></param>
        /// <returns></returns>
        public ActionResult ShowTableExamen(int idEnfermedad, string idExamen)
        {

            List<TipoMuestra> examenListAgregados = new List<TipoMuestra>();
            if (this.Session["ExamenListAgregados"] != null)
            {
                examenListAgregados = (List<TipoMuestra>)this.Session["ExamenListAgregados"];
            }


            if (this.Session["enfermedadList"] != null && this.Session["examenList"] != null)
            {
                List<Enfermedad> enfermedadList = (List<Enfermedad>)this.Session["enfermedadList"];
                List<Examen> examenList = (List<Examen>)this.Session["examenList"];

                Enfermedad enfermedad = enfermedadList.Where(x => x.idEnfermedad == idEnfermedad).FirstOrDefault();
                Examen examen = examenList.Where(x => x.idExamen == Guid.Parse(idExamen)).FirstOrDefault();
                TipoMuestra tipoMuestra = new TipoMuestra();
                tipoMuestra.examen = examen;
                tipoMuestra.enfermedad = enfermedad;
                tipoMuestra.posicion = examenListAgregados.Count + 1;

                examenListAgregados.Add(tipoMuestra);
            }
            //var model = tipoMuestraListAgregados;
            this.Session["ExamenListAgregados"] = examenListAgregados;
            return PartialView("_AgregarTblExamen", examenListAgregados);
        }


        #endregion


        /// <summary>
        /// Descripción: Agrega los cambios y los muestra en pantalla.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017.
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <returns></returns>
        public ActionResult Agregar()
        {
            LoadDefaultData();

            Orden orden = new Orden();
            List<Orden> ordenes = new List<Orden>();
            ViewBag.paso = 1;
            ViewBag.Title = "Registro Orden Análisis Banco de Sangre";

            List<SelectListItem> establecimientos = new List<SelectListItem>();
            establecimientos.Add(new SelectListItem { Text = "", Value = "" });
            ViewBag.establecimientos = establecimientos;

          //Orden orden = new Orden();
            orden.ordenCepaBancoSangre = new List<CepaBancoSangre>();
            /*Tipos de Muestra*/
            List<TipoMuestra> tipoMuestraList = new List<TipoMuestra>();
            if (this.Session["ExamenListAgregados"] != null)
            {
                this.Session["ExamenListAgregados"] = tipoMuestraList;
            }
            ViewBag.tipoMuestraList = tipoMuestraList;
            
            String[] tiposMuestraSeleccionados = new String[0];
            if (this.Session["tiposMuestraSeleccionados"] != null)
            {
                tiposMuestraSeleccionados = (String[])this.Session["tiposMuestraSeleccionados"];
            }
            ViewBag.tiposMuestraSeleccionados = tiposMuestraSeleccionados;
            
            ViewBag.orden = orden;
            ViewBag.ordenes = ordenes;
            return View(orden);
        }

        /// <summary>
        /// Descripción: Muestra la pantalla con los datos a editar y acutalizar.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017.
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <returns></returns>
        public ActionResult Editar()
        {
            LoadDefaultData();
            ViewBag.paso = 2;

            List<Orden> ordenes = new List<Orden>();
            Orden orden = new Orden();
            if (this.Session["OrdenBS"] != null)
            {
                orden = (Orden)this.Session["OrdenBS"];
                ordenes = (List<Orden>)this.Session["OrdenesBS"]; 
            }
            ViewBag.Title = "Confirmación Orden Análisis Banco de Sangre";

            //GET ESTABLECIMIENTO POR ID
            List<SelectListItem> establecimientos = new List<SelectListItem>();
            establecimientos.Add(new SelectListItem { Text = orden.establecimiento.Nombre, Value = orden.idEstablecimiento.ToString() });
            ViewBag.establecimientos = establecimientos;

            /*Examenes*/
            List<TipoMuestra> tipoMuestraList = new List<TipoMuestra>();
            if (this.Session["ExamenListAgregados"] != null)
            {
                tipoMuestraList = (List<TipoMuestra>)this.Session["ExamenListAgregados"];
            }
            ViewBag.tipoMuestraList = tipoMuestraList;


            String[] tiposMuestraSeleccionados = new String[0];
            if (this.Session["tiposMuestraSeleccionados"] != null)
            {
                tiposMuestraSeleccionados = (String[])this.Session["tiposMuestraSeleccionados"];
            }
            ViewBag.tiposMuestraSeleccionados = tiposMuestraSeleccionados;
            
            
            ViewBag.orden = orden;
            ViewBag.ordenes = ordenes;
            return View("Agregar", orden);
        }
        /// <summary>
        /// Descripción: Muestra los datos ingresados con todos los controles inhabilitados.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017.
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <returns></returns>
        public ActionResult Ver()
        {
            LoadDefaultData();
            ViewBag.paso = 2;

            Orden orden = new Orden();
            List<Orden> ordenes = new List<Orden>();
            if (this.Session["OrdenBS"] != null)
            {
                orden = (Orden)this.Session["OrdenBS"];
                ordenes = (List<Orden>)this.Session["OrdenesBS"];
            }
            ViewBag.Title = "Orden Análisis Banco de Sangre N°: " + orden.codigoOrden;

            

            /*Examenes*/
            List<TipoMuestra> tipoMuestraList = new List<TipoMuestra>();
            if (this.Session["ExamenListAgregados"] != null)
            {
                tipoMuestraList = (List<TipoMuestra>)this.Session["ExamenListAgregados"];
            }
            ViewBag.tipoMuestraList = tipoMuestraList;

            ViewBag.nombreUsuarioRegistro = orden.UsuarioRegistro.nombres + " " + orden.UsuarioRegistro.apellidoPaterno + " " + orden.UsuarioRegistro.apellidoMaterno;
            ViewBag.fechaRegistro = orden.FechaRegistro.ToString("dd'/'MM'/'yyyy");
            ViewBag.horaRegistro = orden.FechaRegistro.ToString("HH':'mm");

            ViewBag.orden = orden;
            ViewBag.ordenes = ordenes;
            return View(orden);
        }
        /// <summary>
        /// Descripción: Registra la informacion y libera la memoria de trabajo.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017.
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <param name="idProyecto"></param>
        /// <param name="cmbEstablecimiento"></param>
        /// <param name="cmbEstablecimientoFrecuente"></param>
        /// <param name="observacion"></param>
        /// <param name="nroOficio"></param>
        /// <param name="muestraCodigoExterno"></param>
        /// <param name="muestraFecha"></param>
        /// <param name="muestraHora"></param>
        /// <param name="muestraMaterial"></param>
        /// <param name="paso"></param>
        /// <returns></returns>
        public ActionResult Guardar(String idProyecto, String cmbEstablecimiento, String cmbEstablecimientoFrecuente, String observacion, String nroOficio, String[] muestraCodigoExterno, String[] muestraFecha, String[] muestraHora, String[] muestraMaterial, String paso)
        {
            Orden orden = new Orden();

            if (paso == "2")
            {
                orden = (Orden)this.Session["OrdenBS"];
            }
            //orden.idEnfermedad = cmbEnfermedad;

            orden.idProyecto = int.Parse(idProyecto);
            orden.observacion = observacion;
            orden.nroOficio = nroOficio;

            //Obtenemos la lista de examenes
            List<TipoMuestra> examenListAgregados = new List<TipoMuestra>();
            if (this.Session["ExamenListAgregados"] != null)
            {
                examenListAgregados = (List<TipoMuestra>)this.Session["ExamenListAgregados"];
            }

            //Se construye la orden
            orden.ordenExamenList = new List<OrdenExamen>();

            OrdenExamen ordenExamen = new OrdenExamen();
            foreach (TipoMuestra tipoMuestra in examenListAgregados)
            {
                String[] examenArray = (String[])this.Session["examenesSeleccionados"];
                
                ordenExamen = new OrdenExamen();
                ordenExamen.idExamen = tipoMuestra.examen.idExamen;
                ordenExamen.idEnfermedad = tipoMuestra.enfermedad.idEnfermedad;
                ordenExamen.ordenMuestraList = new List<OrdenMuestra>();

                orden.ordenExamenList.Add(ordenExamen);
            }

            

            if (paso == "1")
            {
                if (cmbEstablecimiento != "")
                {
                    orden.idEstablecimiento = Convert.ToInt32(cmbEstablecimiento);
                }
                else
                {
                    orden.idEstablecimiento = Convert.ToInt32(cmbEstablecimientoFrecuente);
                }
                orden.IdUsuarioRegistro = Logueado.idUsuario;

                if (orden.idEstablecimiento == EstablecimientoUsuario.IdEstablecimiento)
                {
                    orden.establecimiento = EstablecimientoUsuario;
                }
                else
                {
                    EstablecimientoBl establecimientBL = new EstablecimientoBl();
                    orden.establecimiento = establecimientBL.GetEstablecimientoById(orden.idEstablecimiento);
                }

                orden.UsuarioRegistro = Logueado;
                orden.FechaRegistro = DateTime.Now;

                this.Session["OrdenBS"] = orden;
            }

            List<Orden> ordenes = new List<Orden>();
            if (paso == "2")
            {
                orden.IdUsuarioEdicion = Logueado.idUsuario;

                ordenes = (List<Orden>) this.Session["OrdenesBS"];
                this.Session["OrdenBS"] = orden;
            }

            IOrdenBl ordenBl = new OrdenBl();
            int i = 0;
            //orden.ordenCepaBancoSangre = new List<CepaBancoSangre>();

            int countOrdenes = ordenes.Count;

            for (i = 0; i < muestraCodigoExterno.Length; i++)
            {
                Orden ordenT = new Orden();
                ordenT.codigoOrden = "";

                ordenT.idEstablecimiento = orden.idEstablecimiento;
                ordenT.establecimiento = orden.establecimiento;
                ordenT.IdUsuarioRegistro = Logueado.idUsuario;

                if (paso == "2")
                {
                    if (i < countOrdenes)
                    {
                        ordenT = ordenes.ElementAt(i);
                    }
                }

                CepaBancoSangre bs = new CepaBancoSangre();
                bs.codificacion = muestraCodigoExterno[i];
                bs.IdUsuarioRegistro = Logueado.idUsuario;
                bs.tipo = 1;
                bs.horaColeccion = DateTime.ParseExact(muestraFecha[i] + " " + muestraHora[i], "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                bs.fechaColeccion = DateTime.ParseExact(muestraFecha[i], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                //bs.horaColeccion = new DateTime();
                //bs.fechaColeccion = new DateTime();
                bs.idMaterial = int.Parse(muestraMaterial[i]);

                ordenT.cepaBancoSangre = bs;
                ordenT.ordenExamenList = orden.ordenExamenList;
                ordenT.ordenMuestraList = new List<OrdenMuestra>();


                OrdenMuestra ordenMuestra = new OrdenMuestra();
                ordenMuestra.idProyecto = orden.idProyecto;
                ordenMuestra.Estado = 1;
                ordenMuestra.IdUsuarioRegistro = Logueado.idUsuario;
                ordenMuestra.fechaColeccion = bs.fechaColeccion;
                ordenMuestra.horaColeccion = bs.horaColeccion;
                ordenMuestra.TipoMuestra = new TipoMuestra();
                ordenMuestra.TipoMuestra.idTipoMuestra = TipoMuestraSangreId;
                ordenT.ordenMuestraList.Add(ordenMuestra);


                ordenT.ordenMaterialList = new List<OrdenMaterial>();
                OrdenMaterial ordenMaterial = new OrdenMaterial();
                ordenMaterial.cantidad = 1;
                ordenMaterial.ordenMuestra.Establecimiento = new Establecimiento { IdEstablecimiento = orden.idEstablecimiento };
                ordenMaterial.idProyecto = orden.idProyecto;
                ordenMaterial.idMaterial = bs.idMaterial;
                ordenMaterial.IdUsuarioRegistro = Logueado.idUsuario;
                ordenMaterial.Estado = 1;
                ordenT.ordenMaterialList.Add(ordenMaterial);

                ordenT.idProyecto = orden.idProyecto;
                ordenT.observacion = observacion;
                ordenT.nroOficio = nroOficio;
                if (paso == "1")
                {
                    ordenBl.InsertOrdenBancoSangre(ordenT);
                    
                    ordenT.UsuarioRegistro = Logueado;
                    ordenT.FechaRegistro = DateTime.Now;

                    ordenes.Add(ordenT);
                }

                if (paso == "2")
                {

                    if (i >= countOrdenes)
                    {
                        ordenes.Add(ordenT);
                    }
                    ordenT.IdUsuarioEdicion = Logueado.idUsuario;
                    ordenBl.UpdateOrdenBancoSangre(ordenT);
                }

                
            }
            

            if (paso == "1")
            {
                this.Session["OrdenesBS"] = ordenes;
                return RedirectToAction("Editar", "BancoSangre");
            }

            if (paso == "2")
            {
                
                if (i < countOrdenes)
                {
                    countOrdenes--;
                    while(i <= countOrdenes)
                    {
                        //Eliminar orden o cambiar estado
                        ordenBl.DeleteOrden(ordenes.ElementAt(i).idOrden);
                        ordenes.RemoveAt(countOrdenes);
                        countOrdenes--;
                    }
                }

                this.Session["OrdenesBS"] = ordenes;
                return RedirectToAction("Ver", "BancoSangre");
            }
            
            //Agregar Orden Registrada correctamente por Nombre Usuario en el resultado
            return RedirectToAction("Ver", "BancoSangre");
        }

        /// <summary>
        /// Descripción: Mostrar informacion de las enfermedades agregadas.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017.
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no es utilizada.
        /// </summary>
        /// <returns></returns>
        public ActionResult MostrarDatosExamenes()
        {
            ViewBag.examenList = new List<Examen>();
            ViewBag.tipoMuestraList = new List<TipoMuestra>(); ;
            return PartialView("_PartialDatosExamenes");
        }
        /// <summary>
        /// Descripción: Obtiene la lista de materiales por el texto ingresado.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017.
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <returns></returns>
        public ActionResult MostrarDatosMateriales()
        {
            
            return PartialView("_PartialAgregarMateriales");
        }
        /// <summary>
        /// Descripción: Elimina enfermedades agregadas en memoria.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017.
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <param name="idEnfermedad"></param>
        /// <param name="idExamen"></param>
        /// <returns></returns>
        public ActionResult AjaxEliminarExamen(int idEnfermedad, String idExamen)
        {
            ViewBag.Result = "0";

            List<TipoMuestra> examenListAgregados = new List<TipoMuestra>();
            if (this.Session["ExamenListAgregados"] != null)
            {
                examenListAgregados = (List<TipoMuestra>)this.Session["ExamenListAgregados"];

                int i = 0;
                int max = examenListAgregados.Count;
                for (i = 0; i < max; i++)
                {
                    TipoMuestra sel = examenListAgregados.ElementAt(i);
                    if (sel.examen.idExamen == Guid.Parse(idExamen) && sel.enfermedad.idEnfermedad == idEnfermedad)
                    {
                        ViewBag.result = "1";
                        examenListAgregados.RemoveAt(i);
                        i = max;   
                    }
                }
            }

            this.Session["ExamenListAgregados"] = examenListAgregados;

            return PartialView("_AgregarTblExamen", examenListAgregados);
        }

        #region versionesAnteriores
        /// <summary>
        /// Descripción:Agrega examenes y muestra en pantalla lo seleccionado.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017.
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <param name="idExamen"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AgregarExamen(string idExamen)
        {
            IExamenBl examenBl = new ExamenBl();
            Examen examen = examenBl.GetExamenById(Guid.Parse(idExamen));

            ITipoMuestraBl tipoMuestraBl = new TipoMuestraBl();
            //examen.tipoMuestraList = tipoMuestraBl.GetTiposMuestraByIdExamen(Guid.Parse(idExamen));         

            List<ExamenVd> examenVdList = new List<ExamenVd>();
            if (this.Session["examenVdList"] != null)
            {
                examenVdList = (List<ExamenVd>)this.Session["examenVdList"];
            }
            ExamenVd examenVd = new ExamenVd();
            examenVd.idTiposMuestraSeleccionados = new String[1];
            examenVd.tipoMuestraList = tipoMuestraBl.GetTiposMuestraByIdExamen(Guid.Parse(idExamen));
            examenVd.examen = examen;
            examenVd.idBtnEliminarExamen = "btnEliminarExamen" + examen.idExamen;

            examenVdList.Add(examenVd);

            this.Session["examenVdList"] = examenVdList;

            var model = examenVdList;
            return PartialView("_AgregarExamenTabla", model);
        }
        /// <summary>
        /// Descripción: Obtiene la lista de los tipos de muesta por enfermedades.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017.
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <param name="idExamen"></param>
        /// <param name="idTipoMuestra"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetTiposMuestraByIdTipoMuestra(string idExamen, string[] idTipoMuestra)
        {
            List<ExamenVd> examenVdList = (List<ExamenVd>)this.Session["examenVdList"];
            List<ExamenVd> newExamenList = new List<ExamenVd>();
            foreach (ExamenVd examenVd in examenVdList)
            {
                if (examenVd.examen.idExamen == Guid.Parse(idExamen))
                {
                    //Obtener Materiales por tipo de Muestra
                    List<TipoMuestra> tipoMuestraList = new List<TipoMuestra>();

                    IMaterialBl materialBl = new MaterialBl();
                    ITipoMuestraBl tipoMuestraBl = new TipoMuestraBl();

                    for (int i = 0; i < idTipoMuestra.Count(); i++)
                    {
                        TipoMuestra tipoMuestra = tipoMuestraBl.GetTiposMuestraById(Convert.ToInt32(idTipoMuestra[i]));
                        tipoMuestra.materialList = materialBl.GetMaterialesByIdTipoMuestra(Convert.ToInt32(idTipoMuestra[i]));
                        tipoMuestraList.Add(tipoMuestra);
                    }

                    examenVd.examen.tipoMuestraList = tipoMuestraList;
                    break;
                }
            }

            this.Session["examenVdList"] = examenVdList;
            var model = examenVdList;
            return PartialView("_AgregarTipoMuestraTabla", model);
        }
        /// <summary>
        /// Descripción: Eliminar examanes seleccionados
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017.
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <param name="idExamen"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EliminarExamen(string idExamen)
        {
            IAnalitoBl analitoBl = new AnalitoBl();
            ViewBag.analitoList = analitoBl.GetAnalitos();

            List<ExamenVd> examenList = (List<ExamenVd>)this.Session["examenVdList"];
            List<ExamenVd> newExamenVdList = new List<ExamenVd>();
            foreach (ExamenVd examenVd in examenList)
            {
                if (examenVd.examen.idExamen != Guid.Parse(idExamen))
                {
                    newExamenVdList.Add(examenVd);
                }
            }

            this.Session["examenVdList"] = newExamenVdList;

            var model = newExamenVdList;
            return PartialView("_AgregarExamenTabla", model);
        }
        /// <summary>
        /// Descripción: Obtiene la lista de componentes por examen.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017.
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <param name="idExamen"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetAnalitoId(string idExamen)
        {
            var model = new AnalitoVd();
            IAnalitoBl analitoBl = new AnalitoBl();
            model.analito = analitoBl.GetAnalitoById(Guid.Parse(idExamen));
            return PartialView("_AgregarAnalito", model);
        }
        /*
        private async Task<Examen> GetExamenByIdAwait(String idExamen)
        {
            IExamenBl examenBl = new ExamenBl();
            return examenBl.GetExamenById(Guid.Parse(idExamen));
        }
        */

        #endregion
        /// <summary>
        /// Descripción: Carga la informacion y configura los controles de la pagina.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017.
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        public void LoadDefaultData()
        {
            materialList = new List<MaterialVd>();
            if (this.Session["materialListBS"] != null)
            {
                materialList = (List<MaterialVd>)this.Session["materialListBS"];
            }
            else
            {
                List<int> idTipoMuestraList = new List<int>();

                IMaterialBl materialBl = new MaterialBl();
                idTipoMuestraList.Add(Convert.ToInt32(TipoMuestraSangreId));

                foreach (Material material in materialBl.GetMaterialesByIdTipoMuestraIdPresentacion(TipoMuestraSangreId, TipoPresentacionBolsa))
                {
                    materialList.Add(new MaterialVd { material = material, idCmbMaterial = material.idMaterial, textoCmbMaterial = material.Presentacion.glosa + " " + material.Reactivo.glosa + " " + material.volumen.ToString() + "ml" });
                }

                this.Session["materialListBS"] = materialList;
            }

            proyectosBS = new List<Proyecto>();
            if (this.Session["proyectosBS"] != null)
            {
                proyectosBS = (List<Proyecto>)this.Session["proyectosBS"];
            }
            else
            {
                IProyectoBl proyectoBl = new ProyectoBl();
                proyectosBS = proyectoBl.GetProyectosBS();
                this.Session["proyectosBS"] = proyectosBS;
            }

            List<SelectListItem> proyectos = new List<SelectListItem>();

            foreach (Proyecto item in proyectosBS)
            {
                proyectos.Add(new SelectListItem { Text = item.Nombre, Value = item.IdProyecto.ToString()});
            }
            
            ViewBag.proyectos = proyectos;
            ViewBag.materialList = materialList;
        }
    }
}