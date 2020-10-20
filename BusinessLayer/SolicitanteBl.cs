using BusinessLayer.Interfaces;
using DataLayer;
using System.Collections.Generic;
using System;
using Model;

namespace BusinessLayer
{
    public class SolicitanteBl
    {
        /// <summary>
        /// Descripción: Registra nuevo solicitante de una orden
        /// Author: Juan Muga.
        /// Fecha Creacion: 05/06/2017
        /// Fecha Modificación: 
        /// Modificación:  
        /// </summary>
        /// <param name="oSolicitante"></param>
        public int InsertSolicitante(OrdenSolicitante oSolicitante)
        {
            int valor = 0;
            using (var SolicitanteDal = new SolicitanteDal())
            {
                valor = SolicitanteDal.InsertSolicitante(oSolicitante);
            }
            return valor;
        }
        public bool GetCodigoSolicitante(string idSolicitante)
        {
            using (var SolicitanteDal = new SolicitanteDal())
            {
               return SolicitanteDal.GetCodigoSolicitante(idSolicitante);
            }
        }
        public bool GetCodigoSolicitantePorCodigo(string codigoColegio)
        {
            using (var SolicitanteDal = new SolicitanteDal())
            {
                return SolicitanteDal.GetCodigoSolicitantePorCodigo(codigoColegio);
            }
        }
        public List<OrdenSolicitante> GetListaSolicitante(string textoBusqueda)
        {
            using (var solicitanteDAL = new SolicitanteDal())
            {
                return solicitanteDAL.GetListaSOlicitante(textoBusqueda);
            }
        }

        public string GetSolicitanteById(string idSolicitante)
        {
            int id = 0;
            int.TryParse(idSolicitante, out id);
            using (var SolicitanteDal = new SolicitanteDal())
            {
                return SolicitanteDal.GetSolicitanteById(id);
            }
        }

        public List<OrdenSolicitante> BuscarSolicitante(string cadena)
        {
            using (var solicitante = new SolicitanteDal())
            {
                return solicitante.BuscarSolicitante(cadena);
            }
        }

        public OrdenSolicitante ListarSolicitante(int idSolicitante)
        {
            using (var solicitante = new SolicitanteDal())
            {
                return solicitante.ListarSolicitante(idSolicitante);
            }
        }

        public void UpdateSolicitante(int id, string codigoColegio, string Nombre, string ApePat, string ApeMat, 
            string Correo, string dni, string telefono, int profesion,int idEstablecimiento, int idUsuario)
        {
            using (var _solicitante = new SolicitanteDal())
            {
                _solicitante.UpdateSolicitante(id, codigoColegio, Nombre, ApePat, ApeMat, Correo, dni, telefono,profesion,idEstablecimiento,idUsuario);
            }
        }
    }
}
