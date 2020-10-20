using BusinessLayer;
using DataLayer;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class UsuarioEnfermedadExamenBl
    {
        public List<Examen> GetExamenByUsuarioId(int idUsuario)
        {
            using (var UsuarioExamenDal = new UsuarioExamenDal())
            {
                return UsuarioExamenDal.GetExamenByUsuarioId(idUsuario);
            }
        }
        private static ExamenUsuario GetUsuarioExamen(int idUsuarioConf, string examen, int idUsuario)
        {
            return new ExamenUsuario
            {
                idUsuarioEnfermedadExamen = idUsuarioConf,
                IdExamen = new Guid(examen),
                IdUsuarioRegistro = idUsuario
            };
        }
        public void InsertExamenByUsuario(Model.Usuario usuario, string[] examenes, int idUsuario, int TipoUsuarioExamen, int idEnfermedad)
        {
            if (idEnfermedad > 0)
            {
                using (var examenDal = new UsuarioExamenDal())
                {
                    try
                    {
                        examenDal.InsertExamenByUsuarioEnfermedad(usuario.idUsuario, idUsuario, TipoUsuarioExamen, idEnfermedad);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            else
            {
                if (examenes == null || !examenes.Any()) return;

                var examenesByUsuario = GetExamenByUsuarioId(usuario.idUsuario);

                //if (examenesByUsuario != null)
                //{
                //    examenes = examenes.Where(x => examenesByUsuario.All(y => !string.Equals(y.idExamen.ToString(), x, StringComparison.CurrentCultureIgnoreCase))).ToArray();
                //}
                var ListExamenes = examenes.Select(examen => GetUsuarioExamen(usuario.idUsuario, examen, idUsuario)).ToList();

                InsertExamenesByUsuario(ListExamenes, TipoUsuarioExamen);
            }
            
        }
        public void InsertExamenesByUsuario(IEnumerable<ExamenUsuario> examenes, int TipoUsuarioExamen)
        {
            using (var examenDal = new UsuarioExamenDal())
            {
                examenDal.BeginTransaction(IsolationLevel.ReadCommitted);

                try
                {

                    foreach (var item in examenes)
                    {
                        examenDal.InsertExamenByUsuario(item, TipoUsuarioExamen);
                    }
                    examenDal.Commit();
                }
                catch (Exception ex)
                {
                    examenDal.Rollback();
                }
            }
        }
        public void UpdateExamenByUsuario(ExamenUsuario examenUsuario)
        {
            using (var examenDal = new UsuarioExamenDal())
            {
                try
                {
                    examenDal.UpdateExamenByUsuario(examenUsuario);
                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}
