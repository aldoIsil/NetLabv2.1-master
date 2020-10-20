using DataLayer.DalConverter;
using Framework.DAL;
using Framework.DAL.Settings.Implementations;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class MicroRedMantDal:DaoBase
    {

        public MicroRedMantDal(IDalSettings settings) : base(settings)
        {
        }

        public MicroRedMantDal() : this(new NetlabSettings())
        {
        }
        /// <summary>
        /// Descripción: Obtiene las Microredes filtradas por el disa, red y microred.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idDisa"></param>
        /// <param name="idRed"></param>
        /// <param name="idmicrored"></param>
        /// <returns></returns>
        public MicroRedMant GetMicroRedByID(string idDisa, string idRed, string idmicrored)
        {
            var objCommand = GetSqlCommand("pNLS_MicroRedMantById");
            InputParameterAdd.Varchar(objCommand, "iddisa", idDisa);
            InputParameterAdd.Varchar(objCommand, "idred", idRed);
            InputParameterAdd.Varchar(objCommand, "idmicrored", idmicrored);
            var dataTable = Execute(objCommand);

            var microredMant = new MicroRedMant();
            foreach (DataRow row in dataTable.Rows)
            {
                microredMant.iddisa = Converter.GetString(row, "iddisa");
                microredMant.nombredisa = Converter.GetString(row, "nombredisa");
                microredMant.idred = Converter.GetString(row, "idRed");
                microredMant.nombrered = Converter.GetString(row, "nombreRed");
                microredMant.idmicrored= Converter.GetString(row, "idmicrored");
                microredMant.nombremicrored = Converter.GetString(row, "nombremicrored");
                microredMant.Estado = Converter.GetInt(row, "estado");
            }
            return microredMant;
        }
        /// <summary>
        /// Descripción: Obtiene las Microredes filtradas por el nombre.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        public List<MicroRedMant> GetMicroRedes(string nombre)
        {
            var objCommand = GetSqlCommand("pNLS_MicroRedMant");
            InputParameterAdd.Varchar(objCommand, "nombre", nombre);

            return MicroRedMantConvertTo.MicroRedes(Execute(objCommand));
        }
        /// <summary>
        /// Descripción: Insertar una microred.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="microred"></param>
        public void InsertMicroRedes(MicroRedMant microred)
        {
            var objCommand = GetSqlCommand("pNLI_MicroRedMant");

            InputParameterAdd.Varchar(objCommand, "iddisa", microred.iddisa);
            InputParameterAdd.Varchar(objCommand, "idred", microred.idred);
            InputParameterAdd.Varchar(objCommand, "idmicrored", microred.idmicrored);
            InputParameterAdd.Varchar(objCommand, "nombremicrored", microred.nombremicrored);
            InputParameterAdd.Int(objCommand, "estado", microred.Estado);
            InputParameterAdd.Int(objCommand, "iduser", microred.IdUsuarioRegistro);
            ExecuteNonQuery(objCommand); 
        }
        /// <summary>
        /// Descripción: Actualiza MicroRedes.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="microred"></param>
        public void UpdateMicroRedes(MicroRedMant microred)
        {
            var objCommand = GetSqlCommand("pNLU_MicroRedMant"); 

            InputParameterAdd.Varchar(objCommand, "iddisa", microred.iddisa);
            InputParameterAdd.Varchar(objCommand, "idred", microred.idred);
            InputParameterAdd.Varchar(objCommand, "idmicrored", microred.idmicrored);
            InputParameterAdd.Varchar(objCommand, "nombremicrored", microred.nombremicrored);
            InputParameterAdd.Int(objCommand, "estado", microred.Estado);
            InputParameterAdd.Int(objCommand, "iduser", microred.IdUsuarioRegistro);

            ExecuteNonQuery(objCommand);
        }
    }
}
