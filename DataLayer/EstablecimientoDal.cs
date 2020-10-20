using Framework.DAL;
using Framework.DAL.Settings.Implementations;
using System;
using System.Collections.Generic;
using System.Data;
using DataLayer.DalConverter;
using Model;

namespace DataLayer
{
    public class EstablecimientoDal : DaoBase
    {
        public EstablecimientoDal(IDalSettings settings) : base(settings)
        {
        }

        public EstablecimientoDal() : this(new NetlabSettings())
        {
        }
        /// <summary>
        /// Descripción: Obtiene establecimientos filtrado por el nombre del establecimiento.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="textoBusqueda"></param>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public List<Establecimiento> GetEstablecimientosByTextoBusqueda(String textoBusqueda, int idUsuario)
        {
            var objCommand = GetSqlCommand("pNLS_EstablecimientoByTextoBusqueda");
            InputParameterAdd.Varchar(objCommand, "textoBusqueda", textoBusqueda);
            InputParameterAdd.Int(objCommand, "idUsuario", idUsuario);            
            DataTable dataTable = Execute(objCommand);
            List<Establecimiento> establecimientoList = new List<Establecimiento>();

            foreach (DataRow row in dataTable.Rows)
            {
                Establecimiento establecimiento = new Establecimiento
                {
                    IdEstablecimiento = Converter.GetInt(row, "idEstablecimiento"),
                    CodigoInstitucion = Converter.GetString(row, "institucion"),
                    CodigoUnico = Converter.GetString(row, "codigoUnico"),
                    Nombre = Converter.GetString(row, "nombre"),
                    clasificacion = Converter.GetString(row, "clasificacion"),
                    IdLabIns = Converter.GetInt(row, "idLabIns"),
                    Ubigeo = Converter.GetString(row, "ubigeo")
                };
                establecimientoList.Add(establecimiento);
            }
            return establecimientoList;
        }
        /// <summary>
        /// Descripción: Obtiene establecimientos de la base de datos : EstablecimientoCache
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <returns></returns>
        public List<Establecimiento> GetEstablecimientosCache()
        {
            var objCommand = GetSqlCommand("pNLS_EstablecimientosCache");
            DataTable dataTable = Execute(objCommand);
            List<Establecimiento> establecimientoList = new List<Establecimiento>();

            foreach (DataRow row in dataTable.Rows)
            {
                Establecimiento establecimiento = new Establecimiento
                {
                    IdEstablecimiento = Converter.GetInt(row, "idEstablecimiento"),
                    CodigoInstitucion = Converter.GetString(row, "institucion"),
                    CodigoUnico = Converter.GetString(row, "codigoUnico"),
                    Nombre = Converter.GetString(row, "nombre")
                };
                establecimientoList.Add(establecimiento);
            }
            return establecimientoList;
        }
        /// <summary>
        /// Descripción: Obtiene establecimientos filtrado por el Codigo del Usuario
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Establecimiento GetEstablecimientoById(int id)
        {
            var objCommand = GetSqlCommand("[pNLS_EstablecimientoByUser]");
            InputParameterAdd.Int(objCommand, "id", id);

            return UsuarioConvertTo.Establecimiento(Execute(objCommand));
        }

        /// <summary>
        /// Descripción: Obtiene establecimientos filtrado por el Codigo del Usuario
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public Establecimiento GetEstablecimientoByIdUsuario(int idUsuario)
        {
            var objCommand = GetSqlCommand("[pNLS_EstablecimientoActualByIdUsuario]");
            InputParameterAdd.Int(objCommand, "IdUsuario", idUsuario);
            DataTable dataTable = Execute(objCommand);
            Establecimiento establecimiento = new Establecimiento();
            foreach (DataRow row in dataTable.Rows)
            {
                establecimiento = new Establecimiento
                {
                    IdEstablecimiento = Converter.GetInt(row, "idEstablecimiento"),
                    Nombre = Converter.GetString(row, "nombre")
                };
            }
            return establecimiento;
        }

