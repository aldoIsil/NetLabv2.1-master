using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Model;

namespace NetLab.Helpers
{
    public static class ControlExtensions
    {
        /// <summary>
        /// Descripción: Metodo para devolver el tipo de dato y el id del mismo.
        /// en este caso es un TEXTBOX, utilizado para los datos clinicos.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017.
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="ordenDato"></param>
        /// <returns></returns>
        public static MvcHtmlString DatoTextBox(this HtmlHelper helper, OrdenDatoClinico ordenDato)
        {
            var idControl = "valueDato" + ordenDato.Enfermedad.idEnfermedad + ordenDato.Dato.IdDato;

            var html = !ordenDato.noPrecisa
                ? $"<input type = \"text\" id = \"{idControl}\" name = \"{idControl}\" value = \"{ordenDato.ValorString}\" placeholder = \"Ingrese Dato\" class=\"form-control\"/ >"
                : $"<input type = \"text\" disabled=\"disabled\" id = \"{idControl}\" name = \"{idControl}\" value = \"{ordenDato.ValorString}\" placeholder = \"Ingrese Dato\" class=\"form-control\"/ >";

            return new MvcHtmlString(html);
        }
        /// <summary>
        /// Descripción: Metodo para devolver el tipo de dato y el id del mismo.
        /// en este caso es un CHECKBOX, utilizado para los datos clinicos.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017.
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="ordenDato"></param>
        /// <returns></returns>
        public static MvcHtmlString DatoCheckBox(this HtmlHelper helper, OrdenDatoClinico ordenDato)
        {
            var idControl = "valueDato" + ordenDato.Enfermedad.idEnfermedad + ordenDato.Dato.IdDato;
            var html = "";
            if (ordenDato.noPrecisa || ordenDato.ValorString == null)
            {
                html = $"<select name = \"{idControl}\" id = \"{idControl}\"><option value='0'>No Precisa</option><option value='1'>Si</option><option value='2'>No</option></select>";
            }
            else
            {
                if (ordenDato.ValorString.Equals("1"))
                {
                    html = $"<select name = \"{idControl}\" id = \"{idControl}\" ><option value='0'>No Precisa</option><option value='1' selected='selected'>Si</option><option value='2'>No</option></select>";
                }
                else
                {
                    html = $"<select name = \"{idControl}\" id = \"{idControl}\"><option value='0'>No Precisa</option><option value='1' >Si</option><option value='2' selected='selected'>No</option></select>";
                }
            }
            return new MvcHtmlString(html);
        }
        /// <summary>
        /// Descripción: Metodo para devolver el tipo de dato y el id del mismo.
        /// en este caso es un NUMERICO, utilizado para los datos clinicos.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017.
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="ordenDato"></param>
        /// <returns></returns>
        public static MvcHtmlString DatoNumeric(this HtmlHelper helper, OrdenDatoClinico ordenDato)
        {
            var idControl = "valueDato" + ordenDato.Enfermedad.idEnfermedad + ordenDato.Dato.IdDato;
            var keyPressEventAno = string.Empty;

            if (ordenDato.Dato != null && !string.IsNullOrEmpty(ordenDato.Dato.Prefijo))
            {
                if(ordenDato.Dato.Prefijo.Contains("(aaaa)"))
                    keyPressEventAno = "onkeydown=\"return DatoClinicoAno(event);\"";
                else
                    if (ordenDato.Dato.Prefijo.Contains("Glasgow"))
                        keyPressEventAno = "onkeydown=\"return DatoClinicoGlasgow(event);\"";
            }

            var html = !ordenDato.noPrecisa 
                ? $"<input type = \"number\" id = \"{idControl}\" name = \"{idControl}\" value = \"{ordenDato.ValorString}\" placeholder = \"Ingrese Cantidad\" class=\"form-control\" " + keyPressEventAno + " >"
                : $"<input type = \"number\" disabled=\"disabled\" id = \"{idControl}\" name = \"{idControl}\" value = \"{ordenDato.ValorString}\" placeholder = \"Ingrese Cantidad\" class=\"form-control\"/ " + keyPressEventAno + ">";

            return new MvcHtmlString(html);
        }
        /// <summary>
        /// Descripción: Metodo para devolver el tipo de dato y el id del mismo.
        /// en este caso es un DATETIME, utilizado para los datos clinicos.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017.
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="ordenDato"></param>
        /// <returns></returns>
        public static MvcHtmlString DatoDate(this HtmlHelper helper, OrdenDatoClinico ordenDato)
        {
            var idControl = "valueDato" + ordenDato.Enfermedad.idEnfermedad + ordenDato.Dato.IdDato;
            //Juan Muga habilitar ingreso de fecha manualmente - agregar clase dateOnly
            var html = !ordenDato.noPrecisa ? 
                $"<input type = \"text\" id = \"{idControl}\" name = \"{idControl}\" value = \"{ordenDato.ValorString}\" placeholder = \"dd/mm/aaaa\" class=\"datepickerExtension dateOnly form-control\" onchange=\"return OnFechaInicioSintomasChange({idControl});\" autocomplete=\"off\" / >"
                : $"<input type = \"text\" disabled=\"disabled\" id = {idControl} name = \"{idControl}\" value = \"{ordenDato.ValorString}\" placeholder = \"dd/mm/aaaa\" class=\"datepickerExtension dateOnly form-control\"  autocomplete=\"off\" />";

            return new MvcHtmlString(html);
        }
        /// <summary>
        /// Descripción: Metodo para devolver el tipo de dato y el id del mismo.
        /// en este caso es un TEXTAREA, utilizado para los datos clinicos.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017.
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="ordenDato"></param>
        /// <returns></returns>
        public static MvcHtmlString DatoTextArea(this HtmlHelper helper, OrdenDatoClinico ordenDato)
        {
            var idControl = "valueDato" + ordenDato.Enfermedad.idEnfermedad + ordenDato.Dato.IdDato;

            var html = !ordenDato.noPrecisa ? $"<textarea id = \"{idControl}\" name = \"{idControl}\" placeholder = \"Ingrese Dato\" class=\"form-control\"/ >{ordenDato.ValorString}</textarea>"
                : $"<textarea disabled=\"disabled\" id = \"{idControl}\" name = \"{idControl}\" placeholder = \"Ingrese Dato\" class=\"form-control\"/ >{ordenDato.ValorString}</textarea>";

            return new MvcHtmlString(html);
        }
        /// <summary>
        /// Descripción: Metodo para devolver el tipo de dato y el id del mismo.
        /// en este caso es un COMOBO BOX, utilizado para los datos clinicos.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017.
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="ordenDato"></param>
        /// <returns></returns>
        public static MvcHtmlString DatoCombo(this HtmlHelper helper, OrdenDatoClinico ordenDato)
        {
            ordenDato.ValorString = ordenDato.noPrecisa ? "-1" : ordenDato.ValorString;
            ordenDato.ValorString = ordenDato.ValorString == "" ? null : ordenDato.ValorString;

            var idControl = "valueDato" + ordenDato.Enfermedad.idEnfermedad + ordenDato.Dato.IdDato;
            var selectListItem = new List<SelectListItem>();

            if (ordenDato.Dato != null && ordenDato.Dato.ListaDato != null && ordenDato.Dato.ListaDato.Opciones != null)
            {
                selectListItem =
                    ordenDato.Dato.ListaDato.Opciones.Select(
                        opcionDato =>
                            new SelectListItem
                            {
                                Value = opcionDato.IdOpcionDato.ToString(),
                                Text = opcionDato.Valor,
                                Selected = opcionDato.IdOpcionDato == Convert.ToInt32(ordenDato.ValorString)
                            }).ToList();
            }

            var html = ordenDato.Estado == 0 ? helper.DropDownList(idControl, selectListItem, new { id = idControl, @class = "form-control" })
                : helper.DropDownList(idControl, selectListItem, new { id = idControl, @class = "form-control", disabled = "disabled" });

            return html;
        }
    }
}