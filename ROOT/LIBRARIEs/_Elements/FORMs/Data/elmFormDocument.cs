using System;

namespace nlElements
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>Класс-Форма для правки документа</remarks>
    public class elmFormDocument : elmForm
    {
        #region = ДИЗАЙНЕРЫ

        /// <summary>
        /// Сборка объекта
        /// </summary>
        protected override void _mObjectAssembly()
        {
            SuspendLayout();

            base._mObjectAssembly();

            #region /// Размещение компонентов

            Controls.Add(__cAreaDocument);
            Controls.SetChildIndex(__cAreaDocument, 0);

            #endregion Размещение компонентов

            ResumeLayout(false);

            return;
        }

        #endregion ДИЗАЙНЕРЫ

        #region = МЕТОДЫ

        #region - Процедуры

        /// <summary>
        /// Загрузка настроек текущей формы в файл
        /// </summary>
        protected override void _mTunesLoad()
        {
            base._mTunesLoad();

            string vSplitterHorisontal = __oFileIni.__mValueRead(__fClassName_.ToUpper(), "SplitterHorisontal");
            string vSplitterVertical = __oFileIni.__mValueRead(__fClassName_.ToUpper(), "SplitterVertical");
            try
            {
                __cAreaDocument.__fSplitterCaptionContentSplitterDistance_ = Convert.ToInt32(vSplitterHorisontal);
                __cAreaDocument.__fSplitterCaptionLeftRightSplitterDistance_ = Convert.ToInt32(vSplitterVertical);
            }
            catch { }
        }
        /// <summary>
        /// Сохранение настроек текущей формы в файл
        /// </summary>
        protected override void _mTunesSave()
        {
            base._mTunesSave();

            __oFileIni.__mValueWrite(__cAreaDocument.__fSplitterCaptionContentSplitterDistance_.ToString(), __fClassName_.ToUpper(), "SplitterHorisontal");
            __oFileIni.__mValueWrite(__cAreaDocument.__fSplitterCaptionLeftRightSplitterDistance_.ToString(), __fClassName_.ToUpper(), "SplitterVertical");
        }

        #endregion Процедуры

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Компоненты

        /// <summary>
        /// Область для правки документа
        /// </summary>
        public elmAreaDocument __cAreaDocument = new elmAreaDocument();

        #endregion Компоненты

        #endregion ПОЛЯ
    }
}
