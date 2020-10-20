using BusinessLayer;
using Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Mail;
using System.ServiceProcess;

namespace ServicioWindows
{
    partial class EnvioAlertas : ServiceBase
    {
        public EnvioAlertas()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            tmLapso.Start();
        }

        protected override void OnStop()
        {
            tmLapso.Stop();
        }

        private void tmLapso_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            UsuarioBl bl = new UsuarioBl();
            List<Usuario> usuario = new List<Usuario>();
            usuario = bl.ListaDatosUsuariosPorCaducar();
            string asunto = "Caducidad cuenta de usuario Netlab v2";

            string mail = "";
            int dias = 0;

            for (int i = 0; i < usuario.Count; i++)
            {
                string contenido = "";
                dias = usuario[i].estado;
                contenido += "<!DOCTYPE html><html><head></head><body>";
                if (dias != 0)
                {
                    contenido += "Estimado(a) usuario, <b>dentro de " + dias + " días su cuenta caducará,</b> por tal motivo lo(a) invitamos a revisar el siguiente enlace: https://netlabv2.ins.gob.pe/Login., "; 
                }else
                {
                    contenido += "Estimad(a) usuario, su cuenta ha caducado, se le dará 2 días para que pueda revisar el siguiente enlace: https://netlabv2.ins.gob.pe/Login., ";
                }
                contenido += "donde se detalla los pasos a seguir para el correcto llenado del formulario para solicitar la renovación en el Netlab v2. ";
                contenido += "<ul><li> Descargar y llenar correctamente el formulario de solicitud (de preferencia utilizar una computadora porque el formato es editable o escribir con letra imprenta). ";
                contenido += "https://netlabv2.ins.gob.pe/Documentos/SOLICITUD_DE_ACCESO_25.10.19_FOR567.pdf. </li>";

                contenido += "<li> El formulario debe ser llenado correctamente, para mayor información revisar en Documentos para uso del Netlab: Instructivo para correcto llenado del formulario ";
                contenido += "de acceso que se encuentra en el enlace: https://netlabv2.ins.gob.pe/Documentos/ITT_536_CORRECTO_LLENADO_FORMULARIO_ACCESO_NETLAB.pdf ";
                contenido += "El formulario debe ser firmado por el SOLICITANTE y tener la firma y sello del RESPONSABLE que autoriza que usted solicite su acceso para el sistema Netlabv2 en su lugar de trabajo. </li>";

                contenido += "<li> Leer detenidamente <b><i> Acuerdos de confidencialidad </i></b> (pág. 02) y firmarla, colocar su nombre completo y DNI; su firma en este ítem nos indica que usted se compromete a cumplir todos los términos descritos. </li>";

                contenido += "<li> Reenviar solicitud a este mismo correo soportenetlabv2@ins.gob.pe adjuntando <b>DOCUMENTO</b> oficial que acredite las funciones vigentes otorgadas por la autoridad competente de su lugar de trabajo. </li>";

                contenido += "<li> Por medidas de seguridad de la información  considerada en el marco de la NTP 27001 si usted requiere tener la opción para descargar el formato del informe individual del resultado, en versión PDF imprimible ";
                contenido += "(Opción Seguimiento de pacientes), deberá adjuntar un documento donde se indique que usted será el encargado de la descarga e impresión de resultados del paciente e indicará cual será el destino de los resultados";
                contenido += "de pacientes impresos, este documento también deberá estar firmado por la autoridad competente de su de su lugar de trabajo. </li></ul> ";
                contenido += "<br /> Estaremos atentos al envío de los documentos para realizar el proceso de <b>RENOVACIÓN</b> de usuario y contraseña.";
                contenido += "</body></html>";
                mail = usuario[i].correo;
                EnviarCorreoAlerta(mail, asunto, contenido);

                //86400000
            }
        }

        public void EnviarCorreoAlerta(string Destinatario, string Asunto, string Contenido)
        {
            try
            {
                SmtpClient servidor = new SmtpClient();
                MailMessage msg = new MailMessage();
                string correo, contraseña, server;
                correo = "respuestanetlabv2@ins.gob.pe";//"soportenetlabv2@gmail.com";//"netlab@ins.gob.pe";//WebConfigurationManager.AppSettings["Correo"];
                contraseña = "1n$R3Spu3st4.2o2o$";//"s0p0rt3NetL@v2";// WebConfigurationManager.AppSettings["Cclave"];
                server = "mailserver.ins.gob.pe";//"smtp.gmail.com";// WebConfigurationManager.AppSettings["ServidorCorreo"];
                msg.From = new System.Net.Mail.MailAddress(correo);
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
    }
}
