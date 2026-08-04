using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace nlSystemDevices
{
    /// <summary>
    /// Файл ssdMonitors.cs
    /// </summary>
    /// <remarks>Класс для работы с мониторами</remarks>
    public class ssdMonitors
    {
        /// <summary>
        /// Создание PrintScreen для главного экрана компьютера
        /// </summary>
        /// <param name="prFileName">Путь и имя файла для сохранения изображения</param>
        /// <param name="prImagFrmt">Тип файла в который сохраняется изображение</param>
        public void __mPrintScreen(string prFileName, ImageFormat prImagFrmt)
        {
            Bitmap vr_Bit_Map = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Graphics vrGraf = Graphics.FromImage(vr_Bit_Map as Image);
            vrGraf.CopyFromScreen(0, 0, 0, 0, vr_Bit_Map.Size);
            vr_Bit_Map.Save(prFileName, prImagFrmt);
        }
    }
}
