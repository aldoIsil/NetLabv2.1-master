using System.Collections.Generic;
using System.Data;
using System.Linq;
using Framework.DAL;
using Framework.DAL.Settings.Implementations;
using Model;
using System;

namespace DataLayer
{
    public class InstitucionDal : DaoBase
    {
        public InstitucionDal(IDalSettings settings) : base(settings)
        {
        }

        public InstitucionDal() : this(new NetlabSettings())
        {
        }
        /// <summary>
        /// Descripción: Obtiene Informacion de las instituciones, realiza busqueda filtrado por la descripcion de la institucion
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="textoBusqueda"></param>
        /// <returns></returns>
        public List<Institucion> GetInstitucionByTextoBusqueda(string textoBusqueda)
        {

            String storedProcedured = "pNLS_InstitucionByTextoBusqueda";
           
            var objCommand = GetSqlCommand(storedProcedured);
            InputParameterAdd.Varchar(objCommand, "textoBusqueda", textoBusqueda);
            
            var dataTable = Execute(objCommand);
                        
            return (from DataRow row in dataTable.Rows
                select new Institucion
                {
                    codigoInstitucion = Converter.GetInt(row, "codigoInstitucion"),
                    nombreInstitucion = Converter.GetString(row, "nombreInstitucion"),
                    estado = Converter.GetInt(row, "estado")

                }).ToList();
        }
        /// <summary>
        /// Descripción: Obtiene Informacion de todas las instituciones.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <returns></returns>
        public List<Institucion> GetInstituciones()
        {
            var objCommand = GetSqlCommand("pNLS_Institucion");

            var dataTable = Execute(objCommand);

            return (from DataRow row in dataTable.Rows
                select new Institucion
                {
                    codigoInstitucion = Converter.GetInt(row, "codigoInstitucion"),
                    nombreInstitucion = Converter.GetString(row, "nombreInstitucion"),
                    estado = Converter.GetInt(row, "estado")
                }).ToList();
        }
        /// <summary>
        /// Descripción: Obtiene Informacion de las instituciones, realiza busqueda filtrado por el codigo de usuario.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public List<Institucion> GetInstitucionesByUsuarioId(int idUsuario)
        {
            const string storedProcedured = "pNLS_InstitucionByUsuario";

            var objCommand = GetSqlCommand(storedProcedured);
            InputParameterAdd.Int(objCommand, "IdUsuario", idUsuario);

            var dataTable = Execute(objCommand);

            return (from DataRow row in dataTable.Rows
                    select new Institucion
                    {
                        IdInstitucion = Converter.GetString(row, "codigoInstitucion"),
                        nombreInstitucion = Converter.GetString(row, "nombreInstitucion")
                    }).ToList();
        }
        /// <summary>
        /// Descripción: Obtiene Informacion de las instituciones, realiza busqueda filtrado por el codigo de usuario y de la institucion.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <param name="idInstitucion"></param>
        /// <returns></returns>
        public List<DISA> GetInstitucionesByInstitucionUsuario(int idUsuario, int idInstitucion)
        {
            const string storedProcedured = "pNLS_DISAbyInstitucionByUsuario";

            var objCommand = GetSqlCommand(storedProcedured);
            InputParameterAdd.Int(objCommand, "IdUsuario", idUsuario);
            InputParameterAdd.Int(objCommand, "IdInstitucion", idInstitucion);

            var dataTable = Execute(objCommand);

            return (from DataRow row in dataTable.Rows
                    select new DISA
                    {
                        IdDISA = Converter.GetString(row, "idDISA"),
                        NombreDISA = Converter.GetString(row, "nombreDISA")
                    }).ToList();
        }
        /// <summary>
        /// Descripción: Obtiene Informacion de las redes, realiza busqueda filtrado por el codigo de usuario, id institucion y el id de la DISA.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <param name="idInstitucion"></param>
        /// <param name="idDisa"></param>
        /// <returns></returns>
        public List<Red> GetInstitucionesByInstitucionDisaUsuario(int idUsuario, int idInstitucion, string idDisa)
        {
            const string storedProcedured = "pNLS_REDbyDISAbyInstitucionByUsuario";

            var objCommand = GetSqlCommand(storedProcedured);
            InputParameterAdd.Int(objCommand, "IdUsuario", idUsuario);
            InputParameterAdd.Int(objCommand, "IdInstitucion", idInstitucion);
            InputParameterAdd.Varchar(objCommand, "IdDISA", idDisa);

            var dataTable = Execute(objCommand);

            return (from DataRow row in dataTable.Rows
                    select new Red
                    {
                        IdRed = Converter.GetString(row, "idRed"),
                        NombreRed = Converter.GetString(row, "nombreRed"),
                        IdDISA = Converter.GetString(row, "idDISA")
                    }).ToList();
        }
        /// <summary>
        /// Descripción: Obtiene Informacion de las instituciones, realiza busqueda filtrado por el codigo de usuario, id institucion, el id de la DISA y el id de la Red.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <param name="idInstitucion"></param>
        /// <param name="idDisa"></param>
        /// <param name="idRed"></param>
        /// <returns></returns>
        public List<MicroRed> GetInstitucionesByInstitucionDisaRedUsuario(int idUsuario, int idInstitucion, string idDisa, string idRed)
        {
            const string storedProcedured = "pNLS_MicroREDbyDISAbyInstByUsuario";

            var objCommand = GetSqlCommand(storedProcedured);
            InputParameterAdd.Int(objCommand, "IdUsuario", idUsuario);
            InputParameterAdd.Int(objCommand, "IdInstitucion", idInstitucion);
            InputParameterAdd.Varchar(objCommand, "IdDISA", idDisa);
            InputParameterAdd.Varchar(objCommand, "IdRed", idRed);

            var dataTable = Execute(objCommand);

            return (from DataRow row in dataTable.Rows
                    select new MicroRed
                    {
                        IdMicroRed = Converter.GetString(row, "idMicroRed"),
                        NombreMicroRed = Converter.GetString(row, "nombreMicroRed"),
                        IdDISA = Converter.GetString(row, "idDISA"),
                        IdRed = Converter.GetString(row, "idRed")
                    }).ToList();
        }
        /// <summary>
        /// Descripción: Obtiene Informacion de las instituciones, realiza busqueda filtrado por el codigo de usuario, id institucion, el id de la DISA, el id de la Red y el id de la MicroRed.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <param name="idInstitucion"></param>
        /// <param name="idDisa"></param>
        /// <param name="idRed"></param>
        /// <param name="idMicroRed"></param>
        /// <returns></returns>
        public List<Establecimiento> GetInstitucionesByInstitucionDisaRedMicroRedUsuario(int idUsuario, int idInstitucion, string idDisa, string idRed, string idMicroRed)
        {
            const string storedProcedured = "pNLS_EstMicroREDbyDISAbyInstByUsuario";

            var objCommand = GetSqlCommand(storedProcedured);
            InputParameterAdd.Int(objCommand, "IdUsuario", idUsuario);
            InputParameterAdd.Int(objCommand, "IdInstitucion", idInstitucion);
            InputParameterAdd.Varchar(objCommand, "IdDISA", idDisa);
            InputParameterAdd.Varchar(objCommand, "IdRed", idRed);
            InputParameterAdd.Varchar(objCommand, "IdMicroRed", idMicroRed);
            InputParameterAdd.Varchar(objCommand, "Nombre", " ");

            var dataTable = Execute(objCommand);

            return (from DataRow row in dataTable.Rows
                    select new Establecimiento
                    {
                        IdEstablecimiento = Converter.GetInt(row, "idEstablecimiento"),
                        CodigoUnico = Converter.GetString(row, "codigounico"),
                        Nombre = Converter.GetString(row, "nombre"),
                        clasificacion = Converter.GetString(row, "clasificacion"),
                        idCategoria = Converter.GetInt(row, "idCategoria")
                    }).ToList();
        }

