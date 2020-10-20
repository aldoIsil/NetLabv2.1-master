using System.Web.Mvc;
using System.Web.Routing;

namespace NetLab
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    "descargaPDF",                                 // Route name
            //    "ReporteResultados/{idOrdenExamen}/{codigoPruebaRapida}",                               // URL with parameters
            //    new { controller = "ReporteResultados", action = "DescargaResultados", idOrdenExamen = "{0}", codigoPruebaRapida = "{1}"      });
                //new { controller = "ReporteResultados", action = "DescargaResultados", idOrden = "idOrden", idLaboratorio = "idLaboratorio", idexamen = "idexamen" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            //routes.Add(new CustomRoute());
        }
    }
}
