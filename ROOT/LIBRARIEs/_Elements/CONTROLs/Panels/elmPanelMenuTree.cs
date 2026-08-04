using System.Drawing;
using System.Windows.Forms;
using System;

namespace nlElements
{
    /// <summary>
    /// Файл elmPanelMenuTree.cs
    /// </summary>
    /// <remarks>Класс-панель для отображения меню пользователя</remarks>
    public class elmPanelMenuTree : elmComponentSplitter
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

            #region Размещение компонентов

            Panel1.Controls.Add(_cPicture);
            Panel1.Controls.Add(_cLabelStatus);
            Panel1.Controls.Add(_cLabelStatusValue);
            Panel1.Controls.Add(_cLabelUser);
            Panel1.Controls.Add(_cLabelUserValue);
            Panel2.Controls.Add(_cTree);

            #endregion Размещение компонентов

            #region Настройка компонентов

            Dock = DockStyle.Fill;
            IsSplitterFixed = true;
            FixedPanel = FixedPanel.Panel1;
            Orientation = Orientation.Horizontal;
            TabStop = false;

            // _cPicture
            {
                _cPicture.Image = nlResourcesImages.Properties.Resources._Emotion_Glass_y;
                _cPicture.Location = new Point(5, 5);
                _cPicture.SizeMode = PictureBoxSizeMode.Normal;
                _cPicture.Size = new Size(_cPicture.Image.Width, _cPicture.Image.Height);
            }
            // _cLabelRole
            {
                _cLabelStatus.Location = new Point(_cPicture.Width + elmInterface.__fIntervalHorizontal * 2, 10);
                _cLabelStatus.__fCaption_ = "Статус пользователя";
            }
            // __cLabelRoleValue
            {
                _cLabelStatusValue.Location = new Point(200, 10);
                //_cLabelStatusValue.__fCaption_ = (elmApplication.__oData.__mUserAdministrator() == true ? "Администратор" : "");
                //if (elmApplication.__oData.__mUserDesign() == true & _cLabelStatusValue.__fCaption_.Length > 0)
                //{
                //    _cLabelStatusValue.__fCaption_ += ", " ;
                //}
                //if (elmApplication.__oData.__mUserDesign() == true)
                //{
                //    _cLabelStatusValue.__fCaption_ += "Разработчик";
                //}
            }
            // __cLabelUser
            {
                _cLabelUser.Location = new Point(_cPicture.Width + elmInterface.__fIntervalHorizontal * 2, 40);
                _cLabelUser.__fCaption_ = "Пользователь";
            }
            // __cLabelUserValue
            {
                _cLabelUserValue.Location = new Point(200, 40);
            }
            // __cTree
            {
                _cTree.Dock = DockStyle.Fill;
                _cTree.DoubleClick += mTree_DoubleClick;
            }

            #endregion Настройка компонентов

            ResumeLayout();

