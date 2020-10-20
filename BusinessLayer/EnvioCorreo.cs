using RestSharp;
using System;
using System.Configuration;
using System.Net.Mail;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class EnvioCorreo
    {
        /// Descripción: Método donde se configura el servicio de correo.
        /// Author: Marcos Mejia.
        /// Fecha Creacion: 30/04/2018
        public void EnviarCorreo(string Destinatario, string Asunto, string Contenido, string ccDestinatario = null)
        {
            try
            {
                SmtpClient servidor = new SmtpClient();
                MailMessage msg = new MailMessage();
                string correo, contraseña, server;
                correo = ConfigurationManager.AppSettings["Correo"];//"soportenetlabv2@gmail.com";//"netlab@ins.gob.pe";//WebConfigurationManager.AppSettings["Correo"];
                contraseña = ConfigurationManager.AppSettings["Cclave"];//"s0p0rt3NetL@v2";// WebConfigurationManager.AppSettings["Cclave"];
                server = ConfigurationManager.AppSettings["ServidorCorreo"];//"smtp.gmail.com";// WebConfigurationManager.AppSettings["ServidorCorreo"];
                msg.From = new System.Net.Mail.MailAddress(correo);
                //if (Destinatario == "")
                //{
                //    Destinatario = "Sistema de Resultados de Laboratorio <netlab@ins.gob.pe>";
                //}

                msg.To.Add(Destinatario);
                //msg.CC.Add(ccDestinatario);
                msg.Bcc.Add(correo);
                msg.Subject = Asunto;
                msg.Body = Contenido;
                msg.IsBodyHtml = true;
                servidor.Host = server;
                servidor.Credentials = new System.Net.NetworkCredential(correo, contraseña);
                servidor.Port = 587;
                servidor.EnableSsl = true;
                servidor.Send(msg);
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
            }
        }

        public async Task SendEmail(string toEmailAddress, string emailSubject, string emailMessage)
        {
            try
            {
                var message = new MailMessage();
                string correo, contraseña, server;
                correo = ConfigurationManager.AppSettings["Correo"];
                contraseña = ConfigurationManager.AppSettings["Cclave"];
                server = ConfigurationManager.AppSettings["ServidorCorreo"];
                message.From = new System.Net.Mail.MailAddress(correo);
                message.To.Add(toEmailAddress);
                message.Bcc.Add(correo);
                message.Subject = emailSubject;
                message.Body = emailMessage;
                message.IsBodyHtml = true;

                using (var smtpClient = new SmtpClient())
                {
                    smtpClient.Host = server;
                    smtpClient.Credentials = new System.Net.NetworkCredential(correo, contraseña);
                    smtpClient.Port = 587;
                    smtpClient.EnableSsl = true;
                    await smtpClient.SendMailAsync(message);
                }
            }
            catch (Exception e)
            {
                
            }
            
        }

        public string NewSendAlertaPaciente(string phone, string message)
        {
            string api, codapi;
            api = ConfigurationManager.AppSettings["ApiSMS"];
            codapi = ConfigurationManager.AppSettings["codapi"];
            string resul = string.Empty;
            var json = "{\r\n" +
                       "\"codapi\": \"" + codapi + "\",\r\n" +
                       "\"mensajes\": {\r\n" +
                            "\"mensaje\": [\r\n" +
                                "{\r\n" +
                                    "\"destinatario\": \""+ phone + "\",\r\n" +
                                    "\"texto\": \"" + message + "\"\r\n" +
                                "}" +
                                "]\r\n" +
                            "}\r\n" +
                       "}";
            var client = new RestClient(api);
            client.Timeout = 10;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", json, ParameterType.RequestBody);
            var response = client.Execute(request);
            resul = response.ToString();
            return resul;
        }


        
    }
}
