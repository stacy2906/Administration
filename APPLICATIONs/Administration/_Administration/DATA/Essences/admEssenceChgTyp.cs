using nlApplication;
using nlData;
using System;
using System.Data;

namespace nlAdministration
{
	/// <summary>
	/// Класс 'admEssenceChgTyp'
	/// </summary>
	/// <remarks>Сущность - Виды обмена данными</remarks>
	/// <design>Код класса сгенерирован программой 'CS Designer'</design>
	public class admEssenceChgTyp : datUnitEssence
	{
		#region = ДИЗАЙНЕРЫ

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <remarks>Источник данных береться используемый по умолчанию</remarks>
		////// <remarks>Вид удаления данных - пометка</remarks>
		public admEssenceChgTyp() : base()
		{
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="pDeleteType">Порядок удаления записей из таблиц базы данных</param>
		/// /// <remarks>Источник данных береться используемый по умолчанию</remarks>
		public admEssenceChgTyp(DELETETYPES pDeleteType) : base(pDeleteType)
		{
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="pDataSourceAlias">Псевдоним источника данных</param>
		/// <param name="pDeleteType">Порядок удаления записей из таблиц базы данных</param>
		public admEssenceChgTyp(string pDataSourceAlias, DELETETYPES pDeleteType) : base(pDataSourceAlias, pDeleteType)
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
			__fTableDescription = admApplication.__oTunes.__mTranslate("Виды обмена данными");
			__fTableName = "ChgTyp";
			__fTableAlias = "CT";
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
			vDataRow["cgzChgTyp"] = 0;
			vDataRow["dsiChgTyp"] = "";
			vDataRow["optRcv"] = 0;
			vDataRow["optRcvSnd"] = 0;
			vDataRow["optSnd"] = 0;

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
			int vcgzChgTyp = Convert.ToInt32(pDataRow["cgzChgTyp"]); // Сортировка
			string vdsiChgTyp = Convert.ToString(pDataRow["dsiChgTyp"]); // Название
			bool voptRcv = Convert.ToBoolean(pDataRow["optRcv"]); // Опция: Получение
			bool voptRcvSnd = Convert.ToBoolean(pDataRow["optRcvSnd"]); // Опция: Получение и отправка
			bool voptSnd = Convert.ToBoolean(pDataRow["optSnd"]); // Опция: Отправка

			#region 'cgzChgTyp' - Сортировка

			/// Если сортировка не указана, выполняется её расчет
			if (vcgzChgTyp <= 0)
			{
				pDataRow["cgzChgTyp" ] = datApplication.__oData.__mDataSourceGet(__fDataSourceAlias).__mCodeNew(__fTableName, vCLU, __fCodeNewCalculateType, 1);
			}

			#endregion 'cgzChgTyp' - Сортировка

			#region 'dsiChgTyp' - Название

			/// Если название не указано, формируется сообщение об ошибке: 'Название не указано'
			if (vdsiChgTyp.Length == 0)
			{
				_fTriggerErrorsDescriptions.Add(datApplication.__oTunes.__mTranslate("Название не указано"));
			}
			/// Если название указано проверяется его дублирование, если дублирование обнаружено, формируется сообщение об ошибке: 'Название уже используется'
			else
			{
				if (datApplication.__oData.__mDataSourceGet(__fDataSourceAlias).__mNameExists(__fTableName, vdsiChgTyp.Trim(), vCLU) == true)
					_fTriggerErrorsDescriptions.Add(datApplication.__oTunes.__mTranslate("Название уже используется"));
			}

			#endregion 'dsiChgTyp' - Название

			#region Проверка ввода опций

			if(Convert.ToInt32(voptRcv)
				 + Convert.ToInt32(voptRcvSnd)
				 + Convert.ToInt32(voptSnd)
				== 0)
				_fTriggerErrorsDescriptions.Add(datApplication.__oTunes.__mTranslate("Не указана ни одна опция"));
			else
				if(Convert.ToInt32(voptRcv)
					 + Convert.ToInt32(voptRcvSnd)
					 + Convert.ToInt32(voptSnd)
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

