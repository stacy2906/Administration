using nlApplication;
using nlElements;
using System;

namespace nlAdministration
{
	/// <summary>
	/// Файл admInputApp.cs
	/// </summary>
	/// <remarks>Класс поле ввода 'Приложения'</remarks>
	/// <conception>Lucasin V.</conception> // Замысел
	/// <version>2026.01.16 00-00</version> // Дата-время последней корректировки
	/// <design>Код класса сгенерирован программой 'CS Designer'</design>
	public class admInputApp : elmInputCombo
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

			__fCaption_ = "Приложения";
			__fFieldName = "lnkApp";
			__oEssence_ = new admEssenceApp();
			__mItemsEssenceLoad("CLU != 0", "codApp");
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
