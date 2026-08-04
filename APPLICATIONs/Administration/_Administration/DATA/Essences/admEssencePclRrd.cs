using nlApplication;
using nlData;
using System;
using System.Data;

namespace nlAdministration
{
	/// <summary>
	/// Класс 'admEssencePclRrd'
	/// </summary>
	/// <remarks>Сущность - Записи в протоколах</remarks>
	/// <design>Код класса сгенерирован программой 'CS Designer'</design>
	public class admEssencePclRrd : datUnitEssence
	{
		#region = ДИЗАЙНЕРЫ

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <remarks>Источник данных береться используемый по умолчанию</remarks>
		////// <remarks>Вид удаления данных - пометка</remarks>
		public admEssencePclRrd() : base()
		{
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="pDeleteType">Порядок удаления записей из таблиц базы данных</param>
		/// /// <remarks>Источник данных береться используемый по умолчанию</remarks>
		public admEssencePclRrd(DELETETYPES pDeleteType) : base(pDeleteType)
		{
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="pDataSourceAlias">Псевдоним источника данных</param>
		/// <param name="pDeleteType">Порядок удаления записей из таблиц базы данных</param>
		public admEssencePclRrd(string pDataSourceAlias, DELETETYPES pDeleteType) : base(pDataSourceAlias, pDeleteType)
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
			__fTableDescription = admApplication.__oTunes.__mTranslate("Записи в протоколах");
			__fTableName = "PclRrd";
			__fTableAlias = "PR";
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
			vDataRow["lnkPcl"] = 0;
			vDataRow["lnkPclRrdTyp"] = 0;
			vDataRow["dsrErr"] = 0;
			vDataRow["dsrExc"] = 0;
			vDataRow["Tck"] = 0;

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
			int vlnkPcl = Convert.ToInt32(pDataRow["lnkPcl"]); // Ссылка: Протокол
			int vlnkPclRrdTyp = Convert.ToInt32(pDataRow["lnkPclRrdTyp"]); // Ссылка: Вид записи в протоколе
			int vdsrErr = Convert.ToInt32(pDataRow["dsrErr"]); // Описание: Сообщение об ошибке
			int vdsrExc = Convert.ToInt32(pDataRow["dsrExc"]); // Описание: Исключение
			long vTck = Convert.ToInt64(pDataRow["Tck"]); // Время

			#region 'lnkPcl' - Ссылка: Протокол

			/// Если ссылка меньше нуля формируется сообщение об ошибке: 'Ссылка: ... указана ошибочно'
			if(vlnkPcl < 0)
			{
					_fTriggerErrorsDescriptions.Add(datApplication.__oTunes.__mTranslate("Ссылка: Протокол указана ошибочно"));
			}
			/// Если ссылка больше нуля проверяется ее наличие в связанной таблице, если ссылка не обнаружена формируется сообщение оь ошибке: 'Ссылка: ... указана не верно'
			else
			{
				if (datApplication.__oData.__mDataSourceGet(__fDataSourceAlias).__mTableRowsCountWhere("Pcl", "CLU = " + vlnkPcl.ToString()) <= 0)
					_fTriggerErrorsDescriptions.Add(datApplication.__oTunes.__mTranslate("Ссылка: Протокол указана не верно"));
			}

			#endregion 'lnkPcl' - Ссылка: Протокол

			#region 'lnkPclRrdTyp' - Ссылка: Вид записи в протоколе

			/// Если ссылка меньше нуля формируется сообщение об ошибке: 'Ссылка: ... указана ошибочно'
			if(vlnkPclRrdTyp < 0)
			{
					_fTriggerErrorsDescriptions.Add(datApplication.__oTunes.__mTranslate("Ссылка: Вид записи в протоколе указана ошибочно"));
			}
			/// Если ссылка больше нуля проверяется ее наличие в связанной таблице, если ссылка не обнаружена формируется сообщение оь ошибке: 'Ссылка: ... указана не верно'
			else
			{
				if (datApplication.__oData.__mDataSourceGet(__fDataSourceAlias).__mTableRowsCountWhere("PclRrdTyp", "CLU = " + vlnkPclRrdTyp.ToString()) <= 0)
					_fTriggerErrorsDescriptions.Add(datApplication.__oTunes.__mTranslate("Ссылка: Вид записи в протоколе указана не верно"));
			}

			#endregion 'lnkPclRrdTyp' - Ссылка: Вид записи в протоколе

			if (_fTriggerErrorsDescriptions.Count > 0)
				vReturn = false;

			return vReturn;
		}

		#endregion Триггеры

		#endregion МЕТОДЫ
	}
}

