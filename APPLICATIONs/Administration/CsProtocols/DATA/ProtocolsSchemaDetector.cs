using nlData;
using nlDataSourceSqlite;
using System;
using System.Data;

namespace naCsProtocols
{
    /// <summary>
    /// Файл ProtocolsSchemaDetector.cs
    /// </summary>
    /// <remarks>Разные реальные копии 'protocols.db', встреченные в проекте, используют разные варианты
    /// именования ('dsiApp'/'desApp', 'PclRrdTyp'/'RrdTyp', 'Msg'/'Err', опечатка 'Ink*' вместо 'lnk*' и т.д.).
    /// Этот класс определяет фактическую схему по реально существующим таблицам/столбцам, а не по предположению.
    /// Вынесен как самостоятельный, не привязанный к конкретной форме класс, чтобы новые формы (например
    /// 'cspFormCombinedViewer') могли переиспользовать ту же логику, что уже проверена в 'cspFormMain', не
    /// дублируя и не расходясь с ней. 'cspFormMain' при этом НЕ переведён на этот класс намеренно - его
    /// собственная (уже проверенная и работающая) копия логики не тронута, чтобы не рисковать регрессией.</remarks>
    /// <conception>Lucasin V.</conception>
    public class ProtocolsSchemaInfo
    {
        public string AppNameColumn = "dsiApp";
        public string PclTypNameColumn = "dsiPclTyp";
        /// <summary>Столбец связи 'Pcl' -&gt; 'App' ('lnkApp' в правильной схеме, 'InkApp' - опечатка старой версии)</summary>
        public string AppLinkColumn = "lnkApp";
        /// <summary>Столбец связи 'Pcl' -&gt; 'PclTyp' ('lnkPclTyp', либо опечатка 'InkPclTyp')</summary>
        public string PclTypLinkColumn = "lnkPclTyp";
        /// <summary>Столбец связи 'PclRrd' -&gt; 'Pcl' ('lnkPcl', либо опечатка 'InkPcl')</summary>
        public string PclLinkColumn = "lnkPcl";
        /// <summary>[true] - хост/пользователь читаются прямо из 'Pcl.Hst'/'Pcl.Usr' (текстом)</summary>
        public bool HostUserDirectText = false;
        /// <summary>[true] - хост/пользователь читаются через JOIN на 'Cpu'/'Usr' по 'lnkCpu'/'lnkUsr'</summary>
        public bool HasCpuUsrTables = false;
        public string RrdTypTable = "PclRrdTyp";
        public string RrdTypNameColumn = "dsiPclRrdTyp";
        public string RrdLinkColumn = "lnkPclRrdTyp";
        public string MessageColumn = "Msg";
        /// <summary>[true] - 'Pcl.CHG' хранится как .NET ticks (число), а не как дата-строка</summary>
        public bool ChgIsTicks = false;
    }

    /// <summary>
    /// Файл ProtocolsSchemaDetector.cs
    /// </summary>
    /// <remarks>См. примечание к 'ProtocolsSchemaInfo'</remarks>
    /// <conception>Lucasin V.</conception>
    public static class ProtocolsSchemaDetector
    {
        #region = МЕТОДЫ

        #region - Процедуры

