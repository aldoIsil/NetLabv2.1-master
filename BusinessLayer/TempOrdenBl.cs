using DataLayer;
using System;
using Model;
using System.Linq;
using Model.Temp;

namespace BusinessLayer
{
    public class TempOrdenBl
    {
        public Guid InsertTempOrden(TempModel.TempOrden oTempOrden)
        {
            using (var TempordenDal = new TempOrdenDal())
            {
                return TempordenDal.InsertTempOrden(oTempOrden);
            }
        }
        public void UpdateTempOrden(TempModel.TempOrden oTempOrden)
        {
            using (var TempordenDal = new TempOrdenDal())
            {
                TempordenDal.UpdateTempOrden(oTempOrden);
            }
        }
        public void InsertTempOrdenMuestra(TempModel.TempOrdenMuestra oTempOrdenMuestra, Guid idPaciente, Guid idExamen, int idtipoMuestra)
        {
            using (var TempordenDal = new TempOrdenDal())
            {
                TempordenDal.InsertTempOrdenMuestra(oTempOrdenMuestra, idPaciente, idExamen, idtipoMuestra);
            }
        }
        public void DeleTempOrden(Guid idPaciente, int idUsuario)
        {
            using (var TempordenDal = new TempOrdenDal())
            {
                TempordenDal.DeleteTempOrden(idPaciente, idUsuario);
            }
            
        }
        public void DeleteOrden(Guid idOrden, int idUsuario)
        {
            using (var TempordenDal = new TempOrdenDal())
            {
                TempordenDal.DeleteTempOrdenbyNroDocumento(idOrden, idUsuario);
            }

        }
        public void DeleTempOrden(int idUsuario)
        {
            using (var TempordenDal = new TempOrdenDal())
            {
                TempordenDal.DeleteTempOrden(idUsuario);
            }

        }
        public void InsertTempOrdenDatoClinico(TempModel.TempOrden oTempOrden)
        {
            using (var TempordenDal = new TempOrdenDal())
            {
                //return ordenDal.InsertTempOrden(oTempOrden);
                if (oTempOrden.enfermedadList != null) //orden.enfermedadList puede venir null? en que casos?
                {
                    foreach (var enfermedad in oTempOrden.enfermedadList)
                    {
                        foreach (var ordenDatoClinico in enfermedad.categoriaDatoList.Where(
                            categoriaDato => categoriaDato.OrdenDatoClinicoList != null &&
                            categoriaDato.OrdenDatoClinicoList.Count != 0).SelectMany(
                            categoriaDato => categoriaDato.OrdenDatoClinicoList))
                        {
                            ordenDatoClinico.idOrden = oTempOrden.IdOrden;
                            ordenDatoClinico.IdUsuarioEdicion = oTempOrden.idUsuario;
                            ordenDatoClinico.estatus = 1;
                            ordenDatoClinico.Enfermedad = new Enfermedad { idEnfermedad = enfermedad.idEnfermedad };
                            TempordenDal.InsertTempOrdenDatoClinico(ordenDatoClinico, oTempOrden.IdPaciente);
                        }

                    }
                }
            }
        }
        public void InsertPreRecepcion (string tipoValidacion, string Nombres, string apePaterno, string apeMaterno, string nroDocumento, string tipoDocumento, int idusuario)
        {
            using (var TempordenDal = new TempOrdenDal())
            {
                TempordenDal.InsertPreRecepcion(tipoValidacion, Nombres, apePaterno, apeMaterno, nroDocumento, tipoDocumento,idusuario);
            }

        }
    }
}
