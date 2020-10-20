using Framework.DAL;
using Framework.DAL.Settings.Implementations;
using System;
using System.Collections.Generic;
using System.Data;
using Model;
using DataLayer.DalConverter;

namespace DataLayer
{
    public class TipoMuestraDal : DaoBase
    {
        public TipoMuestraDal(IDalSettings settings) : base(settings)
        {
        }

        public TipoMuestraDal() : this(new NetlabSettings())
        {
        }
        /// <summary>
        /// Descripción: Registra tipos de muestra
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="tipoM"></param>
        public void InsertTipoMuestra(TipoMuestra tipoM)
        {
            var objCommand = GetSqlCommand("pNLI_TipoMuestra");

            InputParameterAdd.Varchar(objCommand, "nombre", tipoM.nombre);
            InputParameterAdd.Varchar(objCommand, "descripcion", tipoM.descripcion);
            InputParameterAdd.Varchar(objCommand, "nemo", tipoM.nemo);
            InputParameterAdd.Int(objCommand, "estado", tipoM.Estado);

            ExecuteNonQuery(objCommand);
        }
        /// <summary>
        /// Descripción: Actualiza los tipos de muestra.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="tipoM"></param>
        public void UpdateTipoMuestra(TipoMuestra tipoM)
        {
            var objCommand = GetSqlCommand("pNLU_TipoMuestra");
            InputParameterAdd.Int(objCommand, "idTipoMuestra", tipoM.idTipoMuestra);
            InputParameterAdd.Varchar(objCommand, "nombre", tipoM.nombre);
            InputParameterAdd.Varchar(objCommand, "descripcion", tipoM.descripcion);
            InputParameterAdd.Varchar(objCommand, "nemo", tipoM.nemo);
            InputParameterAdd.Int(objCommand, "estado", tipoM.Estado);

            ExecuteNonQuery(objCommand);
        }
        /// <summary>
        /// Descripción: Obtiene tipos de muestra filtrado por nombre
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        public List<TipoMuestra> GetTipoMuestras(string nombre)
        {
            var objCommand = GetSqlCommand("pNLS_TipoMuestra");
            InputParameterAdd.Varchar(objCommand, "nombre", nombre);

            return TipoMuestraConvertTo.TipoMuestras(Execute(objCommand));
        }

        /// <summary>
        /// Descripción: Obtiene tipos de muestra activos
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        public List<TipoMuestra> GetTipoMuestrasActivas(string nombre)
        {
            var objCommand = GetSqlCommand("pNLS_TipoMuestraActivos");
            InputParameterAdd.Varchar(objCommand, "nombre", nombre);

            return TipoMuestraConvertTo.TipoMuestras(Execute(objCommand));
        }

        /// <summary>
        /// Descripción: Obtiene tipos de muestra por metodo
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idExamen"></param>
        /// <returns></returns>
        public List<TipoMuestra> GetTiposMuestraByIdExamen(Guid idExamen)
        {
            var objCommand = GetSqlCommand("pNLS_TipoMuestraByIdExamen");
            InputParameterAdd.Guid(objCommand, "idExamen", idExamen);
            var dataTable = Execute(objCommand);
            var tipoMuestraList = new List<TipoMuestra>();

            foreach (DataRow row in dataTable.Rows)
            {
                var tipoMuestra = new TipoMuestra
                {
                    idTipoMuestra = Converter.GetInt(row, "idTipoMuestra"),
                    nombre = Converter.GetString(row, "nombre"),
                    descripcion = Converter.GetString(row, "descripcion")
                };
                tipoMuestraList.Add(tipoMuestra);
            }
            return tipoMuestraList;
        }
        /// <summary>
        /// Descripción: Obtener tipos de muestra filtrado por ID
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idTipoMuestra"></param>
        /// <returns></returns>
        public TipoMuestra GetTiposMuestraById(int idTipoMuestra)
        {
            var objCommand = GetSqlCommand("pNLS_TipoMuestraById");
            InputParameterAdd.Int(objCommand, "idTipoMuestra", idTipoMuestra);
            var dataTable = Execute(objCommand);

            var tipoMuestra = new TipoMuestra();
            foreach (DataRow row in dataTable.Rows)
            {
                tipoMuestra.idTipoMuestra = Converter.GetInt(row, "idTipoMuestra");
                tipoMuestra.nombre = Converter.GetString(row, "nombre");
                tipoMuestra.descripcion = Converter.GetString(row, "descripcion");
            }
            return tipoMuestra;
        }
        /// <summary>
        /// Descripción: Obtener tipos de muestra filtrado por ID
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idTipoMuestra"></param>
        /// <returns></returns>
        public TipoMuestra GetTipoMuestraById(int idTipoMuestra)
        {
            var objCommand = GetSqlCommand("pNLS_TipoMuestraById");
            InputParameterAdd.Int(objCommand, "id", idTipoMuestra);

            return TipoMuestraConvertTo.TipoMuestra(Execute(objCommand));
        }
    }

}
