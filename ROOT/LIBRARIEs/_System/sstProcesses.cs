using System.Diagnostics;

namespace nlSystem
{
    /// <summary>
    /// Файл ssmProcesses.cs
    /// </summary>
    /// <remarks>Класс для работы с процессами системы</remarks>
    /// <conception>Lucasin V.</conception> // Замысел
    /// <version>2026.01.13 14-56</version> // Дата-время последней корректировки
    public class sstProcesses
    {
        #region = МЕТОДЫ

        #region - Процедуры

        /// <summary>
        /// Выполнение внешнего исполняемого файла
        /// </summary>
        /// <param name="pApplication">Путь и имя выполняемого файла</param>
        /// <param name="pParameters">Список параметров передаваемых выполняемому файлу</param>
        /// <param name="pWaitForFinish">Ожидать завершения выполнения</param>
        /// <returns>[true] - процесс выполнения запущен, иначе - [false]</returns>
        public bool __mRun(string pApplication, string pParameters, bool pWaitForFinish = true)
        {
            bool vReturn = true; // Возвращаемое значение
            Process vProcess = new Process(); // Создание процесса выполнения

            vProcess.StartInfo.UseShellExecute = false;
            vProcess.StartInfo.RedirectStandardOutput = true;
            vProcess.StartInfo.FileName = pApplication;
            vProcess.StartInfo.Arguments = pParameters;
            vProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            vProcess.StartInfo.CreateNoWindow = true;
            vReturn = vProcess.Start();
            if (pWaitForFinish == true)
            {
                vProcess.WaitForExit();
            }

            return vReturn;
        }

        #endregion Процедуры

        #endregion МЕТОДЫ
    }
}
