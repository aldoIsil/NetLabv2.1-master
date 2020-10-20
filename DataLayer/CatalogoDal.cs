using Framework.DAL;
using Framework.DAL.Settings.Implementations;
using System.Collections.Generic;
using DataLayer.DalConverter;
using Model;

namespace DataLayer
{
    public class CatalogoDal : DaoBase
    {
        public CatalogoDal(IDalSettings settings) : base(settings)
        {
        }

        public CatalogoDal() : this(new NetlabSettings())
        {
        }
        /// <summary>
        /// Descripción: Ejecuta el procedimiento pNLS_Catag_Enfermedad para obtener el detalle de la busqueda por nombre.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        public List<Enfermedad> GetEnfermedades(string nombre)
        {
            var objCommand = GetSqlCommand("pNLS_Catalog_Enfermedad");
            InputParameterAdd.Varchar(objCommand, "nombre", nombre);

            return CatalogoConvertTo.Enfermedades(Execute(objCommand));
        }
/*
        public Presentacion GetPresentacionById(int idPresentacion)
        {
            var objCommand = GetSqlCommand("pNLS_PresentacionById");
            InputParameterAdd.Int(objCommand, "idPresentacion", idPresentacion);

            return PresentacionConvertTo.Presentacion(Execute(objCommand));
        }

        public List<Presentacion> GetPresentacionesByIdTipoMuestra(int? idTipoMuestra)
        {
            var objCommand = GetSqlCommand("pNLS_PresentacionByIdTipoMuestra");
            InputParameterAdd.Int(objCommand, "idTipoMuestra", idTipoMuestra);

            return PresentacionConvertTo.Presentaciones(Execute(objCommand));
        }
        */

    }
}
