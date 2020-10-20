using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BusinessLayer;
using System.Web.Mvc;
using BusinessLayer.Compartido;
using BusinessLayer.Compartido.Enums;
using DataLayer;
using Rotativa;
using Rotativa.Options;

namespace NetLab.Controllers
{
    public class SolicitudExamenController : ParentController
    {
        // GET: SolicitudExamen
        public ActionResult Index(int? page, int? tipoDocumento, string nroDocumento, string apellidoPaterno, string apellidoMaterno, string nombre)
        {
            var listaBl = new ListaBl();
            var tipoDocumentoList = new List<SelectListItem>();
            var tipoDocumentoCriteria = tipoDocumento ?? -1;
            foreach (var itemDetalle in listaBl.GetListaByOpcion(OpcionLista.TipoDocumento).ListaDetalle)
                tipoDocumentoList.Add(new SelectListItem
                {
                    Text = itemDetalle.Glosa,
                    Value = Convert.ToString(itemDetalle.IdDetalleLista)
                });

            ViewBag.tipoDocumentoList = tipoDocumentoList;
            PacienteBl paciente = new PacienteBl();
            List<Paciente> listPaciente = new List<Paciente>();
            listPaciente = paciente.ConsultaPacienteSolicitudVIH(tipoDocumentoCriteria, nroDocumento, apellidoPaterno, apellidoMaterno, nombre);
            return View(listPaciente);
        }

        public ActionResult SolicitudExamenVih(string idPaciente, int tipoSolicitud)
        {
            PacienteBl pacienteBl = new PacienteBl();
            SolicitudExamen solicitud = new SolicitudExamen();
            solicitud = pacienteBl.GetResultadoPacienteVIH(idPaciente, tipoSolicitud);
            solicitud.Establecimiento = new Establecimiento();
            solicitud.Establecimiento.IdEstablecimiento = EstablecimientoSeleccionado.IdEstablecimiento;
            solicitud.Establecimiento.Nombre = EstablecimientoSeleccionado.Nombre;
            solicitud.Solicitante = new OrdenSolicitante();
            solicitud.Solicitante.idSolicitante = Logueado.idUsuario;
            solicitud.Solicitante.Nombres = (Logueado.nombres + " " + Logueado.apellidoPaterno + " " + Logueado.apellidoMaterno);
            solicitud.Solicitante.correo = Logueado.correo;
            solicitud.Solicitante.telefonoContacto = Logueado.telefono;
            solicitud.tipoSolicitud = tipoSolicitud;
            Session["solicitud"] = solicitud;
            return View(solicitud);
        }

