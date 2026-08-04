using nlApplication;
using nlData;
using System;
using System.Data;

namespace nlAdministration
{
	/// <summary>
	/// Класс 'admEssenceRrdAdj'
	/// </summary>
	/// <remarks>Сущность - Корректировки записей</remarks>
	/// <design>Код класса сгенерирован программой 'CS Designer'</design>
	public class admEssenceRrdAdj : datUnitEssence
	{
		#region = ДИЗАЙНЕРЫ

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <remarks>Источник данных береться используемый по умолчанию</remarks>
		////// <remarks>Вид удаления данных - пометка</remarks>
		public admEssenceRrdAdj() : base()
		{
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="pDeleteType">Порядок удаления записей из таблиц базы данных</param>
		/// /// <remarks>Источник данных береться используемый по умолчанию</remarks>
		public admEssenceRrdAdj(DELETETYPES pDeleteType) : base(pDeleteType)
		{
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="pDataSourceAlias">Псевдоним источника данных</param>
		/// <param name="pDeleteType">Порядок удаления записей из таблиц базы данных</param>
		public admEssenceRrdAdj(string pDataSourceAlias, DELETETYPES pDeleteType) : base(pDataSourceAlias, pDeleteType)
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
			__fTableDescription = admApplication.__oTunes.__mTranslate("Корректировки записей");
			__fTableName = "RrdAdj";
			__fTableAlias = "RC";
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
			vDataRow["lnkRrdLck"] = 0;
			vDataRow["Fld"] = "";
			vDataRow["Val"] = "";

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
			+ " Left Join RrdLck as RL On CT.CLU = RC.lnkRrdLck"
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
			int vlnkRrdLck = Convert.ToInt32(pDataRow["lnkRrdLck"]); // Ссылка: Блокировка
			string vFld = Convert.ToString(pDataRow["Fld"]); // Название поля
			string vVal = Convert.ToString(pDataRow["Val"]); // Новое значение

			#region 'lnkRrdLck' - Ссылка: Блокировка

			/// Если ссылка меньше нуля формируется сообщение об ошибке: 'Ссылка: ... указана ошибочно'
			if(vlnkRrdLck < 0)
			{
					_fTriggerErrorsDescriptions.Add(datApplication.__oTunes.__mTranslate("Ссылка: Блокировка указана ошибочно"));
			}
			/// Если ссылка больше нуля проверяется ее наличие в связанной таблице, если ссылка не обнаружена формируется сообщение оь ошибке: 'Ссылка: ... указана не верно'
			else
			{
				if (datApplication.__oData.__mDataSourceGet(__fDataSourceAlias).__mTableRowsCountWhere("RrdLck", "CLU = " + vlnkRrdLck.ToString()) <= 0)
					_fTriggerErrorsDescriptions.Add(datApplication.__oTunes.__mTranslate("Ссылка: Блокировка указана не верно"));
			}

			#endregion 'lnkRrdLck' - Ссылка: Блокировка

			if (_fTriggerErrorsDescriptions.Count > 0)
				vReturn = false;

			return vReturn;
		}

		#endregion Триггеры

		#endregion МЕТОДЫ
	}
}

