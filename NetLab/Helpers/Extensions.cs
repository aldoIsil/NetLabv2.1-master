using System;
using System.Linq;
using System.Web.Mvc;
using BusinessLayer.Compartido.Enums;

namespace NetLab.Helpers
{
    public static class Extensions
    {
        /// <summary>
        /// Descripción: Metodo que retorna el mensaje de error o No se encontraron resultados.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017.
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static MvcHtmlString EmptyTable(this HtmlHelper helper, string message = null)
        {
            message = message ?? "No se encontraron resultados";

            //return new MvcHtmlString($"<p class=\"msj-error\">{message}</p>");
            return new MvcHtmlString($"<p class=\"alert alert-danger\">{message}</p>");
        }
        /// <summary>
        /// Descripción: Metodo que retorna un booleano determinando si 2 textos son iguales.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017.
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="toCheck"></param>
        /// <param name="comp"></param>
        /// <returns></returns>
        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source != null && toCheck != null && source.IndexOf(toCheck, comp) >= 0;
        }
        /// <summary>
        /// Descripción: Metodo que retorna el codigo del ubigeo.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017.
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="ubigeo"></param>
        /// <returns></returns>
        public static string ExtractUbigeo(this string source, Ubigeo ubigeo)
        {
            if (string.IsNullOrEmpty(source)) return null;

            switch (ubigeo)
            {
                case Ubigeo.Departamento:
                    return source.Substring(0, 2);
                case Ubigeo.Provincia:
                    return source.Substring(2, 2);
                case Ubigeo.Distrito:
                    return source.Substring(4, 2);
                default:
                    throw new ArgumentOutOfRangeException(nameof(ubigeo), ubigeo, null);
            }
        }
        /// <summary>
        /// Descripción: Metodo que retorna la edad en texto.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017.
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="dob"></param>
        /// <returns></returns>
        public static string ToAgeString(this DateTime dob)
        {
            var dt = DateTime.Today;

            var months = dt.Month - dob.Month;
            if (months < 0)
            {
                dt = dt.AddYears(-1);
                months += 12;
            }

            var years = dt.Year - dob.Year;

            return $"{years} año{((years == 1) ? "" : "s")}, {months} mes{((months == 1) ? "" : "es")}";
        }
        /// <summary>
        /// Descripción: Metodo que retorna fecha actual.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017.
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToDefaultDateFormatString(this DateTime date)
        {
            return date.ToString("dd/MM/yyyy");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="controller"></param>
        /// <param name="action"></param>
        /// <param name="parameters"></param>
        /// <param name="linkHtmlAttributes"></param>
        /// <param name="src"></param>
        /// <param name="imageHtmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString ActionImageLink(this HtmlHelper helper, string controller, string action, object parameters, object linkHtmlAttributes, string src, object imageHtmlAttributes)
        {
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
            var url = String.IsNullOrWhiteSpace(controller) ? action : urlHelper.Action(action, controller, parameters);

            var imgTagBuilder = new TagBuilder("img");
            var imgUrl = urlHelper.Content(src);

            imgTagBuilder.MergeAttribute("src", imgUrl);

            if (imageHtmlAttributes != null)
            {
                var props = imageHtmlAttributes.GetType().GetProperties();
                props.ToList().ForEach(prop =>
                {
                    imgTagBuilder.MergeAttribute(prop.Name,imageHtmlAttributes.GetType().GetProperty(prop.Name).GetValue(imageHtmlAttributes, null) as String);
                });
            }

            var image = imgTagBuilder.ToString(TagRenderMode.SelfClosing);

            var aTagBuilder = new TagBuilder("a");

            aTagBuilder.MergeAttribute("href", url);

            if (linkHtmlAttributes != null)
            {
                var props = linkHtmlAttributes.GetType().GetProperties();
                props.ToList().ForEach(prop =>
                {
                    aTagBuilder.MergeAttribute(
                        prop.Name,
                        linkHtmlAttributes.GetType().GetProperty(prop.Name).GetValue(linkHtmlAttributes, null) as String);
                });
            }

            aTagBuilder.InnerHtml = image;

            return MvcHtmlString.Create(aTagBuilder.ToString());
        }
    }
}
