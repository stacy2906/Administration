using nlApplication;
using nlData;
using System;
using System.Data;

namespace nlAdministration
{
	/// <summary>
	/// Класс 'admEssencePclTyp'
	/// </summary>
	/// <remarks>Сущность - Виды протоколов</remarks>
	/// <design>Код класса сгенерирован программой 'CS Designer'</design>
	public class admEssencePclTyp : datUnitEssence
	{
		#region = ДИЗАЙНЕРЫ

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <remarks>Источник данных береться используемый по умолчанию</remarks>
		////// <remarks>Вид удаления данных - пометка</remarks>
		public admEssencePclTyp() : base()
		{
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="pDeleteType">Порядок удаления записей из таблиц базы данных</param>
		/// /// <remarks>Источник данных береться используемый по умолчанию</remarks>
		public admEssencePclTyp(DELETETYPES pDeleteType) : base(pDeleteType)
		{
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="pDataSourceAlias">Псевдоним источника данных</param>
		/// <param name="pDeleteType">Порядок удаления записей из таблиц базы данных</param>
		public admEssencePclTyp(string pDataSourceAlias, DELETETYPES pDeleteType) : base(pDataSourceAlias, pDeleteType)
		{
		}

		#endregion ДИЗАЙНЕРЫ

		#region = МЕТОДЫ

		#region - Объект

		///<summary>
		/// Сборка объекта
		///</summary>
		protected override void _mObjectAssembly()
		{
			base._mObjectAssembly();
			__fTableDescription = admApplication.__oTunes.__mTranslate("Виды протоколов");
			__fTableName = "PclTyp";
			__fTableAlias = "PT";
			__fCodeNewCalculateType = CODESNEWTYPES.Skiped;
			__fLockUsed = true;

			return;
		}

		#endregion Объект

		#region - Процедуры

		/// <summary>
		/// Получение пустой записи для указанной таблицы
		/// </summary>
		/// <param name="pDataTable">{DataTable}</param>
		/// <returns>{DataRow} заполненная значениями по умолчанию</returns>
		public override DataRow __mRecordNew(DataTable pDataTable)
		{
			DateTime vDateTime = DateTime.Now; // Текущее время
			DateTime vDateTimeEmpty = new DateTime(1900, 1, 1, 0, 0, 0);
			DataRow vDataRow = pDataTable.NewRow(); // Объект строки

			vDataRow["CHG"] = vDateTime;
			vDataRow["CLU"] = 0;
			vDataRow["ELD"] = 0;
			vDataRow["GID"] = Guid.NewGuid();
			vDataRow["cgzPclTyp"] = 0;
			vDataRow["dsiPclTyp"] = "";
			vDataRow["optAppErr"] = 0;
			vDataRow["optAppExc"] = 0;
			vDataRow["optAppErrPrg"] = 0;
			vDataRow["optAppEvn"] = 0;
			vDataRow["optDatErr"] = 0;
			vDataRow["optDatEvn"] = 0;
			vDataRow["optDevErr"] = 0;
			vDataRow["optDevEvn"] = 0;
			vDataRow["optOth"] = 0;
			vDataRow["optUsrErr"] = 0;
			vDataRow["optUsrEvn"] = 0;
			vDataRow["optUsrMsg"] = 0;

			return vDataRow;
		}
		/// <summary>
		/// Получение табличных данных
		/// </summary>
		/// <param name="pExpressionWhere">Условие выбора данных</param>
		/// <param name="pExpressionOrder">Условие сортировки даных</param>
		/// <returns>{DataTable} заполненный данными</returns>
		public override DataTable __mGrid(string pExpressionWhere, string pExpressionOrder)
		{
			string vQuery = "Select"
			+" " + __fTableAlias + ".*"
 			+ " From " + __fTableName + " as " + __fTableAlias
			;
			if (pExpressionWhere.Length > 0)
				vQuery = vQuery + " Where " + __fTableAlias + ".CLU != 0 and " + pExpressionWhere;
			else
				vQuery = vQuery + " Where " + __fTableAlias + ".CLU != 0";

			if (pExpressionOrder.Length > 0)
				vQuery = vQuery + " Order By " + pExpressionOrder;
			return datApplication.__oData.__mDataSourceGet(__fDataSourceAlias).__mSqlQuery(vQuery);
		}

		#endregion Процедуры

		#region - Триггеры

		/// <summary>
		/// Проверка заполнения полей
		/// </summary>
		/// <param name="pDataRow"></param>
		/// <param name="pTriggerType"></param>
		/// <returns>[true] - данные введены без ошибок, иначе - [false]</returns>
		public override bool __mCheckRecordFieldsFill(ref DataRow pDataRow, TRIGGERTYPEFORCHANGERECORD pTriggerType)
		{
			bool vReturn = true; // Возвращаемое значение
			_fTriggerErrorsDescriptions.Clear(); // Сброс списка ошибок

			DateTime vCHG = Convert.ToDateTime(pDataRow["CHG"]); // Запись: Правка
			int vCLU = Convert.ToInt32(pDataRow["CLU"]); // Запись: Ключ
			int vcgzPclTyp = Convert.ToInt32(pDataRow["cgzPclTyp"]); // Сортировка
			string vdsiPclTyp = Convert.ToString(pDataRow["dsiPclTyp"]); // Название
			bool voptAppErr = Convert.ToBoolean(pDataRow["optAppErr"]); // Опция: Ошибка приложения
			bool voptAppExc = Convert.ToBoolean(pDataRow["optAppExc"]); // Опция: Исключение
			bool voptAppErrPrg = Convert.ToBoolean(pDataRow["optAppErrPrg"]); // Опция: Ошибка программирования
			bool voptAppEvn = Convert.ToBoolean(pDataRow["optAppEvn"]); // Опция: Событие приложения
			bool voptDatErr = Convert.ToBoolean(pDataRow["optDatErr"]); // Опция: Ошибка источника данных
			bool voptDatEvn = Convert.ToBoolean(pDataRow["optDatEvn"]); // Опция: Событие источника данных
			bool voptDevErr = Convert.ToBoolean(pDataRow["optDevErr"]); // Опция: Ошибка устройства
			bool voptDevEvn = Convert.ToBoolean(pDataRow["optDevEvn"]); // Опция: Событие устройства
			bool voptOth = Convert.ToBoolean(pDataRow["optOth"]); // Опция: Прочие
			bool voptUsrErr = Convert.ToBoolean(pDataRow["optUsrErr"]); // Опция: Ошибка пользователя
			bool voptUsrEvn = Convert.ToBoolean(pDataRow["optUsrEvn"]); // Опция: Действия пользователя
			bool voptUsrMsg = Convert.ToBoolean(pDataRow["optUsrMsg"]); // Опция: Сообщения показанные пользователю

			#region 'cgzPclTyp' - Сортировка

			/// Если сортировка не указана, выполняется её расчет
			if (vcgzPclTyp <= 0)
			{
				pDataRow["cgzPclTyp" ] = datApplication.__oData.__mDataSourceGet(__fDataSourceAlias).__mCodeNew(__fTableName, vCLU, __fCodeNewCalculateType, 1);
			}

			#endregion 'cgzPclTyp' - Сортировка

			#region 'dsiPclTyp' - Название

			/// Если название не указано, формируется сообщение об ошибке: 'Название не указано'
			if (vdsiPclTyp.Length == 0)
			{
				_fTriggerErrorsDescriptions.Add(datApplication.__oTunes.__mTranslate("Название не указано"));
			}
			/// Если название указано проверяется его дублирование, если дублирование обнаружено, формируется сообщение об ошибке: 'Название уже используется'
			else
			{
				if (datApplication.__oData.__mDataSourceGet(__fDataSourceAlias).__mNameExists(__fTableName, vdsiPclTyp.Trim(), vCLU) == true)
					_fTriggerErrorsDescriptions.Add(datApplication.__oTunes.__mTranslate("Название уже используется"));
			}

			#endregion 'dsiPclTyp' - Название

			#region Проверка ввода опций

			if(Convert.ToInt32(voptAppErr)
				 + Convert.ToInt32(voptAppExc)
				 + Convert.ToInt32(voptAppErrPrg)
				 + Convert.ToInt32(voptAppEvn)
				 + Convert.ToInt32(voptDatErr)
				 + Convert.ToInt32(voptDatEvn)
				 + Convert.ToInt32(voptDevErr)
				 + Convert.ToInt32(voptDevEvn)
				 + Convert.ToInt32(voptOth)
				 + Convert.ToInt32(voptUsrErr)
				 + Convert.ToInt32(voptUsrEvn)
				 + Convert.ToInt32(voptUsrMsg)
				== 0)
				_fTriggerErrorsDescriptions.Add(datApplication.__oTunes.__mTranslate("Не указана ни одна опция"));
			else
				if(Convert.ToInt32(voptAppErr)
					 + Convert.ToInt32(voptAppExc)
					 + Convert.ToInt32(voptAppErrPrg)
					 + Convert.ToInt32(voptAppEvn)
					 + Convert.ToInt32(voptDatErr)
					 + Convert.ToInt32(voptDatEvn)
					 + Convert.ToInt32(voptDevErr)
					 + Convert.ToInt32(voptDevEvn)
					 + Convert.ToInt32(voptOth)
					 + Convert.ToInt32(voptUsrErr)
					 + Convert.ToInt32(voptUsrEvn)
					 + Convert.ToInt32(voptUsrMsg)
				== 0)
					_fTriggerErrorsDescriptions.Add(datApplication.__oTunes.__mTranslate("Опции указаны не верно"));

			#endregion Проверка ввода опций

			if (_fTriggerErrorsDescriptions.Count > 0)
				vReturn = false;

			return vReturn;
		}

		#endregion Триггеры

		#endregion МЕТОДЫ
	}
}

