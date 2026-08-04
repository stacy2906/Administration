using nlData;
using System;

namespace nlAdministration
{
	/// <summary>
	/// Файл admModelTableChgSnd.cs
	/// </summary>
	/// <remarks>Класс - Cущность 'Протокол отправленных данных'</remarks>
	/// <design>Код класса сгенерирован программой 'CS Designer'</design>
	public class admModelTableChgSnd : datUnitModelTable
	{
		#region = МЕТОДЫ

		#region - Поведение

		/// <summary>
		/// Сборка объекта
		/// </summary>
		protected override void _mObjectAssembly()
		{
			base._mObjectAssembly();

			__fDescription = datApplication.__oTunes.__mTranslate("Протокол отправленных данных");
			__oEssence = new admEssenceChgSnd("Main", DELETETYPES.Mark); // Сущность таблицы
			__fName = "ChgSnd";

			return;
		}

		#endregion Поведение

		#region - Процедуры

		/// <summary>
		/// Построение модели таблицы
		/// </summary>
		/// <returns>[true] - модель создана, иначе - [false]</returns>
		public override void __mModelTableBuilding()
		{
			datUnitModelField vField = new datUnitModelField(); // Модель поля таблицы

			/// CLU - Запись: Ключ
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = true;
				vField.__fIsClue = true;
				vField.__fCaption = "Запись: Ключ";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Int;
				vField.__fName = "CLU";
				vField.__fDescription = "Запись: Ключ";
				__fFieldS.Add(vField);
			}

			/// dtmChgRcvRmt - Время: Получение изменений в ДБД из ТБД
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Время: Получение изменений в ДБД из ТБД";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Datetime;
				vField.__fName = "dtmChgRcvRmt";
				vField.__fDescription = "Время: Получение изменений в ДБД из ТБД";
				vField.__fSize = 8;
				__fFieldS.Add(vField);
			}

			/// dtmChgSndCrn - Время: Отправка изменений из ТБД
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Время: Отправка изменений из ТБД";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Datetime;
				vField.__fName = "dtmChgSndCrn";
				vField.__fDescription = "Время: Отправка изменений из ТБД";
				vField.__fSize = 8;
				__fFieldS.Add(vField);
			}

			/// lnkDbs - Ссылка: Дальняя база данных получившая изменения
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Ссылка: Дальняя база данных получившая изменения";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Int;
				vField.__fName = "lnkDbs";
				vField.__fDescription = "Ссылка: Дальняя база данных получившая изменения";
				__fFieldS.Add(vField);
			}

			return;
		}

		#endregion Процедуры

		#endregion МЕТОДЫ
	}
}

