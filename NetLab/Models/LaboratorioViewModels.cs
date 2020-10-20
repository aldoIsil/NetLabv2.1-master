using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using Model;

namespace NetLab.Models
{
    [Serializable()]
    public class LaboratorioViewModels
    {
        public Laboratorio Laboratorio { get; set; }

        public byte[] InsertarLogoRegional { get; set; }

        public byte[] InsertarLogo { get; set; }

        public byte[] InsertarSello { get; set; }
    }
}