        public List<MicroRed> GetMicroRedes()
        {
            const string storedProcedured = "pNLS_MicroRedes";

            var objCommand = GetSqlCommand(storedProcedured);

            var dataTable = Execute(objCommand);

            return (from DataRow row in dataTable.Rows
                    select new MicroRed
                    {
                        IdMicroRed = Converter.GetString(row, "idMicroRed"),
                        NombreMicroRed = Converter.GetString(row, "nombreMicroRed"),
                        IdDISA = Converter.GetString(row, "idDISA"),
                        IdRed = Converter.GetString(row, "idRed"),
                        IdInstitucion = Converter.GetInt(row, "codigoInstitucion")
                    }).ToList();
        }

        public List<Establecimiento> GetEstablecimientos()
        {
            const string storedProcedured = "pNLS_Establecimientos";

            var objCommand = GetSqlCommand(storedProcedured);
            var dataTable = Execute(objCommand);

            return (from DataRow row in dataTable.Rows
                    select new Establecimiento
                    {
                        IdEstablecimiento = Converter.GetInt(row, "idEstablecimiento"),
                        CodigoMicroRed = Converter.GetString(row, "idMicroRed"),
                        CodigoRed = Converter.GetString(row, "idRed"),
                        CodigoDisa = Converter.GetString(row, "idDISA"),
                        CodigoUnico = Converter.GetString(row, "codigounico"),
                        CodigoInstitucion = Converter.GetString(row, "codigoInstitucion"),
                        Nombre = Converter.GetString(row, "nombre")
                    }).ToList();
        }

