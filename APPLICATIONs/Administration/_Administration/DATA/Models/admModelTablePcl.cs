using nlData;
using System;

namespace nlAdministration
{
	/// <summary>
	/// Файл admModelTablePcl.cs
	/// </summary>
	/// <remarks>Класс - Cущность 'Проекты'</remarks>
	/// <design>Код класса сгенерирован программой 'CS Designer'</design>
	public class admModelTablePcl : datUnitModelTable
	{
		#region = МЕТОДЫ

		#region - Поведение

		/// <summary>
		/// Сборка объекта
		/// </summary>
		protected override void _mObjectAssembly()
		{
			base._mObjectAssembly();

			__fDescription = datApplication.__oTunes.__mTranslate("Проекты");
			__oEssence = new admEssencePcl("Main", DELETETYPES.Mark); // Сущность таблицы
			__fName = "Pcl";

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

			/// cgzPcl - Сортировка
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Сортировка";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Bigint;
				vField.__fName = "cgzPcl";
				vField.__fDescription = "Сортировка";
				__fFieldS.Add(vField);
			}

			/// dtmPclCre - Дата-время: Создание
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Дата-время: Создание";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Datetime;
				vField.__fName = "dtmPclCre";
				vField.__fDescription = "Дата-время: Создание";
				vField.__fSize = 8;
				__fFieldS.Add(vField);
			}

			/// lnkApp - Ссылка: Приложение
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Ссылка: Приложение";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Int;
				vField.__fName = "lnkApp";
				vField.__fDescription = "Ссылка: Приложение";
				__fFieldS.Add(vField);
			}

			/// lnkCpu - Ссылка: Компьютер
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Ссылка: Компьютер";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Int;
				vField.__fName = "lnkCpu";
				vField.__fDescription = "Ссылка: Компьютер";
				__fFieldS.Add(vField);
			}

			/// lnkPclTyp - Ссылка: Вид протокола
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Ссылка: Вид протокола";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Int;
				vField.__fName = "lnkPclTyp";
				vField.__fDescription = "Ссылка: Вид протокола";
				__fFieldS.Add(vField);
			}

			/// lnkUsr - Ссылка: Пользователь
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Ссылка: Пользователь";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Int;
				vField.__fName = "lnkUsr";
				vField.__fDescription = "Ссылка: Пользователь";
				__fFieldS.Add(vField);
			}

			/// Prc - Процедура
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Процедура";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Nchar;
				vField.__fName = "Prc";
				vField.__fDescription = "Процедура";
				__fFieldS.Add(vField);
			}

			/// Fil - Название файл изображения экрана
			{
				vField = new datUnitModelField();
				vField.__fAutoIncrement = false;
				vField.__fIsClue = false;
				vField.__fCaption = "Название файл изображения экрана";
				vField.__fIsNull = false;
				vField.__fDataType = COLUMNSTYPES.Nchar;
				vField.__fName = "Fil";
				vField.__fDescription = "Название файл изображения экрана";
				__fFieldS.Add(vField);
			}

			return;
		}

		#endregion Процедуры

		#endregion МЕТОДЫ
	}
}

