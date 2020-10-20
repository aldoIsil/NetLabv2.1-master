using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using NetLab.App_Code;
using Utilitario;

namespace NetLab
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Task.Factory.StartNew(() => StaticCache.LoadCache());
        }

        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            CultureInfo newCulture = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
            newCulture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            newCulture.DateTimeFormat.DateSeparator = "/";
            Thread.CurrentThread.CurrentCulture = newCulture;
            //var serverVariableLocalAddr = Request.ServerVariables["REMOTE_ADDR"];
            
            ////string hostname = System.Net.Dns.GetHostName();
            ////IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(hostname);
            ////IPAddress[] addr = ipEntry.AddressList;
            ////var addrList = addr.Select(x => x.);
            ////string dnsAddressList = string.Join(",", addrList);
            //string datos = string.Format("Global.asax - Application_BeginRequest - Request.ServerVariables[REMOTE_ADDR]: {0}", serverVariableLocalAddr);
            //new bsPage().LogInfo("LogInfoNetLab", datos, Server.MachineName);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exceptionObject = Server.GetLastError();
            string urlpath = Request.Path;
            string obj = string.Format("Global.asax - path: {0}", urlpath);
            new bsPage().LogError(exceptionObject, "LogNetLab", obj, Server.MachineName);
            Response.Redirect("~/Error");
        }
    }
}
