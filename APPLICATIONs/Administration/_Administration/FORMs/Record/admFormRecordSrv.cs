using nlApplication;
using nlElements;
using nlResourcesImages;

namespace nlAdministration
{
	/// <summary>
	/// Файл admFormRecordSrv.cs
	/// </summary>
	/// <remarks>Класс формы для изменения записи 'Сервера'</remarks>
	/// <design>Код класса сгенерирован программой 'CS Designer'</design>
	public class admFormRecordSrv : elmFormRecord
	{
		#region = МЕТОДЫ

		#region - Объект

		/// <summary>
		/// Загрузка формы
		/// </summary>
		protected override void _mObjectAssembly()
		{
			base._mObjectAssembly();

			SuspendLayout();

			#region /// Размещение компонентов

				__cAreaRecord.__mInputAdd(_cInputAss);
				__cAreaRecord.__mInputAdd(_cInputDsiSrv);
				__cAreaRecord.__mInputAdd(_cInputEml);
				__cAreaRecord.__mInputAdd(null);
				__cAreaRecord.__mInputAdd(_cInputELD);

			#endregion Размещение компонентов

			#region /// Настройка компонентов

			__fCaption_ = "Сервер";
			_fHelpTopic = "";

			//__cAreaRecord
			{
				__cAreaRecord.__oEssence = new admEssenceSrv();

				// _cInputAss
				{
					_cInputAss.__fCaption_ = elmApplication.__oTunes.__mTranslate("Сортировка");
					_cInputAss.__fFieldName = "cgzSrv";
					_cInputAss.__fFillType_ = FILLTYPES.Necessarily;
					_cInputAss.__fPartInt_ = 3;
					_cInputAss.__fValueMaximum_ = 999;
				}
				// _cInputDsiSrv
				{
					_cInputDsiSrv.__fCaption_ = elmApplication.__oTunes.__mTranslate("Название");
					_cInputDsiSrv.__fFieldName = "dsiSrv";
					_cInputDsiSrv.__fFillType_ = FILLTYPES.Necessarily;
					_cInputDsiSrv.__fSymbolsCount_ = 20;
				}
				// _cInputEml
				{
					_cInputDsiSrv.__fCaption_ = elmApplication.__oTunes.__mTranslate("Электронная почта");
				}
				// _cInputELD
				{
					_cInputELD.__fCaption_ = elmApplication.__oTunes.__mTranslate("Запись: Исключена");
					_cInputELD.__fFieldName = "ELD";
				}

			}

			#endregion Настройка компонентов

			ResumeLayout(false);

			return;
		}

		#endregion Объект

		#endregion МЕТОДЫ

		#region = ПОЛЯ

		#region - Компоненты

		///<summary>
		/// Поле ввода 'Сортировка'
		///</summary>
		protected elmInputInteger _cInputAss = new elmInputInteger();
		///<summary>
		/// Поле ввода 'Название'
		///</summary>
		protected elmInputString _cInputDsiSrv = new elmInputString();
		///<summary>
		/// Поле ввода 'Электронная почта'
		///</summary>
		protected elmInputString _cInputEml = new elmInputString();
		///<summary>
		/// Поле ввода 'Запись: Исключена'
		///</summary>
		protected elmInputBool _cInputELD = new elmInputBool();

		#endregion Компоненты

		#endregion ПОЛЯ
	}
}

