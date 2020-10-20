using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NetLab.Controllers
{
    public class ExternoController : ParentController
    {
        // GET: Externo
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ProcesarRegistrosDesatendidos()
        {
            return View();
        }
    }
}