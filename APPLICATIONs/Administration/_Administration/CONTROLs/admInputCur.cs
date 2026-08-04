using nlApplication;
using nlElements;
using System;

namespace nlAdministration
{
	/// <summary>
	/// Файл admInputCur.cs
	/// </summary>
	/// <remarks>Класс поле ввода 'Валюта'</remarks>
	/// <conception>Lucasin V.</conception> // Замысел
	/// <version>2026.01.16 00-00</version> // Дата-время последней корректировки
	/// <design>Код класса сгенерирован программой 'CS Designer'</design>
	public class admInputCur : elmInputCombo
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

			__fCaption_ = "Валюта";
			__fFieldName = "lnkCur";
			__oEssence_ = new admEssenceCur();
			__mItemsEssenceLoad("CLU != 0", "codCur");
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
