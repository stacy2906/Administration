using nlApplication;
using nlData;
using System;
using System.Data;

namespace nlAdministration
{
	/// <summary>
	/// Класс 'admEssenceCur'
	/// </summary>
	/// <remarks>Сущность - Валюты</remarks>
	/// <design>Код класса сгенерирован программой 'CS Designer'</design>
	public class admEssenceCur : datUnitEssence
	{
		#region = ДИЗАЙНЕРЫ

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <remarks>Источник данных береться используемый по умолчанию</remarks>
		////// <remarks>Вид удаления данных - пометка</remarks>
		public admEssenceCur() : base()
		{
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="pDeleteType">Порядок удаления записей из таблиц базы данных</param>
		/// /// <remarks>Источник данных береться используемый по умолчанию</remarks>
		public admEssenceCur(DELETETYPES pDeleteType) : base(pDeleteType)
		{
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="pDataSourceAlias">Псевдоним источника данных</param>
		/// <param name="pDeleteType">Порядок удаления записей из таблиц базы данных</param>
		public admEssenceCur(string pDataSourceAlias, DELETETYPES pDeleteType) : base(pDataSourceAlias, pDeleteType)
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
			__fTableDescription = admApplication.__oTunes.__mTranslate("Валюты");
			__fTableName = "Cur";
			__fTableAlias = "C";
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
			vDataRow["codCur"] = 0;
			vDataRow["dsiCur"] = "";
			vDataRow["mrkNtl"] = 0;
			vDataRow["mrkRrt"] = 0;
			vDataRow["BnkCurCod"] = "";
			vDataRow["BnkCurNam"] = "";
			vDataRow["CurRatBuy"] = 0;
			vDataRow["CurRatBuyCmr"] = 0;
			vDataRow["CurRatSal"] = 0;
			vDataRow["CurRatSalCmr"] = 0;

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
			int vcodCur = Convert.ToInt32(pDataRow["codCur"]); // Сортировка
			string vdsiCur = Convert.ToString(pDataRow["dsiCur"]); // Название
			bool vmrkNtl = Convert.ToBoolean(pDataRow["mrkNtl"]); // Метка: Национальная валюта
			bool vmrkRrt = Convert.ToBoolean(pDataRow["mrkRrt"]); // Метка: Отчетная валюта
			string vBnkCurCod = Convert.ToString(pDataRow["BnkCurCod"]); // Банковский код валюты
			string vBnkCurNam = Convert.ToString(pDataRow["BnkCurNam"]); // Банковское название валюты
			decimal vCurRatBuy = Convert.ToDecimal(pDataRow["CurRatBuy"]); // Официальный курс покупки к отчетной валюте
			decimal vCurRatBuyCmr = Convert.ToDecimal(pDataRow["CurRatBuyCmr"]); // Коммерческий курс покупки к отчетной валюте
			decimal vCurRatSal = Convert.ToDecimal(pDataRow["CurRatSal"]); // Официальный курс продажи к отчетной валюте
			decimal vCurRatSalCmr = Convert.ToDecimal(pDataRow["CurRatSalCmr"]); // Коммерческий курс продажи к отчетной валюте

			#region 'codCur' - Сортировка

			/// Если код не указан, выполняется его расчет
			if (vcodCur <= 0)
			{
				pDataRow["codCur" ] = datApplication.__oData.__mDataSourceGet(__fDataSourceAlias).__mCodeNew(__fTableName, vCLU, __fCodeNewCalculateType, 1);
			}
			/// Если код указан, выполняется его проверка на использование с другим идентификатором, если такая запись обнаружена формируется сообщение сообщение об ошибке: 'Код уже используется'
			else
			{
				if (datApplication.__oData.__mDataSourceGet(__fDataSourceAlias).__mTableRowsCountWhere(__fTableName, "codCur = " + vcodCur.ToString() + " and CLU != " + vCLU.ToString()) > 0)
				{
					_fTriggerErrorsDescriptions.Add(datApplication.__oTunes.__mTranslate("Учетный код уже используется"));
				}
			}

			#endregion 'codcodCur' - Сортировка

			#region 'dsiCur' - Название

			/// Если название не указано, формируется сообщение об ошибке: 'Название не указано'
			if (vdsiCur.Length == 0)
			{
				_fTriggerErrorsDescriptions.Add(datApplication.__oTunes.__mTranslate("Название не указано"));
			}
			/// Если название указано проверяется его дублирование, если дублирование обнаружено, формируется сообщение об ошибке: 'Название уже используется'
			else
			{
				if (datApplication.__oData.__mDataSourceGet(__fDataSourceAlias).__mNameExists(__fTableName, vdsiCur.Trim(), vCLU) == true)
					_fTriggerErrorsDescriptions.Add(datApplication.__oTunes.__mTranslate("Название уже используется"));
			}

			#endregion 'dsiCur' - Название

			if (_fTriggerErrorsDescriptions.Count > 0)
				vReturn = false;

			return vReturn;
		}

		#endregion Триггеры

		#endregion МЕТОДЫ
	}
}

