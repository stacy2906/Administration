using nlApplication;
using nlData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace nlElements
{
    /// <summary>
    /// Файл elmComponentTree.cs
    /// </summary>
    /// <remarks>Класс-компонент для правки древовидных данных</remarks>
    public class elmComponentTree : TreeView
    {
        #region = ДИЗАЙНЕРЫ

        /// <summary>
        /// Конструктор
        /// </summary>
        public elmComponentTree()
        {
            _mObjectAssembly();
        }
        protected virtual void _mObjectAssembly()
        {
            __mImageListLoad();
            //BeforeSelect += mComponentTree_BeforeSelect;
            DrawMode = TreeViewDrawMode.OwnerDrawText;
            DrawNode += ElmComponentTree_DrawNode;
            KeyDown += ElmComponentTree_KeyDown;
        }

        private void ElmComponentTree_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            //// 1-й Вариант
            //// Закрашиваем фон (выделение или стандартный)
            //if ((e.State & TreeNodeStates.Selected) != 0)
            //    e.Graphics.FillRectangle(SystemBrushes.Highlight, e.Bounds);
            //else
            //    e.Graphics.FillRectangle(SystemBrushes.Window, e.Bounds);

            //// Проверяем текст узла
            //if (e.Node.Text.Trim() == @"\-")  // для узлов с текстом "\-"
            //{
            //    // Рисуем линию
            //    using (Pen pen = new Pen(Color.Gray, 1))
            //    {
            //        // Можно использовать 2 вариант
            //        //+int y = e.Bounds.Top + e.Bounds.Height / 2;
            //        //+e.Graphics.DrawLine(pen, e.Bounds.Left, y, e.Bounds.Right, y);

            //        int xStart = e.Bounds.Left; // отступ под иконку
            //        int xEnd = ClientSize.Width; // до конца TreeView
            //        int y = e.Bounds.Top + e.Bounds.Height / 2;
            //        e.Graphics.DrawLine(pen, xStart, y, xEnd, y);
            //    }
            //}
            //else
            //{
            //    // Рисуем обычный текст
            //    TextRenderer.DrawText(e.Graphics, e.Node.Text, this.Font, e.Bounds,
            //                          ((e.State & TreeNodeStates.Selected) != 0) ? SystemColors.HighlightText : SystemColors.WindowText,
            //                          TextFormatFlags.Left);
            //}

            e.DrawDefault = false; // мы сами рисуем

            // --- Фон ---
            if ((e.State & TreeNodeStates.Selected) != 0)
                e.Graphics.FillRectangle(SystemBrushes.Highlight, e.Bounds);
            else
                e.Graphics.FillRectangle(SystemBrushes.Window, e.Bounds);

            // --- Линия вместо текста ---
            if (e.Node.Text.Trim() == @"\-")
            {
                using (Pen pen = new Pen(Color.Black, 1))
                {
                    // Смещение от левой границы с учётом иконки и отступа
                    int xStart = e.Bounds.Left;
                    if (this.ImageList != null && e.Node.ImageIndex >= 0)
                        xStart += this.ImageList.ImageSize.Width + 2;

                    int xEnd = this.ClientSize.Width - 40; // до конца видимой области
                    int y = e.Bounds.Top + e.Bounds.Height / 2; // по вертикали центр узла

                    e.Graphics.DrawLine(pen, xStart, y, xEnd, y);
                }
            }
            else
            {
                // --- Обычный текст ---
                TextRenderer.DrawText(
                    e.Graphics,
                    e.Node.Text,
                    this.Font,
                    e.Bounds,
                    ((e.State & TreeNodeStates.Selected) != 0) ? SystemColors.HighlightText : SystemColors.WindowText,
                    TextFormatFlags.Left
                );
            }
        }

        private void mComponentTree_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            // Проверяем условие (например, по тексту узла или свойству Tag)
            if (e.Node.Text.StartsWith("---") == true)
            {
                e.Cancel = true; // Отменяем выбор
            }
        }
        private void ElmComponentTree_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.SelectedNode == null) return;

            if (e.KeyCode == Keys.Down)
            {
                var next = GetNextVisibleNode(this.SelectedNode);

                while (next != null && ShouldSkip(next))
                    next = GetNextVisibleNode(next);

                if (next != null)
                {
                    this.SelectedNode = next;
                    e.Handled = true;
                }
            }
            else if (e.KeyCode == Keys.Up)
            {
                var prev = GetPrevVisibleNode(this.SelectedNode);

                while (prev != null && ShouldSkip(prev))
                    prev = GetPrevVisibleNode(prev);

                if (prev != null)
                {
                    this.SelectedNode = prev;
                    e.Handled = true;
                }
            }
        }

        #endregion ДИЗАЙНЕРЫ

        #region = МЕТОДЫ

        #region - Поведение

        /// <summary>
        /// Выполняется при отпускании клавиши
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) // Enter
                base.OnDoubleClick(e);

            base.OnKeyUp(e);
        }
        /// <summary>
        /// Выполняется при выборе узла мышью
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNodeMouseClick(TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                if (__eClickRight != null)
                    __eClickRight(this, new EventArgs());
            if (e.Button == MouseButtons.Left)
                if (__eClickLeft != null)
                    __eClickLeft(this, new EventArgs());
            SelectedNode = e.Node;

            base.OnNodeMouseClick(e);
        }

        #endregion Поведение

        TreeNode GetNextVisibleNode(TreeNode node)
        {
            return node.NextVisibleNode;
        }

        TreeNode GetPrevVisibleNode(TreeNode node)
        {
            return node.PrevVisibleNode;
        }
        bool ShouldSkip(TreeNode node)
        {
            // Примеры:

            // 1. По Tag
            if (node.Tag?.ToString() == "skip")
                return true;

            // 2. Например, пропускать узлы без детей
            // if (node.Nodes.Count == 0)
            //     return true;

            // 3. Или каждый второй (пример)
            // return node.Index % 2 == 0;

            return false;
        }

        #region - Процедуры

        #region * Информация о файле

        /// <summary>
        /// Получение пути и имени файла класса
        /// </summary>
        protected string _mClassFilePath([CallerFilePath] string pFilePath = "")
        {
            return pFilePath;
        }
        /// <summary>
        /// Получение значения номера строки в текущей процедуре
        /// </summary>
        protected int _mClassLine(string pMessage = "", [CallerLineNumber] int pLine = 0)
        {
            return pLine;
        }
        /// <summary>
        /// Получение названия текущей процедуры
        /// </summary>
        protected string _mClassProcedure(string message, [CallerMemberName] string pMember = "")
        {
            return pMember;
        }

        #endregion Информация о файле

        /// <summary>
        /// Загрузка данных используя сущность данных и состояния развернутости дерева
        /// </summary>
        /// <param name="pWhereExpresion">Условие отбора данных</param>
        /// <param name="pRefreshData">Признак обновления данных</param>
        /// <returns>[true] - данные загружены без ошибок, иначе - [false]</returns>
        public bool __mDataLoad(string pWhereExpresion = "", bool pRefreshData = false)
        {
            DataTable vDataTable; // Таблица с данными
            elmUnitTreeNode vTreeNode; // Новый узел
            appFileIni vFileIni = (FindForm() as elmForm).__oFileIni;// Объект для работы с конфигурационными файлами
            string vFormName = FindForm().Name;// Имя формы на которой расположен компонент
            string vWhereExpression = "ELD = 0";
            if (pWhereExpresion != "")
                vWhereExpression += " and " + pWhereExpresion;

            if(pRefreshData == true)
                __mNodesStatusesSave(); // Сохранение состояния развернутости дерева

            if (__oEssence != null)
            {
                vDataTable = __oEssence.__mTree(vWhereExpression);
                Nodes.Clear(); // Очистка дерева от узлов. !!! Вниз не перемещать
                _fNodeListOnLoad.Clear(); // Очистка списка добавленных узлов
                {
                    SuspendLayout();
                    /// Основное заполнение дерева
                    foreach (DataRow vDataRow in vDataTable.Rows)
                    {
                        vTreeNode = new elmUnitTreeNode();
                        vTreeNode.__fNodeFolder = true;
                        vTreeNode.__fClue = Convert.ToInt32(vDataRow["CLU"]);
                        vTreeNode.__fClueParent = Convert.ToInt32(vDataRow["lnk" + __oEssence.__fTableName]);
                        vTreeNode.__fSort = Convert.ToInt32(vDataRow["cgz" + __oEssence.__fTableName]);
                        vTreeNode.Tag = vDataRow["CLU"].ToString();
                        vTreeNode.Text = vDataRow["dsi" + __oEssence.__fTableName].ToString().Trim() + new String(' ', 5);
                        if (vDataTable.Columns.Contains("dpn" + __oEssence.__fTableName) == true) // Если в таблице существует поле описания
                            vTreeNode.__fDescription = vDataRow["dpn" + __oEssence.__fTableName].ToString().Trim();
                        if (vDataTable.Columns.Contains("StrImgNam") == true) // Если в таблице существует поле статусного изображения
                            vTreeNode.ImageKey = vDataRow["StrImgNam"].ToString().Trim();
                        if (vTreeNode.__fClueParent == 0) // Узел верхнего уровня
                            __mNodeNew(vTreeNode);
                        else
                            __mNodeSupply(__mNodeGetByClueOnLoad(vTreeNode.__fClueParent), vTreeNode);
                        vTreeNode.Collapse();
                    }
                    /// Добавление пропущенных узлов (из-за порядка связанного с добавлением узлов в базу данных)
                    foreach (DataRow vDataRow in vDataTable.Rows)
                    {
                        vTreeNode = new elmUnitTreeNode();
                        vTreeNode.__fNodeFolder = true;
                        vTreeNode.__fClue = Convert.ToInt32(vDataRow["CLU"]);
                        vTreeNode.__fClueParent = Convert.ToInt32(vDataRow["lnk" + __oEssence.__fTableName]);
                        vTreeNode.__fSort = Convert.ToInt32(vDataRow["cgz" + __oEssence.__fTableName]);
                        vTreeNode.Tag = vDataRow["CLU"].ToString();
                        vTreeNode.Text = vDataRow["dsi" + __oEssence.__fTableName].ToString().Trim() + new String(' ', 5);
                        if (vDataTable.Columns.Contains("dpn" + __oEssence.__fTableName) == true) // Если в таблице существует поле описания
                            vTreeNode.__fDescription = vDataRow["dpn" + __oEssence.__fTableName].ToString().Trim();
                        if (vDataTable.Columns.Contains("StrImgNam") == true) // Если в таблице существует поле статусного изображения
                            vTreeNode.ImageKey = vDataRow["StrImgNam"].ToString().Trim();
                        if (vTreeNode.__fClueParent != 0) // Узел верхнего уровня
                                                          //__mNodeNew(vTreeNode);
                                                          //else
                        {
                            if (vTreeNode.__fClueParent > 0)
                            {
                                if (__mNodeGetByClueOnLoad(vTreeNode.__fClueParent) != null 
                                    & __mNodeGetByClueOnLoad(vTreeNode.__fClue) == null)
                                        __mNodeSupply(__mNodeGetByClueOnLoad(vTreeNode.__fClueParent), vTreeNode);
                            }
                        }
                        vTreeNode.Collapse();
                    }
                    __mNodesStatusesLoad(); // Загрузка состояния развернутости дерева

                    ResumeLayout(false);

                    return true;
                }
            }
            else
            {
                _fError.__mMessageBuild("Источник данных не определен");
                _fError.__fProcedure_ = _fClassProcedure_;
                _fError.__fErrorType_ = ERRORSTYPES.Programming;
                appApplication.__oErrorsHandler.__mShow(_fError);
                _fError.__mClear();

                return false;
            }
        }
        /// <summary>
        /// Загрузка состояния развернутости узлов
        /// </summary>
        public bool __mNodesStatusesLoad()
        {
            bool vReturn = true;
            appFileIni vFileIni = (FindForm() as elmForm).__oFileIni; // Объект для работы с конфигурационными файлами
            string vFormName = (FindForm() as elmForm).Name; // Имя формы на которой расположен контрол
            List<elmUnitTreeNode> vNodeList = __mNodeListFull();
            string vSelectedNodeInfo = vFileIni.__mValueRead(vFormName, "NodeSelected");
            int vSelectedClue = 0;
            if (vSelectedNodeInfo.Contains(",") == true)
                vSelectedClue = Convert.ToInt32(appTypeString.__mWordNumberComma(vSelectedNodeInfo, 0)); // Уровень вложенности узла

            /// Чтение из файла и разворачивание узлов
            foreach (elmUnitTreeNode vTreeNode in vNodeList)
            {
                //string vIniValue = vFileIni.__mValueRead(vFormName, "NodeExpanded_" + vTreeNode.Level.ToString() + "_" + vTreeNode.Index.ToString());
                string vIniValue = vFileIni.__mValueRead(vFormName, "NodeExpanded_" + vTreeNode.__fClue);
                if (vIniValue.Trim().ToUpper() == "TRUE" & vTreeNode.IsVisible == true)
                {
                    vTreeNode.Expand();
                }
                /// Выбор узла
                if (vTreeNode.__fClue == vSelectedClue)
                    SelectedNode = vTreeNode;
            }

            return vReturn;
        }
        /// <summary>
        /// Сохранение состояния развернутости дерева
        /// </summary>
        /// <returns>[true] - данные сохранены без ошибок, иначе - [false]</returns>
        public bool __mNodesStatusesSave()
        {
            bool vReturn = true;
            appFileIni vFileIni = (FindForm() as elmForm).__oFileIni; // Объект для работы с конфигурационными файлами
            string vFormName = (FindForm() as elmForm).Name; // Имя формы на которой расположен контрол
            /// Удаление хранящейся информации о развернутости узлов
            ArrayList vNodeExpandedList = vFileIni.__mParametersListByMaskStart(vFormName, "NodeExpanded");
            foreach (string vNodeName in vNodeExpandedList)
            {
                vFileIni.__mParameterDelete(vFormName, vNodeName);
            }
            List<elmUnitTreeNode> vNodeList = __mNodeListFull();

            /// Сохранение состояния развернутости узлов
            foreach (elmUnitTreeNode vTreeNode in vNodeList)  
            {
                if(vTreeNode.IsExpanded == true & vTreeNode.IsVisible == true)
                    vFileIni.__mValueWrite("True", vFormName, "NodeExpanded_" + vTreeNode.__fClue.ToString());
                /// Сохранение выбранного узла
                if (vTreeNode.IsSelected == true)
                    vFileIni.__mValueWrite(vTreeNode.__fClue.ToString(), vFormName, "NodeSelected"); // Cохранение состояния фокуса узла
            }

            return vReturn;
        }
        /// <summary>
        /// Создание списка изображений
        /// </summary>
        public void __mImageListLoad()
        {
            ImageList vImageList = new ImageList(); // Список изображений

            /// Загрузка изображений
            {
            }

            if (vImageList.Images.Count > 0)
                ImageList = vImageList;
        }
        /// <summary>
        /// Создание нового узла
        /// </summary>
        /// <param name="pTreeNode">Объект нового узла</param>
        /// <returns>[true] - узел добавлен, иначе - [false]</returns>
        public elmUnitTreeNode __mNodeNew(elmUnitTreeNode pTreeNode)
        {
            /// Подключение к новому узлу контекстного меню, если оно содержит подпункты
            if (_cNodeContextMenu.Items.Count > 0)
                pTreeNode.ContextMenuStrip = _cNodeContextMenu;
            /// Новый узел подключается к дереву
            Nodes.Add(pTreeNode);
            _fNodeListOnLoad.Add(pTreeNode);
            return pTreeNode;
        }
        /// <summary>
        /// Создание нового узла с переводом заголовка на язык пользователя
        /// </summary>
        /// <param name="pCaptionText">Заголовок</param>
        /// <param name="pTag">Содержание тэга</param>
        /// <returns>[true] - Узел добавлен, иначе - [false]</returns>
        public elmUnitTreeNode __mNodeNew(string pCaptionText, string pTag)
        {
            return __mNodeNew(pCaptionText, pTag, elmApplication.__oInterface.__mFont(FONTS.NodeNotEdit), elmApplication.__oInterface.__mColor(COLORS.Text));
        }
        /// <summary>
        /// Создание нового узла с переводом заголовка на язык пользователя
        /// </summary>
        /// <param name="pCaptionText">Заголовок</param>
        /// <param name="pTag">Содержание тэга</param>
        /// <returns>[true] - Узел добавлен, иначе - [false]</returns>
        public elmUnitTreeNode __mNodeNew(string pCaptionText, string pTag, int pImageIndexNormal = 10000, int pImageIndexSelected = 10000)
        {
            return __mNodeNew(pCaptionText, pTag, elmApplication.__oInterface.__mFont(FONTS.NodeNotEdit), elmApplication.__oInterface.__mColor(COLORS.Text), pImageIndexNormal, pImageIndexSelected);
        }
        /// <summary>
        /// Создание нового узла с переводом заголовка на язык пользователя
        /// </summary>
        /// <param name="pCaptionText">Заголовок</param>
        /// <param name="pTag">Содержание тэга</param>
        /// <param name="pImageIndexNormal">Индекс нормального изображения</param>
        /// <param name="pImageIndexSelected">Индекс изображения выбранного узла</param>
        /// <param name="pFont">Шрифт</param>
        /// <param name="pColor">Цвет</param>
        /// <returns>[true] - Узел добавлен, иначе - [false]</returns>
        public elmUnitTreeNode __mNodeNew(string pCaptionText, string pTag, Font pFont, Color pColor, int pImageIndexNormal = 10000, int pImageIndexSelected = 10000)
        {
            elmUnitTreeNode vTreeNode = new elmUnitTreeNode(); // Создаваемый узел

            vTreeNode.Name = "Nod" + Nodes.Count + 1;
            vTreeNode.Text = elmApplication.__oTunes.__mTranslate(pCaptionText) + "  ";
            vTreeNode.Tag = pTag;
            if (pImageIndexNormal >= 0)
                vTreeNode.ImageIndex = pImageIndexNormal;
            if (pImageIndexSelected >= 0)
                vTreeNode.SelectedImageIndex = pImageIndexSelected;
            vTreeNode.NodeFont = pFont;
            vTreeNode.ForeColor = pColor;

            __mNodeNew(vTreeNode);

            return vTreeNode;
        }
        /// <summary>
        /// Добавление вложенного узла в указанный узел
        /// </summary>
        /// <param name="pTreeNodeParent">Родительский узел</param>
        /// <param name="pTreeNode">Добавляемый узел</param>
        /// <returns>[true] - узел добавлен, иначе - [false]</returns>
        public elmUnitTreeNode __mNodeSupply(elmUnitTreeNode pTreeNodeParent, elmUnitTreeNode pTreeNode)
        {
            /// Подключение к новому узлу контекстного меню, если оно содержит подпункты
            if (_cNodeContextMenu.Items.Count > 0)
                pTreeNode.ContextMenuStrip = _cNodeContextMenu;
            /// Новый узел подключается к родительскому узлу 'pTreeNodeParent'
            if (pTreeNodeParent != null)
            {
                pTreeNodeParent.Nodes.Add(pTreeNode);
                _fNodeListOnLoad.Add(pTreeNode);
            }
            return pTreeNode;
        }
        /// <summary>
        /// Создание вложенного узла с переводом заголовка на язык пользователя
        /// </summary>
        /// <param name="pCaptionText">Заголовок</param>
        /// <param name="pTag">Содержание тэга</param>
        /// <returns>[true] - Узел добавлен, иначе - [false]</returns>
        public elmUnitTreeNode __mNodeSupply(elmUnitTreeNode pTreeNodeParent, string pCaptionText, string pTag)
        {
            return __mNodeSupply(pTreeNodeParent, pCaptionText, pTag, elmApplication.__oInterface.__mFont(FONTS.NodeNotEdit), elmApplication.__oInterface.__mColor(COLORS.Text));
        }
        /// <summary>
        /// Создание вложенного узла с переводом заголовка на язык пользователя
        /// </summary>
        /// <param name="pTreeNodeParent">Родительский узел</param>
        /// <param name="pCaptionText">Заголовок</param>
        /// <param name="pTag">Содержание тэга</param>
        /// <param name="pImageIndexNormal">Индекс нормального изображения</param>
        /// <param name="pImageIndexSelected">Индекс изображения выбранного узла</param>
        /// <returns>[true] - Узел добавлен, иначе - [false]</returns>
        public elmUnitTreeNode __mNodeSupply(elmUnitTreeNode pTreeNodeParent, string pCaptionText, string pTag, int pImageIndexNormal = 10000, int pImageIndexSelected = 10000)
        {
            return __mNodeSupply(pTreeNodeParent, pCaptionText, pTag, elmApplication.__oInterface.__mFont(FONTS.NodeNotEdit), elmApplication.__oInterface.__mColor(COLORS.Text), pImageIndexNormal, pImageIndexSelected);
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
            elmUnitTreeNode vTreeNode = new elmUnitTreeNode(); // Создаваемый узел

            vTreeNode.Name = "Nod" + Nodes.Count + 1;
            vTreeNode.Text = elmApplication.__oTunes.__mTranslate(pCaptionText) + "  ";
            vTreeNode.Tag = pTag;
            if (pImageIndexNormal >= 0)
                vTreeNode.ImageIndex = pImageIndexNormal;
            if (pImageIndexSelected >= 0)
                vTreeNode.SelectedImageIndex = pImageIndexSelected;
            vTreeNode.NodeFont = pFont;
            vTreeNode.ForeColor = pColor;

            __mNodeSupply(pTreeNodeParent, vTreeNode);

            return vTreeNode;
        }
        /// <summary>
        /// Получение узла дерева по идентификатору записи
        /// </summary>
        /// <param name="pClue">Идентификатор записи</param>
        public elmUnitTreeNode __mNodeGetByClueOnLoad(int pClue)
        {
            elmUnitTreeNode vReturn = null; // Возвращаемое значение

            foreach (elmUnitTreeNode vTreeNode in this._fNodeListOnLoad)
            {
                if (vTreeNode.__fNodeService != true)
                {
                    if (vTreeNode.__fClue == pClue)
                    {
                        vReturn = vTreeNode as elmUnitTreeNode;
                        break;
                    }
                }
            }

            return vReturn;
        }
        /// <summary>
        /// Получение списка вложенных узлов включая вложенные для узла 'prNode'
        /// </summary>
        /// <remarks>Если [ prNodeChldListClea ] = true, список '__NodeChildList' очищается перед выполнением метода</remarks>
        /// <param name="prNode">Узел дерева</param>
        /// <param name="prNodeChldListClea">Указание очищать список узлов '__NodeChildList'</param>
        public virtual List<elmUnitTreeNode> __mNodeList(elmUnitTreeNode prNode, bool prNodeChldListClea)
        {
            if (prNodeChldListClea == true)
                fNodeChildList.Clear();
            TreeNodeCollection vrNodeClct = prNode.Nodes;
            foreach (elmUnitTreeNode vrNode in vrNodeClct)
            {
                fNodeChildList.Add(vrNode);
                __mNodeList(vrNode, false); // Чтение вложенных узлов
            }
            return fNodeChildList;
        }
        /// <summary>
        /// Получение списка узлов дерева, включая вложенные
        /// </summary>
        /// <param name="prNode">Узел дерева</param>
        public virtual List<elmUnitTreeNode> __mNodeListFull()
        {
            fNodeChildList.Clear();
            TreeNodeCollection vrNodeClct = this.Nodes;
            foreach (elmUnitTreeNode vrNode in vrNodeClct)
            {
                fNodeChildList.Add(vrNode);
                __mNodeList(vrNode, false); // Чтение вложенных узлов
            }
            return fNodeChildList;
        }
        /// <summary>
        /// Получение списка выбранных узлов
        /// </summary>
        public List<elmUnitTreeNode> __mNodeListMark()
        {
            List <elmUnitTreeNode> vTreeNodeMarked = new List<elmUnitTreeNode>(); // Список помеченных узлов
            foreach (elmUnitTreeNode vTreeNode in __mNodeListFull())
            {
                if(vTreeNode.Checked == true)
                    vTreeNodeMarked.Add(vTreeNode);
            }

            return vTreeNodeMarked;
        }
        /// <summary>
        /// Добавление пункта в контекстное меню 
        /// </summary>
        /// <param name="pToolStripMenuItem">Пункт контекстного меню</param>
        public void __mContextItemAdd(ToolStripMenuItem pToolStripMenuItem)
        {
            if(pToolStripMenuItem == null)
                _cNodeContextMenu.Items.Add("-");
            else
                _cNodeContextMenu.Items.Add(pToolStripMenuItem);
        }

        #endregion Процедуры

        #endregion МЕТОДЫ

        #region = ПОЛЯ

        #region - Закрытые

        /// <summary>
        /// Список узлов прочитанный с дерева при выполнении методов [ _NodeFullList ]
        /// </summary>
        /// <remarks>Переменная класса, т.к. нужен одновременный доступ из методов [ _NodeFullList ] и [ _NodeList ], которвые работают рекурсивно</remarks>
        private List<elmUnitTreeNode> fNodeChildList = new List<elmUnitTreeNode>();

        #endregion Закрытые

        #region - Компоненты

        /// <summary>
        /// Контекстное меню
        /// </summary>
        /// <remarks>Вызывается только для выбранного узла</remarks>
        protected elmComponentMenuContext _cNodeContextMenu = new elmComponentMenuContext();

        #endregion Компоненты

        #region - Объекты

        /// <summary>
        /// Сушность данных
        /// </summary>
        public datUnitEssence __oEssence;

        #endregion Объекты

        #region - Скрытые 

        /// <summary>
        /// Объект ошибки
        /// </summary>
        protected appUnitError _fError;
        /// <summary>
        /// Список загружаемых узлов в компоненте во время загрузки данных из источника данных, для поиска родительских узлов
        /// Очищается после загрузки данных из источника данных
        /// </summary>
        /// <remarks></remarks>
        protected List<elmUnitTreeNode> _fNodeListOnLoad = new List<elmUnitTreeNode>();

        #endregion Скрытые

        #endregion ПОЛЯ

        #region = СВОЙСТВА 

        #region - Скрытые

        /// <summary>
        /// Путь к файлу текущего класса
        /// </summary>
        protected string _fClassFilePath_
        {
            get { return _mClassFilePath(); }
        }
        /// <summary>
        /// Текущая процедура класса
        /// </summary>
        protected string _fClassProcedure_
        {
            get { return _mClassProcedure(""); }
        }
        /// <summary>
        /// Номер строки в файле класса
        /// </summary>
        protected int _fClassLine_
        {
            get { return _mClassLine(""); }
        }

        #endregion Скрытые

        /// <summary>
        /// Ключ текущей выбранной записи
        /// </summary>
        public int __fRecordClue_
        {
            get
            {
                if (SelectedNode != null)
                    return (SelectedNode as elmUnitTreeNode).__fClue;
                else
                    return -1;
            }
        }

        #endregion СВОЙСТВА

        #region = СОБЫТИЯ

        /// <summary>
        /// Возникает при клике левой кнопки мыши по компоненту
        /// </summary>
        public event EventHandler __eClickLeft;
        /// <summary>
        /// Возникает при клике правой кнопки мыши по компоненту
        /// </summary>
        public event EventHandler __eClickRight;

        #endregion СОБЫТИЯ     
    }
}
