using Framework.DAL;
using Framework.DAL.Settings.Implementations;
using System;
using System.Collections.Generic;
using System.Data;
using Model;
using DataLayer.DalConverter;


namespace DataLayer
{
    public class DisaMantDal:DaoBase
    {
        public DisaMantDal(IDalSettings settings) : base(settings)
        {
        }

        public DisaMantDal() : this(new NetlabSettings())
        {
        }
        /// <summary>
        /// Descripción: Obtiene informacion de las DISAS filtrado por el Nombre
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        public List<DisaMant> GetDisas(string nombre)
        {
            var objCommand = GetSqlCommand("pNLS_DisaMant");
            InputParameterAdd.Varchar(objCommand, "nombre", nombre);

            return DisaMantConverterTo.Disas(Execute(objCommand));
        }
        /// <summary>
        /// Descripción: Registra informacion de una disa.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="disa"></param>
        public void InsertDisas(DisaMant disa)
        {
            var objCommand = GetSqlCommand("pNLI_DisaMant");

            InputParameterAdd.Varchar(objCommand, "id", disa.IdDISA);
            InputParameterAdd.Varchar(objCommand, "nombre", disa.NombreDISA);
            InputParameterAdd.Int(objCommand, "estado", disa.Estado);
            InputParameterAdd.Int(objCommand, "iduser", disa.IdUsuarioRegistro);

            ExecuteNonQuery(objCommand);
        }
        /// <summary>
        /// Descripción: Obtiene informacion de las DISAS filtrado por el Id de Disa.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idDisa"></param>
        /// <returns></returns>
        public DisaMant GetDisaByID(string idDisa)
        {
            var objCommand = GetSqlCommand("pNLS_DisaMantById");
            InputParameterAdd.Varchar(objCommand, "id", idDisa);
            var dataTable = Execute(objCommand);

            var disaMant = new DisaMant();
            foreach (DataRow row in dataTable.Rows)
            {
                disaMant.IdDISA = Converter.GetString(row, "IdDISA");
                disaMant.NombreDISA = Converter.GetString(row, "nombreDISA");
                disaMant.Estado = Converter.GetInt(row, "estado");
            }
            return disaMant;
        }
        /// <summary>
        /// Descripción: Actualiza informacion de una Disa.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="disa"></param>
        public void UpdateDisas(DisaMant disa)
        {
            var objCommand = GetSqlCommand("pNLU_DisaMant");
            InputParameterAdd.Varchar(objCommand, "id", disa.IdDISA);
            InputParameterAdd.Varchar(objCommand, "nombre", disa.NombreDISA);
            InputParameterAdd.Int(objCommand, "estado", disa.Estado);
            InputParameterAdd.Int(objCommand, "iduser", disa.IdUsuarioRegistro);
            ExecuteNonQuery(objCommand);
        }

    }
}