        /// <summary>
        /// Descripción: Obtiene establecimientos frecuentes filtrado por el Codigo del Usuario
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public List<Establecimiento> GetEstablecimientosFrecuentesByIdUsuario(int idUsuario)
        {
            var objCommand = GetSqlCommand("pNLS_EstablecimientosFrecuenteByIdUsuario");
            InputParameterAdd.Int(objCommand, "idUsuario", idUsuario);
            DataTable dataTable = Execute(objCommand);
            List<Establecimiento> establecimientoList = new List<Establecimiento>();

            foreach (DataRow row in dataTable.Rows)
            {
                Establecimiento establecimiento = new Establecimiento
                {
                    IdEstablecimiento = Converter.GetInt(row, "idEstablecimiento"),
                  //  institucion = Converter.GetString(row, "institucion"),
                  //  codigoUnico = Converter.GetString(row, "codigoUnico"),
                    Nombre = Converter.GetString(row, "nombre")
                  //  departamento = Converter.GetString(row, "departamento"),
                  //  provincia = Converter.GetString(row, "provincia"),
                  //  distrito = Converter.GetString(row, "distrito")
                };
                establecimientoList.Add(establecimiento);
            }
            return establecimientoList;
        }

        /// <summary>
        /// Descripción: Obtiene establecimientos filtrado por el Nombre del Establecimiento.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        public List<Establecimiento> GetEstablecimientosByNombre(string nombre)
        {
            var objCommand = GetSqlCommand("pNLS_EstablecimientoByNombre");
            InputParameterAdd.Varchar(objCommand, "nombre", nombre);

            return EstablecimientoConvertTo.Establecimientos(Execute(objCommand));
        }

        /// <summary>
        /// Descripción: Obtiene establecimientos filtrado por el Nombre de la institucion
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="textoBusqueda"></param>
        /// <param name="idInstitucion"></param>
        /// <returns></returns>
        public List<Establecimiento> GetDisaByInstitucionByTextoBusqueda(String textoBusqueda, int idInstitucion)
        {
            var objCommand = GetSqlCommand("pNLS_DisaByInstitucionByTextoBusqueda");
            InputParameterAdd.Varchar(objCommand, "textoBusqueda", textoBusqueda);
            InputParameterAdd.Int(objCommand, "codigoInstitucion", idInstitucion);
            DataTable dataTable = Execute(objCommand);
            List<Establecimiento> establecimientoList = new List<Establecimiento>();

            foreach (DataRow row in dataTable.Rows)
            {
                Establecimiento establecimiento = new Establecimiento
                {
                    IdEstablecimiento = Converter.GetInt(row, "idDisa"),
                    CodigoInstitucion = idInstitucion.ToString(),
                    Nombre = Converter.GetString(row, "nombreDISA")
                };
                establecimientoList.Add(establecimiento);
            }
            return establecimientoList;
        }
        /// <summary>
        /// Descripción: Obtiene establecimientos de la DISA filtrado por el Nombre de la institucion
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="textoBusqueda"></param>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public List<Establecimiento> GetDisaRegionByTextoBusqueda(String textoBusqueda, int idUsuario)
        {
            List<Establecimiento> listaDisasRegion = new List<Establecimiento>();
            var objCommand = GetSqlCommand("pNLS_DisaRegionByTextoBusqueda");
            InputParameterAdd.Varchar(objCommand, "textoBusqueda", textoBusqueda);
            InputParameterAdd.Int(objCommand, "idUsuario", idUsuario);
            DataTable dataTable = Execute(objCommand);

            foreach (DataRow row in dataTable.Rows)
            {
                Establecimiento establecimientoDisa = new Establecimiento
                {
                    IdEstablecimiento = Converter.GetInt(row, "idDisa"),
                    Nombre = Converter.GetString(row, "nombreDISA")
                };
                listaDisasRegion.Add(establecimientoDisa);
            }

            return listaDisasRegion;
        }
        /// <summary>
        /// Descripción: Obtiene establecimientos de la Red filtrado por el Nombre de la institucion, id de la institucion y is de la DISA
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="textoBusqueda"></param>
        /// <param name="idInstitucion"></param>
        /// <param name="idDisa"></param>
        /// <returns></returns>
        public List<Establecimiento> GetRedByDisaByTextoBusqueda(String textoBusqueda,int idInstitucion, int idDisa)
        {
            var objCommand = GetSqlCommand("pNLS_RedByDisaByTextoBusqueda");
            InputParameterAdd.Varchar(objCommand, "textoBusqueda", textoBusqueda);
            InputParameterAdd.Int(objCommand, "codigoInstitucion", idInstitucion);
            InputParameterAdd.Int(objCommand, "idDisa", idDisa);
            DataTable dataTable = Execute(objCommand);
            List<Establecimiento> establecimientoList = new List<Establecimiento>();

            foreach (DataRow row in dataTable.Rows)
            {
                Establecimiento establecimiento = new Establecimiento
                {
                    IdEstablecimiento = Converter.GetInt(row, "idRed"),
                    CodigoInstitucion = idInstitucion.ToString(),
                    Nombre = Converter.GetString(row, "nombreRed")
                };
                establecimientoList.Add(establecimiento);
            }
            return establecimientoList;
        }

