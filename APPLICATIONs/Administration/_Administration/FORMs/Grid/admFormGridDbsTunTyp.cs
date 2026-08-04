using nlApplication;
using nlResourcesImages;
using nlElements;
using System;
using nlAdministration;

namespace nlAdministration
{
	/// <summary>
	/// Файл admFormGridDbsTunTyp.cs
	/// </summary>
	/// <remarks>Класс формы для правки сущности 'Виды настроек базы данных'</remarks>
	/// <design>Код класса сгенерирован программой 'CS Designer'</design>
	public class admFormGridDbsTunTyp : elmFormGrid
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

			#region /// Настройка компонентов

			__fCaption_ = "Виды настройки базы данных";
			_fHelpTopic = "";

			// __cAreaGrid
			{
				__cAreaGrid.__fButtonEditCopyVisible_ = false;
				__cAreaGrid.__oEssence_ = new admEssenceDbsTunTyp();
				__cAreaGrid.__oFormFilter = typeof(admFormFilterDbsTunTyp);
				__cAreaGrid.__oFormOpened = typeof(admFormRecordDbsTunTyp);
				__cAreaGrid.__fFormOpenedType = CONTROLsOPENEDTYPES.FormRecord;

				#region Сетка / Определение колонок

				__cAreaGrid.__mColumnAdd(elmApplication.__oTunes.__mTranslate("Запись: Ключ")
					, elmApplication.__oTunes.__mTranslate("Ключ записи в таблице") + "."
					, "CLU"
					, true
					, false
					, DATAGRIDCOLUMNTYPE.DataGridViewTextBoxColumn);
				__cAreaGrid.__mColumnAdd(elmApplication.__oTunes.__mTranslate("Запись: Правка")
					, elmApplication.__oTunes.__mTranslate("Время последнего изменения записи") + "."
					, "CHG"
					, true
					, false
					, DATAGRIDCOLUMNTYPE.DataGridViewTextBoxColumn);
				__cAreaGrid.__mColumnAdd(elmApplication.__oTunes.__mTranslate("Запись: Исключена")
					, elmApplication.__oTunes.__mTranslate("Метка об исключении записи") + "."
					, "ELD"
					, true
					, false
					, DATAGRIDCOLUMNTYPE.DataGridViewCheckBoxColumn);
				__cAreaGrid.__mColumnAdd(elmApplication.__oTunes.__mTranslate("Запись: Идентификатор")
					, elmApplication.__oTunes.__mTranslate("Уникальный идентификатор записи данных") + "."
					, "GID"
					, true
					, false
					, DATAGRIDCOLUMNTYPE.DataGridViewTextBoxColumn);
				__cAreaGrid.__mColumnAdd(elmApplication.__oTunes.__mTranslate("Сортировка")
					, ""
					, "cgzDbsTunTyp"
					, true
					, true
					, DATAGRIDCOLUMNTYPE.DataGridViewTextBoxColumn);
				__cAreaGrid.__mColumnAdd(elmApplication.__oTunes.__mTranslate("Название")
					, ""
					, "dsiDbsTunTyp"
					, true
					, true
					, DATAGRIDCOLUMNTYPE.DataGridViewTextBoxColumn);

				__cAreaGrid.__mGridBuild();

				#endregion Сетка / Определение колонок
			}

			#endregion /// Настройка компонентов

			#region /// Определение прав пользователей

			/// Назначение сущностей ролей пользователей для работы с правами
			__oEssenceRights = new admEssenceRht();
			__oEssenceUsersRoles = new admEssenceUsrRol();
			/// Определение права доступа к кнопка 'Операции', меню 'Определение прав пользователей'
			__cAreaGrid.__fButtonOperationsEnabled_ = admApplication.__oData.__mDataSourceGet(admApplication.__oData.__fDataSourceCurrentAlias).__fUserAdministrator;
			if (__cAreaGrid.__fButtonOperationsEnabled_ == false)
			{
				__cAreaGrid.__fButtonRefreshEnabled_ = (__oEssenceRights as admEssenceRht).__mGetRight(nButtonRefresh, Name);
				__cAreaGrid.__fButtonEditCreateEnabled_ = (__oEssenceRights as admEssenceRht).__mGetRight(nButtonEditCreate, Name);
				__cAreaGrid.__fButtonEditEditEnabled_ = (__oEssenceRights as admEssenceRht).__mGetRight(nButtonEditEdit, Name);
				__cAreaGrid.__fButtonEditRemoveEnabled_ = (__oEssenceRights as admEssenceRht).__mGetRight(nButtonEditRemove, Name);
				__cAreaGrid.__fButtonEditRestoreEnabled_ = (__oEssenceRights as admEssenceRht).__mGetRight(nButtonEditRestore, Name);
				__cAreaGrid.__fButtonReportsCurrentListEnabled_ = (__oEssenceRights as admEssenceRht).__mGetRight(nButtonReportsCurrentList, Name);
				__cAreaGrid.__fButtonReportsHistoryEnabled_ = (__oEssenceRights as admEssenceRht).__mGetRight(nButtonReportsEditHistory, Name);
			}
			else
			{
				__fRightsList.Add(nButtonRefresh);
				__fRightsList.Add(nButtonEditCreate);
				__fRightsList.Add(nButtonEditEdit);
				__fRightsList.Add(nButtonEditRemove);
				__fRightsList.Add(nButtonEditRestore);
				__fRightsList.Add(nButtonReportsCurrentList);
				__fRightsList.Add(nButtonReportsEditHistory);
			}
			#endregion Определение прав пользователей

			ResumeLayout(false);

			return;
		}

		#endregion Объект

		#endregion МЕТОДЫ

		#region = ПОЛЯ

		#region - Константы

		#region Права пользователей

		private string nButtonRefresh = admApplication.__oTunes.__mTranslate("Кнопка `Обновить`");
		private string nButtonEditCopy = admApplication.__oTunes.__mTranslate("Кнопка `Правка` меню `Копировать`");
		private string nButtonEditCreate = admApplication.__oTunes.__mTranslate("Кнопка `Правка` меню `Создать`");
		private string nButtonEditEdit = admApplication.__oTunes.__mTranslate("Кнопка `Правка` меню `Изменить`");
		private string nButtonEditRemove = admApplication.__oTunes.__mTranslate("Кнопка `Правка` меню `Удалить`");
		private string nButtonEditRestore = admApplication.__oTunes.__mTranslate("Кнопка `Правка` меню `Восстановить`");
		private string nButtonReportsCurrentList = admApplication.__oTunes.__mTranslate("Кнопка `Отчеты` меню `Текущий список`");
		private string nButtonReportsEditHistory = admApplication.__oTunes.__mTranslate("Кнопка `Отчеты` меню `История корректировок`");

		#endregion Права пользователей

		#endregion Константы

		#endregion ПОЛЯ
	}
}

