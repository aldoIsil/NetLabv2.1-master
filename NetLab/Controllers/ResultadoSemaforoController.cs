using BusinessLayer;
using BusinessLayer.Compartido;
using ClosedXML.Excel;
using DataLayer;
using Model;
using Model.ViewData;
using NetLab.Extensions.ActionResults;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Web.Mvc;

namespace NetLab.Controllers
{
    public class ResultadoSemaforoController : ParentController
    {
        // GET: ResultadoSemaforo
        public ActionResult Index()
        {
            ListaBl listaBl = new ListaBl();
            List<SelectListItem> tipoDocumentoList = new List<SelectListItem>();
            foreach (ListaDetalle itemDetalle in listaBl.GetListaByOpcion(BusinessLayer.Compartido.Enums.OpcionLista.TipoDocumento).ListaDetalle)
                tipoDocumentoList.Add(new SelectListItem { Text = itemDetalle.Glosa, Value = Convert.ToString(itemDetalle.IdDetalleLista) });
            ViewBag.tipoDocumentoList = tipoDocumentoList;
            return View();
        }

        /// <summary>
        /// Descripción: Metodo para convertir una lista a un data table y exportar a Excel.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties =
               TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;

        }
        /// <summary>
        /// Descripción: Controlador para la exportacion de los resultados a excel
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <returns></returns>
        public ActionResult ExportExcel()
        {
            List<OrdenIngresarResultadoVd> ordenes = new List<OrdenIngresarResultadoVd>();
            if (this.Session["CER_Ordenes"] != null)
            {
                ordenes = (List<OrdenIngresarResultadoVd>)this.Session["CER_Ordenes"];
            }
            DataTable dtExportData = new DataTable("dtExportData");
            dtExportData = ConvertToDataTable(ordenes);
            dtExportData.Columns.Remove("idOrden");
            dtExportData.Columns.Remove("idPaciente");
            dtExportData.Columns.Remove("idAnimal");
            dtExportData.Columns.Remove("idCBS");
            dtExportData.Columns.Remove("nombreEstablecimiento");
            dtExportData.Columns.Remove("nroOficio");
            dtExportData.Columns.Remove("codigoOrden");
            dtExportData.Columns.Remove("nroDocumento");
            dtExportData.Columns.Remove("tipoOrden");
            dtExportData.Columns.Remove("idEstablecimiento");
            dtExportData.Columns.Remove("estadoOrden");
            dtExportData.Columns.Remove("fechaNacimiento");
            dtExportData.Columns.Remove("fechaRegistro");
            dtExportData.Columns.Remove("fechaSolicitud");
            dtExportData.Columns.Remove("nombrePaciente");
            dtExportData.Columns.Remove("edadPaciente");
            dtExportData.Columns.Remove("edad");
            dtExportData.Columns.Remove("nombreProyecto");
            dtExportData.Columns.Remove("codigoPaciente");
            dtExportData.Columns.Remove("EdadAnios");
            dtExportData.Columns.Remove("Genero");
            dtExportData.Columns.Remove("tipoDocumento");
            dtExportData.Columns.Remove("nombreGenero");
            dtExportData.Columns.Remove("idExamen");
            dtExportData.Columns.Remove("nombreEnfermedad");
            dtExportData.Columns.Remove("nombreExamen");
            dtExportData.Columns.Remove("idLaboratorioProcesamiento");
            dtExportData.Columns.Remove("codigoMuestra");
            dtExportData.Columns.Remove("fechaRecepcion");
            dtExportData.Columns.Remove("fechaColeccion");
            dtExportData.Columns.Remove("metodoExamen");
            dtExportData.Columns.Remove("status");
            dtExportData.Columns.Remove("muestrasValidar");
            dtExportData.Columns.Remove("muestrasPendientesRecepcion");
            dtExportData.Columns.Remove("areas");
            dtExportData.Columns.Remove("step");
            dtExportData.Columns.Remove("ingresado");
            dtExportData.Columns.Remove("validado");
            dtExportData.Columns.Remove("liberado");
            dtExportData.Columns.Remove("statusP");
            dtExportData.Columns.Remove("conformeProceso");
            dtExportData.Columns.Remove("estatusE");
            dtExportData.Columns.Remove("ordenInfoListnew");
            dtExportData.Columns.Remove("resultadosList");
            dtExportData.Columns.Remove("MuestraConforme");
            dtExportData.Columns.Remove("flagResultado");
            dtExportData.Columns.Remove("conformeP");
            dtExportData.Columns.Remove("UbigeoActual");
            dtExportData.Columns.Remove("UbigeoReniec");
            dtExportData.Columns.Remove("ConFechaSolicitud");
            dtExportData.Columns.Remove("ConDepartamentoOrigen");
            dtExportData.Columns.Remove("ConProvinciaOrigen");
            dtExportData.Columns.Remove("ConDistritoOrigen");
            dtExportData.Columns.Remove("ConDisaOrigen");
            dtExportData.Columns.Remove("ConRedOrigen");
            dtExportData.Columns.Remove("ConMicroRedOrigen");
            dtExportData.Columns.Remove("ConDepartamentoDestino");
            dtExportData.Columns.Remove("ConProvinciaDestino");
            dtExportData.Columns.Remove("ConDistritoDestino");
            dtExportData.Columns.Remove("ConDisaDestino");
            dtExportData.Columns.Remove("ConRedDestino");
            dtExportData.Columns.Remove("ConMicroRedDestino");
            dtExportData.Columns.Remove("ConEESS_LAB_Destino");
            dtExportData.Columns.Remove("ConComponente");
            dtExportData.Columns.Remove("ConnResultado");
            dtExportData.Columns.Remove("ConMuestraConforme");
            dtExportData.Columns.Remove("criteriosRechazo");
            dtExportData.Columns.Remove("ConHoraColeccion");
            dtExportData.Columns.Remove("ConFechaHoraRecepcionInsRom");
            dtExportData.Columns.Remove("ConHoraRecepcionROM");
            dtExportData.Columns.Remove("ConHoraRecepcionLab");
            dtExportData.Columns.Remove("ConHoraValidado");
            dtExportData.Columns.Remove("fechaRegistroOrden");
            dtExportData.Columns.Remove("horaRegistroOrden");
            dtExportData.Columns.Remove("fechaRegistroRecepcionMuestra");
            dtExportData.Columns.Remove("horaRegistroRecepcionMuestra");
            dtExportData.Columns.Remove("ConEstatusResultado");
            dtExportData.Columns.Remove("ConEstatusE");
            dtExportData.Columns.Remove("ConFechAddExamen");
            dtExportData.Columns.Remove("error");
            dtExportData.Columns.Remove("ObservacionRechazo");
            dtExportData.Columns.Remove("ConEdad");
            dtExportData.Columns.Remove("ConSexo");
            dtExportData.Columns.Remove("ConDireccionPaciente");
            dtExportData.Columns.Remove("idOrdenExamen");
            dtExportData.Columns.Remove("ConEstablecimientoLatitud");
            dtExportData.Columns.Remove("ConEstablecimientoLongitud");
            dtExportData.Columns.Remove("ConMotivo");
            dtExportData.Columns.Remove("tipoExamen");
            dtExportData.Columns.Remove("FechaSiembraCultivo");
            dtExportData.Columns.Remove("Telefono");
            dtExportData.Columns.Remove("ConInstitucionOrigen");

            dtExportData.TableName = "Registros";

            dtExportData.Columns[0].ColumnName = "Código Orden";
            dtExportData.Columns[1].ColumnName = "Documento Referencia";
            dtExportData.Columns[2].ColumnName = "EE.SS Origen";
            dtExportData.Columns[3].ColumnName = "Documento Identidad";
            dtExportData.Columns[4].ColumnName = "Fecha Nacimiento";
            dtExportData.Columns[5].ColumnName = "Paciente";
            dtExportData.Columns[6].ColumnName = "Codigo Muestra";
            dtExportData.Columns[7].ColumnName = "Codigo Cultivo";
            dtExportData.Columns[8].ColumnName = "Enfermedad";
            dtExportData.Columns[9].ColumnName = "Examen";
            dtExportData.Columns[10].ColumnName = "Fecha - Hora Colección";
            dtExportData.Columns[11].ColumnName = "Fecha - Hora Recepción ROM";
            dtExportData.Columns[12].ColumnName = "Fecha - Hora Recepción LAB";
            dtExportData.Columns[13].ColumnName = "Fecha - Hora Verificación";
            dtExportData.Columns[14].ColumnName = "Color";
            dtExportData.Columns[15].ColumnName = "Días";
            dtExportData.Columns[16].ColumnName = "Catálogo";


            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dtExportData);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;