        /// <summary>
        /// Descripción: Obtiene establecimientos de la Micro Red filtrado por el Nombre de la institucion, id de la institucion, Id de la DISA y el id de la Red
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="textoBusqueda"></param>
        /// <param name="idDisa"></param>
        /// <param name="idInstitucion"></param>
        /// <param name="idRed"></param>
        /// <returns></returns>
        public List<Establecimiento> GetMicroredByRedByTextoBusqueda(String textoBusqueda, int idDisa, int idInstitucion, int idRed)
        {
            var objCommand = GetSqlCommand("pNLS_MicroredByRedByTextoBusqueda");
            InputParameterAdd.Varchar(objCommand, "textoBusqueda", textoBusqueda);
            InputParameterAdd.Int(objCommand, "codigoInstitucion", idInstitucion);
            InputParameterAdd.Int(objCommand, "idDisa", idDisa);
            InputParameterAdd.Int(objCommand, "idRed", idRed);
            DataTable dataTable = Execute(objCommand);
            List<Establecimiento> establecimientoList = new List<Establecimiento>();

            foreach (DataRow row in dataTable.Rows)
            {
                Establecimiento establecimiento = new Establecimiento
                {
                    IdEstablecimiento = Converter.GetInt(row, "idMicroRed"),
                    CodigoInstitucion = idInstitucion.ToString(),
                    Nombre = Converter.GetString(row, "nombreMicroRed")
                };
                establecimientoList.Add(establecimiento);
            }
            return establecimientoList;
        }
        /// <summary>
        /// Descripción: Obtiene establecimientos filtrado por el Nombre de la institucion, id de la institucion, Id de la DISA, el id de la Red y el id de la MicroRed
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
        public List<Establecimiento> GetEstablecimientoByRedByTextoBusqueda(String textoBusqueda, int idDisa, int idInstitucion, int idRed, int idMicrored)
        {
            var objCommand = GetSqlCommand("pNLS_EstablecimientoByMicroredByTextoBusqueda");
            InputParameterAdd.Varchar(objCommand, "textoBusqueda", textoBusqueda);
            InputParameterAdd.Int(objCommand, "codigoInstitucion", idInstitucion);
            InputParameterAdd.Int(objCommand, "idDisa", idDisa);
            InputParameterAdd.Int(objCommand, "idRed", idRed);
            InputParameterAdd.Int(objCommand, "idMicrored", idMicrored);

            DataTable dataTable = Execute(objCommand);
            List<Establecimiento> establecimientoList = new List<Establecimiento>();

            foreach (DataRow row in dataTable.Rows)
            {
                Establecimiento establecimiento = new Establecimiento
                {
                    IdEstablecimiento = Converter.GetInt(row, "idEstablecimiento"),
                    CodigoInstitucion = Converter.GetString(row, "institucion"),
                    CodigoUnico = Converter.GetString(row, "codigoUnico"),
                    Nombre = Converter.GetString(row, "nombre")
                };
                establecimientoList.Add(establecimiento);
            }
            return establecimientoList;
        }
        /// <summary>
        /// Descripción: Obtiene Laboratorios filtrado por el Nombre de la institucion y id de la institucion
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="textoBusqueda"></param>
        /// <param name="idInstitucion"></param>
        /// <returns></returns>
        public List<Establecimiento> GetDisaByInstitucionLabByTextoBusqueda(String textoBusqueda, int idInstitucion)
        {
            var objCommand = GetSqlCommand("pNLS_DisaByInstitucionLabByTextoBusqueda");
            InputParameterAdd.Varchar(objCommand, "textoBusqueda", textoBusqueda);
            InputParameterAdd.Int(objCommand, "codigoInstitucion", idInstitucion);
            DataTable dataTable = Execute(objCommand);
            List<Establecimiento> establecimientoLabList = new List<Establecimiento>();

            foreach (DataRow row in dataTable.Rows)
            {
                Establecimiento establecimiento = new Establecimiento
                {
                    IdEstablecimiento = Converter.GetInt(row, "idDisa"),
                    CodigoInstitucion = idInstitucion.ToString(),
                    Nombre = Converter.GetString(row, "nombreDISA")
                };
                establecimientoLabList.Add(establecimiento);
            }
            return establecimientoLabList;
        }

