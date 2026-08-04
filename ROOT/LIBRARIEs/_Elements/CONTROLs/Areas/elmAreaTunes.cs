using nlApplication;
using System;

namespace nlElements
{
    /// <summary>
    /// Файл elnAreaTunes.cs
    /// </summary>
    /// <remarks>Класс-Область для изменения настроек приложения</remarks>
    public class elmAreaTunes : elmArea
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

            Panel2.Controls.Add(_cBlockInputs);
            Panel2.Controls.SetChildIndex(_cBlockInputs, 0);
            _cToolBar.Items.Insert(1, _cButtonSave);

            #endregion Размещение компонентов

            #region /// Настройка компонентов

            _cBlockInputs.__fMarkShow = false;

            // _cButtonSave
            {
                _cButtonSave.Image = global::nlResourcesImages.Properties.Resources._Computer_Diskette_b32;
                _cButtonSave.ToolTipText = "[ Ctrl + A ] " + elmApplication.__oTunes.__mTranslate("Сохранить");
                _cButtonSave.__eClickLeft += _mButtonSave_MouseClickLeft;
            }

            SplitterDistance = _cHeaderPicture.Top + _cHeaderPicture.Height + elmInterface.__fIntervalVertical * 2;

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
            __mLoad();
        }

        #endregion Объект

        #region = МЕТОДЫ

        #region - Процедуры

        /// <summary>
        /// Загрузка настроек приложения из файла
        /// </summary>
        /// <returns>[true] - Настройки загружены, иначе - [false]</returns>
        public bool __mLoad()
        { 
            bool vReturn = false; // Возвращаемое значение

            for(int fTuneIndex = 0; fTuneIndex < elmApplication.__oTunes.__fTunesCount_; fTuneIndex++) 
            {
                appUnitTune vUnitTune = elmApplication.__oTunes.__mTuneByIndex(fTuneIndex);
                switch (vUnitTune.__fObjectForEdit)
                {
                    case "elmInputBool":
                        elmInputBool vInputBool = new elmInputBool();
                        vInputBool.__fCaption_ = vUnitTune.__fDescription;
                        vInputBool.__fTuneIndex = fTuneIndex;
                        _cBlockInputs.__mInputAdd(vInputBool);
                        vInputBool.__fValue_ = Convert.ToBoolean(vUnitTune.__fValue);
                        break;
                    case "elmInputString":
                        elmInputString vInputChar = new elmInputString();
                        vInputChar.__fCaption_ = vUnitTune.__fDescription;
                        vInputChar.__fSymbolsCount_ = -1;
                        vInputChar.__fTuneIndex = fTuneIndex;
                        _cBlockInputs.__mInputAdd(vInputChar);
                        vInputChar.__fValue_ = Convert.ToString(vUnitTune.__fValue);
                        break;
                    //case "elmInputCombo":
                    //    elmInputCombo vInputCombo = new elmInputCombo();
                    //    vInputCombo.__fCaption_ = vUnitTune.__fDescription;
                    //    vInputCombo.__mItemsAddFromList(vUnitTune.__fValueList);
                    //    vInputCombo.__fTuneIndex = fTuneIndex;
                    //    vInputCombo.__mDataRefresh();
                    //    _cBlockInputs.__mInputAdd(vInputCombo);
                    //    vInputCombo.__fValue_ = vUnitTune.__fValue;
                    //    break;
                }
            }

            return vReturn;
        }
        /// <summary>
        /// Сохранение настроек в файл
        /// </summary>
        /// <returns>[true] - Настройки сохранены, иначе - [false]</returns>
        public bool __mSave()
        { 
            bool vReturn = true; // Возвращаемое значение

            int vTuneIndex = 0;

            foreach (elmInput vInput in _cBlockInputs.Controls)
            {
                if (vInput is elmInputBool)
                {
                    vTuneIndex = (vInput as elmInputBool).__fTuneIndex;
                    vReturn = vReturn & elmApplication.__oTunes.__mTuneWrite(vTuneIndex, vInput.__fValue_);
                }
                if (vInput is elmInputString)
                {
                    vTuneIndex = (vInput as elmInputString).__fTuneIndex;
                    vReturn = vReturn & elmApplication.__oTunes.__mTuneWrite(vTuneIndex, vInput.__fValue_);
                }
                if (vInput is elmInputCombo)
                {
                    vTuneIndex = (vInput as elmInputCombo).__fTuneIndex;
                    vReturn = vReturn & elmApplication.__oTunes.__mTuneWrite(vTuneIndex, vInput.__fValue_);
                }
                if (vInput is elmInputPath)
                {
                    vTuneIndex = (vInput as elmInputPath).__fTuneIndex;
                    vReturn = vReturn & elmApplication.__oTunes.__mTuneWrite(vTuneIndex, vInput.__fValue_);
                }
            }

            if (vReturn == true)
                (FindForm() as elmForm).Close();
            else
                elmApplication.__oMessages.__mShow(MESSAGESTYPES.Error, "Не удалось сохранить настройки");

            return vReturn;
        }

        #endregion Процедуры

        #region - События

        /// <summary>
        /// Выпоняется при выборе кнопки 'Сохранить' левой кнопкой мыши
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _mButtonSave_MouseClickLeft(object sender, EventArgs e)
        {
            __mSave();
        }

        #endregion События

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Компоненты

        /// <summary>
        /// Блок для размещения полей ввода
        /// </summary>
        protected elmBlockInputs _cBlockInputs = new elmBlockInputs();
        /// <summary>
        /// Кнопка 'Сохранить'
        /// </summary>
        protected elmComponentToolbarButton _cButtonSave = new elmComponentToolbarButton();

        #endregion Компоненты

        #endregion ПОЛЯ
    }
}
