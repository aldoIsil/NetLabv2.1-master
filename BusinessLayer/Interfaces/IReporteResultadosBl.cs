using System;
using System.Collections.Generic;
using Model;

namespace BusinessLayer.Interfaces
{
    public interface IReporteResultadosBl
    {
        OrdenResultado GetOrdenResultado(Guid idOrden);
        OrdenResultado GetOrdenResultadoWS(Guid idOrdenExamen);
        OrdenResultado GetOrdenResultado(Guid idOrden, int idLaboratorioDestino, string[] idOrdenExamen, int idUsuario);
        List<MuestraResultado> GetMuestras(Guid idOrden, Guid idExamen, int idLaboratorio);
        List<MuestraResultado> GetMuestrasbyIdOrden(Guid idOrden, int idLaboratorioDestino, string[] idOrdenExamen);
        List<ExamenResultado> GetExamenes(Guid idOrden);
        List<ExamenResultadoDetalle> GetDetalleExamenes(Guid idOrden, Guid idExamen);
        List<ExamenResultadoDetalle> GetDetalleExamenes(Guid idOrden, int idLaboratorioDestino, string[] idOrdenExamen);
        //List<ExamenResultadoInterpretacion> GetInterpretacionExamen(Guid[] idOrdenExamen 
        CargoUsuarioEstablecimiento CargoUsuarioEstablecimiento(int idEstablecimiento, string fechaVerificacion);
    }
}