using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NetLab.Helpers
{
    public static class FileHelpers
    {
        public static byte[] GetBytes(HttpPostedFileBase file)
        {
            if (!(file?.ContentLength > 0)) return null;

            var imgBinaryData = new byte[file.ContentLength];
            file.InputStream.Read(imgBinaryData, 0, file.ContentLength);

            return imgBinaryData;
        }
    }
}