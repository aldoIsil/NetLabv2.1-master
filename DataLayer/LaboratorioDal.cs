using System.Collections.Generic;
using System.Data;
using System.Linq;
using DataLayer.DalConverter;
using Framework.DAL;
using Framework.DAL.Settings.Implementations;
using Model;
using System;

namespace DataLayer
{
    public class LaboratorioDal : DaoBase
    {
        public LaboratorioDal(IDalSettings settings) : base(settings)
        {
        }

        public LaboratorioDal() : this(new NetlabSettings())
        {
        }
        /// <summary>
        /// Descripción: Obtiene informacion de los laboratorios por el Id del Usuario
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="textoBusqueda"></param>
        /// <param name="idUsuario"></param>
        /// <param name="idExamen"></param>
        /// <returns></returns>
        public List<LaboratorioVMSelect> GetLaboratoriosByTextoBusqueda(string textoBusqueda, int idUsuario, Guid? idExamen)
        {

            String storedProcedured = "pNLS_LaboratorioByTextoBusqueda";
            if (idExamen != null)
            {
                storedProcedured = "pNLS_LaboratorioByTextoBusquedaByExamen";
            }
            

            var objCommand = GetSqlCommand(storedProcedured);
            InputParameterAdd.Varchar(objCommand, "textoBusqueda", textoBusqueda);
            InputParameterAdd.Int(objCommand, "idUsuario", idUsuario);
            if (idExamen != null)
                InputParameterAdd.Guid(objCommand, "idExamen", idExamen);
            
            var dataTable = Execute(objCommand);

            return (from DataRow row in dataTable.Rows
                select new LaboratorioVMSelect
                {
                    IdEstablecimiento = Converter.GetInt(row, "idLaboratorio"),
                    CodigoUnico = Converter.GetString(row, "codigoUnico"),
                    Nombre = Converter.GetString(row, "nombre"),
                    Ubigeo = Converter.GetString(row, "ubigeo"),
                    Direccion = Converter.GetString(row, "direccion"),
                    clasificacion = Converter.GetString(row,"clasificacion"),
                    IdLabIns = Converter.GetInt(row, "idLabIns")
                }).ToList();

            /*
            Nombre = Converter.GetString(row, "nombre")
                    Nombre = Converter.GetString(row, "nombre")
                    Nombre = Converter.GetString(row, "nombre")
                    Nombre = Converter.GetString(row, "nombre")*/
        }

        /// <summary>
        /// Descripción: Metodo para obtener informacion de labortorio filtrado por el nombre.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 1.02/02/2017.
        ///                     2.05/04/2017
        /// Modificación: 1.Se agregaron comentarios.
        ///               2.Se cambio el nombre del paratmetro nombre por textoBusqueda. - Juan Muga.
        /// </summary>
        /// <param name="textoBusqueda"></param>
        /// <returns></returns>
        public List<Laboratorio> GetLaboratoriosByNombre(string textoBusqueda)
        {
            var objCommand = GetSqlCommand("pNLS_LaboratorioByNombre");
            InputParameterAdd.Varchar(objCommand, "textoBusqueda", textoBusqueda);

            return LaboratorioConvertTo.Laboratorios(Execute(objCommand));
        }

        /// <summary>
        /// Descripción: Obtiene informacion de los laboratorios por el Id
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Laboratorio GetLaboratorioById(int id)
        {
            var objCommand = GetSqlCommand("pNLS_LaboratorioById");
            InputParameterAdd.Int(objCommand, "idLaboratorio", id);

            return LaboratorioConvertTo.Laboratorio(Execute(objCommand));
        }
        /// <summary>
        /// Descripción: Metodo para obtener informacion de labortorio filtrado por  el codigo de usuario.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public Laboratorio GetLaboratorioByUserId(int idUsuario)
        {
            var objCommand = GetSqlCommand("pNLS_LaboratorioByUserId");
            InputParameterAdd.Int(objCommand, "idUsuario", idUsuario);

            return LaboratorioConvertTo.Laboratorio(Execute(objCommand));
        }
        /// <summary>
        /// Descripción: Metodo para obtener informacion de labortorio filtrado por un texto.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="textoBusqueda"></param>
        /// <returns></returns>
        public List<Laboratorio> GetLaboratoriosAllByTextoBusqueda(string textoBusqueda)
        {
            var objCommand = GetSqlCommand("pNLS_LaboratorioAllByTextoBusqueda");
            InputParameterAdd.Varchar(objCommand, "textoBusqueda", textoBusqueda);
            var dataTable = Execute(objCommand);

            return (from DataRow row in dataTable.Rows
                    select new Laboratorio
                    {
                        IdLaboratorio = Converter.GetInt(row, "idLaboratorio"),
                        CodigoUnico = Converter.GetString(row, "codigoUnico"),
                        Nombre = Converter.GetString(row, "nombre")
                    }).ToList();
        }

