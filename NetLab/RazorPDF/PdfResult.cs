using System;
using System.Collections.Generic;
using System.Web.Mvc;
using NetLab.Models;

namespace RazorPDF
{
    internal class PdfResult : ActionResult
    {
        private List<PDFCodigoMuestraModel> lista;
        private string v;

        public PdfResult(List<PDFCodigoMuestraModel> lista, string v)
        {
            this.lista = lista;
            this.v = v;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            throw new NotImplementedException();
        }
    }
}