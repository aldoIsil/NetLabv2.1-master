﻿using System;

namespace Model
{
    [Serializable]
    public class Proyecto : General
    {
        public int IdProyecto { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
    }
}
