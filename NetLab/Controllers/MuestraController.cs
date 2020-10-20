using DataLayer;
using NetLab.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using BusinessLayer;
using Model;
using System.ComponentModel;
using System.Data;
using System;
using ClosedXML.Excel;
using NetLab.Extensions.ActionResults;
using System.Net;
using System.Threading;
using PagedList;
using NetLab.App_Code;

namespace NetLab.Controllers
{
    public class MuestraController : ParentController
    {
        /// <summary>
        /// Descripción: Registra y obtiene los nuevos codigos de muestra.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <returns></returns>
        public ActionResult GenerarCodigo()
        {
            ViewBag.isPost = false;
            ViewBag.idExtablecimientoUsuario = EstablecimientoSeleccionado.IdEstablecimiento;

            if (Request["txtCantidad"] == null) return View();
            var chkcodigoLineal = Request["chkCodigoLineal"];
            var listaEstablecimientos = Session["establecimientoList"] as List<Establecimiento>;
            var idEstablecimiento = EstablecimientoSeleccionado.IdEstablecimiento;
            var cantidad = int.Parse(Request["txtCantidad"]);
            var idUsuario = Logueado.idUsuario;
            //var establecimiento = idEstablecimiento == 991?listaEstablecimientos.FirstOrDefault(i => i.IdEstablecimiento == int.Parse(Request["idEstablecimiento"])) :
            //                      EstablecimientosUsuario.FirstOrDefault(i => i.IdEstablecimiento == int.Parse(Request["idEstablecimiento"]));
            var establecimiento = listaEstablecimientos.FirstOrDefault(i => i.IdEstablecimiento == int.Parse(Request["idEstablecimiento"]));
            idEstablecimiento = int.Parse(Request["idEstablecimiento"]);
            if (establecimiento == null) return View();

            var muestraBl = new MuestraBl();
            //Consulta si existen codigos sin utilizar.
            var listaCodigos = muestraBl.ConsultaCodigosMuestra(cantidad, idEstablecimiento, idUsuario);
            if (listaCodigos != null)
            {
                int mPendiente = listaCodigos.Count();
                int cantRest = 0;
                if (cantidad > mPendiente)
                {
                    cantRest = cantidad - mPendiente;
                    muestraBl.GeneraCodigosMuestra(cantRest, idEstablecimiento, idUsuario, string.IsNullOrEmpty(chkcodigoLineal) ? 0 : 1);
                }
            }
            else
            {
                muestraBl.GeneraCodigosMuestra(cantidad, idEstablecimiento, idUsuario, string.IsNullOrEmpty(chkcodigoLineal) ? 0 : 1);
            }

            Session["GCM_Cantidad"] = cantidad;
            Session["GCM_Establecimiento"] = idEstablecimiento;

            ViewBag.isPost = true;

            return View();
        }
        /// <summary>
        /// Descripción: Controlador para exportar a excel los codigos generador por muestra de EESS.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <returns></returns>
        public ActionResult ExportExcel()
        {
            //Response.Clear();
            //Response.Buffer = true;
            //Response.AddHeader("content-disposition", "attachment;filename=CodigosGenerados.xls");
            //Response.Charset = "";
            //Response.ContentType = "application/vnd.ms-excel";

            MuestraDal dal = new MuestraDal();

            int cantidad = 0;
            int idEstablecimiento = 0;
            DataTable codigos = null;
            //List<string> codigos = new List<string>();

            if (this.Session["GCM_Cantidad"] != null)
            {
                cantidad = (int)this.Session["GCM_Cantidad"];
                idEstablecimiento = (int)this.Session["GCM_Establecimiento"];
                codigos = dal.UltimosGeneradosExcel(Logueado.idUsuario, idEstablecimiento, cantidad);
            }
            //var response = context.HttpContext.Response;
            try
            {
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(codigos);
                    wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    wb.Style.Font.Bold = true;
                    return new ExcelResult(wb, "CodigosGenerados");
                    //Response.Clear();
                    //Response.Buffer = true;

                    //Response.Charset = "";
                    //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"; 
                    //Response.AddHeader("content-disposition", "attachment;filename= CodigosGenerados.xls");

                    //using (MemoryStream MyMemoryStream = new MemoryStream())
                    //{
                    //    wb.SaveAs(MyMemoryStream);
                    //    MyMemoryStream.WriteTo(Response.OutputStream);
                    //    Response.Flush();
                    //    Response.End();
                    //}
                }
                //Response.BufferOutput = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }

        }

