using nlElements;
using nlResourcesImages;
using System;

namespace nlAdministration
{
	/// <summary>
	/// Файл admFormFilterDbsTunTyp.cs
	/// </summary>
	/// <remarks>Класс формы для построения фильтра 'Виды настроек базы данных'</remarks>
	/// <design>Код класса сгенерирован программой 'CS Designer'</design>
	public class admFormFilterDbsTunTyp : elmFormFilter
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


			#region /// Размещение компонентов

			__cAreaFilter.__mInputAdd(_cInputInt);
			__cAreaFilter.__mInputAdd(_cInputDsiDbsTunTyp);
			__cAreaFilter.__mInputAdd(null);
			__cAreaFilter.__mInputAdd(_cInputELD);

			#endregion Размещение компонентов

			#region /// Настройка компонентов

			__fCaption_ = "Фильтр видов настроек баз данных";
			_fHelpTopic = "";
			// _cInputInt
			{
				_cInputInt.__fCaption_ = "";
				_cInputInt.__fFieldName = "cgzDbsTunTyp";
				_cInputInt.__fPartInt_ = 3;
				_cInputInt.__fValueMaximum_ = 999;
			}
			// _cInputDsiDbsTunTyp
			{
				_cInputDsiDbsTunTyp.__fCaption_ = "";
				_cInputDsiDbsTunTyp.__fFieldName = "dsiDbsTunTyp";
				_cInputDsiDbsTunTyp.__fSymbolsCount_ = 30;
			}
			// _cInputELD
			{
				_cInputELD.__fCaption_ = ""; 
				_cInputELD.__fFieldName = "ELD";
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
		protected elmInputInteger _cInputInt = new elmInputInteger();
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

