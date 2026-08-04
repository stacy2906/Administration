using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace nlSystem
{
    internal class sstClipboard
    {
        public const uint CF_METAFILEPICT = 3;
        public const uint CF_ENHMETAFILE = 14;

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool OpenClipboard(IntPtr hWndNewOwner);
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool CloseClipboard();
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetClipboardData(uint format);
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool IsClipboardFormatAvailable(uint format);

        public static void __mClipbboardToImageFile()
        {
            if (OpenClipboard(new IntPtr(0)))
            {
                if (IsClipboardFormatAvailable(CF_ENHMETAFILE))
                {
                    IntPtr ptr = GetClipboardData(CF_ENHMETAFILE);
                    if (!ptr.Equals(new IntPtr(0)))
                    {
                        Metafile metafile = new Metafile(ptr, true); /// Перенести в библиотеку ResourcesImages
                        metafile.Save("out.bmp");
                    }
                }
                CloseClipboard();
            }
        }
        private static void ClipboardToImageFile()
        {

            var image = Clipboard.GetImage();

            if (image == null)
            {
                Console.WriteLine("No image found");

                return;
            }

            try
            {
                image.Save(@"c:\temp\sample.png", ImageFormat.Png);
                Console.WriteLine("Image saved");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
