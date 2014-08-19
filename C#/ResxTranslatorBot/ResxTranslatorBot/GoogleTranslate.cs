using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace ResxTranslator
{
	public static class GoogleTranslate
	{
        /// <summary>
        /// Translates a string into another language using Google's translate API JSON calls.
        /// <seealso>Class TranslationServices</seealso>
        /// </summary>
        /// <param name="Text">Text to translate. Should be a single word or sentence.</param>
        /// <param name="FromCulture">
        /// Two letter culture (en of en-us, fr of fr-ca, de of de-ch)
        /// </param>
        /// <param name="ToCulture">
        /// Two letter culture (as for FromCulture)
        /// </param>
        public static string TranslateGoogle(string text, string fromCulture, string toCulture)
        {
            fromCulture = fromCulture.ToLower();
            toCulture = toCulture.ToLower();

            // normalize the culture in case something like en-us was passed 
            // retrieve only en since Google doesn't support sub-locales
            string[] tokens = fromCulture.Split('-');
            if (tokens.Length > 1)
                fromCulture = tokens[0];

            // normalize ToCulture
            tokens = toCulture.Split('-');
            if (tokens.Length > 1)
                toCulture = tokens[0];

            string url = string.Format(@"http://translate.google.com/translate_a/t?client=j&text={0}&hl=en&sl={1}&tl={2}",
                                       HttpUtility.UrlEncode(text), fromCulture, toCulture);

            // Retrieve Translation with HTTP GET call
            string html = null;
            try
            {
                WebClient web = new WebClient();

                // MUST add a known browser user agent or else response encoding doen't return UTF-8 (WTF Google?)
                web.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0");
                web.Headers.Add(HttpRequestHeader.AcceptCharset, "UTF-8");

                // Make sure we have response encoding to UTF-8
                web.Encoding = Encoding.UTF8;
                html = web.DownloadString(url);
            }
            catch (Exception ex)
            {
            }

            // Extract out trans":"...[Extracted]...","from the JSON string
            string result = Regex.Match(html, "trans\":(\".*?\"),\"", RegexOptions.IgnoreCase).Groups[1].Value;

            if (string.IsNullOrEmpty(result))
            {
            }

            //return WebUtils.DecodeJsString(result);

            // Result is a JavaScript string so we need to deserialize it properly
            string result2 = result.Substring(1, result.Length - 2);

            result2.Trim();
            return result2;
        }

		/// <summary>
		/// Translate Text using Google Translate
		/// </summary>
		/// <param name="input">The string you want translated</param>
		/// <param name="languagePair">2 letter Language Pair, delimited by "|". 
		/// e.g. "en|da" language pair means to translate from English to Danish</param>
		/// <param name="encoding">The encoding.</param>
		/// <returns>Translated to String</returns>
		public static string TranslateText(string input, string languagePair)
		{
			//string url = String.Format("http://www.google.hr/translate_t?hl=hr&ie=UTF8&text={0}&langpair={1}", Uri.EscapeUriString(input).Replace("#", "%23"), languagePair);
            string url = String.Format("http://translate.google.com/?hl=en&ie=UTF8&text={0}&langpair={1}", Uri.EscapeUriString(input).Replace("#", "%23"), languagePair);


            var webClient = new WebClient { Encoding = Encoding.UTF8 };

            // MUST add a known browser user agent or else response encoding doen't return UTF-8 (WTF Google?)
            webClient.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0");
            webClient.Headers.Add(HttpRequestHeader.AcceptCharset, "UTF-8");

		    string result = webClient.DownloadStringUsingResponseEncoding(url);

            result = result.Substring(result.IndexOf("<span title=\"") + "<span title=\"".Length);
            result = result.Substring(result.IndexOf(">") + 1);
            result = result.Substring(0, result.IndexOf("</span>"));

		    return result.Trim();
		}

        private static String DecodeData(WebResponse w)
        {

            //
            // first see if content length header has charset = calue
            //
            String charset = null;
            String ctype = w.Headers["content-type"];
            if (ctype != null)
            {
                int ind = ctype.IndexOf("charset=");
                if (ind != -1)
                {
                    charset = ctype.Substring(ind + 8);
                    Console.WriteLine("CT: charset=" + charset);
                }
            }

            // save data to a memorystream
            MemoryStream rawdata = new MemoryStream();
            byte[] buffer = new byte[1024];
            Stream rs = w.GetResponseStream();
            int read = rs.Read(buffer, 0, buffer.Length);
            while (read > 0)
            {
                rawdata.Write(buffer, 0, read);
                read = rs.Read(buffer, 0, buffer.Length);
            }

            rs.Close();

            //
            // if ContentType is null, or did not contain charset, we search in body
            //
            if (charset == null)
            {
                MemoryStream ms = rawdata;
                ms.Seek(0, SeekOrigin.Begin);

                StreamReader srr = new StreamReader(ms, Encoding.ASCII);
                String meta = srr.ReadToEnd();

                if (meta != null)
                {
                    int start_ind = meta.IndexOf("charset=");
                    int end_ind = -1;
                    if (start_ind != -1)
                    {
                        end_ind = meta.IndexOf("\"", start_ind);
                        if (end_ind != -1)
                        {
                            int start = start_ind + 8;
                            charset = meta.Substring(start, end_ind - start + 1);
                            charset = charset.TrimEnd(new Char[] { '>', '"' });
                            Console.WriteLine("META: charset=" + charset);
                        }
                    }
                }
            }

            Encoding e = null;
            if (charset == null)
            {
                e = Encoding.ASCII; //default encoding
            }
            else
            {
                try
                {
                    e = Encoding.GetEncoding(charset);
                }
                catch (Exception ee)
                {
                    Console.WriteLine("Exception: GetEncoding: " + charset);
                    Console.WriteLine(ee.ToString());
                    e = Encoding.ASCII;
                }
            }

            rawdata.Seek(0, SeekOrigin.Begin);

            StreamReader sr = new StreamReader(rawdata, e);

            String s = sr.ReadToEnd();

            return s.ToLower();
        }

		private static void UnitTest()
		{
			string tst = TranslateText("hi there #firstname# ", "en|ru");
		}

		private static string DownloadStringUsingResponseEncoding(this WebClient client, string address)
		{
			if (client == null) throw new ArgumentNullException("client");
			return DownloadStringUsingResponseEncodingImpl(client, client.DownloadData(address));
		}

		private static string DownloadStringUsingResponseEncodingImpl(WebClient client, byte[] data)
		{
			Debug.Assert(client != null);
			Debug.Assert(data != null);

			var contentType = client.GetResponseContentType();

			Encoding encoding;
            if (contentType == null || string.IsNullOrEmpty(contentType.CharSet)) encoding = client.Encoding;
            else encoding = Encoding.GetEncoding(contentType.CharSet);

		    encoding = Encoding.GetEncoding("ISO-8859-1");

		    return encoding.GetString(data);
		}

		private static ContentType GetResponseContentType(this WebClient client)
		{
			if (client == null) throw new ArgumentNullException("client");

			var headers = client.ResponseHeaders;
			if (headers == null)
				throw new InvalidOperationException("Response headers not available.");

			var header = headers["Content-Type"];

			return !string.IsNullOrEmpty(header)
				 ? new ContentType(header)
				 : null;
		}
	}
}
