using Framework.DAL;
using Framework.DAL.Settings.Implementations;
using System.Collections.Generic;
using DataLayer.DalConverter;
using Model;

namespace DataLayer
{
    public class ProyectoDal : DaoBase
    {
        public ProyectoDal(IDalSettings settings) : base(settings)
        {
        }

        public ProyectoDal() : this(new NetlabSettings())
        {
        }
        /// <summary>
        /// Descripción: Obtiene el Modulo activo a ingresar.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <returns></returns>
        public List<Proyecto> GetProyectos()
        {
            var objCommand = GetSqlCommand("pNLS_Proyecto");

            return ProyectoConvertTo.Proyectos(Execute(objCommand));
        }
        /// <summary>
        /// Descripción: Obtiene el Modulo activo a ingresar para Banco de Sangre.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <returns></returns>
        public List<Proyecto> GetProyectosBS()
        {
            var objCommand = GetSqlCommand("pNLS_ProyectoBS");
            return ProyectoConvertTo.Proyectos(Execute(objCommand));
        }
    }
}