        /// <summary>
        /// Descripción: Obtiene Laboratorios filtrado por el Nombre de la institucion y id del usuario.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="textoBusqueda"></param>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public List<Establecimiento> GetDisaRegionLabByTextoBusqueda(String textoBusqueda, int idUsuario)
        {
            List<Establecimiento> listaDisasRegionLab = new List<Establecimiento>();
            var objCommand = GetSqlCommand("pNLS_DisaRegionLabByTextoBusqueda");
            InputParameterAdd.Varchar(objCommand, "textoBusqueda", textoBusqueda);
            InputParameterAdd.Int(objCommand, "idUsuario", idUsuario);
            DataTable dataTable = Execute(objCommand);

            foreach (DataRow row in dataTable.Rows)
            {
                Establecimiento establecimientoDisa = new Establecimiento
                {
                    IdEstablecimiento = Converter.GetInt(row, "idDisa"),
                    Nombre = Converter.GetString(row, "nombreDISA")
                };
                listaDisasRegionLab.Add(establecimientoDisa);
            }

            return listaDisasRegionLab;
        }

        /// <summary>
        /// Descripción: Obtiene establecimientos filtrado por el Nombre de la institucion.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="textoBusqueda"></param>
        /// <returns></returns>
        public List<Establecimiento> GetEstablecimientosAllByTextoBusqueda(String textoBusqueda)
        {
            var objCommand = GetSqlCommand("pNLS_EstablecimientoAllByTextoBusqueda");
            InputParameterAdd.Varchar(objCommand, "textoBusqueda", textoBusqueda);

            DataTable dataTable = Execute(objCommand);
            List<Establecimiento> establecimientoAllList = new List<Establecimiento>();

            foreach (DataRow row in dataTable.Rows)
            {
                Establecimiento establecimiento = new Establecimiento
                {
                    IdEstablecimiento = Converter.GetInt(row, "idEstablecimiento"),
                    CodigoInstitucion = Converter.GetString(row, "institucion"),
                    CodigoUnico = Converter.GetString(row, "codigoUnico"),
                    Nombre = Converter.GetString(row, "nombre")
                };
                establecimientoAllList.Add(establecimiento);
            }
            return establecimientoAllList;
        }

