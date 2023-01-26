using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Screean_Server.Classes
{
    internal class ScreenShote
    {
        public MemoryStream TakeScreenShot()
        {
            Bitmap bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            MemoryStream test = new MemoryStream();
            string path;
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.CopyFromScreen(0, 0, 0, 0, Screen.PrimaryScreen.Bounds.Size);
                bmp.Save(test, ImageFormat.Jpeg);
                //path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Images");

                //bmp.Save(path + "\\screenshot" + count.ToString() + ".png");  // saves the image
            }
            return test;
            //      var source = path + "\\screenshot"  + count.ToString() + ".png";
            //    return source;
        }
    }
}
}
