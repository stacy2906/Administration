using nlApplication;
using nlElements;
using nlResourcesImages;

namespace nlAdministration
{
	/// <summary>
	/// Файл admFormRecordChgTyp.cs
	/// </summary>
	/// <remarks>Класс формы для изменения записи 'Виды обмена данными'</remarks>
	/// <design>Код класса сгенерирован программой 'CS Designer'</design>
	public class admFormRecordChgTyp : elmFormRecord
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

				__cAreaRecord.__mInputAdd(_cInputDsiChgTyp);
				__cAreaRecord.__mInputAdd(_cInputOptRcv);
				__cAreaRecord.__mInputAdd(_cInputOptRcvSnd);
				__cAreaRecord.__mInputAdd(_cInputOptSnd);

			#endregion Размещение компонентов

			#region /// Настройка компонентов

			__fCaption_ = "Вид обмена данными";
			_fHelpTopic = "";

			//__cAreaRecord
			{
				__cAreaRecord.__oEssence = new admEssenceChgTyp();

				// _cInputDsiChgTyp
				{
					_cInputDsiChgTyp.__fCaption_ = elmApplication.__oTunes.__mTranslate("Название");
					_cInputDsiChgTyp.__fFieldName = "dsiChgTyp";
					_cInputDsiChgTyp.__fFillType_ = FILLTYPES.Necessarily;
					_cInputDsiChgTyp.__fSymbolsCount_ = 20;
				}
				//_cInputOptRcv
				{
					_cInputOptRcv.__fCaption_ = elmApplication.__oTunes.__mTranslate("Опция: Получение");
					_cInputOptRcv.__fFieldName = "optRcv";
					_cInputOptRcv.__fEnabled_ = false;
				}
				//_cInputOptRcvSnd
				{
					_cInputOptRcvSnd.__fCaption_ = elmApplication.__oTunes.__mTranslate("Опция: Получение и отправка");
					_cInputOptRcvSnd.__fFieldName = "optRcvSnd";
					_cInputOptRcvSnd.__fEnabled_ = false;
				}
				//_cInputOptSnd
				{
					_cInputOptSnd.__fCaption_ = elmApplication.__oTunes.__mTranslate("Опция: Отправка");
					_cInputOptSnd.__fFieldName = "optSnd";
					_cInputOptSnd.__fEnabled_ = false;
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
		/// Поле ввода 'Название'
		///</summary>
		protected elmInputString _cInputDsiChgTyp = new elmInputString();
		///<summary>
		/// Поле ввода 'Опция: Получение'
		///</summary>
		protected elmInputBool _cInputOptRcv = new elmInputBool();
		///<summary>
		/// Поле ввода 'Опция: Получение и отправка'
		///</summary>
		protected elmInputBool _cInputOptRcvSnd = new elmInputBool();
		///<summary>
		/// Поле ввода 'Опция: Отправка'
		///</summary>
		protected elmInputBool _cInputOptSnd = new elmInputBool();

		#endregion Компоненты

		#endregion ПОЛЯ
	}
}

