using nlElements;
using nlResourcesImages;
using System;

namespace nlAdministration
{
	/// <summary>
	/// Файл admFormFilterDbs.cs
	/// </summary>
	/// <remarks>Класс формы для построения фильтра 'Базы данных'</remarks>
	/// <design>Код класса сгенерирован программой 'CS Designer'</design>
	public class admFormFilterDbs : elmFormFilter
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
			__cAreaFilter.__mInputAdd(_cInputDsiDbs);
			__cAreaFilter.__mInputAdd(_cInputMrkMan);
			__cAreaFilter.__mInputAdd(_cInputMrkAdm);
			__cAreaFilter.__mInputAdd(null);
			__cAreaFilter.__mInputAdd(_cInputELD);

			#endregion Размещение компонентов

			#region /// Настройка компонентов

			__fCaption_ = "Фильтр баз данных";
			_fHelpTopic = "";
			// _cInputInt
			{
				_cInputInt.__fCaption_ = "";
				_cInputInt.__fFieldName = "cgzDbs";
				_cInputInt.__fPartInt_ = 3;
				_cInputInt.__fValueMaximum_ = 999;
			}
			// _cInputDsiDbs
			{
				_cInputDsiDbs.__fCaption_ = "";
				_cInputDsiDbs.__fFieldName = "dsiDbs";
				_cInputDsiDbs.__fSymbolsCount_ = 30;
			}
			//_cInputMrkMan
			{
				_cInputMrkMan.__fCaption_ = "";
				_cInputMrkMan.__fFieldName = "mrkMan";
			}
			//_cInputMrkAdm
			{
				_cInputMrkAdm.__fCaption_ = "";
				_cInputMrkAdm.__fFieldName = "mrkAdm";
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
		protected elmInputString _cInputDsiDbs = new elmInputString();
		///<summary>
		/// Поле ввода 'Метка: Главная база данных'
		///</summary>
		protected elmInputBool _cInputMrkMan = new elmInputBool();
		///<summary>
		/// Поле ввода 'Метка: Административный режим'
		///</summary>
		protected elmInputBool _cInputMrkAdm = new elmInputBool();
		///<summary>
		/// Поле ввода 'Запись: Исключена'
		///</summary>
		protected elmInputBool _cInputELD = new elmInputBool();

		#endregion Компоненты

		#endregion ПОЛЯ
	}
}

