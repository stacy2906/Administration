using naCsManual;
using naCsProtocols;
using nlAdministration;
using nlApplication;
using nlElements;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace naAdministration
{
    /// <summary>
    /// Файл admFormMain.cs
    /// </summary>
    /// <remarks>Класс главной формы приложения 'Administration'</remarks>
    /// <design>Код класса сгенерирован программой 'CS Designer'</design>
    public class admFormMain : elmForm
    {
        #region = МЕТОДЫ

        #region - Объект

        /// <summary>
        /// Сборка объекта
        /// </summary>
        protected override void _mObjectAssembly()
        {
            base._mObjectAssembly();

            #region /// Размещение компонентов

            Controls.Add(_cBlockFormMain);
            _cBlockFormMain.Controls.Add(_cSplitter);
            _cBlockFormMain.Controls.SetChildIndex(_cSplitter, 0);
            _cSplitter.Panel1.Controls.Add(_cMenuTree);

            #endregion Размещение компонентов

            #region /// Настройки компонентов

            __fCaption_ = admApplication.__fCaption_;
            ShowInTaskbar = true;

            _cBlockFormMain.__eMenuApplicationUserChangeClick += mBlockFormMain_MenuApplicationUserChangeClick;
            _cMenuTree.__eTreeDoubleClick += mMenuTree_eTreeDoubleClick;

            #endregion Настройки компонентов

            ResumeLayout();

            return;
        }
        /// <summary>
        /// Презентация объекта
        /// </summary>
        protected override void _mObjectPresentation()
        {
            base._mObjectPresentation();
            _mMenuLoad();
        }

        #endregion Объект

        #region - Поведение

        /// <summary>
        /// Выполняется перед закрытием приложения
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosing(CancelEventArgs e)
        {
            if (Convert.ToBoolean(admApplication.__oTunes.__mTuneRead("AskForQuit")) == true)
            {
                if (admApplication.__oMessages.__mShow(MESSAGESTYPES.Question, "Закрыть приложение", "", false, "Не показывать в следующий раз", "") == DialogResult.No)
                    e.Cancel = true;
                else
                    base.OnClosing(e);
            }
            return;
        }
        /// <summary>
        /// Выполняется при выборе пункта меню 'Смена пользователя'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void mBlockFormMain_MenuApplicationUserChangeClick(object sender, EventArgs e)
        {
            ////// Вызов формы регистрации пользователя
            //elmFormLogin vFormLogin = new elmFormLogin();
            //vFormLogin.__fDataSourceAlias = admApplication.__oData.__fDataSourceCurrentAlias;
            //vFormLogin.ShowDialog();
            ////// Если регистрация пользователя прошла, удаляем его зависшие блокировки
            //if (vFormLogin.__fRegistered == true)
            //{
            //	/// - Снятие зависших блокировок для зашедшего пользователя}
            //	admApplication.__oData.__mDataSourceGet().__mLockClear();
            //}
            ////// Обновление пользовательского меню
            //_mMenuLoad();
            ////// Изменение видимости меню 'Проверка прав пользователей'
            //_cBlockFormMain.__fMenuApplicationUserChangeVisible_ = admApplication.__oData.__mUserAdministrator();
            return;
        }
        /// <summary>
        /// Выбор пункта пользовательского меню
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mMenuTree_eTreeDoubleClick(object sender, EventArgs e)
        {
            if ((sender as elmComponentTree).SelectedNode != null)
            {
                switch ((sender as elmComponentTree).SelectedNode.Tag.ToString())
                {
                    case "admProtocolsLoad":
                        // ============================================================
                        // ЗАГРУЗКА ПРОТОКОЛОВ В БД
                        // ============================================================
                        try
                        {
                            string dbPath = @"C:\Users\doy\Documents\GitHub\Administration\DATABASEs\Protocols.db"; ;

                            if (!File.Exists(dbPath))
                            {
                                MessageBox.Show($"Файл БД не найден:\n{dbPath}", "Ошибка",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                            }

                            var loader = new CsProtocols.ProtocolsDbLoader(dbPath);
                            loader.LoadAllFromDisk();

                            MessageBox.Show("✅ Протоколы загружены в БД!", "Готово",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // ← ИСПРАВЛЕНО: vForm → vFormProtocols
                            cspFormMain vFormProtocols = new cspFormMain();
                            vFormProtocols.ShowDialog();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Ошибка загрузки: {ex.Message}", "Ошибка",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        break;

                    case "admProtocolsList":
                        // ← ИСПРАВЛЕНО: vForm → vFormProtocolsList
                        cspFormMain vFormProtocolsList = new cspFormMain();
                        vFormProtocolsList.ShowDialog();
                        break;

                    case "admProtocolsCombine":
                        //admFormGridChgTyp vFormGridChgTyp = new admFormGridChgTyp();
                        //vFormGridChgTyp.__cAreaGrid.__fButtonSelectVisible_ = false;
                        //vFormGridChgTyp.__cAreaGrid.__fButtonRefreshVisible_ = admApplication.__oData.__mDataSourceGet().__fUserAdministrator;
                        //vFormGridChgTyp.__cAreaGrid.__fButtonEditEnabled_ = admApplication.__oData.__mDataSourceGet().__fUserAdministrator;
                        //vFormGridChgTyp.__cAreaGrid.__fButtonEditCopyVisible_ = false;
                        //vFormGridChgTyp.__cAreaGrid.__fButtonEditCreateVisible_ = false;
                        //vFormGridChgTyp.ShowDialog();
                        break;

                    case "admDocumentation":
                        using (cmlFormMain vFormManual = new cmlFormMain())
                        {
                            vFormManual.ShowDialog(this);
                        }
                        break;
                }
            }
            return;
        }

        #endregion Поведение

        #region - Процедуры

        /// <summary>
        /// Загрузка меню
        /// </summary>
        protected void _mMenuLoad()
        {
            //_cMenuTree.__fUserAlias_ = admApplication.__oData.__mDataSourceGet().__fUserAlias;
            //_cMenuTree.__fUserRole_ = admApplication.__oData.__mDataSourceGet().__fUserRoleName;

            #region /// Заполнение массива изображений дерева

            ImageList vImageList = new ImageList();

            vImageList.Images.Add(new Bitmap(global::nlResourcesImages.Properties.Resources._Folder_Tree_a16)); // 0
            vImageList.Images.Add(new Bitmap(global::nlResourcesImages.Properties.Resources._Folder_Tree_b16)); // 1
            vImageList.Images.Add(new Bitmap(global::nlResourcesImages.Properties.Resources._Folder_Tree_g16)); // 2
            vImageList.Images.Add(new Bitmap(global::nlResourcesImages.Properties.Resources._Folder_Tree_r16)); // 3
            vImageList.Images.Add(new Bitmap(global::nlResourcesImages.Properties.Resources._Folder_Tree_e16)); // 4

            vImageList.Images.Add(new Bitmap(global::nlResourcesImages.Properties.Resources._Currency_g16)); // 5
            vImageList.Images.Add(new Bitmap(global::nlResourcesImages.Properties.Resources._Shop_r16)); // 6
            vImageList.Images.Add(new Bitmap(global::nlResourcesImages.Properties.Resources._Bank_Card_w16)); // 7
            vImageList.Images.Add(new Bitmap(global::nlResourcesImages.Properties.Resources._Till_e16)); // 8
            vImageList.Images.Add(new Bitmap(global::nlResourcesImages.Properties.Resources._DatabaseGear_e16)); // 9

            vImageList.Images.Add(new Bitmap(global::nlResourcesImages.Properties.Resources._Bank_Terminal_e16)); // 10
            vImageList.Images.Add(new Bitmap(global::nlResourcesImages.Properties.Resources._Barcode_w16)); // 11

            vImageList.Images.Add(new Bitmap(global::nlResourcesImages.Properties.Resources._Calculator_b16)); // 12
            vImageList.Images.Add(new Bitmap(global::nlResourcesImages.Properties.Resources._Lorry_y16)); // 13
            vImageList.Images.Add(new Bitmap(global::nlResourcesImages.Properties.Resources._Cart_Bag_o16)); // 14
            vImageList.Images.Add(new Bitmap(global::nlResourcesImages.Properties.Resources._Cart_e16)); // 15

            vImageList.Images.Add(new Bitmap(global::nlResourcesImages.Properties.Resources._Money_CoinsDelete_y16)); // 16
            vImageList.Images.Add(new Bitmap(global::nlResourcesImages.Properties.Resources._Money_CoinsAdd_y16)); // 17

            vImageList.Images.Add(new Bitmap(global::nlResourcesImages.Properties.Resources._Database_y16)); // 18
            vImageList.Images.Add(new Bitmap(global::nlResourcesImages.Properties.Resources._Database_b16)); // 19
            vImageList.Images.Add(new Bitmap(global::nlResourcesImages.Properties.Resources._Database_g16)); // 20
            vImageList.Images.Add(new Bitmap(global::nlResourcesImages.Properties.Resources._Database_r16)); // 21
            vImageList.Images.Add(new Bitmap(global::nlResourcesImages.Properties.Resources._PersonsGroup_m16)); // 22

            _cMenuTree.__fImagesList_ = vImageList;

            #endregion Заполнение массива изображений дерева

            /// Очистка меню от старых пунктов
            _cMenuTree._mNodesClear();

            elmUnitTreeNode vNodeProtocols = _cMenuTree.__mNodeNew("Протоколы", "", 1, 3);
            {
                elmUnitTreeNode vNodeProtocolsLoad = _cMenuTree.__mNodeSupply(vNodeProtocols, "Загрузить протокол", "admProtocolsLoad", 5, 3);
                elmUnitTreeNode vNodeProtocolsView = _cMenuTree.__mNodeSupply(vNodeProtocols, "Список протоколов", "admProtocolsList", 5, 3);
                elmUnitTreeNode vNodeProtocolsViewSome = _cMenuTree.__mNodeSupply(vNodeProtocols, "Совмещение протоколов", "admProtocolsCombine", 5, 3);
            }
            elmUnitTreeNode vNodeDocuments = _cMenuTree.__mNodeNew("Документирование", "", 1, 3);
            {
                elmUnitTreeNode vNodeDocumentsDocumentation = _cMenuTree.__mNodeSupply(vNodeDocuments, "Документировать проект", "admDocumentation", 5, 3);
            }
        }

        #endregion Процедуры

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Компоненты

        /// <summary>
        /// Блок главного окна приложения
        /// </summary>
        protected elmBlockFormMain _cBlockFormMain = new elmBlockFormMain();
        /// <summary>
        /// Разделители
        /// </summary>
        protected elmComponentSplitter _cSplitter = new elmComponentSplitter();
        /// <summary>
        /// Пользовательское меню
        /// </summary>
        protected elmPanelMenuTree _cMenuTree = new elmPanelMenuTree();

        #endregion Компоненты

        #endregion ПОЛЯ
    }
}