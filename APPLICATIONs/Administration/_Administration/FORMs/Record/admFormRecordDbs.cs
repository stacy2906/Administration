using nlApplication;
using nlElements;
using nlResourcesImages;

namespace nlAdministration
{
	/// <summary>
	/// Файл admFormRecordDbs.cs
	/// </summary>
	/// <remarks>Класс формы для изменения записи 'Базы данных'</remarks>
	/// <design>Код класса сгенерирован программой 'CS Designer'</design>
	public class admFormRecordDbs : elmFormRecord
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
				__cAreaRecord.__mInputAdd(_cInputDsiDbs);
			__cAreaRecord.__mInputAdd(_cInputLnkSrv);
				__cAreaRecord.__mInputAdd(_cInputMrkMan);
				__cAreaRecord.__mInputAdd(_cInputMrkAdm);
				__cAreaRecord.__mInputAdd(null);
				__cAreaRecord.__mInputAdd(_cInputELD);

			#endregion Размещение компонентов

			#region /// Настройка компонентов

			__fCaption_ = "База данных";
			_fHelpTopic = "";

			//__cAreaRecord
			{
				__cAreaRecord.__oEssence = new admEssenceDbs();

				// _cInputAss
				{
					_cInputAss.__fCaption_ = elmApplication.__oTunes.__mTranslate("Сортировка");
					_cInputAss.__fFieldName = "cgzDbs";
					_cInputAss.__fFillType_ = FILLTYPES.Necessarily;
					_cInputAss.__fPartInt_ = 3;
					_cInputAss.__fValueMaximum_ = 999;
				}
				// _cInputDsiDbs
				{
					_cInputDsiDbs.__fCaption_ = elmApplication.__oTunes.__mTranslate("Название");
					_cInputDsiDbs.__fFieldName = "dsiDbs";
					_cInputDsiDbs.__fFillType_ = FILLTYPES.Necessarily;
					_cInputDsiDbs.__fSymbolsCount_ = 30;
				}
				//_cInputMrkMan
				{
					_cInputMrkMan.__fCaption_ = elmApplication.__oTunes.__mTranslate("Метка: Главная база данных");
					_cInputMrkMan.__fFieldName = "mrkMan";
				}
				//_cInputMrkAdm
				{
					_cInputMrkAdm.__fCaption_ = elmApplication.__oTunes.__mTranslate("Метка: Административный режим");
					_cInputMrkAdm.__fFieldName = "mrkAdm";
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
		protected elmInputString _cInputDsiDbs = new elmInputString();
		///<summary>
		/// Поле ввода 'Ссылка: Сервер'
		///</summary>
		protected admInputSrv _cInputLnkSrv = new admInputSrv();
		///<summary>
		/// Поле ввода 'Метка: Главная база данных'
		///</summary>
		protected elmInputBool _cInputMrkMan = new elmInputBool();
		///<summary>
		/// Поле ввода 'Метка: Административный режим'
		///</summary>
		protected elmInputBool _cInputMrkAdm = new elmInputBool();
		///<summary>
		/// Поле ввода 'Запись: Исключена'
		///</summary>
		protected elmInputBool _cInputELD = new elmInputBool();

		#endregion Компоненты

		#endregion ПОЛЯ
	}
}