        public ActionResult RegistroSolicitudGenotipificacion(string criterio, string CodigoOrden, string[] getDroga)
        {
            SolicitudExamen solicitud = (SolicitudExamen)Session["solicitud"];
            Session["solicitud"] = null;
            solicitud.fechaSolicitud = DateTime.Now.ToString();
            try
            {
                if (solicitud != null)
                {
                    var ListaDrogas = solicitud.ListaDrogas;
                    solicitud.drogas = new DrogaGeno();
                    solicitud.Criterio = Convert.ToInt16(criterio);
                    PacienteBl paciente = new PacienteBl();
                    for (int i = 0; i < ListaDrogas.Count; i++)
                    {
                        solicitud.ListaDrogas[i].valor = getDroga[i].ToString();
                        solicitud.drogas.idDroga = ListaDrogas[i].idDroga;
                        solicitud.drogas.valor = getDroga[i].ToString();
                        paciente.RegistroSolicitudGenotipificacion(solicitud);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //return View(solicitud);
            return new ViewAsPdf("RegistroSolicitudGenotipificacion", solicitud)
            {
                FileName = "Solicitud N-" + solicitud.numeroSolicitud + '-' + solicitud.Paciente.NroDocumento + ".pdf",
                PageSize = Size.A4,
                PageOrientation = Orientation.Portrait,
                PageMargins = new Margins(20, 20, 20, 20)//,
                //CustomSwitches = customSwitches
            };
        }

        public ActionResult RegistroSolicitudTropismo(string criterio, string CodigoOrden, string tipoSolicitud)
        {
            SolicitudExamen solicitud = (SolicitudExamen)Session["solicitud"];
            Session["solicitud"] = null;
            solicitud.fechaSolicitud = DateTime.Now.ToString();
            try
            {
                if (solicitud != null)
                {
                    PacienteBl paciente = new PacienteBl();
                    solicitud.Criterio = Convert.ToInt16(criterio);
                    solicitud.CodigoOrden = CodigoOrden;
                    solicitud.tipoSolicitud = Convert.ToInt16(tipoSolicitud);
                    paciente.RegistroSolicitudTropismo(solicitud);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //return View(solicitud);
            return new ViewAsPdf("RegistroSolicitudTropismo", solicitud)
            {
                FileName = "Solicitud N-" + solicitud.numeroSolicitud + '-' + solicitud.Paciente.NroDocumento + ".pdf",
                PageSize = Size.A4,
                PageOrientation = Orientation.Portrait,
                PageMargins = new Margins(20, 20, 20, 20)//,
                //CustomSwitches = customSwitches
            };
        }

        public string ConsultaHistorialPacienteVIH(Guid idPaciente)
        {
            var dal = new PacienteDal();
            string histPaciente = dal.ConsultaHistorialPacienteVIH(idPaciente);
            return histPaciente;
        }

        public ActionResult ImprimirSolicitudExamen(Guid idPaciente, Guid idExamen, string codigo)
        {
            string vista = "RegistroSolicitudTropismo";
            SolicitudExamen solicitud = new SolicitudExamen();
            var bl = new PacienteBl();
            string datos = bl.GetDatoSolicitudExamenVIH(idPaciente, idExamen);
            string[] fila = datos.Split('$');
            string[] paciente = fila[0].Split('|');
            solicitud.Paciente = new Paciente();
            solicitud.Paciente.Nombres = paciente[0].ToString();
            solicitud.Paciente.NroDocumento = paciente[1].ToString();
            solicitud.Paciente.FechaNacimiento = Convert.ToDateTime(paciente[2]);
            solicitud.Paciente.edadAnios = Convert.ToInt32(paciente[3]);
            solicitud.Paciente.generoNombre = paciente[4].ToString();
            solicitud.Paciente.UbigeoActual = new Model.Ubigeo();
            solicitud.Paciente.UbigeoActual.Departamento = paciente[5].ToString();
            solicitud.Paciente.UbigeoActual.Provincia = paciente[6].ToString();
            solicitud.Paciente.UbigeoActual.Distrito = paciente[7].ToString();
            solicitud.Solicitante = new OrdenSolicitante();
            solicitud.Solicitante.Nombres = paciente[8].ToString();
            solicitud.Solicitante.correo = paciente[9].ToString();
            solicitud.Solicitante.telefonoContacto = paciente[10].ToString();
            solicitud.Establecimiento = new Establecimiento();
            solicitud.Establecimiento.Nombre = paciente[11].ToString();
            solicitud.numeroSolicitud = Convert.ToInt32(paciente[12]);
            solicitud.Criterio = Convert.ToInt32(paciente[13]);
            solicitud.tipoSolicitud = Convert.ToInt32(paciente[14]);
            solicitud.CodigoOrden = paciente[15].ToString();
            solicitud.Resultado = new ExamenResultadoDetalle();
            solicitud.Resultado.Resultado = paciente[16].ToString();
            solicitud.Resultado.FechaEmision = Convert.ToDateTime(paciente[17]);
            solicitud.fechaSolicitud = paciente[18].ToString();
            solicitud.CodigoCD4 = solicitud.CodigoOrden;
            solicitud.ResultadoCD4 = solicitud.Resultado.Resultado;
            solicitud.FechaResultadoCD4 = solicitud.Resultado.FechaEmision.ToString();
            solicitud.pResultado = codigo;      
            solicitud.ListaDrogas = new List<DrogaGeno>();
            if (fila.Length > 2)
            {
                vista = "RegistroSolicitudGenotipificacion";
                for (int i = 1; i < fila.Length; i++)
                {
                    solicitud.drogas = new DrogaGeno();
                    string[] dr = fila[i].Split('|');
                    solicitud.drogas.idDroga = Convert.ToInt32(dr[0]);
                    solicitud.drogas.nombreDroga = dr[1].ToString();
                    solicitud.drogas.valor = dr[2].ToString();
                    solicitud.ListaDrogas.Add(solicitud.drogas);
                }
            }
            
            return new ViewAsPdf(vista, solicitud)
            {
                FileName = "Solicitud N-" + solicitud.numeroSolicitud + '-' + solicitud.Paciente.NroDocumento + ".pdf",
                PageSize = Size.A4,
                PageOrientation = Orientation.Portrait,
                PageMargins = new Margins(20, 20, 20, 20)//,
                //CustomSwitches = customSwitches
            };
        }

        public string ValidaResultadoVih(Guid idPaciente)
        {
            var bl = new PacienteBl();
            string valor = bl.ValidaResultadoVih(idPaciente);
            return valor;
        }
    }
}