using nlData;
using System;

namespace nlAdministration
{
	/// <summary>
	/// Файл admModelTableChgRcv.cs
	/// </summary>
	/// <remarks>Класс - Cущность 'Протокол получения данных'</remarks>
	/// <design>Код класса сгенерирован программой 'CS Designer'</design>
	public class admModelTableChgRcv : datUnitModelTable
	{
		#region = МЕТОДЫ

		#region - Поведение

		/// <summary>
		/// Сборка объекта
		/// </summary>
		protected override void _mObjectAssembly()
		{
			base._mObjectAssembly();

			__fDescription = datApplication.__oTunes.__mTranslate("Протокол получения данных");
			__oEssence = new admEssenceChgRcv("Main", DELETETYPES.Mark); // Сущность таблицы
			__fName = "ChgRcv";

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

			/// dtmChgRcvCrn - Время: Получение изменений в ТБД из ДБД
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Время: Получение изменений в ТБД из ДБД";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Datetime;
				vField.__fName = "dtmChgRcvCrn";
				vField.__fDescription = "Время: Получение изменений в ТБД из ДБД";
				vField.__fSize = 8;
				__fFieldS.Add(vField);
			}

			/// dtmChgSndDst - Время: Отправка изменений в ДБД
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Время: Отправка изменений в ДБД";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Datetime;
				vField.__fName = "dtmChgSndDst";
				vField.__fDescription = "Время: Отправка изменений в ДБД";
				vField.__fSize = 8;
				__fFieldS.Add(vField);
			}

			/// lnkDbs - Ссылка: Дальняя база данных отправившая изменения
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Ссылка: Дальняя база данных отправившая изменения";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Int;
				vField.__fName = "lnkDbs";
				vField.__fDescription = "Ссылка: Дальняя база данных отправившая изменения";
				__fFieldS.Add(vField);
			}

			/// RmtChgSndGid - Идентификатор отправки изменений в ДБД
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Идентификатор отправки изменений в ДБД";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Uniqueidentifier;
				vField.__fName = "RmtChgSndGid";
				vField.__fDescription = "Идентификатор отправки изменений в ДБД";
				__fFieldS.Add(vField);
			}

			return;
		}

		#endregion Процедуры

		#endregion МЕТОДЫ
	}
}

