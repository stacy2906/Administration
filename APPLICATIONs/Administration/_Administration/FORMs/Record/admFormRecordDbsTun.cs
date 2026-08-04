using nlApplication;
using nlElements;
using nlResourcesImages;

namespace nlAdministration
{
	/// <summary>
	/// Файл admFormRecordDbsTun.cs
	/// </summary>
	/// <remarks>Класс формы для изменения записи 'Настройки баз данных'</remarks>
	/// <design>Код класса сгенерирован программой 'CS Designer'</design>
	public class admFormRecordDbsTun : elmFormRecord
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

			__cAreaRecord.__mInputAdd(_cInputLnkDbs);
			__cAreaRecord.__mInputAdd(_cInputLnkDbsTunTyp);
				__cAreaRecord.__mInputAdd(_cInputVal);
				__cAreaRecord.__mInputAdd(null);
				__cAreaRecord.__mInputAdd(_cInputELD);

			#endregion Размещение компонентов

			#region /// Настройка компонентов

			__fCaption_ = "Настройка базы данных";
			_fHelpTopic = "";

			//__cAreaRecord
			{
				__cAreaRecord.__oEssence = new admEssenceDbsTun();

				// _cInputVal
				{
					_cInputLnkDbsTunTyp.__fCaption_ = elmApplication.__oTunes.__mTranslate("Значение");
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
		/// Поле ввода 'Ссылка: База данных'
		///</summary>
		protected admInputDbs _cInputLnkDbs = new admInputDbs();
		///<summary>
		/// Поле ввода 'Ссылка: Вид настройки базы данных'
		///</summary>
		protected admInputDbsTunTyp _cInputLnkDbsTunTyp = new admInputDbsTunTyp();
		///<summary>
		/// Поле ввода 'Значение'
		///</summary>
		protected elmInputString _cInputVal = new elmInputString();
		///<summary>
		/// Поле ввода 'Запись: Исключена'
		///</summary>
		protected elmInputBool _cInputELD = new elmInputBool();

		#endregion Компоненты

		#endregion ПОЛЯ
	}
}

