using nlApplication;
using nlData;
using nlResourcesImages;

namespace nlElements
{
	/// <summary>
	/// Файл rtlFormRecordDiv.cs
	/// </summary>
	/// <remarks>Класс формы для изменения записи 'Подразделения'</remarks>
	/// <design>Код класса сгенерирован программой 'CS Designer'</design>
	public class elmInputQuote : elmInput
	{
        public bool __mDataLoad()
        {
            return false;
        }

        public bool __mDataSave()
		{
			return false;
		}

		public datUnitEssence __oEssence;

		public string __fTableName = "";

		public int __fSymbolsCount_
		{
			get { return _cInput.__fSymbolsCount_; }
			set { _cInput.__fSymbolsCount_ = value; }
		}

			protected elmComponentString _cInput = new elmComponentString();
    }
}