        /// <summary>
        /// Descripción: Obtiene establecimientos filtrado por el id del Usuario.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public List<UsuarioEstablecimiento> GetUsuarioEstablecimientoByUser(int idUsuario)
        {
            List<UsuarioEstablecimiento> listaUsuEstablecimiento = new List<UsuarioEstablecimiento>();
            var objCommand = GetSqlCommand("pNLS_UsuarioEstablecimiento");
            InputParameterAdd.Int(objCommand, "IdUsuario", idUsuario);

            DataTable dataTable = Execute(objCommand);

            if (dataTable != null)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    UsuarioEstablecimiento usuarioEstablecimiento = new UsuarioEstablecimiento();
                    usuarioEstablecimiento.idInstitucion = Converter.GetInt(row, "idInstitucion");
                    usuarioEstablecimiento.nomInstitucion = Converter.GetString(row, "nomInstitucion") == null ? "-" : Converter.GetString(row, "nomInstitucion");
                    usuarioEstablecimiento.idEstablecimiento = Converter.GetInt(row, "idEstablecimiento");
                    usuarioEstablecimiento.nomEstablecimiento = Converter.GetString(row, "nomEstablecimiento") == null ? "-" : Converter.GetString(row, "nomEstablecimiento");
                    usuarioEstablecimiento.idDISA = Converter.GetString(row, "idDisa");
                    usuarioEstablecimiento.nomDisa = Converter.GetString(row, "nomDisa") == null ? "-" : Converter.GetString(row, "nomDisa");
                    usuarioEstablecimiento.idRed = Converter.GetString(row, "idRed");
                    usuarioEstablecimiento.nomRed = Converter.GetString(row, "nomRed") == null ? "-" : Converter.GetString(row, "nomRed");
                    usuarioEstablecimiento.idMicrored = Converter.GetString(row, "idMicrored");
                    usuarioEstablecimiento.nomMicrored = Converter.GetString(row, "nomMicrored") == null ? "-" : Converter.GetString(row, "nomMicrored");

                    listaUsuEstablecimiento.Add(usuarioEstablecimiento);
                }
            }

