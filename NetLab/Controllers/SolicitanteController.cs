using BusinessLayer;
using NetLab.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Model;

namespace NetLab.Controllers
{
    public class SolicitanteController : ParentController
    {
        // GET: Solicitante
        public ActionResult Index(string solicitante)
        {
            //if(solicitante != null)
            //{
            //    SolicitanteBl result = new SolicitanteBl();
            //    List<OrdenSolicitante> sol = result.BuscarSolicitante(solicitante);
            //    return View("Index", sol);
            //}
            //else
            //{
            //    return View();
            //}

            SolicitanteBl result = new SolicitanteBl();
            List<OrdenSolicitante> sol = result.BuscarSolicitante(solicitante);
            return View("Index", sol);
        }

        public ActionResult ShowPopupSolicitante()
        {
            var listaBl = new BusinessLayer.Compartido.ListaBl();
            var profesiones = listaBl.GetListaByOpcion(BusinessLayer.Compartido.Enums.OpcionLista.Profesion);
            var solicitantevm = new SolicitanteViewModels
            {
                Solicitante = new OrdenSolicitante
                {
                    idUbigeoReniec = 0,
                    UbigeoActual = new Ubigeo() { Id = "      " },
                    UbigeoReniec = new Ubigeo() { Id = "      " }
                },
                Profesion = new Models.Shared.ListaDetalleViewModels { Data = profesiones.ListaDetalle }
            };

            return View("_FormPopupSolicitante", solicitantevm);
        }

        public ActionResult Edit(int idSolicitante)
        {
            var listaBl = new BusinessLayer.Compartido.ListaBl();
            var profesiones = listaBl.GetListaByOpcion(BusinessLayer.Compartido.Enums.OpcionLista.Profesion);
            SolicitanteBl result = new SolicitanteBl();
            OrdenSolicitante sol = result.ListarSolicitante(idSolicitante);
            var solicitantevm = new SolicitanteViewModels
            {
                Solicitante = sol,
                Profesion = new Models.Shared.ListaDetalleViewModels { Data = profesiones.ListaDetalle }
            };
            return View("_FormEditarSolicitante", solicitantevm);
        }

        public ActionResult UpdateSolicitante(int id,string codigoColegio,string Nombre,string ApePat,string ApeMat,string Correo,string dni,string telefono, int profesion, string idEstablecimiento)
        {
            try
            {
                SolicitanteBl result = new SolicitanteBl();
                result.UpdateSolicitante(id,codigoColegio,Nombre,ApePat,ApeMat,Correo,dni,telefono,profesion,Convert.ToInt32(idEstablecimiento),Logueado.idUsuario);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetSolicitantes()
        {
            var data = Request.Params["data[q]"];

            List<OrdenSolicitante> solicitantes = new List<OrdenSolicitante>();
            SolicitanteBl solicitanteBL = new SolicitanteBl();
            solicitantes = solicitanteBL.GetListaSolicitante(data);

            var resultado = "{\"q\":\"" + data + "\",\"results\":[";

            var existeDatos = false;

            foreach (var s in solicitantes)
            {
                var text = string.Format("{0} - {1} {2} {3}", s.codigoColegio, s.apellidoPaterno, s.apellidoMaterno, s.Nombres);

                resultado += "{\"id\":\"" + s.idSolicitante + "\",\"text\":\"" + text + "\"},";
                existeDatos = true;
            }

            if (existeDatos)
                resultado = resultado.Substring(0, resultado.Length - 1) + "]}";
            else
                resultado = resultado.Substring(0, resultado.Length) + "]}";

            return resultado;
        }

        public PartialViewResult LoadSolicitante()
        {
            return PartialView("_Solicitante");
        }
    }
}