using Framework.DAL;
using Framework.DAL.Settings.Implementations;
using System.Collections.Generic;
using DataLayer.DalConverter;
using System;
using Model;
using System.Globalization;

namespace DataLayer
{
    public class ExamenDal : DaoBase
    {
        public ExamenDal(IDalSettings settings) : base(settings)
        {
        }

        public ExamenDal() : this(new NetlabSettings())
        {
        }
        /// <summary>
        /// Descripción: Registro de examenes
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="examen"></param>
        public void InsertExamen(Examen examen)
        {
            var objCommand = GetSqlCommand("pNLI_Examen");

            InputParameterAdd.Varchar(objCommand, "nombre", examen.nombre);
            InputParameterAdd.Varchar(objCommand, "descripcion", examen.descripcion);
            InputParameterAdd.Int(objCommand, "cpt", examen.Cpt);
            InputParameterAdd.Varchar(objCommand, "loinc", examen.Loinc);
            InputParameterAdd.Int(objCommand, "idGenero", examen.IdGenero);
            InputParameterAdd.Int(objCommand, "idUsuarioRegistro", examen.IdUsuarioRegistro);

            ExecuteNonQuery(objCommand);
        }
        /// <summary>
        /// Descripción: Actualziacion de examenes
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="examen"></param>
        public void UpdateExamen(Examen examen)
        {
            var objCommand = GetSqlCommand("pNLU_Examen");

            InputParameterAdd.Guid(objCommand, "idExamen", examen.idExamen);
            InputParameterAdd.Varchar(objCommand, "nombre", examen.nombre);
            InputParameterAdd.Varchar(objCommand, "descripcion", examen.descripcion);
            InputParameterAdd.Int(objCommand, "cpt", examen.Cpt);
            InputParameterAdd.Varchar(objCommand, "loinc", examen.Loinc);
            InputParameterAdd.Int(objCommand, "idGenero", examen.IdGenero);
            InputParameterAdd.Int(objCommand, "idUsuarioEdicion", examen.IdUsuarioEdicion);
            InputParameterAdd.Int(objCommand, "estado", examen.Estado);

            ExecuteNonQuery(objCommand);
        }
        /// <summary>
        /// Descripción: Obtiene informacion de los examenes filtrados por Nombre
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        public List<Examen> GetExamenesByNombre(string nombre)
        {
            var objCommand = GetSqlCommand("pNLS_ExamenByNombre");
            InputParameterAdd.Varchar(objCommand, "nombre", nombre);

            return ExamenConvertTo.Examenes(Execute(objCommand));
        }
        /// <summary>
        /// Descripción: Obtiene informacion de los examenes filtrados por idExamen
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idExamen"></param>
        /// <returns></returns>
        public Examen GetExamenById(Guid idExamen)
        {
            var objCommand = GetSqlCommand("pNLS_ExamenById");
            InputParameterAdd.Guid(objCommand, "idExamen", idExamen);

            return ExamenConvertTo.Examen(Execute(objCommand));
        }

