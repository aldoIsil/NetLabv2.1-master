using System;
using System.Data;
using Model;

namespace DataLayer.Area.Pacientes
{
    public class PacienteDal : IPacienteDal
    {
        /// <summary>
        /// Descripción: Registra informacion del paciente.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <param name="paciente"></param>
        public void InsertPaciente(Paciente paciente)
        {
            using (var netlabDal = new NetlabDal())
            {
                netlabDal.BeginTransaction(IsolationLevel.ReadCommitted);
                try
                {
                    netlabDal.InsertPaciente(paciente);
//                    netlabDal.InsertPacienteDatoComplementario(paciente.datoComplementario);

                    netlabDal.Commit();
                }
                catch (Exception)
                {
                    netlabDal.Rollback();
                }
            }
        }
        /// <summary>
        /// Descripción: Actualiza informacion del paciente.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <param name="paciente"></param>
        public void UpdatePaciente(Paciente paciente)
        {
            using (var netlabDal = new NetlabDal())
            {
                netlabDal.BeginTransaction(IsolationLevel.ReadCommitted);
                try
                {
                    netlabDal.UpdatePaciente(paciente);
                    netlabDal.Commit();
                }
                catch (Exception)
                {
                    netlabDal.Rollback();
                }
            }
        }
        /// <summary>
        /// Descripción: Obtiene información del paciente filtrado por el nro de documento.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <param name="nroDocumento"></param>
        /// <returns></returns>
        public Paciente GetPacienteByDocumento(string nroDocumento)
        {
            using (var netlabDal = new NetlabDal())
            {
                return netlabDal.GetPacienteByDocumento(nroDocumento);
            }
        }
    }
}
