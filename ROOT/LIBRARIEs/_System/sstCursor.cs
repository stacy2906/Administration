using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace nlSystem
{
    /// <summary>
    /// Файл sstCursor.cs
    /// </summary>
    /// <remarks>Класс-единица основных свойств файла</remarks>
    /// <conception>Lucasin V.</conception> // Замысел
    /// <version>2026.01.13 14-48</version> // Дата-время последней корректировки
    public class sstCursor
    {
        #region = БИБЛИОТЕКИ

        [DllImport("User32.dll")]
        private static extern IntPtr LoadCursorFromFile(String str);

        #endregion БИБЛИОТЕКИ

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        /// <example>
        /// this.Cursor = sstCursor.Create(Path.Combine(Application.StartupPath, @"D:\coin.ani"));
        /// </example>
        /// <exception cref="ApplicationException"></exception>
        public static Cursor Create(string filename)
        {
            IntPtr hCursor = LoadCursorFromFile(filename);
            if (!IntPtr.Zero.Equals(hCursor))
            {
                return new Cursor(hCursor);
            }
            else
            {
                throw new ApplicationException("Could not create cursor from file " + filename);
            }
        }
    }
}