        /// <summary>
        /// Descripción: Obtiene informacion de los examenes filtrados por Laboratorio, Nombre y genero.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idLaboratorio"></param>
        /// <param name="genero"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<Examen> GetExamenesByIdLaboratorio(int idEnfermedad, int genero, String data)
        {
            var objCommand = GetSqlCommand("pNLS_ExamenByNombreAndLaboratorio");
            InputParameterAdd.Varchar(objCommand, "nombre", data);
            InputParameterAdd.Int(objCommand, "idEnfermedad", idEnfermedad);
            InputParameterAdd.Int(objCommand, "genero", genero);
            //return new List<Examen>();
            return ExamenConvertTo.Examenes(Execute(objCommand));
        }
        /// <summary>
        /// Descripción: Obtiene informacion de los examenes filtrados por Laboratorio, Nombre y genero.
        /// Author: SOTERO BUSTAMANTE.
        /// Fecha Creacion: 19/11/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idLaboratorio"></param>
        /// <param name="genero"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<Examen> GetExamenesByTipoMuestra(string idOrden, string data, int idEnfermedad)
        {
            var objCommand = GetSqlCommand("pNLS_ExamenByNombreAndLaboratorioAndTmuestra");
            InputParameterAdd.Varchar(objCommand, "nombre", data);
            InputParameterAdd.Guid(objCommand, "idOrden", Guid.Parse(idOrden));
            InputParameterAdd.Int(objCommand, "idEnfermedad", idEnfermedad);
            //return new List<Examen>();
            return ExamenConvertTo.Examenes(Execute(objCommand));
        }
        /// <summary>
        /// Descripción: Obtiene información de los examenes filtrados por Nombre y genero
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="genero"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<Examen> GetExamenesByGenero(int genero, String data)
        {
            var objCommand = GetSqlCommand("pNLS_ExamenByNombreAndGenero");
            InputParameterAdd.Varchar(objCommand, "nombre", data);
            InputParameterAdd.Int(objCommand, "genero", genero);
            //return new List<Examen>();
            return ExamenConvertTo.Examenes(Execute(objCommand));
        }

        /// <summary>
        /// Descripción: Obtiene informacion de los examenes filtrados por idExamen y genero
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idEnfermedad"></param>
        /// <param name="genero"></param>
        /// <returns></returns>
        public List<Examen> GetExamenesByIdEnfermedad(int idEnfermedad, int genero, String data)
        {
            var objCommand = GetSqlCommand("pNLS_ExamenByIdEnfermedad");
            InputParameterAdd.Int(objCommand, "idEnfermedad", idEnfermedad);
            InputParameterAdd.Int(objCommand, "genero", genero);
            InputParameterAdd.Varchar(objCommand, "nombre", data);

            return ExamenConvertTo.Examenes(Execute(objCommand));
        }
        public List<Examen> GetExamenesByIdEnfermedadOrden(int idEnfermedad,String data)
        {
            var objCommand = GetSqlCommand("pNLS_ExamenByIdEnfermedadOrden");
            InputParameterAdd.Int(objCommand, "idEnfermedad", idEnfermedad);
            InputParameterAdd.Varchar(objCommand, "nombre", data);

            return ExamenConvertTo.Examenes(Execute(objCommand));
        }
        /// <summary>
        /// Descripción: Obtiene informacion de los examenes filtrados por idExamen
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idExamen"></param>
        /// <returns></returns>
        public List<ExamenMetodo> GetMetodoByExamen(Guid idExamen)
        {
            var objCommand = GetSqlCommand("pNLS_MetodosByExamen");
            InputParameterAdd.Guid(objCommand, "idExamen", idExamen);

            return ExamenConvertTo.ExamenMetodo(Execute(objCommand));
        }
        /// <summary>
        /// Descripción: Registra un examen nuevo
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="examenMetodo"></param>
        public void InsertMetodoByExamen(ExamenMetodo examenMetodo)
        {
            var objCommand = GetSqlCommand("pNLI_ExamenMetodo");

            InputParameterAdd.Guid(objCommand, "idExamen", examenMetodo.IdExamen);
            InputParameterAdd.Varchar(objCommand, "glosa", examenMetodo.Glosa);
            InputParameterAdd.Int(objCommand, "orden", examenMetodo.Orden);
            InputParameterAdd.Int(objCommand, "idUsuarioRegistro", examenMetodo.IdUsuarioRegistro);

            ExecuteNonQuery(objCommand);
        }
        /// <summary>
        /// Descripción: Actualiza un examen.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="examenMetodo"></param>
        public void UpdateMetodoByExamen(ExamenMetodo examenMetodo)
        {
            var objCommand = GetSqlCommand("pNLU_ExamenMetodo");

            InputParameterAdd.Int(objCommand, "idExamenMetodo", examenMetodo.IdExamenMetodo);
            InputParameterAdd.Guid(objCommand, "idExamen", examenMetodo.IdExamen);
            InputParameterAdd.Varchar(objCommand, "glosa", examenMetodo.Glosa);
            InputParameterAdd.Int(objCommand, "ordenMetodo", examenMetodo.Orden);
            InputParameterAdd.Int(objCommand, "idUsuarioEdicion", examenMetodo.IdUsuarioEdicion);
            InputParameterAdd.Int(objCommand, "estado", examenMetodo.Estado);

            ExecuteNonQuery(objCommand);
        }
        public List<Examen> GetExamenesByIdLaboratorioRecepcion(int idLaboratorio, int genero, String data)
        {
            var objCommand = GetSqlCommand("pNLS_ExamenByNombreAndLaboratorioRecepcion");
            InputParameterAdd.Varchar(objCommand, "nombre", data);
            InputParameterAdd.Int(objCommand, "idLaboratorio", idLaboratorio);
            InputParameterAdd.Int(objCommand, "genero", genero);
            //return new List<Examen>();
            return ExamenConvertTo.Examenes(Execute(objCommand));
        }
        public List<Examen> GetExamenUsuario(string data, int idUsuario)
        {
            var objCommand = GetSqlCommand("pNLS_ExamenbyNombreandUsuario");
            InputParameterAdd.Varchar(objCommand, "nombre", data);
            InputParameterAdd.Int(objCommand, "idUsuario", idUsuario);
            //return new List<Examen>();
            return ExamenConvertTo.Examenes(Execute(objCommand));
        }
        public List<Examen> GetExamenesByIdEnfermedadAgrupado(int idEnfermedad)
        {
            var objCommand = GetSqlCommand("pNLS_GetExamenSolicitudUsuario");
            InputParameterAdd.Int(objCommand, "idEnfermedad", idEnfermedad);
            return ExamenConvertTo.ExamenAgrupado(Execute(objCommand));
        }
        public List<TipoMuestra> GetExamenAgrupado(int idExamenAgrupado)
        {
            var objCommand = GetSqlCommand("pNLS_GetExamenAgrupado");
            InputParameterAdd.Int(objCommand, "idExamenAgrupado", idExamenAgrupado);
            return TipoMuestraConvertTo.TipoMuestras(Execute(objCommand));
        }