        /// <summary>
        /// Descripción: Metodo para Actualizar informacion de los laboratorios
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// Actualizado por: Marcos Mejia
        /// Fecha: 19/01/2017
        /// Modificación: Se agrega dato del logo Regional.
        /// </summary>
        /// <param name="laboratorio"></param>
        public void UpdateLaboratorio(Laboratorio laboratorio)
        {


            var objCommand = GetSqlCommand("pNLU_Laboratorio");
            InputParameterAdd.Int(objCommand, "idLaboratorio", laboratorio.IdLaboratorio);
            InputParameterAdd.Varchar(objCommand, "nombre", laboratorio.Nombre);
            InputParameterAdd.Varchar(objCommand, "direccion", laboratorio.Direccion);
            InputParameterAdd.Varchar(objCommand, "correoelectronico", laboratorio.correoElectronico);
            InputParameterAdd.Varchar(objCommand, "clasificacion", laboratorio.clasificacion);
            InputParameterAdd.Int(objCommand, "idcategoria", laboratorio.IdCategoria);
            InputParameterAdd.Int(objCommand, "tipo", laboratorio.tipo);
            InputParameterAdd.Varchar(objCommand, "latitud", laboratorio.Latitud);
            InputParameterAdd.Varchar(objCommand, "longitud", laboratorio.Longitud);
            InputParameterAdd.VarBinary(objCommand, "logoReg", laboratorio.LogoRegional);
            InputParameterAdd.VarBinary(objCommand, "logo", laboratorio.Logo);
            InputParameterAdd.VarBinary(objCommand, "sello", laboratorio.Sello);
            InputParameterAdd.Varchar(objCommand, "website", laboratorio.Website);
            InputParameterAdd.Int(objCommand, "estado", laboratorio.Estado);
            InputParameterAdd.Int(objCommand, "idUsuarioEdicion", laboratorio.IdUsuarioEdicion);

            ExecuteNonQuery(objCommand);
        }

        /// <summary>
        /// Descripción: Metodo para Registrar informacion de los laboratorios
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        ///  Actualizado por: Marcos Mejia
        /// Fecha: 19/01/2017
        /// Modificación: Se agrega dato del logo Regional.
        /// </summary>
        /// <param name="laboratorio"></param>
        public void InsertLaboratorio(Laboratorio laboratorio)
        {
            var objCommand = GetSqlCommand("pNLI_Laboratorio");
            InputParameterAdd.Int(objCommand, "codigoinstitucion", laboratorio.CodigoInstitucion);
            InputParameterAdd.Varchar(objCommand, "codigounico", laboratorio.CodigoUnico);
            InputParameterAdd.Varchar(objCommand, "nombre", laboratorio.Nombre);
            InputParameterAdd.Varchar(objCommand, "clasificacion", laboratorio.clasificacion);
            InputParameterAdd.Varchar(objCommand, "ubigueo", laboratorio.Ubigeo);
            InputParameterAdd.Varchar(objCommand, "direccion", laboratorio.Direccion);

            InputParameterAdd.Varchar(objCommand, "iddisa", laboratorio.IdDisa);
            InputParameterAdd.Varchar(objCommand, "idred", laboratorio.IdRed);
            InputParameterAdd.Varchar(objCommand, "idmicrored", laboratorio.IdMicroRed);
            InputParameterAdd.Int(objCommand, "idcategoria", laboratorio.IdCategoria);

            InputParameterAdd.Varchar(objCommand, "latitud", laboratorio.Latitud);
            InputParameterAdd.Varchar(objCommand, "longitud", laboratorio.Longitud);

            InputParameterAdd.Int(objCommand, "tipo", laboratorio.tipo);

            InputParameterAdd.Varchar(objCommand, "website", laboratorio.Website);
            InputParameterAdd.Varchar(objCommand, "correoElectronico", laboratorio.correoElectronico);
            InputParameterAdd.VarBinary(objCommand, "logoRegional", laboratorio.LogoRegional);
            InputParameterAdd.VarBinary(objCommand, "logo", laboratorio.Logo);
            InputParameterAdd.VarBinary(objCommand, "sello", laboratorio.Sello);
          
            
            InputParameterAdd.Int(objCommand, "idUsuario", laboratorio.IdUsuarioRegistro);

            ExecuteNonQuery(objCommand);
        }

