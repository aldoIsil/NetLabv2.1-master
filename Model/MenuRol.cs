using System;

namespace Model
{
    [Serializable]
    public class MenuRol:General
    {
        public int idMenu {get;set;}//] [int] NOT NULL,
        public int idRol { get; set; }//] [int] NOT NULL,
    }
}
