using System;
using System.DirectoryServices;

namespace nlSystem
{
    /* In Solution Explorer, right-click References, 

    * then click Add Reference. Then add a reference 

    * to the System.DirectoryServices.dll assembly. 

    */
    /// <summary>
    /// Добавление и удаление учетных записей пользователей
    /// </summary>
    public class sstAccounts
    {
        static DirectoryEntry AD;

        [STAThread]
        static void Main(string[] args)
        {
            AD = new DirectoryEntry("WinNT://" + Environment.MachineName + ", computer");
            ShowEntry("Users", "group");
            ShowEntry("Guest", "user");

            Console.Write("\nPress Enter to close this window...");
            Console.ReadLine();
        }

        static void ShowEntry(string entryname, string entrytype)
        {
            Console.WriteLine("\n" + entrytype + "." + entryname + ":\n");
            DirectoryEntry entry;
            try
            {
                entry = AD.Children.Find(entryname, entrytype);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }
            foreach (string name in entry.Properties.PropertyNames)
            {
                foreach (object value in entry.Properties[name])
                {
                    Console.WriteLine(name + " : " + value);
                }
            }
        }

        static void ListChildren()
        {
            foreach (DirectoryEntry child in AD.Children)
            {
                Console.WriteLine(child.Name);
            }
        }
    }
}
