using BusinessLayer.ProcesoDesatendido;
using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NetLab.Controllers
{
    public class ProcesoDesatendidoController : ParentController
    {
        // GET: ProcesoDesatendido
        public ActionResult Index(string error)
        {
            if (!string.IsNullOrEmpty(error))
            {
                ViewData["errors"] = error;
            }

            return View();
        }

        public ActionResult Importar(FormCollection form)
        {
            HttpPostedFileBase file = Request.Files[0];

            if (file.ContentLength > 0)
            {
                var filenameArray = file.FileName.Split('.');
                string fileext = filenameArray.Last();
                DirectoryInfo dInfo = new DirectoryInfo(HttpContext.Request.MapPath("~/ExcelDesatendidos"));
                string newSubDir = string.Format("{0}", DateTime.UtcNow.ToFileTimeUtc().ToString());
                string fileName = string.Format("tmp_{0}.xlsx", filenameArray.First());
                DirectoryInfo subDirInfo = dInfo.CreateSubdirectory(newSubDir);
                string subDir = string.Format("~/ExcelDesatendidos/{0}", newSubDir);
                string fileUploading = Path.Combine(HttpContext.Request.MapPath(subDir), fileName);

                try
                {
                    file.SaveAs(fileUploading);//Guarda una copia local
                    ViewData["fileUploading"] = fileUploading;
                    ExcelImportacion importacion = new ExcelImportacion();

                    int idUsuarioRegistro = ((Usuario)this.Session["UsuarioLogin"]).idUsuario;

                    importacion.ImportarExcel(fileUploading, idUsuarioRegistro);
                }
                catch (System.Data.OleDb.OleDbException ex)
                {
                    string msg = "Had the following error to upload file Excel:\n\r" + ex.Message;
                    return RedirectToAction("Index", new { error = msg });
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return View();
        }

        public ActionResult RegistroPacientes()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegistrarPacientes(FormCollection form)
        {
            HttpPostedFileBase file = Request.Files[0];

            if (file.ContentLength > 0)
            {
                var filenameArray = file.FileName.Split('.');
                string fileext = filenameArray.Last();
                DirectoryInfo dInfo = new DirectoryInfo(HttpContext.Request.MapPath("~/ExcelDesatendidos/RegistroPacientes"));
                string newSubDir = string.Format("{0}", DateTime.UtcNow.ToFileTimeUtc().ToString());
                string fileName = string.Format("tmp_{0}.xlsx", filenameArray.First());
                DirectoryInfo subDirInfo = dInfo.CreateSubdirectory(newSubDir);
                string subDir = string.Format("~/ExcelDesatendidos/RegistroPacientes/{0}", newSubDir);
                string fileUploading = Path.Combine(HttpContext.Request.MapPath(subDir), fileName);

                try
                {
                    file.SaveAs(fileUploading);//Guarda una copia local
                    ViewData["fileUploading"] = fileUploading;
                    ExcelImportacion importacion = new ExcelImportacion();

                    int idUsuarioRegistro = ((Usuario)this.Session["UsuarioLogin"]).idUsuario;

                    try
                    {
                        importacion.RegistrarPacientesExcel(fileUploading, idUsuarioRegistro);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                    try
                    {
                        importacion.CrearOrdenes(fileUploading, idUsuarioRegistro);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                    //try
                    //{
                    //    //importacion.AgregarOrdenExamen(fileUploading, idUsuarioRegistro);
                    //    importacion.CrearOrdenes(fileUploading, idUsuarioRegistro);
                    //}
                    //catch (Exception ex)
                    //{
                    //    throw ex;
                    //}

                }
                catch (System.Data.OleDb.OleDbException ex)
                {
                    string msg = "Erro en importacion de excel:\n\r" + ex.Message;
                    return RedirectToAction("Index", new { error = msg });
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return RedirectToAction("RegistroPacientes");
        }
    }
}