using System.DrawingCore;
using System.DrawingCore.Imaging;
using System.IO;

namespace LTMCompanyName.YoyoCmsTemplate.Helpers
{
    public class ImageFormatHelper
    {
        public static ImageFormat GetRawImageFormat(byte[] fileBytes)
        {
            using (var ms = new MemoryStream(fileBytes))
            {
                var fileImage = Image.FromStream(ms);
                return fileImage.RawFormat;
            }
        }
    }
}

