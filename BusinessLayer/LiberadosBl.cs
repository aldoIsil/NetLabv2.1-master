using BusinessLayer.Interfaces;
using DataLayer;
using System.Collections.Generic;
using System;
using Model;

namespace BusinessLayer
{
    public class LiberadosBl 
    {
        /// <summary>
        /// Descripción: Obtiene las ordenes por Codigo de Orden
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, no se utiliza esta opcion.
        /// </summary>
        /// <param name="idOrden"></param>
        /// <returns></returns>
        public Orden GetOrdenById(Guid idOrden)
        {
            Orden orden = null;
            using (var liberadoDal = new LiberadosDal())
            {
                orden = liberadoDal.GetOrdenById(idOrden);
            }
            return orden;
        }
        /// <summary>
        /// Descripción: ACtualiza los datos de la orden liberada.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, no se utiliza esta opcion.
        /// </summary>
        /// <param name="libera"></param>
        public void UpdateDatosLiberados(Liberados libera)
        {
            using (LiberadosDal liberadosDal = new LiberadosDal())
            {
                liberadosDal.UpdateLiberado(libera);
            }
        }


    }
}
