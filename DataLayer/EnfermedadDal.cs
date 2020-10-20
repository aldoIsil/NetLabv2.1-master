using System.Collections.Generic;
using DataLayer.DalConverter;
using Framework.DAL;
using Framework.DAL.Settings.Implementations;
using Model;

namespace DataLayer
{
    public class EnfermedadDal : DaoBase
    {

        public EnfermedadDal(IDalSettings settings) : base(settings)
        {
        }

        public EnfermedadDal() : this(new NetlabSettings())
        {
        }
        /// <summary>
        /// Descripción: Obtiene todas las enfermedades.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <returns></returns>
        public List<Enfermedad> GetEnfermedades()
        {
            var objCommand = GetSqlCommand("pNLS_Enfermedad");

            return EnfermedadConvertTo.Enfermedades(Execute(objCommand));
        }
        /// <summary>
        /// Descripción: Obtiene todas las enfermedades filtrado por el Nombre
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        public List<Enfermedad> GetEnfermedadesByNombre(string nombre)
        {
            var objCommand = GetSqlCommand("pNLS_EnfermedadByNombre");
            InputParameterAdd.Varchar(objCommand, "nombre", nombre);

            return EnfermedadConvertTo.Enfermedades(Execute(objCommand));
        }
        /// <summary>
        /// Descripción: Obtiene todas las enfermedades filtrado por el Nombre, Id Laboratorio y Id Usuario.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="idLaboratorio"></param>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public List<Enfermedad> GetEnfermedadesByNombre(string nombre, int idLaboratorio, int idUsuario)
        {
            var objCommand = GetSqlCommand("pNLS_EnfermedadByNombreAndLaboratorio");
            InputParameterAdd.Varchar(objCommand, "nombre", nombre);
            InputParameterAdd.Int(objCommand, "idLaboratorio", idLaboratorio);
            InputParameterAdd.Int(objCommand, "idUsuario", idUsuario);
            return EnfermedadConvertTo.Enfermedades(Execute(objCommand));
        }
        public List<Enfermedad> GetEnfermedadesByNombre(string nombre, string idOrden)
        {
            var objCommand = GetSqlCommand("pNLS_EnfermedadAnalistaByNombre");
            InputParameterAdd.Varchar(objCommand, "nombre", nombre);
            InputParameterAdd.Varchar(objCommand, "idOrden", idOrden);
            return EnfermedadConvertTo.Enfermedades(Execute(objCommand));
        }
    }
}
