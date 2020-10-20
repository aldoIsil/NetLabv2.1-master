using System.Collections.Generic;
using DataLayer.DalConverter;
using Framework.DAL;
using Framework.DAL.Settings.Implementations;
using Model;
using System.Data;

namespace DataLayer
{
    public class UsuarioAreaProcesamientoDal : DaoBase
    {
        public UsuarioAreaProcesamientoDal(IDalSettings settings) : base(settings)
        {
        }

        public UsuarioAreaProcesamientoDal() : this(new NetlabSettings())
        {
        }
        /// <summary>
        /// Descripción: Obtiene Informacion de las areas enlazadas a un usuario.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public List<AreaProcesamiento> GetAreaProcesamientoByUsuarioId(int idUsuario)
        {
            List<AreaProcesamiento> listaArea = new List<AreaProcesamiento>();

            var objCommand = GetSqlCommand("pNLS_AreaByUsuarioId");
            InputParameterAdd.Int(objCommand, "idUsuario", idUsuario);

            DataTable dataArea = Execute(objCommand);
            if (dataArea.Rows.Count == 0)
                return null;

            for (int i = 0; i < dataArea.Rows.Count; i++)
            {
                var row = dataArea.Rows[i];
                AreaProcesamiento area = new AreaProcesamiento();
                area.IdAreaProcesamiento = int.Parse(row["idAreaProcesamiento"].ToString());
                area.Nombre = row["nombre"].ToString();
                area.Descripcion = row["descripcion"].ToString();

                listaArea.Add(area);
            }

            return listaArea;
        }
        /// <summary>
        /// Descripción: Registra informacion del area de procesamiento para un usuario.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="usuarioAreaProcesamiento"></param>
        public void InsertAreaByUsuario(UsuarioAreaProcesamiento usuarioAreaProcesamiento)
        {
            var objCommand = GetSqlCommand("pNLI_UsuarioAreaProcesamiento");

            InputParameterAdd.Int(objCommand, "idUsuario", usuarioAreaProcesamiento.idUsuario);
            InputParameterAdd.Int(objCommand, "idAreaProcesamiento", usuarioAreaProcesamiento.idAreaProcesamiento);
            InputParameterAdd.Int(objCommand, "idUsuarioRegistro", usuarioAreaProcesamiento.IdUsuarioRegistro);

            ExecuteNonQuery(objCommand);
        }
        /// <summary>
        /// Descripción: Actualiza informacion del area de procesamiento para un usuario.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="usuarioAreaProcesamiento"></param>
        public void UpdateAreaByUsuario(UsuarioAreaProcesamiento usuarioAreaProcesamiento)
        {
            var objCommand = GetSqlCommand("pNLU_UsuarioAreaProcesamiento");

            InputParameterAdd.Int(objCommand, "idUsuario", usuarioAreaProcesamiento.idUsuario);
            InputParameterAdd.Int(objCommand, "idAreaProcesamiento", usuarioAreaProcesamiento.idAreaProcesamiento);
            InputParameterAdd.Int(objCommand, "estado", usuarioAreaProcesamiento.Estado);
            InputParameterAdd.Int(objCommand, "idUsuarioEdicion", usuarioAreaProcesamiento.IdUsuarioEdicion);

            ExecuteNonQuery(objCommand);
        }
    }
}