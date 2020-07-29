using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using XY.ZnshBusiness.IService;

namespace XY.ZnshBusiness.Service
{
    public class QRCodeService:IQRCodeService
    {
        #region  QRCode

        public Bitmap GetQRCode(string plainText, int pixel)
        {
            var generator = new QRCodeGenerator();
            var qrCodeData = generator.CreateQrCode(plainText, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new QRCoder.QRCode(qrCodeData);

            var bitmap = qrCode.GetGraphic(pixel);

            return bitmap;
        }
        #endregion
    }
}
