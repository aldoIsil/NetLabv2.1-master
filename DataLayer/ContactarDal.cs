using Framework.DAL;
using Framework.DAL.Settings.Implementations;
using System.Collections.Generic;
using DataLayer.DalConverter;
using Model;
namespace DataLayer
{
    public class ContactarDal : DaoBase
    {
        public ContactarDal(IDalSettings settings) : base(settings)
        {
        }

        public ContactarDal() : this(new NetlabSettings())
        {
        }
        /// <summary>
        /// Descripción: Registra datos para la sugerencia
        /// Author: Jose Chavez
        /// Fecha Creación : 15/06/2019
        /// </summary>
        /// <param name="contactar"></param>
        public void InsertQueja (Contactar contactar)
        {
            var objCommand = GetSqlCommand("pNLI_Queja");
            InputParameterAdd.Varchar(objCommand, "nombres", contactar.nombres);
            InputParameterAdd.Varchar(objCommand, "apellidos", contactar.apellidos);
            InputParameterAdd.Varchar(objCommand, "celular", contactar.celular);
            InputParameterAdd.Varchar(objCommand, "cargo", contactar.cargo);
            InputParameterAdd.Int(objCommand, "idEstablecimiento", contactar.idEstablecimiento);
            InputParameterAdd.Varchar(objCommand, "email", contactar.email);
            InputParameterAdd.Varchar(objCommand, "mensaje", contactar.mensaje);


            ExecuteNonQuery(objCommand);
        }
    }
}