        public List<Institucion> ObtenerInstitucionPorTexto(string textoBusqueda)
        {
            List<Institucion> institucion = new List<Institucion>();
            var objCommand = GetSqlCommand("pNLM_MantenedorInstitucion");
            InputParameterAdd.Int(objCommand, "Operacion", 1);
            InputParameterAdd.Varchar(objCommand, "nombreInstitucion", textoBusqueda);
            DataTable dataTable = Execute(objCommand);
            if (dataTable.Rows.Count > 0)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    var row = dataTable.Rows[i];
                    Institucion lista = new Institucion();
                    lista.codigoInstitucion = int.Parse(row["codigoInstitucion"].ToString());
                    lista.nombreInstitucion = row["nombreInstitucion"].ToString();
                    lista.descripcion = row["descripcion"].ToString();
                    lista.estado = int.Parse(row["estado"].ToString());
                    institucion.Add(lista);
                }
            }else
            {
                return institucion;
            }
            return institucion;
        }

        public void IngresarInstitucion(string nombreInstitucion, int idUsuario, string descripcion)
        {
            var objCommand = GetSqlCommand("pNLM_MantenedorInstitucion");
            InputParameterAdd.Int(objCommand, "Operacion", 2);
            InputParameterAdd.Varchar(objCommand, "nombreInstitucion", nombreInstitucion);
            InputParameterAdd.Int(objCommand, "idUsuario", idUsuario);
            InputParameterAdd.Varchar(objCommand, "descripcion", descripcion);
            ExecuteNonQuery(objCommand);
        }

        public void ActualizarInstitucion(int codigoInstitucion, string nombreInstitucion, int idUsuario, string descripcion, int estado)
        {
            var objCommand = GetSqlCommand("pNLM_MantenedorInstitucion");
            InputParameterAdd.Int(objCommand, "Operacion", 3);
            InputParameterAdd.Varchar(objCommand, "nombreInstitucion", nombreInstitucion);
            InputParameterAdd.Int(objCommand, "idUsuario", idUsuario);
            InputParameterAdd.Varchar(objCommand, "descripcion", descripcion);
            InputParameterAdd.Int(objCommand, "estado", estado);
            InputParameterAdd.Int(objCommand, "codigoInstitucion", codigoInstitucion);
            ExecuteNonQuery(objCommand);
        }

        public Institucion ObtenerInstitucionPorId(int id)
        {
            Institucion institucion = new Institucion();
            var objCommand = GetSqlCommand("pNLM_MantenedorInstitucion");
            InputParameterAdd.Int(objCommand, "Operacion", 4);
            InputParameterAdd.Int(objCommand, "codigoInstitucion", id);
            DataTable dataTable = Execute(objCommand);
            if (dataTable.Rows.Count > 0)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    var row = dataTable.Rows[i];
                    institucion.codigoInstitucion = int.Parse(row["codigoInstitucion"].ToString());
                    institucion.nombreInstitucion = row["nombreInstitucion"].ToString();
                    institucion.descripcion = row["descripcion"].ToString();
                    institucion.estado = int.Parse(row["estado"].ToString());
                }
            }
            else
            {
                return institucion;
            }
            return institucion;
        }
    }
}

