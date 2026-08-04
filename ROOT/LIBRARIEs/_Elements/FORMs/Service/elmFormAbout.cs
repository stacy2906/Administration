using System.Drawing;
using System.Windows.Forms;
using System;

namespace nlElements
{
    /// <summary>
    /// Файл elmFormAbout.cs
    /// </summary>
    /// <remarks>Класс-форма описания приложения</remarks>
    public class elmFormAbout : elmForm
    {
        #region = МЕТОДЫ

        #region - Объект

        protected override void _mObjectAssembly()
        {
            SuspendLayout();

            base._mObjectAssembly();

            #region /// Размещение компонентов

            Controls.Add(_cSplitter);
            Controls.SetChildIndex(_cSplitter, 0);

            _cSplitter.Panel1.Controls.Add(_cLabelLogotype);
            _cSplitter.Panel1.Controls.Add(_cLabelDesigner);
            _cSplitter.Panel1.Controls.Add(_cLabelNative);
            _cSplitter.Panel1.Controls.Add(_cLabelApplications);
            _cSplitter.Panel2.Controls.Add(_cLabelCaption);
            _cSplitter.Panel2.Controls.Add(_cLabelPacket);
            _cSplitter.Panel2.Controls.Add(_cLabelDescription);
            _cSplitter.Panel2.Controls.Add(_cLabelVersion);
            _cSplitter.Panel2.Controls.Add(_cLabelPrefix);
            _cSplitter.Panel2.Controls.Add(_cLabelOwner);
            _cSplitter.Panel2.Controls.Add(_cLabelHelp);
            _cSplitter.Panel2.Controls.Add(_cLabelHelpFile);
            _cSplitter.Panel2.Controls.Add(_cLabelProcessClue);
            _cSplitter.Panel2.Controls.Add(_cLabelProcessName);

            #endregion Размещение компонентов

            #region /// Настройка компонентов

            __fCaption_ = "О приложении";

            //_cSplitter
            {
                _cSplitter.FixedPanel = FixedPanel.Panel1;
                _cSplitter.IsSplitterFixed = true;

                // _cSplitter.Panel1
                {
                    // _cLabelLogotype
                    {
                        _cLabelLogotype.Text = "LuNA`";
                        _cLabelLogotype.Font = new Font("Broadway", 48);
                        _cLabelLogotype.Location = new Point(20, 20);
                    }
                    // _cLabelDesigner
                    {
                        _cLabelDesigner.__fLabelType_ = LABELTYPES.Title;
                        _cLabelDesigner.AutoSize = false;
                        _cLabelDesigner.Text = "Lukashin";
                        _cLabelDesigner.Location = new Point(0, 100);
                        _cLabelDesigner.Size = new System.Drawing.Size(270, 23);
                        _cLabelDesigner.TextAlign = ContentAlignment.MiddleCenter;
                    }
                    // _cLabelNative
                    {
                        _cLabelNative.__fLabelType_ = LABELTYPES.Title;
                        _cLabelNative.AutoSize = false;
                        _cLabelNative.Text = "Native";
                        _cLabelNative.Location = new Point(0, 125);
                        _cLabelNative.Size = new System.Drawing.Size(270, 23);
                        _cLabelNative.TextAlign = ContentAlignment.MiddleCenter;
                    }
                    // _cLabelApplications
                    {
                        _cLabelApplications.__fLabelType_ = LABELTYPES.Title;
                        _cLabelApplications.AutoSize = false;
                        _cLabelApplications.Text = "Applications";
                        _cLabelApplications.Location = new Point(0, 150);
                        _cLabelApplications.Size = new System.Drawing.Size(270, 23);
                        _cLabelApplications.TextAlign = ContentAlignment.MiddleCenter;
                    }
                }
                // _cSplitter.Panel2
                {
                    // _cLabelCaption
                    {
                        _cLabelCaption.Text = elmApplication.__oTunes.__mTranslate("Название") + ": " + elmApplication.__fCaption_;
                        _cLabelCaption.Location = new Point(10, 20);
                    }
                    // _cLabelPacket
                    {
                        _cLabelPacket.Text = elmApplication.__oTunes.__mTranslate("Пакет приложений") + ": " + elmApplication.__fPacket_;
                        _cLabelPacket.Location = new Point(10, 45);
                    }
                    // _cLabelDescription
                    {
                        _cLabelDescription.Text = elmApplication.__oTunes.__mTranslate("Назначение") + ": " + elmApplication.__fDescription_;
                        _cLabelDescription.Location = new Point(10, 70);
                    }
                    // _cLabelVersion
                    {
                        _cLabelVersion.Text = elmApplication.__oTunes.__mTranslate("Версия") + ": " + elmApplication.__fVersion_;
                        _cLabelVersion.Location = new Point(10, 95);
                    }
                    // _cLabelPrefix
                    {
                        _cLabelPrefix.Text = elmApplication.__oTunes.__mTranslate("Префикс создаваемых файлов") + ": " + elmApplication.__fPrefix_;
                        _cLabelPrefix.Location = new Point(10, 120);
                    }
                    // _cLabelHelp
                    {
                        _cLabelHelp.Text = elmApplication.__oTunes.__mTranslate("Помощь") + ": ";
                        _cLabelHelp.Location = new Point(10, 145);
                    }
                    // _cLabelHelpFile
                    {
                        _cLabelHelpFile.Text = elmApplication.__fHelpFileName_;
                        _cLabelHelpFile.__fLabelType_ = LABELTYPES.Button;
                        _cLabelHelpFile.Location = new Point(70, 145);
                        _cLabelHelpFile.__eClickLeft += mLabelHelpFile_MouseClickLeft;
                    }
                    // _cLabelOwner
                    {
                        _cLabelOwner.Text = elmApplication.__oTunes.__mTranslate("Владелец") + ": " + elmApplication.__fOwner_;
                        _cLabelOwner.Location = new Point(10, 180);
                    }
                    // _cLabelProcessClue
                    {
                        _cLabelProcessClue.Text = elmApplication.__oTunes.__mTranslate("Процесс") + ": " + elmApplication.__fProcessName_.Trim() + " [" + elmApplication.__fProcessClue_.ToString() + "]";
                        _cLabelProcessClue.Location = new Point(10, 205);
                    }
                }
            }

            #endregion Настройка компонентов

            ResumeLayout();

            return;
        }

