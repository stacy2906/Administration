using System.Data;

namespace nlData
{
    /// <summary>
    /// Файл datTypeDataTable.cs
    /// </summary>
    /// <remarks>Класс-тип для работы с данными 'DataTable'</remarks>
  	/// <conception>Lucasin V.</conception> // Замысел
	/// <version>2026.01.13 14-13</version> // Дата-время последней корректировки
    public class datTypeDataTable
    {
        /// <summary>  
        /// Сравнение двух DataTable
        /// </summary>  
        /// <param name="pDataTableFirst">Первая таблица</param>  
        /// <param name="pDataTableSecond">Вторая таблица</param>  
        /// <returns>[true] - таблицы одинаковы, иначе [false]</returns>  
        public bool __mDataTableCompare(DataTable pDataTableFirst, DataTable pDataTableSecond)
        {
            bool vReturn = true; // Возвращаемое значение

            /// Сверка названий DataTable`s
            if (pDataTableFirst.TableName != pDataTableSecond.TableName)
            {
                vReturn = false;
                goto Exit;
            }
            /// Сверка количества колонок DataTable`s
            if (pDataTableFirst.Columns.Count != pDataTableSecond.Columns.Count)
            {
                vReturn = false;
                goto Exit;
            }
            /// Сверка количества строк DataTable`s
            if (pDataTableFirst.Rows.Count != pDataTableSecond.Rows.Count)
            {
                vReturn = false;
                goto Exit;
            }
            /// Сверка значений ячеек строк
            for (int vAmount = 0; vAmount < pDataTableFirst.Rows.Count; vAmount++)
            {
                foreach (DataColumn vColumn in pDataTableFirst.Columns)
                {
                    if (vColumn.ColumnName.ToString() != vColumn.ColumnName.ToString())
                    {
                        vReturn = false;
                        return vReturn;
                    }
                    if (pDataTableFirst.Rows[vAmount][vColumn.ColumnName].ToString() != pDataTableSecond.Rows[vAmount][vColumn.ColumnName].ToString())
                    {
                        vReturn = false;
                        break;
                    }
                }
            }
            /// Если есть хотябы одно не ссответствие возвращается [false]
            Exit:

            return vReturn;
        }
    }
}
