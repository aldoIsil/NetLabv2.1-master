using Framework.DAL;
using Framework.DAL.Settings.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using DataLayer.DalConverter;
using System.Data;

namespace DataLayer
{
    public class RedMantDal:DaoBase
    {

        public RedMantDal(IDalSettings settings) : base(settings)
        { 
        }

        public RedMantDal() : this(new NetlabSettings())
        {
        }
        /// <summary>
        /// Descripción: Obtener las redes filtrado por id disa y el id red.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idDisa"></param>
        /// <param name="idRed"></param>
        /// <returns></returns>
        public RedMant GetRedByID(string idDisa, string idRed)
        {
            var objCommand = GetSqlCommand("pNLS_RedMantById");
            InputParameterAdd.Varchar(objCommand, "iddisa", idDisa);
            InputParameterAdd.Varchar(objCommand, "idred", idRed);
            var dataTable = Execute(objCommand);

            var redMant = new RedMant();
            foreach (DataRow row in dataTable.Rows)
            {
                redMant.idDisa = Converter.GetString(row, "iddisa");
                redMant.nombredisa = Converter.GetString(row, "nombredisa");
                redMant.idred = Converter.GetString(row, "idRed");
                redMant.nombrered = Converter.GetString(row, "nombreRed");
                redMant.Estado = Converter.GetInt(row, "estado");
            }
            return redMant;
        }
        /// <summary>
        /// Descripción: Obtiene las redes filtrado por id disa.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idDisa"></param>
        /// <returns></returns>
        public List<RedMant> GetRedByIDDisa(string idDisa)
        {
            var objCommand = GetSqlCommand("pNLS_RedMantbyIdDisa");
            InputParameterAdd.Varchar(objCommand, "idDisa", idDisa);

            return RedMantConverterTo.RedesCombo(Execute(objCommand));
        }
        /// <summary>
        /// Descripción: Obtiene las redes filtrado por el Nombre.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        public List<RedMant> GetRedes(string nombre)
        {
            var objCommand = GetSqlCommand("pNLS_RedMant");
            InputParameterAdd.Varchar(objCommand, "nombre", nombre);

            return RedMantConverterTo.Redes(Execute(objCommand));
        }

        /// <summary>
        /// Descripción: Registra las redes.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="red"></param>
        public void InsertRedes(RedMant red)
        {
            var objCommand = GetSqlCommand("pNLI_RedMant");

            InputParameterAdd.Varchar(objCommand, "iddisa", red.idDisa);
            InputParameterAdd.Varchar(objCommand, "idred", red.idred);
            InputParameterAdd.Varchar(objCommand, "nombrered", red.nombrered);
            InputParameterAdd.Int(objCommand, "estado", red.Estado);
            InputParameterAdd.Int(objCommand, "iduser", red.IdUsuarioRegistro);
            ExecuteNonQuery(objCommand);
        }

        /// <summary>
        /// Descripción: Actualiza redes.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="red"></param>
        public void UpdateRedes(RedMant red)
        {
            var objCommand = GetSqlCommand("pNLU_RedMant");

            InputParameterAdd.Varchar(objCommand, "iddisa", red.idDisa);
            InputParameterAdd.Varchar(objCommand, "idred", red.idred);
            InputParameterAdd.Varchar(objCommand, "nombrered", red.nombrered);
            InputParameterAdd.Int(objCommand, "estado", red.Estado);
            InputParameterAdd.Int(objCommand, "iduser", red.IdUsuarioRegistro);
            ExecuteNonQuery(objCommand);
        }

    }
}