        /// <summary>
        /// Descripción: Metodo para obtener informacion de los laboratorios filtrados por Disa,Institucion,Red y microred.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="textoBusqueda"></param>
        /// <param name="idDisa"></param>
        /// <param name="idInstitucion"></param>
        /// <param name="idRed"></param>
        /// <param name="idMicrored"></param>
        /// <returns></returns>
        public List<Laboratorio> GetLaboratorioByMicroredByTextoBusqueda(String textoBusqueda, int idInstitucion, int idDisa, int idRed, int idMicrored)
        {
            var objCommand = GetSqlCommand("pNLS_LaboratorioByMicroredByTextoBusqueda");
            InputParameterAdd.Varchar(objCommand, "textoBusqueda", textoBusqueda);
            InputParameterAdd.Int(objCommand, "codigoInstitucion", idInstitucion);
            InputParameterAdd.Int(objCommand, "idDisa", idDisa);
            InputParameterAdd.Int(objCommand, "idRed", idRed);
            InputParameterAdd.Int(objCommand, "idMicrored", idMicrored);

            DataTable dataTable = Execute(objCommand);
            List<Laboratorio> laboratorioList = new List<Laboratorio>();

            foreach (DataRow row in dataTable.Rows)
            {
                Laboratorio laboratorio = new Laboratorio
                {
                    IdLaboratorio = Converter.GetInt(row, "idLaboratorio"),
                    CodigoInstitucion = Converter.GetInt(row, "institucion"),
                    CodigoUnico = Converter.GetString(row, "codigoUnico"),
                    Nombre = Converter.GetString(row, "nombre")
                };
                laboratorioList.Add(laboratorio);
            }
            return laboratorioList;
        }

        /// <summary>
        /// Descripción: Metodo para obtener informacion de los laboratorios por Usuario.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public List<Laboratorio> GetListaLaboratorioByUserId(int idUsuario)
        {
            var objCommand = GetSqlCommand("pNLS_LaboratoriosByUserId");
            InputParameterAdd.Int(objCommand, "idUser", idUsuario);

            var dataTable = Execute(objCommand);

            return (from DataRow row in dataTable.Rows
                    select new Laboratorio
                    {
                        IdLaboratorio = Converter.GetInt(row, "idLaboratorio"),
                        CodigoUnico = Converter.GetString(row, "codigoUnico"),
                        Nombre = Converter.GetString(row, "nombre")
                    }).ToList();
        }
        /// <summary>
        /// Descripción: Metodo para obtener informacion de los laboratorios por Usuario.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public List<UsuarioLaboratorio> GetUsuarioLaboratorioByUser(int idUsuario)
        {
            List<UsuarioLaboratorio> listaUsuLaboratorio = new List<UsuarioLaboratorio>();
            var objCommand = GetSqlCommand("pNLS_UsuarioLaboratorio");
            InputParameterAdd.Int(objCommand, "IdUsuario", idUsuario);

            DataTable dataTable = Execute(objCommand);

            if (dataTable != null)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    UsuarioLaboratorio usuarioLaboratorio = new UsuarioLaboratorio
                    {
                        idInstitucion = Converter.GetInt(row, "idInstitucion"),
                        nomInstitucion = Converter.GetString(row, "nomInstitucion"),
                        idLaboratorio = Converter.GetInt(row, "idLaboratorio"),
                        nomLaboratorio = Converter.GetString(row, "nomLaboratorio"),
                        idDISA = Converter.GetInt(row, "idDisa"),
                        nomDisa = Converter.GetString(row, "nomDisa"),
                        idRed = Converter.GetInt(row, "idRed"),
                        nomRed = Converter.GetString(row, "nomRed"),
                        idMicrored = Converter.GetInt(row, "idMicrored"),
                        nomMicrored = Converter.GetString(row, "nomMicrored")
                    };
                    listaUsuLaboratorio.Add(usuarioLaboratorio);
                }
            }

            return listaUsuLaboratorio;
        }

        public List<LaboratorioVMSelect> GetLaboratoriossStaticCache()
        {
            string storedProcedured = "pNLS_Laboratorios";
            var objCommand = GetSqlCommand(storedProcedured);
            var dataTable = Execute(objCommand);

            return (from DataRow row in dataTable.Rows
                    select new LaboratorioVMSelect
                    {
                        IdEstablecimiento = Converter.GetInt(row, "idEstablecimiento"),
                        CodigoUnico = Converter.GetString(row, "codigoUnico"),
                        Nombre = Converter.GetString(row, "nombre"),
                        NombreEstablecimiento = Converter.GetString(row, "NombreEstablecimiento"),
                        Ubigeo = Converter.GetString(row, "ubigeo"),
                        Direccion = Converter.GetString(row, "direccion"),
                        clasificacion = Converter.GetString(row, "clasificacion"),
                        IdLabIns = Converter.GetIntNullable(row, "idLabIns")
                    }).ToList();
        }
    }
}

