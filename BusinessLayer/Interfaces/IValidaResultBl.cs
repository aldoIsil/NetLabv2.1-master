using Model;
using System.Collections.Generic;
using System;


namespace BusinessLayer.Interfaces
{
    public interface IValidaResultBl
    {
        List<ValidaResult> GetValidaciones(int idUsuarioIngreso, int fechaSolicitud, string codigoOrden, DateTime fechaDesde, DateTime fechaHasta, string nroOficio, string nroDocumento, int idLaboratorio,int estadoResultado, int idEnfermedad, Guid idExamen);

    }
}
