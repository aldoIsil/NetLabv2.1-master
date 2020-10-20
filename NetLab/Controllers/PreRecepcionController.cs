using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLayer;
using BusinessLayer.Compartido;
using BusinessLayer.Compartido.Enums;
using Model;

namespace NetLab.Controllers
{
    public class PreRecepcionController : ParentController
    {
        // GET: PreRecepcion
        public ActionResult Index()
        {
            var listaBl = new ListaBl();
            var tipoDocumentoList = new List<SelectListItem>();

            foreach (var itemDetalle in listaBl.GetListaByOpcion(OpcionLista.TipoDocumento).ListaDetalle)
                tipoDocumentoList.Add(new SelectListItem
                {
                    Text = itemDetalle.Glosa,
                    Value = Convert.ToString(itemDetalle.IdDetalleLista)
                });

            ViewBag.tipoDocumentoList = tipoDocumentoList;
            return View();
        }
        public string ValidarPersona(string nroDocumento)
        {
            var pacienteBl = new PacienteBl();
            var paciente = new Paciente();
            var res = "";
            paciente.NroDocumento = nroDocumento;
            List<Paciente> pacienteList = pacienteBl.getPacientes(1, 10, 1, paciente.NroDocumento, "", "");
            if (!pacienteList.Any())
            {
                if (paciente.TipoDocumento == 1)
                {
                    var reniecPaciente = pacienteBl.getReniec(paciente);
                    if (reniecPaciente != null)//si existe en reniec
                    {
                        reniecPaciente.IdUsuarioRegistro = Logueado.idUsuario;
                        reniecPaciente.TipoDocumento = 1;
                        reniecPaciente.UbigeoActual = new Model.Ubigeo { Id = reniecPaciente.UbigeoReniec.Id };
                        reniecPaciente.IdPaciente = pacienteBl.InsertPaciente(reniecPaciente);
                        paciente = pacienteBl.getPacienteById(reniecPaciente.IdPaciente);
                        res = paciente.Nombres + "/" + paciente.ApellidoPaterno + "/" + paciente.ApellidoMaterno;
                    }
                }
            }
            else
            {
                paciente = pacienteList.FirstOrDefault();
                res = paciente.Nombres + "/" + paciente.ApellidoPaterno + "/" + paciente.ApellidoMaterno;
            }
            return res;
        }
        public string save(string tipoValidacion, string Nombres, string apePaterno, string apeMaterno, string nroDocumento,string tipoDocumento)
        {
            var res = "Registro con Exito";
            try
            {
                new TempOrdenBl().InsertPreRecepcion(tipoValidacion, Nombres, apePaterno, apeMaterno, nroDocumento, tipoDocumento,Logueado.idUsuario);
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }
    }
}