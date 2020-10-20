using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
using System.Web;

namespace Utilitario
{
    public class bsPage
    {
        private string sFechaLog = string.Empty;
        private string sFechaTxt = string.Empty;
        private string sPathFile = ConfigurationManager.AppSettings["RutaPathEr"].ToString();

        public bsPage()
        {
            sFechaLog = DateTime.Now.ToLongTimeString().ToString() + " ";

            string sYear = DateTime.Now.Year.ToString();
            string sMont = DateTime.Now.Month.ToString();
            string sDays = DateTime.Now.Day.ToString();
            sFechaTxt = sYear + sMont.PadLeft(2, '0') + sDays.PadLeft(2, '0');
        }

        public void LogError(Exception expException, string strUsuario)
        {
            StreamWriter oStream = new StreamWriter(HttpContext.Current.Server.MapPath(sPathFile) + "Log_" + sFechaTxt + ".netlab", true);
            oStream.WriteLine(sFechaLog + strUsuario + " - Error : " + expException);
            oStream.Flush();
            oStream.Close();
        }

        public void LogInfo(string strUsuario, string lineaInfo, string datosLog = "")
        {
            try
            {
                StreamWriter oStream = new StreamWriter(HttpContext.Current.Server.MapPath(sPathFile) + "LogInfo_" + sFechaTxt+"-" + datosLog+ ".netlab", true);
                oStream.WriteLine(sFechaLog + strUsuario + " - LineaInfo: " + lineaInfo + " - InfoDatos : " + datosLog);
                oStream.Flush();
                oStream.Close();
            }
            catch (Exception ex)
            {
            }
        }

        public void LogError(Exception expException, string strUsuario, string objeto, string lineaDeCorte = "")
        {
            try
            {
                StreamWriter oStream = new StreamWriter(HttpContext.Current.Server.MapPath(sPathFile) + "Log_" + sFechaTxt + ".netlab", true);
                oStream.WriteLine(sFechaLog + strUsuario + " - zona de exception aprox: " + lineaDeCorte + " - Datos de Objeto : " + objeto + " - fin de Datos --- Error : " + expException);
                oStream.Flush();
                oStream.Close();
            }
            catch (Exception ex)
            {
            }
        }

        public void LogInfoValidacionResultados(string strUsuario, string lineaInfo, string datosLog = "")
        {
            try
            {
                StreamWriter oStream = new StreamWriter(HttpContext.Current.Server.MapPath(sPathFile) + "ValidacionResultadosLogInfo_" + sFechaTxt + ".netlab", true);
                oStream.WriteLine(sFechaLog + strUsuario + " - LineaInfo: " + lineaInfo + " - InfoDatos : " + datosLog);
                oStream.Flush();
                oStream.Close();
            }
            catch (Exception ex)
            {
            }
        }

        public void LogErrorValidacionResultados(Exception expException, string strUsuario, string objeto, string lineaDeCorte = "")
        {
            try
            {
                StreamWriter oStream = new StreamWriter(HttpContext.Current.Server.MapPath(sPathFile) + "ValidacionResultadosLogError_" + sFechaTxt + ".netlab", true);
                oStream.WriteLine(sFechaLog + strUsuario + " - zona de exception aprox: " + lineaDeCorte + " - Datos de Objeto : " + objeto + " - fin de Datos --- Error : " + expException);
                oStream.Flush();
                oStream.Close();
            }
            catch (Exception ex)
            {
            }
        }

        public void LogInfoLogin(string strUsuario, string remoteaddress, string userHostAddress, string forwardedfor)
        {
            try
            {
                StreamWriter oStream = new StreamWriter(HttpContext.Current.Server.MapPath(sPathFile) + "LoginLogInfo_" + sFechaTxt + ".netlab", true);
                oStream.WriteLine(string.Format("{0} Login - user login: {1} - Request.ServerVariables[REMOTE_ADDR]: {2} - Request.UserHostAddress : {3} - Request.ServerVariables[HTTP_X_FORWARDED_FOR] - {4}", sFechaLog, strUsuario, remoteaddress, userHostAddress, forwardedfor));
                oStream.Flush();
                oStream.Close();
            }
            catch (Exception ex)
            {
            }
        }

        public void LogCrearOrdenInfo(string strUsuario, string lineaInfo, string datosLog = "")
        {
            try
            {
                StreamWriter oStream = new StreamWriter(HttpContext.Current.Server.MapPath(sPathFile) + "LogCrearOrdenInfo_" + sFechaTxt + "-" + datosLog + ".netlab", true);
                oStream.WriteLine(sFechaLog + strUsuario + " - LineaInfo: " + lineaInfo + " - InfoDatos : " + datosLog);
                oStream.Flush();
                oStream.Close();
            }
            catch (Exception ex)
            {
            }
        }
    }
}
