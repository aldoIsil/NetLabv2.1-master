using System;

namespace Model
{
    [Serializable]
    public class OrdenExamenOrdenMuestra:General
    {
        public Guid idOrdenExamen {get;set;}//] [uniqueidentifier] NOT NULL,
        public Guid idOrdenMuestra { get; set; }//] [uniqueidentifier] NOT NULL,

        public int estatus { get; set; }
    }
}
