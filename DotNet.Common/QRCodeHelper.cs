using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QRCoder;

namespace DotNet.Common
{
    /// <summary>
    /// 二维码帮助类
    /// </summary>
    public class QRCodeHelper
    {
        public static MemoryStream CreateQRCode(string text)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            var ms = new System.IO.MemoryStream();
            qrCodeImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return ms;
        }
    }
}
