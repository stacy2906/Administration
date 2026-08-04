using nlData;
using System;

namespace nlAdministration
{
	/// <summary>
	/// Файл admModelTableRrdAdj.cs
	/// </summary>
	/// <remarks>Класс - Cущность 'Корректировки записей'</remarks>
	/// <design>Код класса сгенерирован программой 'CS Designer'</design>
	public class admModelTableRrdAdj : datUnitModelTable
	{
		#region = МЕТОДЫ

		#region - Поведение

		/// <summary>
		/// Сборка объекта
		/// </summary>
		protected override void _mObjectAssembly()
		{
			base._mObjectAssembly();

			__fDescription = datApplication.__oTunes.__mTranslate("Корректировки записей");
			__oEssence = new admEssenceRrdAdj("Main", DELETETYPES.Mark); // Сущность таблицы
			__fName = "RrdAdj";

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

			/// lnkRrdLck - Ссылка: Блокировка
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Ссылка: Блокировка";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Int;
				vField.__fName = "lnkRrdLck";
				vField.__fDescription = "Ссылка: Блокировка";
				__fFieldS.Add(vField);
			}

			/// Fld - Название поля
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Название поля";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Nchar;
				vField.__fName = "Fld";
				vField.__fDescription = "Название поля";
				__fFieldS.Add(vField);
			}

			/// Val - Новое значение
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Новое значение";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Nchar;
				vField.__fName = "Val";
				vField.__fDescription = "Новое значение";
				__fFieldS.Add(vField);
			}

			return;
		}

		#endregion Процедуры

		#endregion МЕТОДЫ
	}
}

