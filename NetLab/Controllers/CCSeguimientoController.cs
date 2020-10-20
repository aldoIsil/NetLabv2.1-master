using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NetLab.Models;
using Model;
using PagedList;

namespace NetLab.Controllers
{
    public class CCSeguimientoController : ParentController
    {
        // GET: CCSeguimiento
        public ActionResult index(int? page, string fechaDesde, string fechaHasta, string idEnfermedad, string idExamen, string hddDato)
        {
            var listSeguimiento = new List<CCSeguimientoCab>();
            if ((page == null) && (fechaDesde == null) && (fechaHasta == null) && (idEnfermedad == null) && (idExamen == null) && (hddDato == null))
                return View();
            else
            {
                var pageNumber = page ?? 1;
                var seguimientoBl = new SeguimientoBl();
                var sDato = hddDato == "" ? "0" : hddDato;
                var sEnfermedad = idEnfermedad == "" ? "0" : idEnfermedad;
                var sExamen = idExamen == "" ? Guid.Empty : Guid.Parse(idExamen);
                listSeguimiento = seguimientoBl.GetSeguimientos(DateTime.Parse(fechaDesde), DateTime.Parse(fechaHasta), int.Parse(sDato), int.Parse(sEnfermedad), sExamen);
                var pageOfSeg = listSeguimiento.ToPagedList(pageNumber, GetSetting<int>(PageSize));
                return View(pageOfSeg);
            }

        }
        public ActionResult EditarSeguimiento()
        {
            return PartialView("_EditarSeguimiento");
        }
        public ActionResult EliminarSeguimiento()
        {
            return null;
        }
        public ActionResult NuevoSeguimiento()
        {
            return PartialView("_NuevoSeguimiento");
        }
        public string AddSeguimiento(int idEstablecimiento, int idEnfermedad, string idExamen, int ejecutadx, int ejecutacc, int cumpliocc, int infraestructura, int equipo, int material, int personal)
        {
            //Registro Seguimiento
            return new SeguimientoBl().InsertSeguimiento(new CCSeguimientoCab() { idEnfermedad = idEnfermedad,
            idEstablecimiento = idEstablecimiento,
            idExamen = Guid.Parse(idExamen),
            ejecutacc = ejecutacc,
            ejecutadx = ejecutadx,
            cumpliocc = cumpliocc,
            idusuarioregistro = ((Usuario)Session["UsuarioLogin"]).idUsuario
            },new CCSEguimientoDetalle()
            {
                tipoinfraestructura = infraestructura,
                estadoequipo = equipo,
                estadomaterial = material,
                personal = personal,
               idusuarioregistro = ((Usuario)Session["UsuarioLogin"]).idUsuario
            });
        }
    }
}