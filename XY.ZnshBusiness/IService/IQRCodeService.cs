using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace XY.ZnshBusiness.IService
{
    public interface IQRCodeService
    {
        Bitmap GetQRCode(string url, int pixel);
    }
}
