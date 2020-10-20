using BusinessLayer.Interfaces;
using DataLayer;
using System.Collections.Generic;
using System;
using Model;
using System.Linq;

namespace BusinessLayer
{
    public class DetalleCatalogoBl
    {
        /// <summary>
        /// Descripción: Obtiene el detalle de la Enfermedad seleccionada.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idEnfermedad"></param>
        /// <returns></returns>
        public List<DetalleCatalogo> GetCatalogoByIdEnfermedad(int idEnfermedad, string CodigoUnico)
        {
            using (var detallecatalogoDal = new DetalleCatalogoDal())
            {
                var detallecatalogo = detallecatalogoDal.GetTiposMuestraByIdExamen(idEnfermedad);
                if (!String.IsNullOrEmpty(CodigoUnico))
                {
                    return detallecatalogo.Where(x => x.CodigoUnico == CodigoUnico).ToList();
                }
                else
                {
                    return detallecatalogo;
                }

            }
        }

    }
}
