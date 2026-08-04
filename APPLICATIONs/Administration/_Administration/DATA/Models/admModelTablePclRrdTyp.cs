using nlData;
using System;

namespace nlAdministration
{
	/// <summary>
	/// Файл admModelTablePclRrdTyp.cs
	/// </summary>
	/// <remarks>Класс - Cущность 'Виды записей в пртоколе'</remarks>
	/// <design>Код класса сгенерирован программой 'CS Designer'</design>
	public class admModelTablePclRrdTyp : datUnitModelTable
	{
		#region = МЕТОДЫ

		#region - Поведение

		/// <summary>
		/// Сборка объекта
		/// </summary>
		protected override void _mObjectAssembly()
		{
			base._mObjectAssembly();

			__fDescription = datApplication.__oTunes.__mTranslate("Виды записей в пртоколе");
			__oEssence = new admEssencePclRrdTyp("Main", DELETETYPES.Mark); // Сущность таблицы
			__fName = "PclRrdTyp";

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

			/// cgzPclRrdTyp - Сортировка
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Сортировка";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Int;
				vField.__fName = "cgzPclRrdTyp";
				vField.__fDescription = "Сортировка";
				__fFieldS.Add(vField);
			}

			/// dsiPclRrdTyp - Название
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Название";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Nchar;
				vField.__fName = "dsiPclRrdTyp";
				vField.__fDescription = "Название";
				__fFieldS.Add(vField);
			}

			/// optAns - Опция: Решениние пользователя
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Опция: Решениние пользователя";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Bit;
				vField.__fName = "optAns";
				vField.__fDescription = "Опция: Решениние пользователя";
				vField.__fSize = 1;
				__fFieldS.Add(vField);
			}

			/// optDtl - Опция: Подробности
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Опция: Подробности";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Bit;
				vField.__fName = "optDtl";
				vField.__fDescription = "Опция: Подробности";
				vField.__fSize = 1;
				__fFieldS.Add(vField);
			}

			/// optExc - Опция: Исключение
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Опция: Исключение";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Bit;
				vField.__fName = "optExc";
				vField.__fDescription = "Опция: Исключение";
				vField.__fSize = 1;
				__fFieldS.Add(vField);
			}

			/// optImg - Опция: Изображение
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Опция: Изображение";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Bit;
				vField.__fName = "optImg";
				vField.__fDescription = "Опция: Изображение";
				vField.__fSize = 1;
				__fFieldS.Add(vField);
			}

			/// optMsg - Опция: Сообщение
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Опция: Сообщение";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Bit;
				vField.__fName = "optMsg";
				vField.__fDescription = "Опция: Сообщение";
				vField.__fSize = 1;
				__fFieldS.Add(vField);
			}

			/// optObjPrp - Опция: Свойства объекта
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Опция: Свойства объекта";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Bit;
				vField.__fName = "optObjPrp";
				vField.__fDescription = "Опция: Свойства объекта";
				vField.__fSize = 1;
				__fFieldS.Add(vField);
			}

			/// optRsn - Опция: Причины ошибок
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Опция: Причины ошибок";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Bit;
				vField.__fName = "optRsn";
				vField.__fDescription = "Опция: Причины ошибок";
				vField.__fSize = 1;
				__fFieldS.Add(vField);
			}

			return;
		}

		#endregion Процедуры

		#endregion МЕТОДЫ
	}
}

