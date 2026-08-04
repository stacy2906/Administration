using nlData;
using System;

namespace nlAdministration
{
	/// <summary>
	/// Файл admModelTableRhtUsrRol.cs
	/// </summary>
	/// <remarks>Класс - Cущность 'Права ролей пользователей'</remarks>
	/// <design>Код класса сгенерирован программой 'CS Designer'</design>
	public class admModelTableRhtUsrRol : datUnitModelTable
	{
		#region = МЕТОДЫ

		#region - Поведение

		/// <summary>
		/// Сборка объекта
		/// </summary>
		protected override void _mObjectAssembly()
		{
			base._mObjectAssembly();

			__fDescription = datApplication.__oTunes.__mTranslate("Права ролей пользователей");
			__oEssence = new admEssenceRhtUsrRol("Main", DELETETYPES.Mark); // Сущность таблицы
			__fName = "RhtUsrRol";

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

			/// lnkRht - Ссылка: Право
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Ссылка: Право";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Int;
				vField.__fName = "lnkRht";
				vField.__fDescription = "Ссылка: Право";
				__fFieldS.Add(vField);
			}

			/// lnkUsrRol - Ссылка: Роль пользователей
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Ссылка: Роль пользователей";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Int;
				vField.__fName = "lnkUsrRol";
				vField.__fDescription = "Ссылка: Роль пользователей";
				__fFieldS.Add(vField);
			}

			/// Stt - Статус
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Статус";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Bit;
				vField.__fName = "Stt";
				vField.__fDescription = "Статус";
				__fFieldS.Add(vField);
			}

			return;
		}

		#endregion Процедуры

		#endregion МЕТОДЫ
	}
}

