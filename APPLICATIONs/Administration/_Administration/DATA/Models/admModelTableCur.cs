using nlData;
using System;

namespace nlAdministration
{
	/// <summary>
	/// Файл admModelTableCur.cs
	/// </summary>
	/// <remarks>Класс - Cущность 'Валюты'</remarks>
	/// <design>Код класса сгенерирован программой 'CS Designer'</design>
	public class admModelTableCur : datUnitModelTable
	{
		#region = МЕТОДЫ

		#region - Поведение

		/// <summary>
		/// Сборка объекта
		/// </summary>
		protected override void _mObjectAssembly()
		{
			base._mObjectAssembly();

			__fDescription = datApplication.__oTunes.__mTranslate("Валюты");
			__oEssence = new admEssenceCur("Main", DELETETYPES.Mark); // Сущность таблицы
			__fName = "Cur";

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

			/// codCur - Сортировка
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Сортировка";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Int;
				vField.__fName = "codCur";
				vField.__fDescription = "Сортировка";
				__fFieldS.Add(vField);
			}

			/// dsiCur - Название
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Название";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Nchar;
				vField.__fName = "dsiCur";
				vField.__fDescription = "Название";
				__fFieldS.Add(vField);
			}

			/// mrkNtl - Метка: Национальная валюта
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Метка: Национальная валюта";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Bit;
				vField.__fName = "mrkNtl";
				vField.__fDescription = "Метка: Национальная валюта";
				vField.__fSize = 1;
				__fFieldS.Add(vField);
			}

			/// mrkRrt - Метка: Отчетная валюта
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Метка: Отчетная валюта";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Bit;
				vField.__fName = "mrkRrt";
				vField.__fDescription = "Метка: Отчетная валюта";
				vField.__fSize = 1;
				__fFieldS.Add(vField);
			}

			/// BnkCurCod - Банковский код валюты
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Банковский код валюты";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Nchar;
				vField.__fName = "BnkCurCod";
				vField.__fDescription = "Банковский код валюты";
				__fFieldS.Add(vField);
			}

			/// BnkCurNam - Банковское название валюты
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Банковское название валюты";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Nchar;
				vField.__fName = "BnkCurNam";
				vField.__fDescription = "Банковское название валюты";
				__fFieldS.Add(vField);
			}

			/// CurRatBuy - Официальный курс покупки к отчетной валюте
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Официальный курс покупки к отчетной валюте";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Numeric;
				vField.__fName = "CurRatBuy";
				vField.__fDescription = "Официальный курс покупки к отчетной валюте";
				__fFieldS.Add(vField);
			}

			/// CurRatBuyCmr - Коммерческий курс покупки к отчетной валюте
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Коммерческий курс покупки к отчетной валюте";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Numeric;
				vField.__fName = "CurRatBuyCmr";
				vField.__fDescription = "Коммерческий курс покупки к отчетной валюте";
				__fFieldS.Add(vField);
			}

			/// CurRatSal - Официальный курс продажи к отчетной валюте
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Официальный курс продажи к отчетной валюте";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Numeric;
				vField.__fName = "CurRatSal";
				vField.__fDescription = "Официальный курс продажи к отчетной валюте";
				__fFieldS.Add(vField);
			}

			/// CurRatSalCmr - Коммерческий курс продажи к отчетной валюте
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Коммерческий курс продажи к отчетной валюте";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Numeric;
				vField.__fName = "CurRatSalCmr";
				vField.__fDescription = "Коммерческий курс продажи к отчетной валюте";
				__fFieldS.Add(vField);
			}

			return;
		}

		#endregion Процедуры

		#endregion МЕТОДЫ
	}
}