        /// <summary>
        /// Descripción: Controlador para imprimir los codigos de muestra por EESS.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <returns></returns>
        public ActionResult ImprimirGenerados()
        {
            MuestraDal dal = new MuestraDal();

            int cantidad = 0;
            int idEstablecimiento = 0;
            int duplicados = 1;
            string tipo = string.Empty;
            string codigoBarras = string.Empty;

            //Inicio - Sotero 03/07/2019 Generacion Codigo de Barra.
            string urlTemp = "";
            int intMilliseconds = 1000;
            List<PDFCodigoMuestraModel> lista = new List<PDFCodigoMuestraModel>();
            var varCodigoBarra = Request.RequestContext.RouteData.Values["id"];
            var varCodigoBarra2 = Request.Form["chkCodigoLineal"];
            if (varCodigoBarra2 != null && varCodigoBarra2 == "on")
            {
                this.Session["GCM_codigoLineal"] = "on";
            }
            else
            {
                this.Session["GCM_codigoLineal"] = null;
            }
            List<string> codigos = new List<string>();
            if (varCodigoBarra != null)
            {

                tipo = "B";
                codigoBarras = varCodigoBarra.ToString();
                urlTemp = string.Format("{0}://{1}{2}{3}/",
                       Request.Url.Scheme,
                       Request.Url.Host,
                       Request.Url.Port == 80 ? string.Empty : ":" + Request.Url.Port,
                       Request.ApplicationPath);

                urlTemp = urlTemp + "QR/WFCodBarra.aspx?barcode=" + varCodigoBarra.ToString();
                HttpWebRequest request = WebRequest.Create(urlTemp) as HttpWebRequest;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                Stream stream = response.GetResponseStream();

                Thread.Sleep(intMilliseconds);

                lista.Add(new PDFCodigoMuestraModel()
                {
                    codigo = varCodigoBarra.ToString(),
                    url = Url.Content("~") + "QR/Images/" + varCodigoBarra.ToString() + ".png",
                    cantidad = duplicados,
                    Tipo = "B"
                });

            }
            else
            {
                tipo = "Q";
                if (this.Session["GCM_Cantidad"] != null)
                {
                    cantidad = (int)this.Session["GCM_Cantidad"];
                    idEstablecimiento = (int)this.Session["GCM_Establecimiento"];
                    if (Request["txtCantidad"] != null)
                    {
                        duplicados = int.Parse(Request["txtCantidad"].ToString());
                    }
                    else
                    {
                        duplicados = (int)this.Session["GCM_Duplicados"];
                    }
                    
                    if (duplicados < 1) duplicados = 1;

                    string columna = varCodigoBarra2 != null && varCodigoBarra2 == "on" ? "codigoLineal" : "codificacion";
                    codigos = dal.UltimosGenerados(Logueado.idUsuario, idEstablecimiento, cantidad, columna);
                }

                if (this.Session["GCM_ListaCodigos"] != null)
                {
                    duplicados = (int)this.Session["GCM_Duplicados"];
                    codigos = (List<string>)this.Session["GCM_ListaCodigos"];
                }
                if (this.Session["GCM_codigoLineal"] != null)
                {
                    tipo = "B";
                }
            }
            //List<PDFCodigoMuestraModel> lista = new List<PDFCodigoMuestraModel>();
            //Pendiente = Calibrar los margenes
            //string urlTemp = "";
            //Fin - Sotero 03/07/2019 Generacion Codigo de Barra.
            foreach (string codigo in codigos)
            {
                if (tipo == "B")
                {
                    codigoBarras = codigo;
                    urlTemp = string.Format("{0}://{1}{2}{3}/",
                           Request.Url.Scheme,
                           Request.Url.Host,
                           Request.Url.Port == 80 ? string.Empty : ":" + Request.Url.Port,
                           Request.ApplicationPath);

                    urlTemp = urlTemp + "QR/WFCodBarra.aspx?barcode=" + codigo;
                    HttpWebRequest request = WebRequest.Create(urlTemp) as HttpWebRequest;
                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                    Stream stream = response.GetResponseStream();

                    Thread.Sleep(intMilliseconds);

                    lista.Add(new PDFCodigoMuestraModel()
                    {
                        codigo = codigo,
                        url = Url.Content("~") + "QR/Images/" + codigo + ".png",
                        cantidad = duplicados,
                        Tipo = "B"
                    });
                }
                else
                {
                    urlTemp = Url.Content("~") + "QR/WFCodBar.aspx?barcode=" + codigo +
                    "&MODE=" + 1 +
                    "&PFMT=" + 0 +
                    "&IR=" + 20 +
                    "&X=" + "0.300" +
                    "&Left_Margin=" + "0.05" +
                    "&Top_Margin=" + "0.05" +
                    "&FORMAT=" + "GIF";
                    lista.Add(new PDFCodigoMuestraModel()
                    {
                        codigo = codigo,
                        url = urlTemp,
                        cantidad = duplicados,
                        Tipo = "Q" // Sotero 03/07/2019 Generacion Codigo de Barra.
                    });
                }

            }

            ViewBag.lista = lista;
            ViewBag.tipo = tipo;
            //ViewBag.codigoBarras = codigoBarras;
            return View(lista);
        }