            return listaUsuEstablecimiento;
        }

        /// <summary>
        /// Descripción: Elimina o inhabilita un establecimiento filtrado por el codigo de usuario y el de usuario de edicion.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idUsuarioEdicion"></param>
        /// <param name="idUsuario"></param>
        public void EliminarUsuarioEstablecimiento(int idUsuarioEdicion, int idUsuario)
        {
            var objCommand = GetSqlCommand("pNLD_UsuarioEstablecimiento");
            InputParameterAdd.Int(objCommand, "IdUsuarioEdicion", idUsuarioEdicion);
            InputParameterAdd.Int(objCommand, "IdUsuario", idUsuario);

            ExecuteNonQuery(objCommand);
        }
        /// <summary>
        /// Descripción: Obtiene establecimientos filtrado por el Id Usuario.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="establecimientoId"></param>
        /// <returns></returns>
        public Establecimiento GetEstablecimientosById(int establecimientoId)
        {
            var objCommand = GetSqlCommand("pNLS_EstablecimientoById");
            InputParameterAdd.Int(objCommand, "id", establecimientoId);

            DataTable dataTable = Execute(objCommand);

            if (dataTable.Rows.Count == 0)
                return new Establecimiento();

            var row = dataTable.Rows[0];

            return new Establecimiento()
            {
                IdEstablecimiento = Converter.GetInt(row, "idEstablecimiento"),
                CodigoInstitucion = Converter.GetString(row, "institucion"),
                CodigoUnico = Converter.GetString(row, "codigoUnico"),
                Nombre = Converter.GetString(row, "nombre")
            };
        }
        /// <summary>
        /// Descripción: Registra establecimientos por usuario.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idUsuarioRegistro"></param>
        /// <param name="idUsuario"></param>
        /// <param name="idInstitucion"></param>
        /// <param name="idDisa"></param>
        /// <param name="idRed"></param>
        /// <param name="idMicroRed"></param>
        public void InsertarUsuarioEstablecimientoByMicroRed(int idUsuarioRegistro, int idUsuario, int idInstitucion, string idDisa, string idRed, string idMicroRed)
        {
            var objCommand = GetSqlCommand("pNLI_InsertarUsuarioEstablecimientoByMicroRed");

            InputParameterAdd.Int(objCommand, "IdUsuarioRegistro", idUsuarioRegistro);
            InputParameterAdd.Int(objCommand, "IdUsuario", idUsuario);
            InputParameterAdd.Int(objCommand, "IdInstitucion", idInstitucion);
            InputParameterAdd.Varchar(objCommand, "IdDISA", idDisa);
            InputParameterAdd.Varchar(objCommand, "IdRed", idRed);
            InputParameterAdd.Varchar(objCommand, "IdMicroRed", idMicroRed);

            ExecuteNonQuery(objCommand);
        }
        /// <summary>
        /// Descripción: Registra establecimientos por usuario y Red.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idUsuarioRegistro"></param>
        /// <param name="idUsuario"></param>
        /// <param name="idInstitucion"></param>
        /// <param name="idDisa"></param>
        /// <param name="idRed"></param>
        public void InsertarUsuarioEstablecimientoByRed(int idUsuarioRegistro, int idUsuario, int idInstitucion, string idDisa, string idRed)
        {
            var objCommand = GetSqlCommand("pNLI_InsertarUsuarioEstablecimientoByRed");

            InputParameterAdd.Int(objCommand, "IdUsuarioRegistro", idUsuarioRegistro);
            InputParameterAdd.Int(objCommand, "IdUsuario", idUsuario);
            InputParameterAdd.Int(objCommand, "IdInstitucion", idInstitucion);
            InputParameterAdd.Varchar(objCommand, "IdDISA", idDisa);
            InputParameterAdd.Varchar(objCommand, "IdRed", idRed);

            ExecuteNonQuery(objCommand);
        }
        /// <summary>
        /// Descripción: Registra establecimientos por usuario, Red y MicroRed.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idUsuarioRegistro"></param>
        /// <param name="idUsuario"></param>
        /// <param name="idInstitucion"></param>
        /// <param name="idDisa"></param>
        public void InsertarUsuarioEstablecimientoByDisa(int idUsuarioRegistro, int idUsuario, int idInstitucion, string idDisa)
        {
            var objCommand = GetSqlCommand("pNLI_InsertarUsuarioEstablecimientoByDisa");

            InputParameterAdd.Int(objCommand, "IdUsuarioRegistro", idUsuarioRegistro);
            InputParameterAdd.Int(objCommand, "IdUsuario", idUsuario);
            InputParameterAdd.Int(objCommand, "IdInstitucion", idInstitucion);
            InputParameterAdd.Varchar(objCommand, "IdDISA", idDisa);

            ExecuteNonQuery(objCommand);
        }
        /// <summary>
        /// Descripción: Registra establecimientos por usuario y Id Institucion.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idUsuarioRegistro"></param>
        /// <param name="idUsuario"></param>
        /// <param name="idInstitucion"></param>
        public void InsertarUsuarioEstablecimientoByInstitucion(int idUsuarioRegistro, int idUsuario, int idInstitucion)
        {
            var objCommand = GetSqlCommand("pNLI_InsertarUsuarioEstablecimientoByInstitucion");

            InputParameterAdd.Int(objCommand, "IdUsuarioRegistro", idUsuarioRegistro);
            InputParameterAdd.Int(objCommand, "IdUsuario", idUsuario);
            InputParameterAdd.Int(objCommand, "IdInstitucion", idInstitucion);

            ExecuteNonQuery(objCommand);
        }
        /// <summary>
        /// Descripción: Elimina o inhabilita un establecimiento filtrado por el codigo de usuario y el de usuario de edicion.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idUsuarioEdicion"></param>
        /// <param name="idUsuario"></param>
        /// <param name="idEstablecimiento"></param>
        public void EliminarEstablecimiento(int idUsuarioEdicion, int idUsuario, int idEstablecimiento)
        {
            var objCommand = GetSqlCommand("pNLD_Establecimiento");

            InputParameterAdd.Int(objCommand, "IdUsuarioEdicion", idUsuarioEdicion);
            InputParameterAdd.Int(objCommand, "IdUsuario", idUsuario);
            InputParameterAdd.Int(objCommand, "IdEstablecimiento", idEstablecimiento);

            ExecuteNonQuery(objCommand);
        }

        public List<Establecimiento> GetLaboratorioINS()
        {
            List<Establecimiento> labIns = new List<Establecimiento>();
            var objCommand = GetSqlCommand("pNLS_LaboratorioINS");
            DataTable dataTable = Execute(objCommand);
            foreach (DataRow row in dataTable.Rows)
            {
                Establecimiento item = new Establecimiento
                {
                    IdEstablecimiento = Converter.GetInt(row, "idEstablecimiento"),
                    Nombre = Converter.GetString(row, "nombre")
                };

                labIns.Add(item);
            }
            return labIns;
        }

    }
}
