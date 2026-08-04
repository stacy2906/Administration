using nlData;
using System;

namespace nlAdministration
{
	/// <summary>
	/// Файл admModelTablePclTyp.cs
	/// </summary>
	/// <remarks>Класс - Cущность 'Виды протоколов'</remarks>
	/// <design>Код класса сгенерирован программой 'CS Designer'</design>
	public class admModelTablePclTyp : datUnitModelTable
	{
		#region = МЕТОДЫ

		#region - Поведение

		/// <summary>
		/// Сборка объекта
		/// </summary>
		protected override void _mObjectAssembly()
		{
			base._mObjectAssembly();

			__fDescription = datApplication.__oTunes.__mTranslate("Виды протоколов");
			__oEssence = new admEssencePclTyp("Main", DELETETYPES.Mark); // Сущность таблицы
			__fName = "PclTyp";

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

			/// CHG - Запись: Правка
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Запись: Правка";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Datetime;
				vField.__fName = "CHG";
				vField.__fDescription = "Запись: Правка";
				vField.__fSize = 8;
				__fFieldS.Add(vField);
			}

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

			/// ELD - Запись: Исключена
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Запись: Исключена";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Bit;
				vField.__fName = "ELD";
				vField.__fDescription = "Запись: Исключена";
				vField.__fSize = 1;
				__fFieldS.Add(vField);
			}

			/// GID - Запись: Идентификатор
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Запись: Идентификатор";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Uniqueidentifier;
				vField.__fName = "GID";
				vField.__fDescription = "Запись: Идентификатор";
				vField.__fSize = 16;
				__fFieldS.Add(vField);
			}

			/// cgzPclTyp - Сортировка
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Сортировка";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Int;
				vField.__fName = "cgzPclTyp";
				vField.__fDescription = "Сортировка";
				__fFieldS.Add(vField);
			}

			/// dsiPclTyp - Название
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Название";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Nchar;
				vField.__fName = "dsiPclTyp";
				vField.__fDescription = "Название";
				__fFieldS.Add(vField);
			}

			/// optAppErr - Опция: Ошибка приложения
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Опция: Ошибка приложения";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Bit;
				vField.__fName = "optAppErr";
				vField.__fDescription = "Опция: Ошибка приложения";
				vField.__fSize = 1;
				__fFieldS.Add(vField);
			}

			/// optAppExc - Опция: Исключение
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Опция: Исключение";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Bit;
				vField.__fName = "optAppExc";
				vField.__fDescription = "Опция: Исключение";
				vField.__fSize = 1;
				__fFieldS.Add(vField);
			}

			/// optAppErrPrg - Опция: Ошибка программирования
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Опция: Ошибка программирования";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Bit;
				vField.__fName = "optAppErrPrg";
				vField.__fDescription = "Опция: Ошибка программирования";
				vField.__fSize = 1;
				__fFieldS.Add(vField);
			}

			/// optAppEvn - Опция: Событие приложения
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Опция: Событие приложения";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Bit;
				vField.__fName = "optAppEvn";
				vField.__fDescription = "Опция: Событие приложения";
				vField.__fSize = 1;
				__fFieldS.Add(vField);
			}

			/// optDatErr - Опция: Ошибка источника данных
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Опция: Ошибка источника данных";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Bit;
				vField.__fName = "optDatErr";
				vField.__fDescription = "Опция: Ошибка источника данных";
				vField.__fSize = 1;
				__fFieldS.Add(vField);
			}

			/// optDatEvn - Опция: Событие источника данных
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Опция: Событие источника данных";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Bit;
				vField.__fName = "optDatEvn";
				vField.__fDescription = "Опция: Событие источника данных";
				vField.__fSize = 1;
				__fFieldS.Add(vField);
			}

			/// optDevErr - Опция: Ошибка устройства
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Опция: Ошибка устройства";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Bit;
				vField.__fName = "optDevErr";
				vField.__fDescription = "Опция: Ошибка устройства";
				vField.__fSize = 1;
				__fFieldS.Add(vField);
			}

			/// optDevEvn - Опция: Событие устройства
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Опция: Событие устройства";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Bit;
				vField.__fName = "optDevEvn";
				vField.__fDescription = "Опция: Событие устройства";
				vField.__fSize = 1;
				__fFieldS.Add(vField);
			}

			/// optOth - Опция: Прочие
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Опция: Прочие";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Bit;
				vField.__fName = "optOth";
				vField.__fDescription = "Опция: Прочие";
				vField.__fSize = 1;
				__fFieldS.Add(vField);
			}

			/// optUsrErr - Опция: Ошибка пользователя
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Опция: Ошибка пользователя";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Bit;
				vField.__fName = "optUsrErr";
				vField.__fDescription = "Опция: Ошибка пользователя";
				vField.__fSize = 1;
				__fFieldS.Add(vField);
			}

			/// optUsrEvn - Опция: Действия пользователя
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Опция: Действия пользователя";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Bit;
				vField.__fName = "optUsrEvn";
				vField.__fDescription = "Опция: Действия пользователя";
				vField.__fSize = 1;
				__fFieldS.Add(vField);
			}

			/// optUsrMsg - Опция: Сообщения показанные пользователю
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Опция: Сообщения показанные пользователю";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Bit;
				vField.__fName = "optUsrMsg";
				vField.__fDescription = "Опция: Сообщения показанные пользователю";
				vField.__fSize = 1;
				__fFieldS.Add(vField);
			}

			return;
		}

		#endregion Процедуры

		#endregion МЕТОДЫ
	}
}

