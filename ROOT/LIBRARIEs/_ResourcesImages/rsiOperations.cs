using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace nlResourcesImages
{
    #region = ПЕРЕЧИСЛЕНИЯ

    internal enum DrawingOptions
    {
        PRF_CHECKVISIBLE = 0x00000001,
        PRF_NONCLIENT = 0x00000002,
        PRF_CLIENT = 0x00000004,
        PRF_ERASEBKGND = 0x00000008,
        PRF_CHILDREN = 0x00000010,
        PRF_OWNED = 0x00000020
    }

    #endregion ПЕРЕЧИСЛЕНИЯ

    /// <summary>
    /// Файл rsiOperations.cs
    /// </summary>
    /// <remarks>Класс для работы с изображениями</remarks>
    /// <conception>Lucasin V.</conception> // Замысел
    /// <version>2026.01.13 15-50</version> // Дата-время последней корректировки
    public class rsiOperations : IDisposable
    {
        #region = ДИЗАЙНЕРЫ

        #region Dispose модуль

        // общедоступная реализация шаблона Dispose, вызываемого потребителями.
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        // Защищенная реализация шаблона Dispose.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                fs.Dispose();
                // Free any other managed objects here.
                //
            }

            disposed = true;
        }
        // Флаг: Dispose уже был вызван?
        bool disposed = false;
        // Instantiate a FileStream instance.
        FileStream fs = new FileStream("test.txt", FileMode.OpenOrCreate);

        #endregion Dispose модуль

        #endregion ДИЗАЙНЕРЫ

        #region = БИБЛИОТЕКИ

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int msg, IntPtr dc, DrawingOptions opts);
        [DllImport("user32.dll")]
        public extern static IntPtr GetDesktopWindow();
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hwnd);
        [DllImport("gdi32.dll")]
        public static extern UInt64 BitBlt 
            (
                IntPtr hDestDC // дескриптор DC назначения
                , int x // X - координата левой верхней точки копируемой в изображение области
                , int y // Y - координата левой верхней точки копируемой в изображение области
                , int nWidth // ширина копируемой в изображение области
                , int nHeight // высота копируемой в изображение области
                , IntPtr hSrcDC // дескриптор источника DC
                , int xSrc // x-координата исходного левого верхнего угла
                , int ySrc // y-координата исходного верхнего левого угла
                , Int32 dwRop // растровый код операции
            );

        #endregion БИБЛИОТЕКИ

        #region = МЕТОДЫ

        #region - Процедуры

        public static void __mBuildGifAnimation()
        {
            // Загружаем GIF
            Image img = Image.FromFile(@"o009.gif");

            // Число фреймов в анимированном gif
            FrameDimension dimension = new FrameDimension(img.FrameDimensionsList[0]);
            int frameCount = img.GetFrameCount(dimension);
            Console.WriteLine("Фреймов: {0}", frameCount);

            // Переписываем gif в набор bmp
            for (int i = 0; i < frameCount; i++)
            {
                img.SelectActiveFrame(dimension, i);
                MemoryStream ms = new MemoryStream();
                img.Save(ms, ImageFormat.Bmp);
                Image outImg = Image.FromStream(ms);
                outImg.Save(string.Format("out{0}.bmp", i));
            }

            Console.ReadLine();
        }
        /// <summary>
        /// Получение формата для создания файлов по расширению файлов
        /// </summary>
        /// <param name="pExtension">Расширение файлов без точки</param>
        /// <returns>{ImageFormat}</returns>
        public static ImageFormat __mImageFormatByExtension(string pExtension)
        {
            ImageFormat vReturn = ImageFormat.Bmp; // Возвращаемое значение

            switch (pExtension.ToLower()) 
            {
                case "emf":
                    vReturn = ImageFormat.Emf;
                    break;
                case "exif":
                    vReturn = ImageFormat.Exif;
                    break;
                case "gif":
                    vReturn = ImageFormat.Gif;
                    break;
                case "ico":
                    vReturn = ImageFormat.Icon;
                    break;
                case "jpg":
                    vReturn = ImageFormat.Jpeg;
                    break;
                case "jpeg":
                    vReturn = ImageFormat.Jpeg;
                    break;
                case "png":
                    vReturn = ImageFormat.Png;
                    break;
                case "tiff":
                    vReturn = ImageFormat.Tiff;
                    break;
                case "wmf":
                    vReturn = ImageFormat.Wmf;
                    break;
                default:
                    break;
            }

            return vReturn;
        }
        /// <summary>
        /// Изменение типа файла изображение в иконку
        /// </summary>
        /// <param name="pImagePath">Путь к картинке</param>
        /// <param name="pIconPath">Путь к создаваемой иконке</param>
        /// <param name="pIconSize">Размер создаваемой иконки (необязательный параметр)</param>
        public static void __mFileImageToIcon(string pImagePath, string pIconPath, int pIconSize = 0)
        {
            Icon vIcon;
            Image vImage = Image.FromFile(pImagePath, true);
            Size vSize = vImage.Size;
            Bitmap vBitMap = new Bitmap(pIconSize, pIconSize, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics vGraphics = Graphics.FromImage(vBitMap);

            if(pIconSize == 0)
                vGraphics.DrawImage(vImage, 0, 0, vSize.Width, vSize.Height);
            else
                vGraphics.DrawImage(vImage, 0, 0, pIconSize, pIconSize);

            vIcon = Icon.FromHandle(vBitMap.GetHicon());

            FileStream vFileStream = File.Create(pIconPath);

            vIcon.Save(vFileStream);
            vFileStream.Close();
            vIcon.Dispose();
            vBitMap.Dispose();
            vImage.Dispose();
        }
        /// <summary>
        /// Изменение типа файла изображение в иконку
        /// </summary>
        /// <param name="pImage">Путь к картинке</param>
        /// <param name="pIconPath">Путь к создаваемой иконке</param>
        /// <param name="pIconSize">Размер создаваемой иконки (необязательный параметр)</param>
        //public static Icon __mImageToIcon(Image pImage, string pIconPath, int pIconSize = 0)
        public static Icon __mImageToIcon(Image pImage, int pIconSize = 32)
        {
            Icon vIcon; // Возвращаемое значение
            Image vImage = pImage;
            Size vSize = vImage.Size;
            Bitmap vBitMap = new Bitmap(vSize.Width, vSize.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            switch (pIconSize)
                {
                case 16:
                    vBitMap = new Bitmap(vSize.Width, vSize.Height, System.Drawing.Imaging.PixelFormat.Format16bppArgb1555);
                    break;
                case 24:
                    vBitMap = new Bitmap(vSize.Width, vSize.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                    break;
                case 48:
                    vBitMap = new Bitmap(vSize.Width, vSize.Height, System.Drawing.Imaging.PixelFormat.Format48bppRgb);
                    break;
            }

            Graphics vGraphics = Graphics.FromImage(vBitMap);

            //if (pIconSize == 0)
                vGraphics.DrawImage(vImage, 0, 0, vSize.Width, vSize.Height);
            //else
                //vGraphics.DrawImage(vImage, 0, 0, pIconSize, pIconSize);

            vIcon = Icon.FromHandle(vBitMap.GetHicon());

            //FileStream vFileStream = File.Create(pIconPath);

            //vIcon.Save(vFileStream);
            //vFileStream.Close();
            //vIcon.Dispose();
            vBitMap.Dispose();
            vImage.Dispose();

            return vIcon;
        }
        /// <summary>
        /// Создание изображения формы с заголовком
        /// </summary>
        /// <param name="pForm">Форма</param>
        /// <param name="pFilePath">Путь с создаваемому файлу изображения</param>
        /// <returns>[true] - если файл создан, иначе - [false]</returns>
        public static bool __mImageOfForm(Form pForm, string pFilePath = "ImageOfForm.png")
        {
            /// Удаление файла 'pFilePath', если он есть на диске
            if (File.Exists(pFilePath) == true)
                File.Delete(pFilePath);
            /// Создание файла изображения на диске
            using (Bitmap vBitmap = new Bitmap(pForm.Width, pForm.Height))
            {
                using (Graphics vGraphics = Graphics.FromImage(vBitmap))
                {
                    IntPtr dc = vGraphics.GetHdc();
                    try
                    {
                        SendMessage(pForm.Handle, WM_PRINT, dc,
                            DrawingOptions.PRF_CHILDREN |
                            DrawingOptions.PRF_CLIENT |
                            DrawingOptions.PRF_NONCLIENT);
                    }
                    finally
                    {
                        vGraphics.ReleaseHdc(dc);
                    }

                    vBitmap.Save(pFilePath, __mImageFormatByExtension(Path.GetExtension(pFilePath)));
                }
            }
            /// Проверка существования созданного файла и возвращение результата
            return File.Exists(pFilePath);
        }
        //public static bool __mIamgeOfFormPart(Form pForm, string pFilePath)
        //{
        //    return __mImageOfFormPart(pForm.ClientRectangle.Width, pForm.ClientRectangle.Height, pForm.CreateGraphics());
        //}
        //public static bool __mImageOfFormPart(int pWidth, int pHeight, Graphics pGraphics)
        //{
        //    Graphics g1 = this.CreateGraphics();
        //    Image MyImage = new Bitmap(this.ClientRectangle.Width / 2, this.ClientRectangle.Height / 2, g1);
        //    Graphics g2 = Graphics.FromImage(MyImage);
        //    IntPtr dc1 = g1.GetHdc();
        //    IntPtr dc2 = g2.GetHdc();
        //    BitBlt(dc2, 0, 0, this.ClientRectangle.Width, this.ClientRectangle.Height, dc1, 0, 0, 13369376);
        //    g1.ReleaseHdc(dc1);
        //    g2.ReleaseHdc(dc2);
        //    MyImage.Save("out.bmp", ImageFormat.Bmp);
        //}

        /// <summary>
        /// Создание изображения главного монитора
        /// </summary>
        /// <param name="pFilePath"></param>
        /// <returns>[true] - файл создан, иначе - [false]</returns>
        public static bool __mImageOfMainMonitor(string pFilePath = "ImageOfScreen.png")
        {
            //Screen[] vScreens = Screen.AllScreens;

            /// Удаление файла 'pFilePath', если он есть на диске
            if (File.Exists(pFilePath) == true)
                File.Delete(pFilePath);
            using (Bitmap vBitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height))
            {
                using (Graphics vGraphics = Graphics.FromImage(vBitmap))
                {
                    IntPtr dc1 = vGraphics.GetHdc();
                    IntPtr dc2 = GetWindowDC(GetDesktopWindow());

                    BitBlt(dc1, 0, 0, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, dc2, 0, 0, 13369376);

                    vGraphics.ReleaseHdc(dc1);

                    vBitmap.Save(pFilePath, __mImageFormatByExtension(Path.GetExtension(pFilePath)));
                }
            }
            /// Проверка существования созданного файла и возвращение результата
            return File.Exists(pFilePath);
        }
        public static void __mImageChangeSize()
        {
            // Входной файл
            Bitmap inBmp = new Bitmap(@"Add_IWshRuntimeLibrary.bmp");

            Bitmap outBmp;

            // Простое преобразование размера
            outBmp = new Bitmap(inBmp, inBmp.Width / 2, inBmp.Height / 2);
            outBmp.Save("out_2.bmp");

            // Интерполированное преобразование размера
            outBmp = new Bitmap(inBmp.Width / 2, inBmp.Height / 2, PixelFormat.Format24bppRgb);
            Graphics g = Graphics.FromImage(outBmp);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(inBmp, 0, 0, outBmp.Width, outBmp.Height);
            outBmp.Save("out_inter.bmp");
        }
        public static void __mImageChangeSize(int b)
        {
            // Исходный файл
            string fromFileName = @"IMGP0517.jpg";
            // Файл результата
            string toFileName = @"IMGP0517_.jpg";

            // Нужные размеры
            int sizeX = 200;
            int sizeY = 200;

            // Загружаем исходную картинку
            Image image = Image.FromFile(fromFileName);

            // Создаем bitmap нужного размера
            Bitmap bmp = new Bitmap(sizeX, sizeY);
            bmp.MakeTransparent();

            // Рисуем на bitmap картинку
            Graphics graphics = Graphics.FromImage(bmp);
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.DrawImage(image, 0, 0, sizeX, sizeY);
            graphics.Flush();

            // Сохраняем результат (с сохранением RawFormat)
            File.Delete(toFileName);
            FileStream stream = new FileStream(toFileName, FileMode.Create);
            bmp.Save(stream, image.RawFormat);
            stream.Close();

            // image больше не нужен
            image.Dispose();
        }
        public static void __mImageChangeSize(bool b)
        {
            // Входной файл
            Bitmap inBmp = new Bitmap(@"Add_IWshRuntimeLibrary.bmp");

            // Преобразование в пиктограмму заданного размера
            Bitmap outBmp = (Bitmap)inBmp.GetThumbnailImage(75, 75, new Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);

            // Сохраняем выходной файл
            outBmp.Save("out.bmp");
        }

        public static void __mFileToFileMonochrome()
        {
            /// Есть решение в папке Mono
            /// 
            // Загружаем входной файл
            Bitmap bmp = new Bitmap(@"Add_IWshRuntimeLibrary.bmp");

            // Атрибуты серого изображения
            ImageAttributes ia = new ImageAttributes();
            ColorMatrix cm = new ColorMatrix();
            cm.Matrix00 = 1 / 3f;
            cm.Matrix01 = 1 / 3f;
            cm.Matrix02 = 1 / 3f;
            cm.Matrix10 = 1 / 3f;
            cm.Matrix11 = 1 / 3f;
            cm.Matrix12 = 1 / 3f;
            cm.Matrix20 = 1 / 3f;
            cm.Matrix21 = 1 / 3f;
            cm.Matrix22 = 1 / 3f;
            ia.SetColorMatrix(cm);

            // Рисуем серое
            Graphics g = Graphics.FromImage(bmp);
            g.DrawImage(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0,
                bmp.Width, bmp.Height,
                 GraphicsUnit.Pixel, ia);

            // Сохраняем выходной файл
            bmp.Save("out.bmp");
        }

        public static void __mFileToFileNegative()
        {
            Bitmap bmp = new Bitmap(@"Add_IWshRuntimeLibrary.bmp");
            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    System.Drawing.Color c = bmp.GetPixel(x, y);
                    bmp.SetPixel(x, y, System.Drawing.Color.FromArgb(255 - c.R, 255 - c.G, 255 - c.B));
                }
            }
            bmp.Save("out.bmp");
        }
        public static void __mFileToFileReplaceColors()
        {
            // Загружаем входной файл
            Bitmap bmp = new Bitmap(@"Add_IWshRuntimeLibrary.bmp");

            // Атрибуты серого изображения
            ImageAttributes
                ia = new ImageAttributes();
            ColorMap[] clrmap = new ColorMap[1] { new ColorMap() };
            clrmap[0].OldColor = Color.Black;
            clrmap[0].NewColor = Color.Red;
            ia.SetRemapTable(clrmap);
            // Рисуем 
            Graphics g = Graphics.FromImage(bmp);
            g.DrawImage(bmp, new Rectangle(0, 0,
                bmp.Width, bmp.Height), 0, 0,
                bmp.Width, bmp.Height,
                GraphicsUnit.Pixel, ia);
            // Сохраняем выходной файл
            bmp.Save("out.bmp");
        }



        ///// <summary>
        ///// Converts a PNG image to a icon (ico)
        ///// </summary>
        ///// <param name="input">The input stream</param>
        ///// <param name="output">The output stream</param>
        ///// <param name="size">The size (16x16 px by default)</param>
        ///// <param name="preserveAspectRatio">Preserve the aspect ratio</param>
        ///// <returns>Wether or not the icon was succesfully generated</returns>
        //public static bool __mIconToImage(Stream input, Stream output, int size = 16, bool preserveAspectRatio = false)
        //{
        //    Bitmap inputBitmap = (Bitmap)Bitmap.FromStream(input);
        //    if (inputBitmap != null)
        //    {
        //        int width, height;
        //        if (preserveAspectRatio)
        //        {
        //            width = size;
        //            height = inputBitmap.Height / inputBitmap.Width * size;
        //        }
        //        else
        //        {
        //            width = height = size;
        //        }
        //        Bitmap newBitmap = new Bitmap(inputBitmap, new Size(width, height));
        //        if (newBitmap != null)
        //        {
        //            // save the resized png into a memory stream for future use
        //            using (MemoryStream memoryStream = new MemoryStream())
        //            {
        //                newBitmap.Save(memoryStream, ImageFormat.Png);

        //                BinaryWriter iconWriter = new BinaryWriter(output);
        //                if (output != null && iconWriter != null)
        //                {
        //                    // 0-1 reserved, 0
        //                    iconWriter.Write((byte)0);
        //                    iconWriter.Write((byte)0);

        //                    // 2-3 image type, 1 = icon, 2 = cursor
        //                    iconWriter.Write((short)1);

        //                    // 4-5 number of images
        //                    iconWriter.Write((short)1);

        //                    // image entry 1
        //                    // 0 image width
        //                    iconWriter.Write((byte)width);
        //                    // 1 image height
        //                    iconWriter.Write((byte)height);

        //                    // 2 number of colors
        //                    iconWriter.Write((byte)0);

        //                    // 3 reserved
        //                    iconWriter.Write((byte)0);

        //                    // 4-5 color planes
        //                    iconWriter.Write((short)0);

        //                    // 6-7 bits per pixel
        //                    iconWriter.Write((short)32);

        //                    // 8-11 size of image data
        //                    iconWriter.Write((int)memoryStream.Length);

        //                    // 12-15 offset of image data
        //                    iconWriter.Write((int)(6 + 16));

        //                    // write image data
        //                    // png data must contain the whole png data file
        //                    iconWriter.Write(memoryStream.ToArray());

        //                    iconWriter.Flush();

        //                    return true;
        //                }
        //            }
        //        }
        //        return false;
        //    }
        //    return false;
        //}

        ///// <summary>
        ///// Converts a PNG image to a icon (ico)
        ///// </summary>
        ///// <param name="inputPath">The input path</param>
        ///// <param name="outputPath">The output path</param>
        ///// <param name="size">The size (16x16 px by default)</param>
        ///// <param name="preserveAspectRatio">Preserve the aspect ratio</param>
        ///// <returns>Wether or not the icon was succesfully generated</returns>
        //public static bool __mIconToImage(string inputPath, string outputPath, int size = 16, bool preserveAspectRatio = false)
        //{
        //    using (FileStream inputStream = new FileStream(inputPath, FileMode.Open))
        //    using (FileStream outputStream = new FileStream(outputPath, FileMode.OpenOrCreate))
        //    {
        //        return __mIconToImage(inputStream, outputStream, size, preserveAspectRatio);
        //    }
        //}

        #endregion Процедуры

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Константы

        private const int WM_PRINT = 0x0317;
        private const int WM_PRINTCLIENT = 0x0318;

        #endregion Константы

        #endregion ПОЛЯ

        public static bool ThumbnailCallback()
        {
            return false;
        }
    }
}
