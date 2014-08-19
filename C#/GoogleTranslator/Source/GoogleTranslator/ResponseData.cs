using System.Runtime.Serialization;

namespace AlexMG.GoogleTranslator
{
    /// <summary>
    ///     The data part of the response from Google.
    /// </summary>
    [DataContract(Name = "responseData")]
    public class ResponseData
    {
        /// <summary>
        ///     Gets or sets the translated text.
        /// </summary>
        [DataMember(Name = "translatedText")]
        public string TranslatedText { get; set; }

        /// <summary>
        ///     Gets or sets the detected source language.
        /// </summary>
        /// <remarks>
        ///     This value is only present when the source language was not provided
        ///     in the request and needed to be detected automatically.
        /// </remarks>
        [DataMember(Name = "detectedSourceLanguage")]
        public string DetectedSourceLanguage { get; set; }
    }
}