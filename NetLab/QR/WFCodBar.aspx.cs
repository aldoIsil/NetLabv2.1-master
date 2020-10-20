using System;

namespace NetLab.QR
{
    public partial class WFCodBar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            AztecCode();
            
        }

        private void AztecCode()
        {
            //During the page load event we will set all parameters for the control			
            //Checking the Parameters using Params.Get is NOT case sensitive.  
            //However, when we use the Equals method of the string data type it IS case sensitive
            
            //Set the Data to Encode
            if (Request.Params.Get("Barcode") != "" && Request.Params.Get("Barcode") != null)
                AztecBarcode1.DataToEncode = Request.Params.Get("Barcode");

            //Set the x dimension
            if (Request.Params.Get("X") != "" && Request.Params.Get("X") != null)
                AztecBarcode1.XDimensionCM = Request.Params.Get("X");

            //set the left margin
            if (Request.Params.Get("Left_Margin") != "" && Request.Params.Get("Left_Margin") != null)
                AztecBarcode1.LeftMarginCM = Request.Params.Get("Left_Margin");

            //set the top margin
            if (Request.Params.Get("Top_Margin") != "" && Request.Params.Get("Top_Margin") != null)
                AztecBarcode1.TopMarginCM = Request.Params.Get("Top_Margin");

            //Set the resolution of the returned image
            if (Request.Params.Get("IR") != "" && Request.Params.Get("IR") != null)
                try
                {
                    AztecBarcode1.ImageResolution = Convert.ToInt32(Request.Params.Get("IR"));
                }
                catch
                {
                    System.Diagnostics.Debug.Write("Invalid entry for Image Resolution parameter");
                }
                

            //Determine if(ApplyTilde is turned on
            if (Request.Params.Get("PT") != "" && Request.Params.Get("PT") != null)
            {
                string tildeValue = Request.Params.Get("PT");
                if (tildeValue.Equals("Y") == true || tildeValue.Equals("Yes") == true ||
                    tildeValue.Equals("True") == true || tildeValue.Equals("TRUE") == true ||
                    tildeValue.Equals("true") == true || tildeValue.Equals("yes") == true ||
                    tildeValue.Equals("y") == true)
                    AztecBarcode1.ApplyTilde = true;
                else
                    AztecBarcode1.ApplyTilde = false;
            }

            //set the rotation
            if (Request.Params.Get("Rotate") != "" && Request.Params.Get("Rotate") != null)
            {
                if (Request.Params.Get("Rotate").Equals("90"))
                    AztecBarcode1.RotationAngle = IDAutomation.AztecServerControl.AztecBarcode.RotationAngles.Ninety_Degrees;
                else if (Request.Params.Get("Rotate").Equals("180"))
                    AztecBarcode1.RotationAngle = IDAutomation.AztecServerControl.AztecBarcode.RotationAngles.One_Hundred_Eighty_Degrees;
                else if (Request.Params.Get("Rotate").Equals("270"))
                    AztecBarcode1.RotationAngle = IDAutomation.AztecServerControl.AztecBarcode.RotationAngles.Two_Hundred_Seventy_Degrees;
                else
                    AztecBarcode1.RotationAngle = IDAutomation.AztecServerControl.AztecBarcode.RotationAngles.Zero_Degrees;
            }



            if (Request.Params.Get("FORMAT") != "" && Request.Params.Get("FORMAT") != null)
            {
                if (Request.Params.Get("FORMAT").Equals("GIF"))
                {
                    Response.ContentType = "image/gif";
                    AztecBarcode1.GetBitmapImageForBinaryStream().Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Gif);
                }
                else
                {
                    Response.ContentType = "image/jpeg";
                    AztecBarcode1.GetBitmapImageForBinaryStream().Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
            }
            else
            {
                Response.ContentType = "image/gif";
                AztecBarcode1.GetBitmapImageForBinaryStream().Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            
        }
    }
}