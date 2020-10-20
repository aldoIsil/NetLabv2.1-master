using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Serializable]
    public class MicroRedMant:General
    {
        public string iddisa  { get; set; }

        public string idred { get; set; }

        public string idmicrored { get; set; }


        public string nombredisa { get; set; }

        public string nombrered { get; set; }

        public string nombremicrored { get; set; }

        public int estado { get; set; }


    }
}
