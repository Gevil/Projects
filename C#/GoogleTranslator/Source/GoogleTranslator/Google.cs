using System;
using System.Runtime.Serialization.Json;
using System.Threading;

using Microsoft.Http;

namespace AlexMG.GoogleTranslator
{
    /// <summary>
    ///     Wrapper for the Google AJAX Language API.
    /// </summary>
    public static class Google
    {
        /// <summary>
        ///     Translates the specified text.
        /// </summary>
        /// <param name="text">
        ///     The text to translate.
        /// </param>
        /// <param name="destinationLanguage">
        ///     The language to translate to.
        /// </param>
        /// <returns>
        ///     A response that includes the translated text and status information.
        /// </returns>
        /// <exception cref="ArgumentException">
        ///     Thrown if the text to translate or destination language is null or empty.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown if the HTTP response status code is not 200.
        /// </exception>
        /// <exception cref="ApplicationException">
        ///     Thrown if the response fails due to a non-communication related problem.
        ///     The response details returned from Google are included in the exception message.
        /// </exception>
        /// <remarks>
        ///     The source language will be automatically detected.
        /// </remarks>
        public static TranslationResponse Translate(string text, string destinationLanguage)
        {
            return Translate(text, Language.Unknown, destinationLanguage, TextFormat.Text);
        }

        /// <summary>
        ///     Translates the specified text.
        /// </summary>
        /// <param name="text">
        ///     The text to translate.
        /// </param>
        /// <param name="sourceLanguage">
        ///     The language to translate from.
        /// </param>
        /// <param name="destinationLanguage">
        ///     The language to translate to.
        /// </param>
        /// <returns>
        ///     A response that includes the translated text and status information.
        /// </returns>
        /// <exception cref="ArgumentException">
        ///     Thrown if the text to translate or destination language is null or empty.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown if the HTTP response status code is not 200.
        /// </exception>
        /// <exception cref="ApplicationException">
        ///     Thrown if the response fails due to a non-communication related problem.
        ///     The response details returned from Google are included in the exception message.
        /// </exception>
        /// <remarks>
        ///     If the source language is not provided it will be automatically detected.
        /// </remarks>
        public static TranslationResponse Translate(string text, string sourceLanguage, string destinationLanguage)
        {
            return Translate(text, sourceLanguage, destinationLanguage, TextFormat.Text);
        }

        /// <summary>
        ///     Translates the specified text.
        /// </summary>
        /// <param name="text">
        ///     The text to translate.
        /// </param>
        /// <param name="sourceLanguage">
        ///     The language to translate from.
        /// </param>
        /// <param name="destinationLanguage">
        ///     The language to translate to.
        /// </param>
        /// <param name="textFormat">
        ///     The format of the text to be translated.
        /// </param>
        /// <returns>
        ///     A response that includes the translated text and status information.
        /// </returns>
        /// <exception cref="ArgumentException">
        ///     Thrown if the text to translate or destination language is null or empty.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown if the HTTP response status code is not 200.
        /// </exception>
        /// <exception cref="ApplicationException">
        ///     Thrown if the response fails due to a non-communication related problem.
        ///     The response details returned from Google are included in the exception message.
        /// </exception>
        /// <remarks>
        ///     If the source language is not provided it will be automatically detected.
        /// </remarks>
        public static TranslationResponse Translate(string text, string sourceLanguage, string destinationLanguage, TextFormat textFormat)
        {
            if (string.IsNullOrEmpty(text)) throw new ArgumentException("The 'text' parameter is invalid.", "text");
            if (string.IsNullOrEmpty(destinationLanguage)) throw new ArgumentException("The 'destinationLanguage' parameter is invalid.", "destinationLanguage");

            HttpClient client = new HttpClient("http://ajax.googleapis.com/");

            HttpQueryString queryString = new HttpQueryString
            {
                {"v", "1.0"},
                {"hl", Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName},
                {"langpair", string.Format("{0}|{1}", sourceLanguage.ToLowerInvariant(), destinationLanguage.ToLowerInvariant())},
                {"q", text},
                {"format", textFormat.ToString().ToLower()}
            };

            Uri serviceUri = new Uri("ajax/services/language/translate", UriKind.Relative);

            HttpResponseMessage responseMessage = client.Get(serviceUri, queryString);
            responseMessage.EnsureStatusIsSuccessful();

            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(TranslationResponse));
            return (TranslationResponse)serializer.ReadObject(responseMessage.Content.ReadAsStream());
        }
    }
}