using System;
using System.Collections.Generic;
using Model;


namespace BusinessLayer.Interfaces
{
    public interface ITipoMuestraPresentacionBl
    {
        void AgregarTipoMuestrasPorPresentacion(Examen examen, int[] enfermedades, int idUsuario);

        TipoMuestraPresentacion GetTipoMuestraPresentacion(Examen examen, int idEnfermedad, int idUsuario);




    }
}
