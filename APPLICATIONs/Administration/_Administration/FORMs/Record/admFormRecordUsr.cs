using nlApplication;
using nlElements;
using nlResourcesImages;

namespace nlAdministration
{
	/// <summary>
	/// Файл admFormRecordUsr.cs
	/// </summary>
	/// <remarks>Класс формы для изменения записи 'Пользователи'</remarks>
	/// <design>Код класса сгенерирован программой 'CS Designer'</design>
	public class admFormRecordUsr : elmFormRecord
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

				__cAreaRecord.__mInputAdd(_cInputCod);
				__cAreaRecord.__mInputAdd(_cInputDsiUsr);
				__cAreaRecord.__mInputAdd(_cInputMrkAdm);
				__cAreaRecord.__mInputAdd(_cInputPswCod);
				__cAreaRecord.__mInputAdd(null);
				__cAreaRecord.__mInputAdd(_cInputELD);

			#endregion Размещение компонентов

			#region /// Настройка компонентов

			__fCaption_ = "Пользователь";
			_fHelpTopic = "";

			//__cAreaRecord
			{
				__cAreaRecord.__oEssence = new admEssenceUsr();

				// _cInputCod
				{
					_cInputCod.__fCaption_ = elmApplication.__oTunes.__mTranslate("Код");
					_cInputCod.__fFieldName = "codUsr";
					_cInputCod.__fFillType_ = FILLTYPES.Necessarily;
					_cInputCod.__fPartInt_ = 3;
					_cInputCod.__fValueMaximum_ = 999;
				}
				// _cInputDsiUsr
				{
					_cInputDsiUsr.__fCaption_ = elmApplication.__oTunes.__mTranslate("Псевдоним");
					_cInputDsiUsr.__fFieldName = "dsiUsr";
					_cInputDsiUsr.__fFillType_ = FILLTYPES.Necessarily;
					_cInputDsiUsr.__fSymbolsCount_ = 30;
				}
				//_cInputMrkAdm
				{
					_cInputMrkAdm.__fCaption_ = elmApplication.__oTunes.__mTranslate("Метка: Администратор");
					_cInputMrkAdm.__fFieldName = "mrkAdm";
				}
				// _cInputPswCod
				{
					_cInputMrkAdm.__fCaption_ = elmApplication.__oTunes.__mTranslate("Пароль");
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
		/// Поле ввода 'Код'
		///</summary>
		protected elmInputInteger _cInputCod = new elmInputInteger();
		///<summary>
		/// Поле ввода 'Псевдоним'
		///</summary>
		protected elmInputString _cInputDsiUsr = new elmInputString();
		///<summary>
		/// Поле ввода 'Метка: Администратор'
		///</summary>
		protected elmInputBool _cInputMrkAdm = new elmInputBool();
		///<summary>
		/// Поле ввода 'Пароль'
		///</summary>
		protected elmInputString _cInputPswCod = new elmInputString();
		///<summary>
		/// Поле ввода 'Запись: Исключена'
		///</summary>
		protected elmInputBool _cInputELD = new elmInputBool();

		#endregion Компоненты

		#endregion ПОЛЯ
	}
}