        public ActionResult ImprimirCodigosGenerados()
        {
            MuestraDal dal = new MuestraDal();

            int cantidad = 0;
            int idEstablecimiento = 0;
            int duplicados = 1;
            string tipo = string.Empty;
            string codigoBarras = string.Empty;

            //Inicio - Sotero 03/07/2019 Generacion Codigo de Barra.
            string urlTemp = "";
            int intMilliseconds = 1000;
            List<PDFCodigoMuestraModel> lista = new List<PDFCodigoMuestraModel>();
            var varCodigoBarra = Request.RequestContext.RouteData.Values["id"];
            var varCodigoBarra2 = Request.Form["chkCodigoLineal"];
            if (varCodigoBarra2 != null && varCodigoBarra2 == "on")
            {
                this.Session["GCM_codigoLineal"] = "on";
            }
            else
            {
                this.Session["GCM_codigoLineal"] = null;
            }
            List<string> codigos = new List<string>();
            if (varCodigoBarra != null)
            {

                tipo = "B";
                codigoBarras = varCodigoBarra.ToString();
                urlTemp = string.Format("{0}://{1}{2}{3}/",
                       Request.Url.Scheme,
                       Request.Url.Host,
                       Request.Url.Port == 80 ? string.Empty : ":" + Request.Url.Port,
                       Request.ApplicationPath);

                urlTemp = urlTemp + "QR/WFCodBarra.aspx?barcode=" + varCodigoBarra.ToString();
                HttpWebRequest request = WebRequest.Create(urlTemp) as HttpWebRequest;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                Stream stream = response.GetResponseStream();

                Thread.Sleep(intMilliseconds);

                lista.Add(new PDFCodigoMuestraModel()
                {
                    codigo = varCodigoBarra.ToString(),
                    url = Url.Content("~") + "QR/Images/" + varCodigoBarra.ToString() + ".png",
                    cantidad = duplicados,
                    Tipo = "B"
                });

            }
            else
            {
                tipo = "Q";
                if (this.Session["GCM_Cantidad"] != null)
                {
                    cantidad = (int)this.Session["GCM_Cantidad"];
                    idEstablecimiento = (int)this.Session["GCM_Establecimiento"];
                    if (Request["txtCantidad"] != null)
                    {
                        duplicados = int.Parse(Request["txtCantidad"].ToString());
                    }
                    else if(Session["GCM_Duplicados"] != null)
                    {
                        duplicados = (int)Session["GCM_Duplicados"];
                    }

                    if (duplicados < 1) duplicados = 1;

                    string columna = varCodigoBarra2 != null && varCodigoBarra2 == "on" ? "codigoLineal" : "codificacion";
                    codigos = dal.UltimosGenerados(Logueado.idUsuario, idEstablecimiento, cantidad, columna);
                }

                if (this.Session["GCM_ListaCodigos"] != null)
                {
                    duplicados = (int)this.Session["GCM_Duplicados"];
                    codigos = (List<string>)this.Session["GCM_ListaCodigos"];
                }
                if (this.Session["GCM_codigoLineal"] != null)
                {
                    tipo = "B";
                }

                idEstablecimiento = Request["idEstablecimiento"]!=""?int.Parse(Request["idEstablecimiento"].ToString()):0;
                string solicitud = Request["txtNumSolicitud"] == null ? "": Request["txtNumSolicitud"].ToString();
                string desde = Request["txtDesde"].ToString();
                string hasta = Request["txtHasta"].ToString();
                if (solicitud != "")
                {
                    idEstablecimiento = 0;
                    desde = "-";
                    hasta = "-";
                }

                if (!codigos.Any() && Session["GCM_ListaCodigos"] == null && !string.IsNullOrWhiteSpace(desde) && !string.IsNullOrWhiteSpace(hasta))
                {
                    string columna = varCodigoBarra2 != null && varCodigoBarra2 == "on" ? "codigoLineal" : "codificacion";
                    codigos = dal.GetGeneradosByEstablecimiento(idEstablecimiento, desde, hasta, solicitud, columna);
                    codigos = codigos.Where(x => x != "").ToList();
                }
            }
            //List<PDFCodigoMuestraModel> lista = new List<PDFCodigoMuestraModel>();
            //Pendiente = Calibrar los margenes
            //string urlTemp = "";
            //Fin - Sotero 03/07/2019 Generacion Codigo de Barra.
            foreach (string codigo in codigos)
            {
                if (tipo == "B")
                {
                    codigoBarras = codigo;
                    urlTemp = string.Format("{0}://{1}{2}{3}/",
                           Request.Url.Scheme,
                           Request.Url.Host,
                           Request.Url.Port == 80 ? string.Empty : ":" + Request.Url.Port,
                           Request.ApplicationPath);

                    urlTemp = urlTemp + "QR/WFCodBarra.aspx?barcode=" + codigo;
                    HttpWebRequest request = WebRequest.Create(urlTemp) as HttpWebRequest;
                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                    Stream stream = response.GetResponseStream();

                    Thread.Sleep(intMilliseconds);

                    lista.Add(new PDFCodigoMuestraModel()
                    {
                        codigo = codigo,
                        url = Url.Content("~") + "QR/Images/" + codigo + ".png",
                        cantidad = duplicados,
                        Tipo = "B"
                    });
                }
                else
                {
                    urlTemp = Url.Content("~") + "QR/WFCodBar.aspx?barcode=" + codigo +
                    "&MODE=" + 1 +
                    "&PFMT=" + 0 +
                    "&IR=" + 20 +
                    "&X=" + "0.300" +
                    "&Left_Margin=" + "0.05" +
                    "&Top_Margin=" + "0.05" +
                    "&FORMAT=" + "GIF";
                    lista.Add(new PDFCodigoMuestraModel()
                    {
                        codigo = codigo,
                        url = urlTemp,
                        cantidad = duplicados,
                        Tipo = "Q" // Sotero 03/07/2019 Generacion Codigo de Barra.
                    });
                }

            }

            ViewBag.lista = lista;
            ViewBag.tipo = tipo;
            //ViewBag.codigoBarras = codigoBarras;
            return View("ImprimirGenerados", lista);
        }
        /// <summary>
        /// Descripción: Controlador para obtener e imprimir los codigos de muestra por EESS.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <returns></returns>
        public ActionResult MostrarCodigo()
        {
            ViewBag.isPost = false;
            MuestraDal dal = new MuestraDal();
            ViewBag.idExtablecimientoUsuario = EstablecimientoSeleccionado.IdEstablecimiento;

            if (Request["idEstablecimiento"] != null)
            {
                int idEstablecimiento = int.Parse(Request["idEstablecimiento"].ToString());
                string desde = Request["txtDesde"].ToString();
                string hasta = Request["txtHasta"].ToString();
                int cantidad = int.Parse(Request["txtCantidad"].ToString());
                var chkcodigoLineal = Request["chkCodigoLineal"];

                if (desde.Length == 5 && hasta.Length == 5)
                {
                    var listaEstablecimientos = Session["establecimientoList"] as List<Establecimiento>;

                    var renaes = listaEstablecimientos.FirstOrDefault(i => i.IdEstablecimiento == idEstablecimiento);
                    //EstablecimientosUsuario.Where(i => i.IdEstablecimiento == idEstablecimiento).FirstOrDefault().CodigoUnico;
                    string codigoRenaes = renaes.CodigoUnico;

                    codigoRenaes = codigoRenaes.Substring(codigoRenaes.Length - 5, 5);
                    desde = codigoRenaes + desde;
                    hasta = codigoRenaes + hasta;
                }
                if (cantidad <= 0) cantidad = 1;


                List<string> listaCodigos = dal.GetGeneradosByEstablecimiento(idEstablecimiento, desde, hasta);
                ViewBag.listaCodigos = listaCodigos;
                ViewBag.notFound = false;

                if (listaCodigos == null || listaCodigos.Count == 0) { ViewBag.notFound = true; }
                else
                {
                    this.Session["GCM_ListaCodigos"] = listaCodigos;
                    this.Session["GCM_Duplicados"] = cantidad;
                    this.Session["GCM_codigoLineal"] = chkcodigoLineal;
                    return RedirectToAction("ImprimirGenerados", "Muestra");
                }
                ViewBag.isPost = true;
            }
            else
            {
            }


            return View();
        }

