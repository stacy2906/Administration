using nlApplication;
using nlElements;
using System;

namespace nlAdministration
{
	/// <summary>
	/// Файл admInputChgTyp.cs
	/// </summary>
	/// <remarks>Класс поле ввода 'Вид обмена данными'</remarks>
	/// <conception>Lucasin V.</conception> // Замысел
	/// <version>2026.01.16 00-00</version> // Дата-время последней корректировки
	/// <design>Код класса сгенерирован программой 'CS Designer'</design>
	public class admInputChgTyp : elmInputCombo
	{
		#region = МЕТОДЫ

		#region - Поведение

		///<summary>
		/// Загрузка контрола
		///</summary>
		protected override void _mObjectAssembly()
		{
			SuspendLayout();

			base._mObjectAssembly();

			#region /// Настройка контрола

			__fCaption_ = "Вид обмена данными";
			__fFieldName = "lnkChgTyp";
			__oEssence_ = new admEssenceChgTyp();
			__mItemsEssenceLoad("CLU != 0", "codChgTyp");
			__mDataRefresh();

			#endregion Настройка контрола

			ResumeLayout();

			_fError = new appUnitError(_fClassFilePath_);
			return;
		}

		#endregion Поведение

		#endregion МЕТОДЫ
	}
}
