using System.Windows.Forms;
using System;

namespace nlElements
{
    /// <summary>
    /// Файл elmFormDocument.cs
    /// </summary>
    /// <remarks>Класс-форма для правки документа</remarks>
    public class elmFormPages : elmForm
    {
        #region = МЕТОДЫ

        #region - Объект

        /// <summary>
        /// Сборка объекта
        /// </summary>
        protected override void _mObjectAssembly()
        {
            SuspendLayout();

            base._mObjectAssembly();

            #region /// Размещение компонентов

            Controls.Add(__cAreaPages);
            Controls.SetChildIndex(__cAreaPages, 0);

            #endregion Размещение компонентов

            #region /// Настройка компонентов

            Text = "Базовая форма для правки документов";
            
            // _cAreaDocument
            {
                __cAreaPages.Dock = DockStyle.Fill;
            }

            #endregion Настройка компонентов

            ResumeLayout(false);

            return;
        }

        #endregion Объект

        #region - Процедуры

        /// <summary> 
        /// Загрузка настроек текущей формы из файла
        /// </summary>
        protected override void _mTunesLoad()
        {
            base._mTunesLoad();

            string vString = __oFileIni.__mValueRead(Name.ToUpper(), "SplitterCaptionContent");
            if (vString.Length == 0)
                vString = "200";
            __cAreaPages.__fSplitterCaptionContentSplitterDistance_ = Convert.ToInt32(vString);

            vString = __oFileIni.__mValueRead(Name.ToUpper(), "SplitterCaptionLeftRight");
            if (vString.Length == 0)
                vString = "200";
            __cAreaPages.__fSplitterCaptionLeftRightSplitterDistance_ = Convert.ToInt32(vString);

            return;
        }
        /// <summary> 
        /// Сохранение настроек текущей формы в файл
        /// </summary>
        protected override void _mTunesSave()
        {
            base._mTunesSave();

            __oFileIni.__mValueWrite(__cAreaPages.__fSplitterCaptionContentSplitterDistance_.ToString(), Name.ToUpper(), "SplitterCaptionContent");
            __oFileIni.__mValueWrite(__cAreaPages.__fSplitterCaptionLeftRightSplitterDistance_.ToString(), Name.ToUpper(), "SplitterCaptionLeftRight");

            return;
        }

        #endregion Процедуры

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Компоненты

        /// <summary>
        /// Область для правки документов
        /// </summary>
        public elmAreaPages __cAreaPages = new elmAreaPages();

        #endregion Компоненты

        #endregion ПОЛЯ
    }
}
