using nlApplication;
using nlData;
using System;
using System.Data;

namespace nlAdministration
{
	/// <summary>
	/// Класс 'admEssenceDbsTunTyp'
	/// </summary>
	/// <remarks>Сущность - Виды настроек базы данных</remarks>
	/// <design>Код класса сгенерирован программой 'CS Designer'</design>
	public class admEssenceDbsTunTyp : datUnitEssence
	{
		#region = ДИЗАЙНЕРЫ

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <remarks>Источник данных береться используемый по умолчанию</remarks>
		////// <remarks>Вид удаления данных - пометка</remarks>
		public admEssenceDbsTunTyp() : base()
		{
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="pDeleteType">Порядок удаления записей из таблиц базы данных</param>
		/// /// <remarks>Источник данных береться используемый по умолчанию</remarks>
		public admEssenceDbsTunTyp(DELETETYPES pDeleteType) : base(pDeleteType)
		{
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="pDataSourceAlias">Псевдоним источника данных</param>
		/// <param name="pDeleteType">Порядок удаления записей из таблиц базы данных</param>
		public admEssenceDbsTunTyp(string pDataSourceAlias, DELETETYPES pDeleteType) : base(pDataSourceAlias, pDeleteType)
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
			__fTableDescription = admApplication.__oTunes.__mTranslate("Виды настроек базы данных");
			__fTableName = "DbsTunTyp";
			__fTableAlias = "DTT";
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
			vDataRow["cgzDbsTunTyp"] = 0;
			vDataRow["dsiDbsTunTyp"] = "";

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
			int vcgzDbsTunTyp = Convert.ToInt32(pDataRow["cgzDbsTunTyp"]); // Сортировка
			string vdsiDbsTunTyp = Convert.ToString(pDataRow["dsiDbsTunTyp"]); // Название

			#region 'cgzDbsTunTyp' - Сортировка

			/// Если сортировка не указана, выполняется её расчет
			if (vcgzDbsTunTyp <= 0)
			{
				pDataRow["cgzDbsTunTyp" ] = datApplication.__oData.__mDataSourceGet(__fDataSourceAlias).__mCodeNew(__fTableName, vCLU, __fCodeNewCalculateType, 1);
			}

			#endregion 'cgzDbsTunTyp' - Сортировка

			#region 'dsiDbsTunTyp' - Название

			/// Если название не указано, формируется сообщение об ошибке: 'Название не указано'
			if (vdsiDbsTunTyp.Length == 0)
			{
				_fTriggerErrorsDescriptions.Add(datApplication.__oTunes.__mTranslate("Название не указано"));
			}
			/// Если название указано проверяется его дублирование, если дублирование обнаружено, формируется сообщение об ошибке: 'Название уже используется'
			else
			{
				if (datApplication.__oData.__mDataSourceGet(__fDataSourceAlias).__mNameExists(__fTableName, vdsiDbsTunTyp.Trim(), vCLU) == true)
					_fTriggerErrorsDescriptions.Add(datApplication.__oTunes.__mTranslate("Название уже используется"));
			}

			#endregion 'dsiDbsTunTyp' - Название

			if (_fTriggerErrorsDescriptions.Count > 0)
				vReturn = false;

			return vReturn;
		}

		#endregion Триггеры

		#endregion МЕТОДЫ
	}
}