        public JsonResult GetEstablecimientos(string Prefix)
        {
            //var establecimientoBl = new EstablecimientoBl();
            var establecimientoList = new List<Establecimiento>();
            //var listaEstablecimientos = Session["loginEstablecimientoList"] as List<Establecimiento>;
            //establecimientoList = establecimientoBl.GetEstablecimientosByTextoBusqueda(Prefix, ((Usuario)Session["UsuarioLogin"]).idUsuario, listaEstablecimientos);
            //establecimientoList = establecimientoBl.GetEstablecimientosByTextoBusqueda(Prefix, 1);
            var laboratorioList = StaticCache.ObtenerLaboratorios();
            var filtrados = laboratorioList.Where(x => (x.IdLabIns == 0 || x.IdLabIns == 2) && x.NombreEstablecimiento.ToLower().Contains(Prefix.ToLower()) || x.CodigoUnico.ToLower().Contains(Prefix.ToLower()));

            if (filtrados.Any())
            {
                filtrados.Take(50).ToList().ForEach(x =>
                {
                    establecimientoList.Add(new Establecimiento
                    {
                        IdEstablecimiento = x.IdEstablecimiento,
                        CodigoInstitucion = string.Empty,
                        CodigoUnico = x.CodigoUnico,
                        Nombre = x.Nombre,
                        clasificacion = x.clasificacion,
                        IdLabIns = x.IdLabIns ?? 0, // x.IdLabIns.Value : 0,
                        Ubigeo = x.Ubigeo
                    });
                });
            }
            
            Session["establecimientoList"] = establecimientoList;

            return Json(establecimientoList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ConsultaCodigosMuestra(string fechaDesde, string fechaHasta, string idEstablecimiento, string idMuestra, string estado)
        {
            if (idEstablecimiento == null) return View();
            List<MuestraCodificacion> lista = new List<MuestraCodificacion>();
            var muestraBl = new MuestraBl();
            lista = muestraBl.ConsultaCodigosGenerados(fechaDesde, fechaHasta, idEstablecimiento, idMuestra, estado, EstablecimientoSeleccionado.IdEstablecimiento);
            Session["lista_Codigos"] = lista;
            return View(lista);
        }

        public ActionResult ConsultaCodigosMuestraEstado(string inicio, string fin, int idEstablecimiento)
        {
            List<MuestraCodificacion> lista = new List<MuestraCodificacion>();
            var muestraBl = new MuestraBl();
            lista = muestraBl.ConsultaCodigosMuestraEstado(inicio,fin, idEstablecimiento);
            var lista2 = lista.ToPagedList(1, 1000);
            return PartialView("_EstadoCodigoMuestra", lista2);
        }

        public ActionResult ExportarCodigosExcel()
        {
            List<MuestraCodificacion> lista = new List<MuestraCodificacion>();
            if (this.Session["lista_Codigos"] != null)
            {
                lista = (List<MuestraCodificacion>)this.Session["lista_Codigos"];
            }

            DataTable dtlista = new DataTable("dtlista");
            dtlista = ConvertToDataTable(lista);
            dtlista.Columns.Remove("idMuestraCod");
            dtlista.Columns.Remove("idEstablecimiento");
            dtlista.Columns.Remove("idCodificacion");
            dtlista.Columns.Remove("Estado");
            dtlista.Columns.Remove("FechaRegistro");
            dtlista.Columns.Remove("IdUsuarioRegistro");
            dtlista.Columns.Remove("FechaEdicion");
            dtlista.Columns.Remove("IdUsuarioEdicion");
            dtlista.Columns.Remove("UsuarioRegistro");
            dtlista.Columns.Remove("EstadoDesc");
            dtlista.Columns.Remove("EstadoCheck");
            dtlista.TableName = "Lista Codigos";
            dtlista.Columns[0].ColumnName = "Codigo Muestra";
            dtlista.Columns[1].ColumnName = "Codigo Lineal";
            dtlista.Columns[2].ColumnName = "Estado";
            dtlista.Columns[3].ColumnName = "Fecha de Generacion";
            dtlista.Columns[4].ColumnName = "Usuario Generador";

            //using (XLWorkbook wb = new XLWorkbook())
            //{
            //    wb.Worksheets.Add(dtlista);
            //    wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            //    wb.Style.Font.Bold = true;

            //    Response.Clear();
            //    Response.Buffer = true;
            //    Response.Charset = "";
            //    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //    Response.AddHeader("content-disposition", "attachment;filename= ListaCodigos.xlsx");

            //    using (MemoryStream MyMemoryStream = new MemoryStream())
            //    {
            //        wb.SaveAs(MyMemoryStream);
            //        MyMemoryStream.WriteTo(Response.OutputStream);
            //        Response.Flush();
            //        Response.End();
            //    }
            //}

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dtlista);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                return new ExcelResult(wb, "ListaCodigos");
            }

            //return View();
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

        public ActionResult PrintCodigoSeleccionado(string codes, string tipo)
        {
            string urlTemp = "";
            int intMilliseconds = 1000;
            List<PDFCodigoMuestraModel> lista = new List<PDFCodigoMuestraModel>();
            var code = codes.Split(',');

            for (int i = 0; i < code.Length; i++)
            {
                if (tipo == "B")
                {
                    urlTemp = string.Format("{0}://{1}{2}{3}/",
                           Request.Url.Scheme,
                           Request.Url.Host,
                           Request.Url.Port == 80 ? string.Empty : ":" + Request.Url.Port,
                           Request.ApplicationPath);

                    urlTemp = urlTemp + "QR/WFCodBarra.aspx?barcode=" + code[i].ToString();
                    HttpWebRequest request = WebRequest.Create(urlTemp) as HttpWebRequest;
                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                    Stream stream = response.GetResponseStream();

                    Thread.Sleep(intMilliseconds);

                    lista.Add(new PDFCodigoMuestraModel()
                    {
                        codigo = code[i].ToString(),
                        url = Url.Content("~") + "QR/Images/" + code[i].ToString() + ".png",
                        cantidad = 1,
                        Tipo = "B"
                    });
                }
                else
                {
                    urlTemp = Url.Content("~") + "QR/WFCodBar.aspx?barcode=" + code[i].ToString() +
                    "&MODE=" + 1 +
                    "&PFMT=" + 0 +
                    "&IR=" + 20 +
                    "&X=" + "0.300" +
                    "&Left_Margin=" + "0.05" +
                    "&Top_Margin=" + "0.05" +
                    "&FORMAT=" + "GIF";
                    lista.Add(new PDFCodigoMuestraModel()
                    {
                        codigo = code[i].ToString(),
                        url = urlTemp,
                        cantidad = 1,
                        Tipo = "Q" 
                    });
                }  
            }
            return View("ImprimirGenerados", lista);
        }
    }
}