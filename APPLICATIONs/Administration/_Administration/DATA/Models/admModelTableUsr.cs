using nlData;
using System;

namespace nlAdministration
{
	/// <summary>
	/// Файл admModelTableUsr.cs
	/// </summary>
	/// <remarks>Класс - Cущность 'Пользователи'</remarks>
	/// <design>Код класса сгенерирован программой 'CS Designer'</design>
	public class admModelTableUsr : datUnitModelTable
	{
		#region = МЕТОДЫ

		#region - Поведение

		/// <summary>
		/// Сборка объекта
		/// </summary>
		protected override void _mObjectAssembly()
		{
			base._mObjectAssembly();

			__fDescription = datApplication.__oTunes.__mTranslate("Пользователи");
			__oEssence = new admEssenceUsr("Main", DELETETYPES.Mark); // Сущность таблицы
			__fName = "Usr";

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

			/// codUsr - Код
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Код";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Int;
				vField.__fName = "codUsr";
				vField.__fDescription = "Код";
				__fFieldS.Add(vField);
			}

			/// dsiUsr - Псевдоним
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Псевдоним";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Nchar;
				vField.__fName = "dsiUsr";
				vField.__fDescription = "Псевдоним";
				__fFieldS.Add(vField);
			}

			/// mrkAdm - Метка: Администратор
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Метка: Администратор";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Bit;
				vField.__fName = "mrkAdm";
				vField.__fDescription = "Метка: Администратор";
				vField.__fSize = 1;
				__fFieldS.Add(vField);
			}

			/// PswCod - Пароль
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Пароль";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Nchar;
				vField.__fName = "PswCod";
				vField.__fDescription = "Пароль";
				__fFieldS.Add(vField);
			}

			return;
		}

		#endregion Процедуры

		#endregion МЕТОДЫ
	}
}

