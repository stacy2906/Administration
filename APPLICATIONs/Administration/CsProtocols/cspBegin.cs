using nlCsProtocols;
using nlDataSourceSqlite;
using System;
using System.Windows.Forms;

namespace naCsProtocols
{
    /// <summary>
    /// Файл cspBegin.cs
    /// </summary>
    /// <fixed>Класс назывался 'cmlBegin', а комментарий гласил "для приложения 'CsManual'" - явно
    /// скопировано из 'csManual\cmlBegin.cs' без переименования. Само приложение при этом запускалось
    /// нормально (имя класса точки входа не обязано совпадать с именем файла), но это вводило в
    /// заблуждение при чтении кода - переименовано в 'cspBegin', комментарии поправлены</fixed>
    /// <remarks>Главная точка входа приложения 'CsProtocols'. Сначала показывает форму загрузки
    /// протоколов ('cspFormLoad' - Form 1), затем - главный просмотрщик ('cspFormMain' - Form 2)</remarks>
    internal static class cspBegin
    {
        #region = МЕТОДЫ

        #region - Процедуры

        /// <summary>
        /// Точка входа
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetCompatibleTextRenderingDefault(false);

            /// ИСПРАВЛЕНО: 'appApplication.__oProtocols' по умолчанию равен 'new appProtocols()' (файловая
            /// реализация - см. 'appApplication.cs') и НИГДЕ не переопределялся на 'dsqProtocols'. Из-за
            /// этого 'cspApplication.__oProtocols as dsqProtocols' в 'cspFormMain.mProtocolsAutoLoad' ВСЕГДА
            /// давал [null] (приведение типов от базового класса к потомку невозможно) - автозагрузка
            /// каждый раз молча уходила в ветку "нет открытой базы", независимо от любых исправлений схемы
            cspApplication.__oProtocols = new dsqProtocols();

            if (cspApplication.__oEventsHandler.__mBegin())
            {
                cspFormLoader vFormLoad = new cspFormLoader();
                vFormLoad.ShowDialog();

                cspFormMain vFormMain = new cspFormMain();
                vFormMain.ShowDialog();

                cspApplication.__oEventsHandler.__mEnd();
            }
        }

        #endregion Процедуры

        #endregion МЕТОДЫ
    }
}

