using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Serializable]
    public class RedMant:General
    {

        public string idDisa { get; set; }

        public string idred { get; set; }
        

        public string nombredisa { get; set; }
        public string nombrered { get; set; }

        public int Estado { get; set; }


        public List<DisaMant> ListaDisa { get; set; }



    }
}
