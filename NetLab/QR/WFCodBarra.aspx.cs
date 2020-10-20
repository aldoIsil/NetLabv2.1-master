using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NetLab.QR
{
    public partial class WFCodBarra : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var strCodigoBarra ="";
            int intMilliseconds = 1000;

            if (Request.Params.Get("Barcode") != "" && Request.Params.Get("Barcode") != null)
            {
                strCodigoBarra = Request.Params.Get("Barcode");
                Process myProcess = new Process();
                try
                {
                    myProcess.StartInfo.UseShellExecute = false;
                    myProcess.StartInfo.FileName = Server.MapPath("BarCodeGenerate.exe");
                    myProcess.StartInfo.Arguments = strCodigoBarra;
                    myProcess.StartInfo.CreateNoWindow = true;
                    myProcess.Start();
                }
                catch (Exception ex)
                {
                    //Response.Write(ex.Message);
                }
                                
                Thread.Sleep(intMilliseconds);

                ImgCodigoBarra.ImageUrl = "/images/" + strCodigoBarra + ".png";
            }

        }
    }
}