using dnslabwin.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace dnslabwin.Extensions
{
    public static class ImageExtensions
    {
        public static void SetAlert(this Image image, AlertEnum alert)
        {
            string imgDir = AppDomain.CurrentDomain.BaseDirectory + "/Assets/Images/";

            switch (alert)
            {
                case AlertEnum.Success:
                    imgDir += "success.png";
                    break;
                case AlertEnum.Warning:
                    imgDir += "warning.png";
                    break;
                case AlertEnum.Danger:
                    imgDir += "danger.png";
                    break;
            }

            image.Source = new BitmapImage(new Uri(imgDir, UriKind.Absolute));
        }
    }
}
