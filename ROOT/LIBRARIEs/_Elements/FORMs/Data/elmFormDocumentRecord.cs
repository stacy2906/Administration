using System.Windows.Forms;

namespace nlElements
{
    /// <summary>
    /// Файл elmFormRecord.cs
    /// </summary>
    /// <remarks>Класс-форма для изменения записи документа</remarks>
    public class elmFormDocumentRecord : elmForm
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

            Controls.Add(__cAreaDocumentRecord);
            Controls.SetChildIndex(__cAreaDocumentRecord, 0);

            #endregion Размещение компонентов

            #region /// Настройка компонентов

            Text = "Базовая форма для изменения записи документа";

            // _cAreaDocumentRecord
            {
                __cAreaDocumentRecord.Dock = DockStyle.Fill;
                __cAreaDocumentRecord.__fBlockInputsCheckShow_ = false;
            }

            #endregion Настройка компонентов

            ResumeLayout(false);

            return;
        }

        #endregion ДИЗАЙНЕРЫ

        #region = ПОЛЯ

        #region - Компоненты

        public elmAreaDocumentRecord __cAreaDocumentRecord = new elmAreaDocumentRecord();

        #endregion Компоненты

        #endregion ПОЛЯ
    }
}
