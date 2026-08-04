using nlData;
using System;
using System.Data;

namespace nlAdministration
{
	/// <summary>
	/// Класс 'admEssenceRht'
	/// </summary>
	/// <remarks>Сущность - Права</remarks>
	/// <design>Код класса сгенерирован программой 'CS Designer'</design>
	public class admEssenceRht : datUnitEssence
	{
		#region = ДИЗАЙНЕРЫ

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <remarks>Источник данных береться используемый по умолчанию</remarks>
		////// <remarks>Вид удаления данных - пометка</remarks>
		public admEssenceRht() : base()
		{
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="pDeleteType">Порядок удаления записей из таблиц базы данных</param>
		/// /// <remarks>Источник данных береться используемый по умолчанию</remarks>
		public admEssenceRht(DELETETYPES pDeleteType) : base(pDeleteType)
		{
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="pDataSourceAlias">Псевдоним источника данных</param>
		/// <param name="pDeleteType">Порядок удаления записей из таблиц базы данных</param>
		public admEssenceRht(string pDataSourceAlias, DELETETYPES pDeleteType) : base(pDataSourceAlias, pDeleteType)
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
			__fTableDescription = admApplication.__oTunes.__mTranslate("Права");
			__fTableName = "Rht";
			__fTableAlias = "R";
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
			vDataRow["codRht"] = 0;
			vDataRow["dsiRht"] = "";
			vDataRow["FrmNam"] = "";

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
        /// <summary>
        /// Получение идентификатора права и создание его в случае отсутствия
        /// </summary>
        /// <param name="pRightName">Название права</param>
        /// <param name="pFormName">Название формы</param>
        /// <returns>Идентификатор права</returns>
        public int __mGetRightClue(string pRightName, string pFormName)
        {
            int vReturn = -1; // Возвращаемое значение
            if (admApplication.__oData.__mTableRowsCountWhere("Rht", "desRht = '" + pRightName + "' and FrmNam = '" + pFormName + "'") > 0)
            { /// Проверка существоания полученного права, если право найдено получаем идентификатор права 
				DataTable vDataTable = admApplication.__oData.__mSqlQuery("Select CLU From Rht Where desRht = '" + pRightName + "' and FrmNam = '" + pFormName + "'");
                vReturn = Convert.ToInt32(vDataTable.Rows[0][0]); // Идентификатор найденного права
            }
            if (vReturn <= 0)
            { /// Если идентификатор не найден создаем новое право
				if (admApplication.__oData.__mSqlCommand("Insert Into " + __fTableName + "(desRht, FrmNam) Values ('" + pRightName + "','" + pFormName + "')") > 0)
                {
                    vReturn = admApplication.__oData.__mClueLastInserted(__fTableName); // Идентификатор созданного права
                }
            }

            return vReturn;
        }
        /// <summary>
        /// Получение статуса права
        /// </summary>
        /// <param name="pRightName">Название права</param>
        /// <param name="pFormName">Название формы</param>
        /// <returns>[true] - разрешение на выполнение операции, иначе - [false] </returns>
        public bool __mGetRight(string pRightName, string pFormName)
        {
            int vUserClue = admApplication.__oData.__mUserClue(); // Идентификатор текущего пользователя 
            int vUserRoleClue = admApplication.__oData.__mUserRoleClue(); // Идентификатор роли текущего пользователя
            int vRightClue = __mGetRightClue(pRightName, pFormName); // Идентификатор полученного права

            DataTable vDataTableUserRights = admApplication.__oData.__mSqlQuery("Select CLU, Stt From RhtUsr Where lnkRht = " + vRightClue + " and lnkUsr = " + vUserClue.ToString());
            /// Пользователи не содержат указанного права = право добавляется
            if (vDataTableUserRights.Rows.Count == 0)
            {
                admApplication.__oData.__mSqlCommand("Insert Into RhtUsr(lnkRht, lnkUsr, Stt) Values(" + vRightClue.ToString() + ", " + vUserClue.ToString() + ", 1)");
            }
            DataTable vDataTableUserRoleRights = admApplication.__oData.__mSqlQuery("Select CLU, Stt From RhtUsrRol Where lnkRht = " + vRightClue + " and lnkUsrRol = " + vUserRoleClue.ToString());
            /// Роли пользователей не содержат указанного права = право добавляется
            if (vDataTableUserRoleRights.Rows.Count == 0)
            {
                admApplication.__oData.__mSqlCommand("Insert Into RhtUsrRol(lnkRht, lnkUsrRol, Stt) Values(" + vRightClue.ToString() + ", " + vUserClue.ToString() + ", 1)");
            }

            return Convert.ToBoolean(admApplication.__oData.__mSqlValue("Select Stt From RhtUsrRol Where lnkRht = " + vRightClue.ToString() + " and lnkUsrRol = " + vUserRoleClue.ToString()))
                & Convert.ToBoolean(admApplication.__oData.__mSqlValue("Select Stt From RhtUsr Where lnkRht = " + vRightClue.ToString() + " and lnkUsr = " + vUserClue.ToString()));
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
			int vcodRht = Convert.ToInt32(pDataRow["codRht"]); // Код
			string vdsiRht = Convert.ToString(pDataRow["dsiRht"]); // Название
			string vFrmNam = Convert.ToString(pDataRow["FrmNam"]); // Название формы

			#region 'codRht' - Код

			/// Если код не указан, выполняется его расчет
			if (vcodRht <= 0)
			{
				pDataRow["codRht" ] = datApplication.__oData.__mDataSourceGet(__fDataSourceAlias).__mCodeNew(__fTableName, vCLU, __fCodeNewCalculateType, 1);
			}
			/// Если код указан, выполняется его проверка на использование с другим идентификатором, если такая запись обнаружена формируется сообщение сообщение об ошибке: 'Код уже используется'
			else
			{
				if (datApplication.__oData.__mDataSourceGet(__fDataSourceAlias).__mTableRowsCountWhere(__fTableName, "codRht = " + vcodRht.ToString() + " and CLU != " + vCLU.ToString()) > 0)
				{
					_fTriggerErrorsDescriptions.Add(datApplication.__oTunes.__mTranslate("Учетный код уже используется"));
				}
			}

			#endregion 'codcodRht' - Код

			#region 'dsiRht' - Название

			/// Если название не указано, формируется сообщение об ошибке: 'Название не указано'
			if (vdsiRht.Length == 0)
			{
				_fTriggerErrorsDescriptions.Add(datApplication.__oTunes.__mTranslate("Название не указано"));
			}
			/// Если название указано проверяется его дублирование, если дублирование обнаружено, формируется сообщение об ошибке: 'Название уже используется'
			else
			{
				if (datApplication.__oData.__mDataSourceGet(__fDataSourceAlias).__mNameExists(__fTableName, vdsiRht.Trim(), vCLU) == true)
					_fTriggerErrorsDescriptions.Add(datApplication.__oTunes.__mTranslate("Название уже используется"));
			}

			#endregion 'dsiRht' - Название

			if (_fTriggerErrorsDescriptions.Count > 0)
				vReturn = false;

			return vReturn;
		}

		#endregion Триггеры

		#endregion МЕТОДЫ
	}
}

