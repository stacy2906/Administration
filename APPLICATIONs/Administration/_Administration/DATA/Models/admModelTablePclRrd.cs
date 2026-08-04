using nlData;
using System;

namespace nlAdministration
{
	/// <summary>
	/// Файл admModelTablePclRrd.cs
	/// </summary>
	/// <remarks>Класс - Cущность 'Записи в протоколах'</remarks>
	/// <design>Код класса сгенерирован программой 'CS Designer'</design>
	public class admModelTablePclRrd : datUnitModelTable
	{
		#region = МЕТОДЫ

		#region - Поведение

		/// <summary>
		/// Сборка объекта
		/// </summary>
		protected override void _mObjectAssembly()
		{
			base._mObjectAssembly();

			__fDescription = datApplication.__oTunes.__mTranslate("Записи в протоколах");
			__oEssence = new admEssencePclRrd("Main", DELETETYPES.Mark); // Сущность таблицы
			__fName = "PclRrd";

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

			/// lnkPcl - Ссылка: Протокол
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Ссылка: Протокол";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Int;
				vField.__fName = "lnkPcl";
				vField.__fDescription = "Ссылка: Протокол";
				__fFieldS.Add(vField);
			}

			/// lnkPclRrdTyp - Ссылка: Вид записи в протоколе
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Ссылка: Вид записи в протоколе";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Int;
				vField.__fName = "lnkPclRrdTyp";
				vField.__fDescription = "Ссылка: Вид записи в протоколе";
				__fFieldS.Add(vField);
			}

			/// dsrErr - Описание: Сообщение об ошибке
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Описание: Сообщение об ошибке";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Int;
				vField.__fName = "dsrErr";
				vField.__fDescription = "Описание: Сообщение об ошибке";
				__fFieldS.Add(vField);
			}

			/// dsrExc - Описание: Исключение
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Описание: Исключение";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Int;
				vField.__fName = "dsrExc";
				vField.__fDescription = "Описание: Исключение";
				__fFieldS.Add(vField);
			}

			/// Tck - Время
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Время";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Bigint;
				vField.__fName = "Tck";
				vField.__fDescription = "Время";
				__fFieldS.Add(vField);
			}

			return;
		}

		#endregion Процедуры

		#endregion МЕТОДЫ
	}
}

