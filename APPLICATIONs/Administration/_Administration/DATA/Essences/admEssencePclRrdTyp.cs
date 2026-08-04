using nlApplication;
using nlData;
using System;
using System.Data;

namespace nlAdministration
{
	/// <summary>
	/// Класс 'admEssencePclRrdTyp'
	/// </summary>
	/// <remarks>Сущность - Виды записей в пртоколе</remarks>
	/// <design>Код класса сгенерирован программой 'CS Designer'</design>
	public class admEssencePclRrdTyp : datUnitEssence
	{
		#region = ДИЗАЙНЕРЫ

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <remarks>Источник данных береться используемый по умолчанию</remarks>
		////// <remarks>Вид удаления данных - пометка</remarks>
		public admEssencePclRrdTyp() : base()
		{
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="pDeleteType">Порядок удаления записей из таблиц базы данных</param>
		/// /// <remarks>Источник данных береться используемый по умолчанию</remarks>
		public admEssencePclRrdTyp(DELETETYPES pDeleteType) : base(pDeleteType)
		{
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="pDataSourceAlias">Псевдоним источника данных</param>
		/// <param name="pDeleteType">Порядок удаления записей из таблиц базы данных</param>
		public admEssencePclRrdTyp(string pDataSourceAlias, DELETETYPES pDeleteType) : base(pDataSourceAlias, pDeleteType)
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
			__fTableDescription = admApplication.__oTunes.__mTranslate("Виды записей в пртоколе");
			__fTableName = "PclRrdTyp";
			__fTableAlias = "PRT";
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
			vDataRow["cgzPclRrdTyp"] = 0;
			vDataRow["dsiPclRrdTyp"] = "";
			vDataRow["optAns"] = 0;
			vDataRow["optDtl"] = 0;
			vDataRow["optExc"] = 0;
			vDataRow["optImg"] = 0;
			vDataRow["optMsg"] = 0;
			vDataRow["optObjPrp"] = 0;
			vDataRow["optRsn"] = 0;

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
			int vcgzPclRrdTyp = Convert.ToInt32(pDataRow["cgzPclRrdTyp"]); // Сортировка
			string vdsiPclRrdTyp = Convert.ToString(pDataRow["dsiPclRrdTyp"]); // Название
			bool voptAns = Convert.ToBoolean(pDataRow["optAns"]); // Опция: Решениние пользователя
			bool voptDtl = Convert.ToBoolean(pDataRow["optDtl"]); // Опция: Подробности
			bool voptExc = Convert.ToBoolean(pDataRow["optExc"]); // Опция: Исключение
			bool voptImg = Convert.ToBoolean(pDataRow["optImg"]); // Опция: Изображение
			bool voptMsg = Convert.ToBoolean(pDataRow["optMsg"]); // Опция: Сообщение
			bool voptObjPrp = Convert.ToBoolean(pDataRow["optObjPrp"]); // Опция: Свойства объекта
			bool voptRsn = Convert.ToBoolean(pDataRow["optRsn"]); // Опция: Причины ошибок

			#region 'cgzPclRrdTyp' - Сортировка

			/// Если сортировка не указана, выполняется её расчет
			if (vcgzPclRrdTyp <= 0)
			{
				pDataRow["cgzPclRrdTyp" ] = datApplication.__oData.__mDataSourceGet(__fDataSourceAlias).__mCodeNew(__fTableName, vCLU, __fCodeNewCalculateType, 1);
			}

			#endregion 'cgzPclRrdTyp' - Сортировка

			#region 'dsiPclRrdTyp' - Название

			/// Если название не указано, формируется сообщение об ошибке: 'Название не указано'
			if (vdsiPclRrdTyp.Length == 0)
			{
				_fTriggerErrorsDescriptions.Add(datApplication.__oTunes.__mTranslate("Название не указано"));
			}
			/// Если название указано проверяется его дублирование, если дублирование обнаружено, формируется сообщение об ошибке: 'Название уже используется'
			else
			{
				if (datApplication.__oData.__mDataSourceGet(__fDataSourceAlias).__mNameExists(__fTableName, vdsiPclRrdTyp.Trim(), vCLU) == true)
					_fTriggerErrorsDescriptions.Add(datApplication.__oTunes.__mTranslate("Название уже используется"));
			}

			#endregion 'dsiPclRrdTyp' - Название

			#region Проверка ввода опций

			if(Convert.ToInt32(voptAns)
				 + Convert.ToInt32(voptDtl)
				 + Convert.ToInt32(voptExc)
				 + Convert.ToInt32(voptImg)
				 + Convert.ToInt32(voptMsg)
				 + Convert.ToInt32(voptObjPrp)
				 + Convert.ToInt32(voptRsn)
				== 0)
				_fTriggerErrorsDescriptions.Add(datApplication.__oTunes.__mTranslate("Не указана ни одна опция"));
			else
				if(Convert.ToInt32(voptAns)
					 + Convert.ToInt32(voptDtl)
					 + Convert.ToInt32(voptExc)
					 + Convert.ToInt32(voptImg)
					 + Convert.ToInt32(voptMsg)
					 + Convert.ToInt32(voptObjPrp)
					 + Convert.ToInt32(voptRsn)
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

