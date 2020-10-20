using DataLayer;
using Model;

namespace BusinessLayer
{
    public class MuestraBl
    {
        /// <summary>
        /// Descripción: Registra y obtiene los nuevos codigos de muestra.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="cantidad"></param>
        /// <param name="idEstablecimiento"></param>
        /// <param name="idUsuario"></param>
        public void GeneraCodigosMuestra(int cantidad, int idEstablecimiento, int idUsuario, int icodigoLineal)
        {
            var dal = new MuestraDal();

            dal.GenerarCodigos(cantidad, idEstablecimiento, idUsuario, icodigoLineal);
        }

        public string GeneraCodigosMuestra(int idEstablecimiento, int idUsuario, int icodigoLineal)
        {
            var dal = new MuestraDal();

            return dal.GenerarCodigos(idEstablecimiento, idUsuario, icodigoLineal);
        }
        public MuestraCodificacion GeneraCodigosMuestraKobo(int idEstablecimiento, int idUsuario, int icodigoLineal)
        {
            var dal = new MuestraDal();

            return dal.GenerarCodigosKobo(idEstablecimiento, idUsuario, icodigoLineal);
        }
        public System.Collections.Generic.List<Model.MuestraCodificacion> ConsultaCodigosMuestra(int cantidad, int idEstablecimiento, int idUsuario)
        {
            var dal = new MuestraDal();

            return dal.ConsultaCodigosMuestra(cantidad, idEstablecimiento, idUsuario);
        }
        public System.Collections.Generic.List<Model.MuestraCodificacion> ConsultaCodigosGenerados(string fechaDesde, string fechaHasta, string idEstablecimiento, string codigo, string estado, int laboratorio)
        {
            var dal = new MuestraDal();

            return dal.ConsultaCodigosGenerados(fechaDesde, fechaHasta, idEstablecimiento, codigo, estado,laboratorio);
        }
        public System.Collections.Generic.List<Model.MuestraCodificacion> ConsultaCodigosMuestraEstado(string inicio, string fin, int idEstablecimiento)
        {
            var dal = new MuestraDal();

            return dal.ConsultaCodigosMuestraEstado(inicio, fin, idEstablecimiento);
        }
    }
}
