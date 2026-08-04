using nlApplication; 

namespace nlAdministration
{
	public class admAbout : appInterfaceAbout
	{
		/// <summary>
		/// Название и расширение файла помощи приложения
		/// </summary>
		public string __fHelpFileName_ { get { return "Administration.chm"; } }
		/// <summary>
		/// Пакет приложений которому принадлежит приложение
		/// </summary>
		public string __fPacket_ { get { return "Administration"; } }
		/// <summary>
		/// Производственная версия продукта
		/// </summary>
		public string __fProductionVersion_ { get { return "6.0"; } }
		/// <summary>
		/// Префикс файлов используемых в приложении и создаваемых приложением
		/// </summary>
		public string __fPrefix_ { get { return "adm"; } }
	}
}

