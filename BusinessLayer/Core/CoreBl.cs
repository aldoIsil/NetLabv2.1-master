using DataLayer;
using Enums;
using Model.Entidades;
using Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public interface ICoreBl : IDisposable //Hay que ver bien esto como funciona
    {
        Guid CrearOrdenInicial(Guid pacienteId, int idUsuario, TipoRegistroOrden tipoRegistro);
        void CrearOrdenExamenCore(CrearOrdenExamenVM model);
        Paciente ObtenerPacientePorId(Guid idpaciente);
        void CrearOrdenDatosClinicos(List<OrdenDatoClinicoCore> datosclinicos, List<CrearOrdenExamenTabla> ordenexamenes);
    }

    public class CoreBl : ICoreBl
    {
        private static CoreBl instancia;

        public void Dispose()
        {
            instancia = null;
        }

        public Guid CrearOrdenInicial(Guid pacienteId, int idUsuario, TipoRegistroOrden tipoRegistro)
        {
            var ordenId = Guid.NewGuid();
            using (var dal = new CoreDal())
            {
                dal.CrearOrdenInicial(ordenId, pacienteId, idUsuario, tipoRegistro);
            }

            return ordenId;
        }

        public void CrearOrdenExamenCore(CrearOrdenExamenVM model)
        {
            using (var dal = new CoreDal())
            {
                dal.CrearOrdenExamenCore(model);
            }
        }

        public Orden ObtenerOrdenPorId(Guid idorden)
        {
            using (var d = new CoreDal())
            {
                return d.ObtenerOrdenPorId(idorden);
            }
        }

        public Paciente ObtenerPacientePorId(Guid idpaciente)
        {
            using (var d = new CoreDal())
            {
                return d.ObtenerPacientePorId(idpaciente);
            }
        }

        public List<Examen> GetExamenesPorEnfermedad(int idEnfermedad)
        {
            using (var dal = new CoreDal())
            {
                return dal.GetExamenesPorEnfermedad(idEnfermedad);
            }
        }

        public List<TipoMuestra> ObtenerTipoMuestraPorExamen(Guid idExamen)
        {
            using (var dal = new CoreDal())
            {
                return dal.ObtenerTipoMuestraPorExamen(idExamen);
            }
        }

        public List<CrearOrdenExamenTabla> ObtenerOrdenExamenesActivos(Guid idOrden)
        {
            using (var dal = new CoreDal())
            {
                return dal.ObtenerOrdenExamenesActivos(idOrden);
            }
        }

        public List<EnfermedadDatos> ObtenerDatosClinicosPorEnfermedad(Guid idOrden)
        {
            using (var dal = new CoreDal())
            {
                return dal.ObtenerDatosClinicosPorEnfermedad(idOrden);
            }
        }

        public void EliminarOrdenExamen(Guid ordenExamenId)
        {
            using (var dal = new CoreDal())
            {
                dal.EliminarOrdenExamen(ordenExamenId);
            }
        }

        public void CrearOrdenDatosClinicos(List<OrdenDatoClinicoCore> datosclinicos, List<CrearOrdenExamenTabla> ordenexamenes)
        {
            using (var dal = new CoreDal())
            {
                dal.CrearOrdenDatosClinicos(datosclinicos, ordenexamenes);
            }
        }

        public string FinalizarCreacionOrden(CrearOrdenVM orden)
        {
            using (var dal = new CoreDal())
            {
                return dal.FinalizarCreacionOrden(orden);
            }
        }

        public void DesactivarReniec()
        {
            using (var dal = new CoreDal())
            {
                dal.DesactivarReniec();
            }
        }

        public void ActivarReniec()
        {
            using (var dal = new CoreDal())
            {
                dal.ActivarReniec();
            }
        }

        public void CrearOrdenDatosClinicosTest(int i)
        {
            using (var dal = new CoreDal())
            {
                dal.CrearOrdenDatosClinicosTest(i);
            }
        }
    }
}
