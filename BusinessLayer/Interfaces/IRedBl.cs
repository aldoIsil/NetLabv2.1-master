using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IRedBl
    {
        List<RedMant> GetRedes(string nombre = null);

        List<RedMant> GetRedByIDDisa(string idDisa);


        void InsertRedes(RedMant red);

        RedMant GetRedByID(string idDisa,string idRed);

     
        void UpdateRedes(RedMant red);
    }
}
