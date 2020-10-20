using Framework.DAL;
using Framework.DAL.Settings.Implementations;
using System;
using System.Data;
using System.Collections.Generic;
using DataLayer.DalConverter;
using Model;
using Model.DTO;

namespace DataLayer
{
    public class DISADal : DaoBase
    {
        public DISADal(IDalSettings settings) : base(settings)
        {
        }

        public DISADal() : this(new NetlabSettings())
        {
        }
        /// <summary>
        /// Descripción: Obtiene las Disas activas
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <returns></returns>
        //JRCR-REPORTE01
        public List<DISA> ObtenerDISAs()
        {
            var objCommand = GetSqlCommand("pNLS_DISA");

            return DISAConvertTo.DISAs(Execute(objCommand));
        }
        /// <summary>
        /// Descripción: Obtiene las redes fitlrado por una disa
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idDISA"></param>
        /// <returns></returns>
        public List<Red> ObtenerRedes(string idDISA)
        {
            var objCommand = GetSqlCommand("pNLS_Red");
            InputParameterAdd.Varchar(objCommand, "idDISA", idDISA);

            return DISAConvertTo.Redes(Execute(objCommand));
        }
        /// <summary>
        /// Descripción: Obtiene las microredes filtrado por la disa y la red.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idDISA"></param>
        /// <param name="idRed"></param>
        /// <returns></returns>
        public List<MicroRed> ObtenerMicroRedes(string idDISA, string idRed)
        {
            var objCommand = GetSqlCommand("pNLS_MicroRed");
            InputParameterAdd.Varchar(objCommand, "idDISA", idDISA);
            InputParameterAdd.Varchar(objCommand, "idRed", idRed);

            return DISAConvertTo.MicroRedes(Execute(objCommand));
        }

        public List<Red> ObtenerRedes()
        {
            var objCommand = GetSqlCommand("pNLS_Redes");

            return DISAConvertTo.Redes(Execute(objCommand));
        }
    }
}
