using System;

namespace nlElements
{
    /// <summary>
    /// Файл elmInputPathDirectory.cs
    /// </summary>
    /// <remarks>Класс-поле ввода пути к папке</remarks>
    public class elmInputPathDirectory : elmInputPath
    {
        #region = ДИЗАЙНЕРЫ

        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public elmInputPathDirectory()
        {
            __fPathType_ = PATHTYPES.Directory;
        }

        #endregion ДИЗАЙНЕРЫ
    }
}