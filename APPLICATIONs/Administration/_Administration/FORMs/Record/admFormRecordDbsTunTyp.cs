using nlApplication;
using nlElements;
using nlResourcesImages;

namespace nlAdministration
{
	/// <summary>
	/// Файл admFormRecordDbsTunTyp.cs
	/// </summary>
	/// <remarks>Класс формы для изменения записи 'Виды настроек базы данных'</remarks>
	/// <design>Код класса сгенерирован программой 'CS Designer'</design>
	public class admFormRecordDbsTunTyp : elmFormRecord
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

				__cAreaRecord.__mInputAdd(_cInputAss);
				__cAreaRecord.__mInputAdd(_cInputDsiDbsTunTyp);
				__cAreaRecord.__mInputAdd(null);
				__cAreaRecord.__mInputAdd(_cInputELD);

			#endregion Размещение компонентов

			#region /// Настройка компонентов

			__fCaption_ = "Вид настроек базы данных";
			_fHelpTopic = "";

			//__cAreaRecord
			{
				__cAreaRecord.__oEssence = new admEssenceDbsTunTyp();

				// _cInputAss
				{
					_cInputAss.__fCaption_ = elmApplication.__oTunes.__mTranslate("Сортировка");
					_cInputAss.__fFieldName = "cgzDbsTunTyp";
					_cInputAss.__fFillType_ = FILLTYPES.Necessarily;
					_cInputAss.__fPartInt_ = 3;
					_cInputAss.__fValueMaximum_ = 999;
				}
				// _cInputDsiDbsTunTyp
				{
					_cInputDsiDbsTunTyp.__fCaption_ = elmApplication.__oTunes.__mTranslate("Название");
					_cInputDsiDbsTunTyp.__fFieldName = "dsiDbsTunTyp";
					_cInputDsiDbsTunTyp.__fFillType_ = FILLTYPES.Necessarily;
					_cInputDsiDbsTunTyp.__fSymbolsCount_ = 30;
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
		protected elmInputInteger _cInputAss = new elmInputInteger();
		///<summary>
		/// Поле ввода 'Название'
		///</summary>
		protected elmInputString _cInputDsiDbsTunTyp = new elmInputString();
		///<summary>
		/// Поле ввода 'Запись: Исключена'
		///</summary>
		protected elmInputBool _cInputELD = new elmInputBool();

		#endregion Компоненты

		#endregion ПОЛЯ
	}
}

