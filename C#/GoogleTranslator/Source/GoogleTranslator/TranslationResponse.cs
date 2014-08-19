using System.Runtime.Serialization;

namespace AlexMG.GoogleTranslator
{
    /// <summary>
    ///     The translation response returned from Google.
    /// </summary>
    [DataContract(Name = "translateResponse")]
    public class TranslationResponse
    {
        /// <summary>
        ///     Gets or sets the response data.
        /// </summary>
        [DataMember(Name = "responseData")]
        public ResponseData ResponseData { get; set; }

        /// <summary>
        ///     Gets or sets the response details.
        /// </summary>
        /// <remarks>
        ///     This value is only present when the request fails
        ///     and will contain a diagnostic string.
        /// </remarks>
        [DataMember(Name = "responseDetails")]
        public string ResponseDetails { get; set; }

        /// <summary>
        ///     Gets or sets the response status.
        /// </summary>
        /// <remarks>
        ///     A status other than 200 indicates failure.
        /// </remarks>
        [DataMember(Name = "responseStatus")]
        public int ResponseStatus { get; set; }
    }
}