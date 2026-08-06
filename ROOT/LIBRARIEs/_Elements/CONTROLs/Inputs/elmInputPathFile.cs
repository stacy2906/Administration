using System;

namespace nlElements
{
    /// <summary>
    /// Файл elmInputPathFile.cs
    /// </summary>
    /// <remarks>Класс-поле ввода пути к файлу</remarks>
    internal class elmInputPathFile : elmInputPath
    {
        #region = ДИЗАЙНЕРЫ

        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public elmInputPathFile()
        {
            __fPathType_ = PATHTYPES.File;
        }

        #endregion ДИЗАЙНЕРЫ
    }
}