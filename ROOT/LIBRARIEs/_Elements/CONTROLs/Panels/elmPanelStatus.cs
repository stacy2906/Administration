using System;
using System.Drawing;
using System.Windows.Forms;

namespace nlElements
{
    /// <summary>
    /// Файл elmPanelStatus.cs
    /// </summary>
    /// <remarks>Класс-панель для отображения статуса формы</remarks>
    public class elmPanelStatus : elmComponentPanel
    {
        #region = МЕТОДЫ

        /// <summary>
        /// Сборка объекта
        /// </summary>
        protected override void _mObjectAssembly()
        {
            SuspendLayout();

            base._mObjectAssembly();

            #region /// Размещение компонентов

            Controls.Add(_cSplitter);
            _cSplitter.Panel1.Controls.Add(_cLabel);
            _cSplitter.Panel2.Controls.Add(_cProgress);

            #endregion Размещение компонентов

            #region /// Настройка компонента

            BackColor = Color.Transparent;
            Dock = DockStyle.Bottom;

            Width = 400;
            Height = 27;

            // _cSplitter
            {
                _cSplitter.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
                _cSplitter.BorderStyle = BorderStyle.Fixed3D;
                _cSplitter.IsSplitterFixed = true;
                _cSplitter.FixedPanel = FixedPanel.Panel2;
                _cSplitter.Size = new Size(Width - Height, Height);
                _cSplitter.SplitterDistance = _cSplitter.Width - _cProgress.Width - 6;
                _cSplitter.TabStop = false;
            }
            // _cTimer
            {
                _cTimer.Interval = 5000;
                _cTimer.Tick += _cTimer_Tick;
            }

            #endregion Настройка копонента

            ResumeLayout(false);

            return;
        }
        #region - Поведение

        /// <summary>
        /// Выполняется при тике таймера
        /// </summary>
        private void _cTimer_Tick(object sender, EventArgs e)
        {
        }
        private void mTimer_Tick(object sender, EventArgs e)
        {
            switch (fPanelStatus)
            {
                // Отображение текста м процента выполнения задач 
                case STATUSPANELTYPEs.TextAndPercent:
                    break;
                // Отображение текста и движения прогресса по таймеру
                case STATUSPANELTYPEs.TextAndTimer:
                    _cProgress.Value++;
                    if (_cProgress.Value >= 100)
                        _cProgress.Value = 0;
                    break;
                // Отображение текста по таймеру
                case STATUSPANELTYPEs.TextByTimer:
                    _cLabel.Text = "";
                    _cTimer.Stop();
                    break;
                // Отображение текста до выполнения метода '__mClear'
                case STATUSPANELTYPEs.Text:
                    _cTimer.Stop();
                    break;
            }
            _cLabel.Text = "";
            _cTimer.Stop();
        }

        /// <summary>
        /// Выполняется после создания формы
        /// </summary>
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            _mObjectPresentation();
        }

        #endregion Поведение

        #region - Процедуры

        /// <summary>
        /// Сборка выражения с параметрами и перевод выражения на язык интерфейса 
        /// </summary>
        /// <param name="pString">Текст</param>
        /// <param name="pParameters">Список дополнительных парамметров</param>
        public void __mCaptionBuilding(string pString, params object[] pParameters)
        {
            fTextWithOutTranslate = String.Format(pString, pParameters);
            _cLabel.Text = elmApplication.__oTunes.__mTranslate(pString, pParameters);
            _cTimer.Start();
        }

        /// <summary>
        /// Очистка панели статуса
        /// </summary>
        public void __mClear()
        {
            _cLabel.Text = "";
            _cTimer.Stop();
        }

        #endregion Процедуры

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Служебные

        /// <summary>
        /// Не переведенное сообщение
        /// </summary>
        private string fTextWithOutTranslate = "";
        /// <summary>
        /// Время до очистки текста
        /// </summary>
        private int fTextClearSeconds = 0;
        /// <summary>
        /// Режим работы панели статуса
        /// </summary>
        private STATUSPANELTYPEs fPanelStatus = STATUSPANELTYPEs.TextAndPercent;

        #endregion Служебные

        #region - Компоненты

        /// <summary>
        /// Надпись
        /// </summary>
        protected elmComponentLabel _cLabel = new elmComponentLabel();
        /// <summary>
        /// Прогресс
        /// </summary>
        protected elmComponentProgress _cProgress = new elmComponentProgress();
        /// <summary>
        /// Разделитель
        /// </summary>
        protected elmComponentSplitter _cSplitter = new elmComponentSplitter();
        /// <summary>
        /// Таймер
        /// </summary>
        protected Timer _cTimer = new Timer();

        #endregion Компоненты

        #endregion ПОЛЯ

        #region = СВОЙСТВА

        /// <summary>
        /// Текст заголовка
        /// </summary>
        /// <remarks>Отображаемый текст переводиться на язык интерфейса. Возвращается не переведенный текст</remarks>
        public string __fCaption_
        {
            get { return fTextWithOutTranslate; }
            set
            {
                fTextWithOutTranslate = value;
                _cLabel.Text = elmApplication.__oTunes.__mTranslate(fTextWithOutTranslate);
                if (fPanelStatus > STATUSPANELTYPEs.TextByTimer)
                    _cTimer.Start();
            }
        }
        /// <summary>
        /// Процент выполненой работы
        /// </summary>
        public int __fPercent_
        {
            get { return _cProgress.Value; }
            set
            {
                if (fPanelStatus == STATUSPANELTYPEs.TextAndPercent)
                    _cProgress.Value = value;
            }
        }
        /// <summary>
        /// Время до очистки текста
        /// </summary>
        public int __fTextClearSeconds_
        {
            get { return _cTimer.Interval / 1000; }
            set
            {
                _cTimer.Stop();
            }
        }
        /// <summary>
        /// Режим работы панели статуса
        /// </summary>
        public STATUSPANELTYPEs __fPanelStatus_
        {
            get { return fPanelStatus; }
            set
            {
                fPanelStatus = value;
                switch (fPanelStatus)
                {
                    // Отображение текста м процента выполнения задач 
                    case STATUSPANELTYPEs.TextAndPercent:
                        break;
                    // Отображение текста и движения прогресса по таймеру
                    case STATUSPANELTYPEs.TextAndTimer:
                        _cTimer.Interval = 1000;
                        _cTimer.Start();
                        break;
                    // Отображение текста по таймеру
                    case STATUSPANELTYPEs.TextByTimer:
                        break;
                    // Отображение текста до выполнения метода '__mClear'
                    case STATUSPANELTYPEs.Text:
                        break;
                }
            }
        }

        #endregion СВОЙСТВА
    }
}
