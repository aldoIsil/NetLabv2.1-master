using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Model.ViewData;

namespace Model
{
    [Serializable]
    public class Menu:General
    {
       	public int idMenu {get;set; } //] [int] NOT NULL,
	    public string nombre {get;set; } //] [varchar](300) NULL,
	    public string descripcion {get;set; } //] [varchar](1000) NULL,
	    public string URL {get;set; } //] [varchar](2000) NULL,
        public string icon { get; set; } //] [varchar](50) NULL,
        public int idMenuPadre {get;set; } //] [int] NULL,
	    public int orden {get;set; } //] [int] NULL,

        public List<Menu> hijos { get; set; } //] [List<Menu>] NULL,
    }
}