        /// <summary>
        /// Определение схемы для активного логгера 'dsqProtocols'
        /// </summary>
        public static ProtocolsSchemaInfo DetectFor(dsqProtocols pProtocols)
        {
            if (pProtocols == null)
                return new ProtocolsSchemaInfo();

            return mDetect(pProtocols.__mTableExists, pProtocols.__mColumnExists, pProtocols.__mQuery);
        }
        /// <summary>
        /// Определение схемы для стороннего файла '*.db', открытого напрямую (без 'dsqProtocols')
        /// </summary>
        public static ProtocolsSchemaInfo DetectFor(datUnitDataSource pDataSource)
        {
            if (pDataSource == null)
                return new ProtocolsSchemaInfo();

            return mDetect(
                pTable => pDataSource.__mTableExists(pTable),
                (pTable, pColumn) => ColumnExistsFor(pDataSource, pTable, pColumn),
                pDataSource.__mSqlQuery);
        }
        /// <summary>
        /// Проверка существования столбца в таблице через 'PRAGMA table_info' (у 'datUnitDataSource' нет
        /// готового метода проверки столбцов)
        /// </summary>
        public static bool ColumnExistsFor(datUnitDataSource pDataSource, string pTableName, string pColumnName)
        {
            DataTable vColumns = pDataSource.__mSqlQuery("PRAGMA table_info(" + pTableName + ")");
            if (vColumns == null)
                return false;

            foreach (DataRow vColumn in vColumns.Rows)
            {
                if (vColumn["name"] != DBNull.Value && string.Equals(vColumn["name"].ToString(), pColumnName, StringComparison.OrdinalIgnoreCase) == true)
                    return true;
            }

            return false;
        }
        /// <summary>
        /// Основная логика определения схемы по фактически существующим таблицам/столбцам
        /// </summary>
        private static ProtocolsSchemaInfo mDetect(Func<string, bool> pTableExists, Func<string, string, bool> pColumnExists, Func<string, DataTable> pQuery)
        {
            ProtocolsSchemaInfo vSchema = new ProtocolsSchemaInfo();

            vSchema.AppNameColumn = pColumnExists("App", "dsiApp") == true ? "dsiApp" : "desApp";
            vSchema.PclTypNameColumn = pColumnExists("PclTyp", "dsiPclTyp") == true ? "dsiPclTyp" : "desPclTyp";

            vSchema.AppLinkColumn = pColumnExists("Pcl", "lnkApp") == true ? "lnkApp" : (pColumnExists("Pcl", "InkApp") == true ? "InkApp" : "lnkApp");
            vSchema.PclTypLinkColumn = pColumnExists("Pcl", "lnkPclTyp") == true ? "lnkPclTyp" : (pColumnExists("Pcl", "InkPclTyp") == true ? "InkPclTyp" : "lnkPclTyp");
            vSchema.PclLinkColumn = pColumnExists("PclRrd", "lnkPcl") == true ? "lnkPcl" : (pColumnExists("PclRrd", "InkPcl") == true ? "InkPcl" : "lnkPcl");

            if (pColumnExists("Pcl", "Hst") == true && pColumnExists("Pcl", "Usr") == true)
                vSchema.HostUserDirectText = true;
            else if (pTableExists("Cpu") == true && pTableExists("Usr") == true)
                vSchema.HasCpuUsrTables = true;

            if (pTableExists("PclRrdTyp") == true)
            {
                vSchema.RrdTypTable = "PclRrdTyp";
                vSchema.RrdTypNameColumn = pColumnExists("PclRrdTyp", "dsiPclRrdTyp") == true ? "dsiPclRrdTyp" : "desPclRrdTyp";
            }
            else
            {
                vSchema.RrdTypTable = "RrdTyp";
                vSchema.RrdTypNameColumn = pColumnExists("RrdTyp", "dsiRrdTyp") == true ? "dsiRrdTyp" : "desRrdTyp";
            }

            vSchema.RrdLinkColumn = pColumnExists("PclRrd", "lnkPclRrdTyp") == true ? "lnkPclRrdTyp" : (pColumnExists("PclRrd", "InkPclRrdTyp") == true ? "InkPclRrdTyp" : "lnkRrdTyp");
            vSchema.MessageColumn = pColumnExists("PclRrd", "Msg") == true ? "Msg" : "Err";

            vSchema.ChgIsTicks = mChgIsTicksDetect(pQuery);

            return vSchema;
        }
        /// <summary>
        /// Определяет, хранится ли 'CHG' как .NET ticks (по первой непустой строке 'Pcl')
        /// </summary>
        private static bool mChgIsTicksDetect(Func<string, DataTable> pQuery)
        {
            try
            {
                DataTable vSample = pQuery("SELECT CHG FROM Pcl WHERE CHG IS NOT NULL LIMIT 1");
                if (vSample == null || vSample.Rows.Count == 0 || vSample.Rows[0]["CHG"] == DBNull.Value)
                    return false;

                string vRaw = vSample.Rows[0]["CHG"].ToString().Trim();
                if (vRaw.IndexOfAny(new char[] { '-', ':', ' ', 'T' }) >= 0)
                    return false;

                long vTicks;
                if (long.TryParse(vRaw, out vTicks) == true && vTicks > 600000000000000000L)
                    return true;
            }
            catch { }

            return false;
        }

        #endregion Процедуры

        #endregion МЕТОДЫ
    }
}
