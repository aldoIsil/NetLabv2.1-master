
using System.Collections.Generic;
using Model;
using BusinessLayer;
using BusinessLayer.Interfaces;
using System.Web.Mvc;
using BusinessLayer.Compartido;
using BusinessLayer.Compartido.Enums;
using System;
using System.Linq;
using DocumentFormat.OpenXml.Drawing;

namespace NetLab.App_Code
{
    public class StaticCache
    {
        private static List<Establecimiento> _establecimientos = null;
        private static List<SelectListItem> _tiposSeguroSalud = null;
        private static List<Proyecto> _listaProyectos = null;
        private static List<LaboratorioVMSelect> _listaLaboratorios = null;
        private static List<DatosOrdenExamenSesison> _listacargaDatosOrdenExamen = null;

        public static void CargarDatosOrdenExamen(DatosOrdenExamenSesison cargaDatosOrdenExamen)
        {
            if (cargaDatosOrdenExamen != null)
            {
                _listacargaDatosOrdenExamen = _listacargaDatosOrdenExamen.Where(x => x.idUsuarioLogin != cargaDatosOrdenExamen.idUsuarioLogin).ToList();

                _listacargaDatosOrdenExamen.Add(cargaDatosOrdenExamen);
            }
        }
        public static List<DatosOrdenExamenSesison> DevuelveDatosOrdenExamen(int idUsuario)
        {
            if (_listacargaDatosOrdenExamen != null)
            {
                if (_listacargaDatosOrdenExamen.Any(x => x.idUsuarioLogin == idUsuario))
                {
                    return _listacargaDatosOrdenExamen.Where(x => x.idUsuarioLogin == idUsuario).ToList();
                }
                else
                {
                    _listacargaDatosOrdenExamen = new List<DatosOrdenExamenSesison>();
                    return _listacargaDatosOrdenExamen;
                }
            }
            else
            {
                _listacargaDatosOrdenExamen = new List<DatosOrdenExamenSesison>();
                return _listacargaDatosOrdenExamen;
            }
        }
        /// <summary>
        /// Descripción: Obtiene establecimientos de la base de datos : EstablecimientoCache
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        public static void LoadCache()
        {
            //_listaLaboratorios = CargarLaboratorios();

            _tiposSeguroSalud = CargarTipoSeguroSalud();
            _listaProyectos = CargarListaProyectos();
        }
        /// <summary>
        /// Descripción: Obtiene establecimientos de la base de datos : EstablecimientoCache
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <returns></returns>
        public static List<Establecimiento> GetEstablecimientoCache()
        {
            if (_establecimientos == null)
            {
                CargarEstablecimientosCache();
            }

            return _establecimientos;
        }

        private static void CargarEstablecimientosCache()
        {
            var establecimientoBl = new EstablecimientoBl();
            _establecimientos = establecimientoBl.GetEstablecimientosCache();
        }

        private static List<SelectListItem> CargarTipoSeguroSalud()
        {
            List<SelectListItem> seguroList = new List<SelectListItem>();
            ListaBl listaBl = new ListaBl();
            foreach (ListaDetalle itemDetalle in listaBl.GetListaByOpcion(OpcionLista.TipoSeguroSalud).ListaDetalle)
                seguroList.Add(new SelectListItem { Text = itemDetalle.Glosa, Value = Convert.ToString(itemDetalle.IdDetalleLista) });

            //Session["OrdenTipoSeguro"] = seguroList;
            return seguroList;
        }

        private static List<Proyecto> CargarListaProyectos()
        {
            IProyectoBl proyectoBl = new ProyectoBl();
            var proyectos = proyectoBl.GetProyectos();
            //Session["OrdenProyectos"] = proyectos;

            return proyectos;
        }

        private static void CargarLaboratorios()
        {
            var laboratorioBl = new LaboratorioBl();
            var laboratorioList = laboratorioBl.GetLaboratoriossStaticCache();
            _listaLaboratorios = laboratorioList;
        }

        public static List<SelectListItem> ObtenerTipoSeguroSalud()
        {
            return _tiposSeguroSalud;
        }

        public static List<Proyecto> ObtenerListaProyectos()
        {
            return _listaProyectos;
        }

        public static List<LaboratorioVMSelect> ObtenerLaboratorios()
        {
            if (_listaLaboratorios == null)
            {
                CargarLaboratorios();
            }

            return _listaLaboratorios;
        }

        public static void RecargarLaboratorios()
        {
            CargarLaboratorios();
        }

        public static void EliminarDatosExamenPorUsuario(int idUsuario)
        {
            if(_listacargaDatosOrdenExamen != null)
            {
                _listacargaDatosOrdenExamen = _listacargaDatosOrdenExamen.Where(x => x.idUsuarioLogin != idUsuario).ToList();
            }
        }

        public static void LimpiarTodosDatosExamenes()
        {
            _listacargaDatosOrdenExamen = new List<DatosOrdenExamenSesison>();
        }
    }
}