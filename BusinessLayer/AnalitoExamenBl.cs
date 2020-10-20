using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BusinessLayer.Interfaces;
using DataLayer;
using Model;

namespace BusinessLayer
{
    public class AnalitoExamenBl : IAnalitoExamenBl
    {
        public void AgregarAnalitosPorExamen(Examen examen, string[] analitos, int idUsuario)
        {
            if (analitos == null || !analitos.Any()) return;

            var analitosByExamen = GetAnalitoByExamenId(examen.idExamen);

            analitos = analitos.Where(x => analitosByExamen.All(y => y.IdAnalito.ToString() != x)).ToArray();

            var listAnalitos = analitos.Select(idAnalito => GetAnalitoExamen(examen.idExamen, idAnalito, idUsuario)).ToList();

            InsertAnalitoByExamen(listAnalitos);
        }
        /// <summary>
        /// Descripción: Registrar información  de un analito y el examen.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="analito"></param>
        private static void InsertAnalitoByExamen(IEnumerable<ExamenAnalito> analitos)
        {
            using (var analitoExamenDal = new AnalitoExamenDal())
            {
                analitoExamenDal.BeginTransaction(IsolationLevel.ReadCommitted);

                try
                {
                    foreach (var item in analitos)
                    {
                        analitoExamenDal.InsertAnalitoByExamen(item);
                    }

                    analitoExamenDal.Commit();
                }
                catch (Exception)
                {
                    analitoExamenDal.Rollback();
                }
            }
        }

        private static ExamenAnalito GetAnalitoExamen(Guid idExamen, string idAnalito, int idUsuario)
        {
            return new ExamenAnalito
            {
                IdExamen = idExamen,
                IdAnalito = new Guid(idAnalito),
                IdUsuarioRegistro = idUsuario
            };
        }
        /// <summary>
        /// Descripción: Obtener informacion de un analito por el Id.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idExamen"></param>
        /// <returns></returns>
        public List<Analito> GetAnalitoByExamenId(Guid idExamen)
        {
            using (var analitoDal = new AnalitoExamenDal())
            {
                return analitoDal.GetAnalitoByExamenId(idExamen);
            }
        }
        /// <summary>
        /// Descripción: Actualiza información  de un analito y el examen.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="examenAnalito"></param>
        public void UpdateAnalitoByExamen(ExamenAnalito examenAnalito)
        {
            using (var analitoExamenDal = new AnalitoExamenDal())
            {
                analitoExamenDal.UpdateAnalitoByExamen(examenAnalito);
            }
        }
    }
}