using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IMicroRedBI
    {
        List<MicroRedMant> GetMicroRedes(string nombre = null);

        void InsertMicroRedes(MicroRedMant microred);

        void UpdateMicroRedes(MicroRedMant microred);


        MicroRedMant GetMicroRedByID(string idDisa, string idRed,string idmicrored);
    }
}
