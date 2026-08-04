using nlElements;
using nlResourcesImages;
using System;

namespace nlAdministration
{
	/// <summary>
	/// Файл admFormFilterUsr.cs
	/// </summary>
	/// <remarks>Класс формы для построения фильтра 'Пользователи'</remarks>
	/// <design>Код класса сгенерирован программой 'CS Designer'</design>
	public class admFormFilterUsr : elmFormFilter
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

			__cAreaFilter.__mInputAdd(null);
			__cAreaFilter.__mInputAdd(_cInputELD);

			#endregion Размещение компонентов

			#region /// Настройка компонентов

			__fCaption_ = "Фильтр пользователей";
			_fHelpTopic = "";
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
		/// Поле ввода 'Запись: Исключена'
		///</summary>
		protected elmInputBool _cInputELD = new elmInputBool();

		#endregion Компоненты

		#endregion ПОЛЯ
	}
}

