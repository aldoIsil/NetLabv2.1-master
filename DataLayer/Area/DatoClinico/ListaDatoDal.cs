using System.Collections.Generic;
using DataLayer.DalConverter;
using Framework.DAL;
using Framework.DAL.Settings.Implementations;
using Model;

namespace DataLayer.Area.DatoClinico
{
    public class ListaDatoDal : DaoBase
    {
        public ListaDatoDal(IDalSettings settings) : base(settings)
        {
        }

        public ListaDatoDal() : this(new NetlabSettings())
        {
        }
        /// <summary>
        /// Descripción: Obtiene informacion de las Lista de datos activos
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <returns></returns>
        public List<ListaDato> GetListaDato()
        {
            var objCommand = GetSqlCommand("pNLS_ListaDato");

            return ListaDatoConvertTo.ListaDatos(Execute(objCommand));
        }
        /// <summary>
        /// Descripción: Obtiene informacion de las listas de datos activos filtrado por el Id de ListaDato.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idListaDato"></param>
        /// <returns></returns>
        public ListaDato GetListaDatoById(int idListaDato)
        {
            var objCommand = GetSqlCommand("pNLS_ListaDatoById");
            InputParameterAdd.Int(objCommand, "idListaDato", idListaDato);

            return ListaDatoConvertTo.ListaDato(Execute(objCommand));
        }
        /// <summary>
        /// Descripción: Obtiene informacion de las opciones activas filtradas por el id de listas.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idListaDato"></param>
        /// <returns></returns>
        public List<OpcionDato> GetOpcionDatoByLista(int idListaDato)
        {
            var objCommand = GetSqlCommand("pNLS_OpcionDatoByLista");
            InputParameterAdd.Int(objCommand, "idListaDato", idListaDato);

            return ListaDatoConvertTo.Opciones(Execute(objCommand));
        }
        /// <summary>
        /// Descripción: Registra informacion de la Lista de Datos.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="listaDato"></param>
        public void InsertListaDato(ListaDato listaDato)
        {
            var objCommand = GetSqlCommand("pNLI_ListaDato");

            InputParameterAdd.Varchar(objCommand, "nombre", listaDato.Nombre);
            InputParameterAdd.Varchar(objCommand, "descripcion", listaDato.Descripcion);
            InputParameterAdd.Int(objCommand, "idUsuarioRegistro", listaDato.IdUsuarioRegistro);

            ExecuteNonQuery(objCommand);
        }
        /// <summary>
        /// Descripción: Actualiza informacion de las listas/datos.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="listaDato"></param>
        public void UpdateListaDato(ListaDato listaDato)
        {
            var objCommand = GetSqlCommand("pNLU_ListaDato");

            InputParameterAdd.Int(objCommand, "idListaDato", listaDato.IdListaDato);
            InputParameterAdd.Varchar(objCommand, "nombre", listaDato.Nombre);
            InputParameterAdd.Varchar(objCommand, "descripcion", listaDato.Descripcion);
            InputParameterAdd.Int(objCommand, "estado", listaDato.Estado);
            InputParameterAdd.Int(objCommand, "idUsuarioEdicion", listaDato.IdUsuarioEdicion);

            ExecuteNonQuery(objCommand);
        }
        /// <summary>
        /// Descripción: Registra información de una nueva Opcion de Dato.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="opcionDato"></param>
        public void InsertOpcionDato(OpcionDato opcionDato)
        {
            var objCommand = GetSqlCommand("pNLI_OpcionDato");

            InputParameterAdd.Int(objCommand, "idOpcionDato", opcionDato.IdOpcionDato);
            InputParameterAdd.Int(objCommand, "idListaDato", opcionDato.IdListaDato);
            InputParameterAdd.Varchar(objCommand, "valor", opcionDato.Valor);
            InputParameterAdd.Int(objCommand, "orden", opcionDato.Orden);
            InputParameterAdd.Int(objCommand, "idUsuarioRegistro", opcionDato.IdUsuarioRegistro);

            ExecuteNonQuery(objCommand);
        }
        /// <summary>
        /// Descripción: Actualiza información de una Opcion de Dato.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="opcionDato"></param>
        public void UpdateOpcionDato(OpcionDato opcionDato)
        {
            var objCommand = GetSqlCommand("pNLU_OpcionDato");

            InputParameterAdd.Int(objCommand, "idOpcionDato", opcionDato.IdOpcionDato);
            InputParameterAdd.Varchar(objCommand, "valor", opcionDato.Valor);
            InputParameterAdd.Int(objCommand, "orden", opcionDato.Orden);
            InputParameterAdd.Int(objCommand, "estado", opcionDato.Estado);
            InputParameterAdd.Int(objCommand, "idUsuarioEdicion", opcionDato.IdUsuarioEdicion);

            ExecuteNonQuery(objCommand);
        }
        /// <summary>
        /// Descripción: Obtiene informacion de las opciones de datos filtrado por el id.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public OpcionDato GetOpcionDatoById(int id)
        {
            var objCommand = GetSqlCommand("pNLS_OpcionDatoById");

            InputParameterAdd.Int(objCommand, "idOpcionDato", id);

            return ListaDatoConvertTo.Opcion(Execute(objCommand));
        }
    }
}