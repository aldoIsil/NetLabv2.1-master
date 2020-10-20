using Framework.DAL;
using Framework.DAL.Settings.Implementations;
using System;
using System.Collections.Generic;
using System.Data;
using Model;
using DataLayer.DalConverter;

namespace DataLayer
{
    public class DetalleCatalogoDal : DaoBase
    {

        public DetalleCatalogoDal(IDalSettings settings) : base(settings)
        {
        }

        public DetalleCatalogoDal() : this(new NetlabSettings())
        {
        }
        /// <summary>
        /// Descripción: Obtiene el detalle de la Enfermedad seleccionada.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idEnfermedad"></param>
        /// <returns></returns>
        public List<DetalleCatalogo> GetTiposMuestraByIdExamen(int idEnfermedad)
        {
            var objCommand = GetSqlCommand("pNLS_DetalleCatalogo");
            InputParameterAdd.Int (objCommand, "idEnfermedad", idEnfermedad);
            var dataTable = Execute(objCommand);
            var detallecatalogoList = new List<DetalleCatalogo>();

            foreach (DataRow row in dataTable.Rows)
            {
                var detallecatalogo = new DetalleCatalogo
                {
                    Prueba = Converter.GetString(row, "Prueba"),
                    Patogeno = Converter.GetString(row,"Patogeno"),
                    Muestra = Converter.GetString(row, "Muestra"),
                    Caracteristica = Converter.GetString(row, "Caracteristica"),
                    Transporte = Converter.GetString(row,"Transporte"),
                    Laboratorio = Converter.GetString(row,"Laboratorio"),
                    DiasEmision = Converter.GetInt(row,"DiasEmision"),
                    DiasEntrega = Converter.GetInt(row,"DiasEntrega"),
                    CodigoUnico = Converter.GetString(row, "CodigoUnico")
                };
                detallecatalogoList.Add(detallecatalogo);
            }
            return detallecatalogoList;
        }

    }
}
