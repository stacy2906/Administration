using nlElements;
using nlResourcesImages;
using System;

namespace nlAdministration
{
	/// <summary>
	/// Файл admFormFilterDbsTun.cs
	/// </summary>
	/// <remarks>Класс формы для построения фильтра 'Настройки баз данных'</remarks>
	/// <design>Код класса сгенерирован программой 'CS Designer'</design>
	public class admFormFilterDbsTun : elmFormFilter
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

			__cAreaFilter.__mInputAdd(_cInputVal);
			__cAreaFilter.__mInputAdd(null);
			__cAreaFilter.__mInputAdd(_cInputELD);

			#endregion Размещение компонентов

			#region /// Настройка компонентов

			__fCaption_ = "Фильтр настроек базы данных";
			_fHelpTopic = "";
			// _cInputVal
			{
				_cInputVal.__fCaption_ = "";
				_cInputVal.__fFieldName = "Val";
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

