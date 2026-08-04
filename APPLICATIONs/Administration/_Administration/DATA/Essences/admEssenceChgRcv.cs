using nlApplication;
using nlData;
using System;
using System.Data;

namespace nlAdministration
{
	/// <summary>
	/// Класс 'admEssenceChgRcv'
	/// </summary>
	/// <remarks>Сущность - Протокол получения данных</remarks>
	/// <design>Код класса сгенерирован программой 'CS Designer'</design>
	public class admEssenceChgRcv : datUnitEssence
	{
		#region = ДИЗАЙНЕРЫ

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <remarks>Источник данных береться используемый по умолчанию</remarks>
		////// <remarks>Вид удаления данных - пометка</remarks>
		public admEssenceChgRcv() : base()
		{
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="pDeleteType">Порядок удаления записей из таблиц базы данных</param>
		/// /// <remarks>Источник данных береться используемый по умолчанию</remarks>
		public admEssenceChgRcv(DELETETYPES pDeleteType) : base(pDeleteType)
		{
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="pDataSourceAlias">Псевдоним источника данных</param>
		/// <param name="pDeleteType">Порядок удаления записей из таблиц базы данных</param>
		public admEssenceChgRcv(string pDataSourceAlias, DELETETYPES pDeleteType) : base(pDataSourceAlias, pDeleteType)
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
			__fTableDescription = admApplication.__oTunes.__mTranslate("Протокол получения данных");
			__fTableName = "ChgRcv";
			__fTableAlias = "CR";
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

			vDataRow["CLU"] = 0;
			vDataRow["dtmChgRcvCrn"] = Convert.ToDateTime("01.01.1900");
			vDataRow["dtmChgSndDst"] = Convert.ToDateTime("01.01.1900");
			vDataRow["lnkDbs"] = 0;
			vDataRow["RmtChgSndGid"] = Guid.Empty;

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
			 + ", D.desDbs"
 			+ " From " + __fTableName + " as " + __fTableAlias
			+ " Left Join Dbs as D On CT.CLU = CR.lnkDbs"
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

			int vCLU = Convert.ToInt32(pDataRow["CLU"]); // Запись: Ключ
			DateTime vdtmChgRcvCrn = Convert.ToDateTime(pDataRow["dtmChgRcvCrn"]); // Время: Получение изменений в ТБД из ДБД
			DateTime vdtmChgSndDst = Convert.ToDateTime(pDataRow["dtmChgSndDst"]); // Время: Отправка изменений в ДБД
			int vlnkDbs = Convert.ToInt32(pDataRow["lnkDbs"]); // Ссылка: Дальняя база данных отправившая изменения
			Guid vRmtChgSndGid; // Идентификатор отправки изменений в ДБД
			Guid.TryParse(Convert.ToString(pDataRow["RmtChgSndGid"]), out vRmtChgSndGid);

			#region 'lnkDbs' - Ссылка: Дальняя база данных отправившая изменения

			/// Если ссылка меньше нуля формируется сообщение об ошибке: 'Ссылка: ... указана ошибочно'
			if(vlnkDbs < 0)
			{
					_fTriggerErrorsDescriptions.Add(datApplication.__oTunes.__mTranslate("Ссылка: Дальняя база данных отправившая изменения указана ошибочно"));
			}
			/// Если ссылка больше нуля проверяется ее наличие в связанной таблице, если ссылка не обнаружена формируется сообщение оь ошибке: 'Ссылка: ... указана не верно'
			else
			{
				if (datApplication.__oData.__mDataSourceGet(__fDataSourceAlias).__mTableRowsCountWhere("Dbs", "CLU = " + vlnkDbs.ToString()) <= 0)
					_fTriggerErrorsDescriptions.Add(datApplication.__oTunes.__mTranslate("Ссылка: Дальняя база данных отправившая изменения указана не верно"));
			}

			#endregion 'lnkDbs' - Ссылка: Дальняя база данных отправившая изменения

			if (_fTriggerErrorsDescriptions.Count > 0)
				vReturn = false;

			return vReturn;
		}

		#endregion Триггеры

		#endregion МЕТОДЫ
	}
}

