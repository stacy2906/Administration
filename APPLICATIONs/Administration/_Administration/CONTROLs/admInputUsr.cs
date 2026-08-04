using nlApplication;
using nlElements;
using System;

namespace nlAdministration
{
	/// <summary>
	/// Файл admInputUsr.cs
	/// </summary>
	/// <remarks>Класс поле ввода 'Пользователь'</remarks>
	/// <conception>Lucasin V.</conception> // Замысел
	/// <version>2026.01.16 00-00</version> // Дата-время последней корректировки
	/// <design>Код класса сгенерирован программой 'CS Designer'</design>
	public class admInputUsr : elmInputFormName
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

			__fCaption_ = "Пользователь";
			__fFieldName = "lnkUsr";
			__oEssence = new admEssenceUsr();


			#endregion Настройка контрола

			ResumeLayout();

			_fError = new appUnitError(_fClassFilePath_);
			return;
		}

		#endregion Поведение

		#endregion МЕТОДЫ
	}
}
