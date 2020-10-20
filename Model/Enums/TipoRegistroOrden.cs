using System;

namespace Enums
{
    /// <summary>
    /// Descripción: Enum aloja el tipo de ejecucion de la pantalla de registro de una orden.
    /// permitiendo seleccionar una de las plantillas creadas para una enfermedad
    /// Author: Terceros.
    /// Fecha Creacion: 01/01/2017
    /// Fecha Modificación: 02/02/2017.
    /// Modificación: Se agregaron comentarios.
    /// </summary>
    [Serializable]
    public enum TipoRegistroOrden
    {
        SOLO_ORDEN,
        ORDEN_RECEPCION,
        SOLO_ORDEN_MUESTRA,//Juan Muga
        EDITAR_ORDEN_DATOCLINICO,
        VACIO = -1
    }
}