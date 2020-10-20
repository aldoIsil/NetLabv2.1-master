using Renaes.Entities;
using Renaes.Interfaces;
using System.Data;

namespace Renaes
{
    public class RenaesConverter : IRenaesConverter
    {
        public Establecimiento Convert(DataSet dsEstablecimiento)
        {
            var rows = dsEstablecimiento.Tables[0].Rows;
            
            if (rows.Count == 0) return null;

            return new Establecimiento
            {
                Codigo = (int)rows[0]["EST_CODUNICO"],
                Nombre = rows[0]["EST_NOMBRE"].ToString()
            };
        }
    }
}
