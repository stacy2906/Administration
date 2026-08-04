using System.Globalization;
using System.Speech.Synthesis;

namespace nlResourcesSounds
{
    /// <summary>
    /// Файл rssVoice.cs
    /// </summary>
    /// <remarks>Класс-тип голос в операционной системе</remarks>
    public class rssVoice
    {
        #region = ПОЛЯ

        #region - Атрибуты

        /// <summary>
        /// Возраст
        /// </summary>
        public VoiceAge __fAge = VoiceAge.NotSet;
        /// <summary>
        /// Культура (государственность)
        /// </summary>
        public CultureInfo __fCulture = CultureInfo.CurrentCulture;
        /// <summary>
        /// Описание
        /// </summary>
        public string __fDescription = "";
        /// <summary>
        /// Пол
        /// </summary>
        public VoiceGender __fGender = VoiceGender.NotSet;
        /// <summary>
        /// Идентификатор
        /// </summary>
        public string __fId = "";

        /// <summary>
        /// Название
        /// </summary>
        public string __fName = "";

        #endregion Атрибуты

        #endregion ПОЛЯ
    }
}
