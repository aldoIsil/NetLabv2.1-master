using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NetLab.Controllers
{
    public class ErrorController : ParentController
    {
        // GET: Shared
        public ActionResult Index()
        {
            return RedirectToAction("Error");
        }

        public ActionResult Error()
        {
            return View();
        }
    }
}