            return;
        }
        /// <summary>
        /// Сборка объектов
        /// </summary>
        protected override void _mObjectPresentation()
        {
            base._mObjectPresentation();
            SplitterDistance = _cPicture.Width + elmInterface.__fIntervalVertical * 2;
        }
        /// <summary>
        /// Выполняется при получении фокуса
        /// </summary>
        /// <param name="e"></param>
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            _cTree.Focus();
        }

        #endregion Объект

        #region - Поведение

        /// <summary>
        /// Выполняется при двойном клике мыши по узлу 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mTree_DoubleClick(object sender, System.EventArgs e)
        {
            if (__eTreeDoubleClick != null)
                __eTreeDoubleClick(_cTree, new EventArgs());
        }

        #endregion Поведение

        #region - Процедуры

        /// <summary>
        /// Передача фокуса дереву
        /// </summary>
        public void _mFocus()
        {
            _cTree.Focus();
        }
        /// <summary>Удаление узлов из контрола
        /// </summary>
        public void _mNodesClear()
        {
            _cTree.Nodes.Clear();
        }
        /// <summary>
        /// Создание нового узла с переводом заголовка на язык пользователя
        /// </summary>
        /// <param name="pCaptionText">Заголовок</param>
        /// <param name="pTag">Содержание тэга</param>
        /// <returns>[true] - Узел добавлен, иначе - [false]</returns>
        public elmUnitTreeNode __mNodeNew(string pCaptionText, string pTag)
        {
            return _cTree.__mNodeNew(pCaptionText, pTag);
        }
        /// <summary>
        /// Создание нового узла с переводом заголовка на язык пользователя
        /// </summary>
        /// <param name="pCaptionText">Заголовок</param>
        /// <param name="pTag">Содержание тэга</param>
        /// <returns>[true] - Узел добавлен, иначе - [false]</returns>
        public elmUnitTreeNode __mNodeNew(string pCaptionText, string pTag, int pImageIndexNormal = 10000, int pImageIndexSelected = 10000)
        {
            return _cTree.__mNodeNew(pCaptionText, pTag, pImageIndexNormal, pImageIndexSelected);
        }
        /// <summary>
        /// Создание вложенного узла с переводом заголовка на язык пользователя
        /// </summary>
        /// <param name="pTreeNodeParent">Родительский узел</param>
        /// <param name="pCaptionText">Заголовок</param>
        /// <param name="pTag">Содержание тэга</param>
        /// <param name="pImageIndexNormal">Индекс нормального изображения</param>
        /// <param name="pImageIndexSelected">Индекс изображения выбранного узла</param>
        /// <param name="pFont">Шрифт</param>
        /// <param name="pColor">Цвет</param>
        /// <returns>[true] - Узел добавлен, иначе - [false]</returns>
        public elmUnitTreeNode __mNodeSupply(elmUnitTreeNode pTreeNodeParent, string pCaptionText, string pTag, Font pFont, Color pColor, int pImageIndexNormal = 10000, int pImageIndexSelected = 10000)
        {
            return _cTree.__mNodeSupply(pTreeNodeParent, pCaptionText, pTag, pFont, pColor);
        }
        /// <summary>
        /// Создание вложенного узла с переводом заголовка на язык пользователя
        /// </summary>
        /// <param name="pCaptionText">Заголовок</param>
        /// <param name="pTag">Содержание тэга</param>
        /// <returns>[true] - Узел добавлен, иначе - [false]</returns>
        public elmUnitTreeNode __mNodeSupply(elmUnitTreeNode pTreeNodeParent, string pCaptionText, string pTag)
        {
            return _cTree.__mNodeSupply(pTreeNodeParent, pCaptionText, pTag);
        }
        /// <summary>
        /// Создание вложенного узла с переводом заголовка на язык пользователя
        /// </summary>
        /// <param name="pCaptionText">Заголовок</param>
        /// <param name="pTag">Содержание тэга</param>
        /// <returns>[true] - Узел добавлен, иначе - [false]</returns>
        public elmUnitTreeNode __mNodeSupply(elmUnitTreeNode pTreeNodeParent, string pCaptionText, string pTag, int pImageIndexNormal = 10000, int pImageIndexSelected = 10000)
        {
            return _cTree.__mNodeSupply(pTreeNodeParent, pCaptionText, pTag, pImageIndexNormal, pImageIndexSelected);
        }
        /// <summary>
        /// Очистка списка изображений
        /// </summary>
        public void __mTreeImagesClear()
        {
            _cTree.ImageList.Images.Clear();
        }

        #endregion - Процедуры

        #endregion = МЕТОДЫ

        #region = ПОЛЯ

        #region - Компоненты

        /// <summary>
        /// Изображение пользователя
        /// </summary>
        protected elmComponentPicture _cPicture = new elmComponentPicture();
        /// <summary>
        /// Заголовок 'Статусы пользователя'
        /// </summary>
        protected elmComponentLabel _cLabelStatus = new elmComponentLabel();
        /// <summary>
        /// Название статуса пользователя
        /// </summary>
        protected elmComponentLabel _cLabelStatusValue = new elmComponentLabel();
        /// <summary>
        /// Заголовок 'Псевдоним пользователя'
        /// </summary>
        protected elmComponentLabel _cLabelUser = new elmComponentLabel();
        /// <summary>
        /// Название псевдонима пользователя
        /// </summary>
        protected elmComponentLabel _cLabelUserValue = new elmComponentLabel();
        /// <summary>
        /// Меню пользователя
        /// </summary>
        protected elmComponentTree _cTree = new elmComponentTree();

        #endregion - Компоненты

        #endregion ПОЛЯ

        #region = СВОЙСТВА
        public ImageList __fImagesList_
        {
            set { _cTree.ImageList = value; }
        }
        /// <summary>
        /// Статус пользователя
        /// </summary>
        public string __fUserDesign_
        {
            set { _cLabelStatusValue.Text = value; }
        }
        /// <summary>
        /// Псевдоним пользователя
        /// </summary>
        public string __fUserAlias_
        {
            set { _cLabelUserValue.Text = value; }
        }
        /// <summary>
        /// Псевдоним пользователя
        /// </summary>
        public string __fUserRole_
        {
            set { _cLabelUserValue.Text = value; }
        }

        #endregion СВОЙСТВА

        #region = СОБЫТИЯ

        /// <summary>
        /// Возникает при двойном клике по пункту пользовательского меню
        /// </summary>
        public event EventHandler __eTreeDoubleClick;

        #endregion СОБЫТИЯ
    }
}
