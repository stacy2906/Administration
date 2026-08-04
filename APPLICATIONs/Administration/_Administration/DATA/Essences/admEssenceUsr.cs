using nlApplication;
using nlData;
using System;
using System.Data;

namespace nlAdministration
{
	/// <summary>
	/// Класс 'admEssenceUsr'
	/// </summary>
	/// <remarks>Сущность - Пользователи</remarks>
	/// <design>Код класса сгенерирован программой 'CS Designer'</design>
	public class admEssenceUsr : datUnitEssence
	{
		#region = ДИЗАЙНЕРЫ

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <remarks>Источник данных береться используемый по умолчанию</remarks>
		////// <remarks>Вид удаления данных - пометка</remarks>
		public admEssenceUsr() : base()
		{
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="pDeleteType">Порядок удаления записей из таблиц базы данных</param>
		/// /// <remarks>Источник данных береться используемый по умолчанию</remarks>
		public admEssenceUsr(DELETETYPES pDeleteType) : base(pDeleteType)
		{
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="pDataSourceAlias">Псевдоним источника данных</param>
		/// <param name="pDeleteType">Порядок удаления записей из таблиц базы данных</param>
		public admEssenceUsr(string pDataSourceAlias, DELETETYPES pDeleteType) : base(pDataSourceAlias, pDeleteType)
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
			__fTableDescription = admApplication.__oTunes.__mTranslate("Пользователи");
			__fTableName = "Usr";
			__fTableAlias = "U";
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
			vDataRow["codUsr"] = 0;
			vDataRow["dsiUsr"] = "";
			vDataRow["mrkAdm"] = 0;
			vDataRow["PswCod"] = "";

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
			int vcodUsr = Convert.ToInt32(pDataRow["codUsr"]); // Код
			string vdsiUsr = Convert.ToString(pDataRow["dsiUsr"]); // Псевдоним
			bool vmrkAdm = Convert.ToBoolean(pDataRow["mrkAdm"]); // Метка: Администратор
			string vPswCod = Convert.ToString(pDataRow["PswCod"]); // Пароль

			#region 'codUsr' - Код

			/// Если код не указан, выполняется его расчет
			if (vcodUsr <= 0)
			{
				pDataRow["codUsr" ] = datApplication.__oData.__mDataSourceGet(__fDataSourceAlias).__mCodeNew(__fTableName, vCLU, __fCodeNewCalculateType, 1);
			}
			/// Если код указан, выполняется его проверка на использование с другим идентификатором, если такая запись обнаружена формируется сообщение сообщение об ошибке: 'Код уже используется'
			else
			{
				if (datApplication.__oData.__mDataSourceGet(__fDataSourceAlias).__mTableRowsCountWhere(__fTableName, "codUsr = " + vcodUsr.ToString() + " and CLU != " + vCLU.ToString()) > 0)
				{
					_fTriggerErrorsDescriptions.Add(datApplication.__oTunes.__mTranslate("Учетный код уже используется"));
				}
			}

			#endregion 'codcodUsr' - Код

			#region 'dsiUsr' - Псевдоним

			/// Если название не указано, формируется сообщение об ошибке: 'Название не указано'
			if (vdsiUsr.Length == 0)
			{
				_fTriggerErrorsDescriptions.Add(datApplication.__oTunes.__mTranslate("Название не указано"));
			}
			/// Если название указано проверяется его дублирование, если дублирование обнаружено, формируется сообщение об ошибке: 'Название уже используется'
			else
			{
				if (datApplication.__oData.__mDataSourceGet(__fDataSourceAlias).__mNameExists(__fTableName, vdsiUsr.Trim(), vCLU) == true)
					_fTriggerErrorsDescriptions.Add(datApplication.__oTunes.__mTranslate("Название уже используется"));
			}

			#endregion 'dsiUsr' - Псевдоним

			if (_fTriggerErrorsDescriptions.Count > 0)
				vReturn = false;

			return vReturn;
		}

		#endregion Триггеры

		#endregion МЕТОДЫ
	}
}

