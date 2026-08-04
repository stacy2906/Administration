using nlApplication;
using nlElements;
using nlResourcesImages;

namespace nlAdministration
{
	/// <summary>
	/// Файл admFormRecordUsrRol.cs
	/// </summary>
	/// <remarks>Класс формы для изменения записи 'Роли пользователей'</remarks>
	/// <design>Код класса сгенерирован программой 'CS Designer'</design>
	public class admFormRecordUsrRol : elmFormRecord
	{
		#region = МЕТОДЫ

		#region - Объект

		/// <summary>
		/// Загрузка формы
		/// </summary>
		protected override void _mObjectAssembly()
		{
			base._mObjectAssembly();

			SuspendLayout();

			#region /// Размещение компонентов

				__cAreaRecord.__mInputAdd(_cInputCod);
				__cAreaRecord.__mInputAdd(_cInputDsiUsrRol);
				__cAreaRecord.__mInputAdd(null);
				__cAreaRecord.__mInputAdd(_cInputELD);

			#endregion Размещение компонентов

			#region /// Настройка компонентов

			__fCaption_ = "Роль пользователей";
			_fHelpTopic = "";

			//__cAreaRecord
			{
				__cAreaRecord.__oEssence = new admEssenceUsrRol();

				// _cInputCod
				{
					_cInputCod.__fCaption_ = elmApplication.__oTunes.__mTranslate("Сортировка");
					_cInputCod.__fFieldName = "codUsrRol";
					_cInputCod.__fFillType_ = FILLTYPES.Necessarily;
					_cInputCod.__fPartInt_ = 3;
					_cInputCod.__fValueMaximum_ = 999;
				}
				// _cInputDsiUsrRol
				{
					_cInputDsiUsrRol.__fCaption_ = elmApplication.__oTunes.__mTranslate("Название");
					_cInputDsiUsrRol.__fFieldName = "dsiUsrRol";
					_cInputDsiUsrRol.__fFillType_ = FILLTYPES.Necessarily;
					_cInputDsiUsrRol.__fSymbolsCount_ = 30;
				}
				// _cInputELD
				{
					_cInputELD.__fCaption_ = elmApplication.__oTunes.__mTranslate("Запись: Исключена");
					_cInputELD.__fFieldName = "ELD";
				}

			}

			#endregion Настройка компонентов

			ResumeLayout(false);

			return;
		}

		#endregion Объект

		#endregion МЕТОДЫ

		#region = ПОЛЯ

		#region - Компоненты

		///<summary>
		/// Поле ввода 'Сортировка'
		///</summary>
		protected elmInputInteger _cInputCod = new elmInputInteger();
		///<summary>
		/// Поле ввода 'Название'
		///</summary>
		protected elmInputString _cInputDsiUsrRol = new elmInputString();
		///<summary>
		/// Поле ввода 'Запись: Исключена'
		///</summary>
		protected elmInputBool _cInputELD = new elmInputBool();

		#endregion Компоненты

		#endregion ПОЛЯ
	}
}

