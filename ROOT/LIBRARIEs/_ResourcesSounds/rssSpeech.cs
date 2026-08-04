using nlResourcesSounds;
using System.Collections.Generic;
using System.Globalization;
using System.Speech.AudioFormat;
using System.Speech.Synthesis;

namespace nlResourcesSounds
{
    // Разговор с компьютером https://habrahabr.ru/post/125432/
    public class rssSpeech
    {
        /// <summary>
        /// Озвучивание текста
        /// </summary>
        /// <param name="pText">Текст для озвучивания</param>
        /// <param name="pVoiceName">Название языка</param>
        public void _mSpeech(string pText, string pVoiceName = "")
        {
            SpeechSynthesizer vSpeechSynthesizer = new SpeechSynthesizer();
            /// Чтение списка установленных голосовых движков
            if (pVoiceName.Trim().Length != 0)
            {
                foreach (InstalledVoice vVoice in vSpeechSynthesizer.GetInstalledVoices())
                {
                    if (vVoice.VoiceInfo.Culture.ToString() == pVoiceName)
                    {
                        vSpeechSynthesizer.SelectVoice(vVoice.VoiceInfo.Name);
                    }
                }
            }
            vSpeechSynthesizer.SetOutputToDefaultAudioDevice();
            vSpeechSynthesizer.Speak(pText);
        }
        /// <summary>
        /// Озвучивание текста ассинхронное
        /// </summary>
        /// <param name="pText">Текст для озвучивания</param>
        /// <param name="pCulture">Название языка</param>
        public void _mSpeechAsynk(string pText, string pCulture = "")
        {
            SpeechSynthesizer vSpeechSynthesizer = new SpeechSynthesizer();
            /// Чтение списка установленных голосовых движков
            if (pCulture.Trim().Length == 0)
            {
                foreach (InstalledVoice vVoice in vSpeechSynthesizer.GetInstalledVoices())
                {
                    if (vVoice.VoiceInfo.Culture.ToString() == pCulture)
                    {
                        vSpeechSynthesizer.SelectVoice(vVoice.VoiceInfo.Name);
                    }
                }
            }
            vSpeechSynthesizer.SetOutputToDefaultAudioDevice();
            vSpeechSynthesizer.SpeakAsync(pText);
        }
        public void __mSpeechToFile(string pText, string pPathFile, string pCulture = "")
        {
            if (pCulture.Trim().Length == 0)
                pCulture = "es-ES";

            using (SpeechSynthesizer oSpeechSynthesizer = new SpeechSynthesizer())
            {
                /// Конфигурирование аудио
                oSpeechSynthesizer.SetOutputToWaveFile(pPathFile, new SpeechAudioFormatInfo(32000, AudioBitsPerSample.Sixteen, AudioChannel.Mono));
                /// Выбор культуры
                CultureInfo oCultureInfo = new CultureInfo(pCulture, false);
                PromptBuilder oPromptBuilder = new PromptBuilder(oCultureInfo);
                /// Подключение текста
                oPromptBuilder.AppendText(pText);

                // Произношение текста
                oSpeechSynthesizer.Speak(oPromptBuilder);
            }
        }
        /// <summary>
        /// Получение списка установленных голосов
        /// </summary>
        public List<rssVoice> __mVoiceList()
        {
            List<rssVoice> vReturn = new List<rssVoice>();
            using (SpeechSynthesizer synth = new SpeechSynthesizer())
            {
                /// Чтение списка установленных голосовых движков
                foreach (InstalledVoice oInstalledVoice in synth.GetInstalledVoices())
                {
                    rssVoice oVoice = new rssVoice(); // Тип - голос
                    oVoice.__fAge = oInstalledVoice.VoiceInfo.Age;
                    oVoice.__fCulture = oInstalledVoice.VoiceInfo.Culture;
                    oVoice.__fDescription = oInstalledVoice.VoiceInfo.Description;  
                    oVoice.__fGender = oInstalledVoice.VoiceInfo.Gender;
                    oVoice.__fId = oInstalledVoice.VoiceInfo.Id;
                    oVoice.__fName = oInstalledVoice.VoiceInfo.Name;
                    vReturn.Add(oVoice);
                }
            }

            return vReturn;
        }
    }
}