        /// <summary>
        /// Презентация объекта
        /// </summary>
        protected override void _mObjectPresentation()
        {
            base._mObjectPresentation();
            _cSplitter.SplitterDistance = 270;
        }

        #endregion Объект

        #region - Поведение

        /// <summary>
        /// Выполняется при выборе надписи левой кнопкой мыши
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mLabelHelpFile_MouseClickLeft(object sender, EventArgs e)
        {
            elmApplication.__oEventsHandler.__mHelp(elmApplication.__fHelpFileName_);
        }

        #endregion Поведение

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Компоненты

        /// <summary>
        /// Разделитель
        /// </summary>
        protected elmComponentSplitter _cSplitter = new elmComponentSplitter();
        /// <summary>
        /// Надпись - логотип
        /// </summary>
        protected elmComponentLabel _cLabelLogotype = new elmComponentLabel();
        /// <summary>
        /// Разработчик
        /// </summary>
        protected elmComponentLabel _cLabelDesigner = new elmComponentLabel();
        /// <summary>
        /// Нативный
        /// </summary>
        protected elmComponentLabel _cLabelNative = new elmComponentLabel();
        /// <summary>
        /// Приложения
        /// </summary>
        protected elmComponentLabel _cLabelApplications = new elmComponentLabel();

        /// <summary>
        /// Заголовок приложения
        /// </summary>
        protected elmComponentLabel _cLabelCaption = new elmComponentLabel();
        /// <summary>
        /// Пакет программ
        /// </summary>
        protected elmComponentLabel _cLabelPacket = new elmComponentLabel();
        /// <summary>
        /// Описание приложения
        /// </summary>
        protected elmComponentLabel _cLabelDescription = new elmComponentLabel();
        /// <summary>
        /// Версия приложения
        /// </summary>
        protected elmComponentLabel _cLabelVersion = new elmComponentLabel();
        /// <summary>
        /// Префикс файлов
        /// </summary>
        protected elmComponentLabel _cLabelPrefix = new elmComponentLabel();
        /// <summary>
        /// Владелец
        /// </summary>
        protected elmComponentLabel _cLabelOwner = new elmComponentLabel();
        /// <summary>
        /// Надпись 'Помощь'
        /// </summary>
        protected elmComponentLabel _cLabelHelp = new elmComponentLabel();
        /// <summary>
        /// Имя файла помощи приложения
        /// </summary>
        protected elmComponentLabel _cLabelHelpFile = new elmComponentLabel();
        /// <summary>
        /// Идентификатор текущего процесса
        /// </summary>
        protected elmComponentLabel _cLabelProcessClue = new elmComponentLabel();
        /// <summary>
        /// Название текущего процесса
        /// </summary>
        protected elmComponentLabel _cLabelProcessName = new elmComponentLabel();

        #endregion Компоненты

        #endregion ПОЛЯ
    }
}