        public List<Examen> GetExamenesPorEnfermedad(int idEnfermedad)
        {
            var objCommand = GetSqlCommand("pNLS_ExamenesPorEnfermedad");
            InputParameterAdd.Int(objCommand, "idEnfermedad", idEnfermedad);
            //return new List<Examen>();
            return ExamenConvertTo.Examenes(Execute(objCommand));
        }

        public string VerListaPlataformaExamen(Guid idExamen)
        {
            var objCommand = GetSqlCommand("pNLS_ListaPlataformaExamen");
            InputParameterAdd.Guid(objCommand, "idExamen", idExamen);
            string plataforma = objCommand.ExecuteScalar().ToString();
            return plataforma;
        }
        public string VerListaPlataforma()
        {
            var objCommand = GetSqlCommand("pNLS_ConsultarPlataforma");
            string plataforma = objCommand.ExecuteScalar().ToString();
            return plataforma;
        }
        public string RegistrarExamenPlataforma(string plataforma, Guid idExamen, int idUsuario) 
        {
            var objCommand = GetSqlCommand("pNLI_RegistrarExamenPlataforma");
            InputParameterAdd.Varchar(objCommand, "idPlataforma", plataforma);
            InputParameterAdd.Guid(objCommand, "idExamen", idExamen);
            InputParameterAdd.Int(objCommand, "idUsuario", idUsuario);
            string estado = objCommand.ExecuteScalar().ToString();
            return estado;
        }

        public string EliminarExamenPlataforma(int plataforma, Guid idExamen, int idUsuario)
        {
            var objCommand = GetSqlCommand("pNLU_EliminarExamenPlataforma");
            InputParameterAdd.Int(objCommand, "idPlataforma", plataforma);
            InputParameterAdd.Guid(objCommand, "idExamen", idExamen);
            InputParameterAdd.Int(objCommand, "idUsuario", idUsuario);
            string estado = objCommand.ExecuteScalar().ToString();
            return estado;
        }
    }
}