                return new ExcelResult(wb, "SemaforoResultados");
            }
        }

        /// <summary>
        /// Descripción: Controlador que realiza la busqueda de resultados según el Semáforo
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 10/06/2019.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult SelectFilter(string fechaDesde, string fechaHasta, string codigoOrden, string codigoMuestra, string nroOficio, 
           string nroDocumento, string hdnEnfermedad, string hdnExamen, string nombrePaciente, string apellidoPaciente, string apellidoPaciente2,
            string tipoOrdenacion, string Semaforo, string esTipoDato)
        {
            int tipoConsulta = Convert.ToInt16(esTipoDato);
            int idEnfermedad = (hdnEnfermedad == "") ? 0 : Convert.ToInt32(hdnEnfermedad);
            int tipoOrden = Convert.ToInt16(tipoOrdenacion);
            int CSemaforo = Convert.ToInt16(Semaforo);

            var ordenBl = new ConsultaResultadosBl();
            List<OrdenIngresarResultadoVd> ordenes = new List<OrdenIngresarResultadoVd>();
            ordenes = ordenBl.ConsultaSemaforoResultados(tipoConsulta, EstablecimientoSeleccionado.IdEstablecimiento, fechaDesde, fechaHasta, codigoOrden, codigoMuestra, nroOficio, idEnfermedad, 
                                                                hdnExamen, nroDocumento, nombrePaciente, apellidoPaciente, apellidoPaciente2, tipoOrden, CSemaforo);

            ViewBag.OrdenesList = ordenes;
            Session["CER_Ordenes"] = ordenes;
            
            return View("Index", ordenes);
        }
    }
}