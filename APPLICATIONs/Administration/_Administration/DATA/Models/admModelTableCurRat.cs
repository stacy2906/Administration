using nlData;
using System;

namespace nlAdministration
{
	/// <summary>
	/// Файл admModelTableCurRat.cs
	/// </summary>
	/// <remarks>Класс - Cущность 'Протокол изменения валют'</remarks>
	/// <design>Код класса сгенерирован программой 'CS Designer'</design>
	public class admModelTableCurRat : datUnitModelTable
	{
		#region = МЕТОДЫ

		#region - Поведение

		/// <summary>
		/// Сборка объекта
		/// </summary>
		protected override void _mObjectAssembly()
		{
			base._mObjectAssembly();

			__fDescription = datApplication.__oTunes.__mTranslate("Протокол изменения валют");
			__oEssence = new admEssenceCurRat("Main", DELETETYPES.Mark); // Сущность таблицы
			__fName = "CurRat";

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

			/// dtmCurRat - Время: Изменение
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Время: Изменение";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Datetime;
				vField.__fName = "dtmCurRat";
				vField.__fDescription = "Время: Изменение";
				vField.__fSize = 8;
				__fFieldS.Add(vField);
			}

			/// lnkCur - Ссылка: Валюта
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Ссылка: Валюта";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Int;
				vField.__fName = "lnkCur";
				vField.__fDescription = "Ссылка: Валюта";
				__fFieldS.Add(vField);
			}

			/// lnkUsr - Ссылка: Пользователь выполнивший корректировку
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Ссылка: Пользователь выполнивший корректировку";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Int;
				vField.__fName = "lnkUsr";
				vField.__fDescription = "Ссылка: Пользователь выполнивший корректировку";
				__fFieldS.Add(vField);
			}

			/// CurRatBuy - Новый официальный курс покупки к отчетной валюте
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Новый официальный курс покупки к отчетной валюте";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Numeric;
				vField.__fName = "CurRatBuy";
				vField.__fDescription = "Новый официальный курс покупки к отчетной валюте";
				__fFieldS.Add(vField);
			}

			/// CurRatBuyCmr - Новый коммерческий курс покупки к отчетной валюте
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Новый коммерческий курс покупки к отчетной валюте";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Numeric;
				vField.__fName = "CurRatBuyCmr";
				vField.__fDescription = "Новый коммерческий курс покупки к отчетной валюте";
				__fFieldS.Add(vField);
			}

			/// CurRatSal - Новый официальный курс продажи к отчетной валюте
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Новый официальный курс продажи к отчетной валюте";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Numeric;
				vField.__fName = "CurRatSal";
				vField.__fDescription = "Новый официальный курс продажи к отчетной валюте";
				__fFieldS.Add(vField);
			}

			/// CurRatSalCmr - Новый коммерческий курс продажи к отчетной валюте
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Новый коммерческий курс продажи к отчетной валюте";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Numeric;
				vField.__fName = "CurRatSalCmr";
				vField.__fDescription = "Новый коммерческий курс продажи к отчетной валюте";
				__fFieldS.Add(vField);
			}

			return;
		}

		#endregion Процедуры

		#endregion МЕТОДЫ
	}
}

