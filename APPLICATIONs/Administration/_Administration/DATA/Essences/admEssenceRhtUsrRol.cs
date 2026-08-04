using nlApplication;
using nlData;
using System;
using System.Data;

namespace nlAdministration
{
	/// <summary>
	/// Класс 'admEssenceRhtUsrRol'
	/// </summary>
	/// <remarks>Сущность - Права ролей пользователей</remarks>
	/// <design>Код класса сгенерирован программой 'CS Designer'</design>
	public class admEssenceRhtUsrRol : datUnitEssence
	{
		#region = ДИЗАЙНЕРЫ

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <remarks>Источник данных береться используемый по умолчанию</remarks>
		////// <remarks>Вид удаления данных - пометка</remarks>
		public admEssenceRhtUsrRol() : base()
		{
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="pDeleteType">Порядок удаления записей из таблиц базы данных</param>
		/// /// <remarks>Источник данных береться используемый по умолчанию</remarks>
		public admEssenceRhtUsrRol(DELETETYPES pDeleteType) : base(pDeleteType)
		{
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="pDataSourceAlias">Псевдоним источника данных</param>
		/// <param name="pDeleteType">Порядок удаления записей из таблиц базы данных</param>
		public admEssenceRhtUsrRol(string pDataSourceAlias, DELETETYPES pDeleteType) : base(pDataSourceAlias, pDeleteType)
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
			__fTableDescription = admApplication.__oTunes.__mTranslate("Права ролей пользователей");
			__fTableName = "RhtUsrRol";
			__fTableAlias = "RUR";
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
			vDataRow["lnkRht"] = 0;
			vDataRow["lnkUsrRol"] = 0;
			vDataRow["Stt"] = 0;

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
			 + ", R.desRht"
			 + ", UR.desUsrRol"
 			+ " From " + __fTableName + " as " + __fTableAlias
			+ " Left Join Rht as R On CT.CLU = RUR.lnkRht"
			+ " Left Join UsrRol as UR On CT.CLU = RUR.lnkUsrRol"
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
			int vlnkRht = Convert.ToInt32(pDataRow["lnkRht"]); // Ссылка: Право
			int vlnkUsrRol = Convert.ToInt32(pDataRow["lnkUsrRol"]); // Ссылка: Роль пользователей
			bool vStt = Convert.ToBoolean(pDataRow["Stt"]); // Статус

			#region 'lnkRht' - Ссылка: Право

			/// Если ссылка меньше нуля формируется сообщение об ошибке: 'Ссылка: ... указана ошибочно'
			if(vlnkRht < 0)
			{
					_fTriggerErrorsDescriptions.Add(datApplication.__oTunes.__mTranslate("Ссылка: Право указана ошибочно"));
			}
			/// Если ссылка больше нуля проверяется ее наличие в связанной таблице, если ссылка не обнаружена формируется сообщение оь ошибке: 'Ссылка: ... указана не верно'
			else
			{
				if (datApplication.__oData.__mDataSourceGet(__fDataSourceAlias).__mTableRowsCountWhere("Rht", "CLU = " + vlnkRht.ToString()) <= 0)
					_fTriggerErrorsDescriptions.Add(datApplication.__oTunes.__mTranslate("Ссылка: Право указана не верно"));
			}

			#endregion 'lnkRht' - Ссылка: Право

			#region 'lnkUsrRol' - Ссылка: Роль пользователей

			/// Если ссылка меньше нуля формируется сообщение об ошибке: 'Ссылка: ... указана ошибочно'
			if(vlnkUsrRol < 0)
			{
					_fTriggerErrorsDescriptions.Add(datApplication.__oTunes.__mTranslate("Ссылка: Роль пользователей указана ошибочно"));
			}
			/// Если ссылка больше нуля проверяется ее наличие в связанной таблице, если ссылка не обнаружена формируется сообщение оь ошибке: 'Ссылка: ... указана не верно'
			else
			{
				if (datApplication.__oData.__mDataSourceGet(__fDataSourceAlias).__mTableRowsCountWhere("UsrRol", "CLU = " + vlnkUsrRol.ToString()) <= 0)
					_fTriggerErrorsDescriptions.Add(datApplication.__oTunes.__mTranslate("Ссылка: Роль пользователей указана не верно"));
			}

			#endregion 'lnkUsrRol' - Ссылка: Роль пользователей

			if (_fTriggerErrorsDescriptions.Count > 0)
				vReturn = false;

			return vReturn;
		}

		#endregion Триггеры

		#endregion МЕТОДЫ
	}
}

