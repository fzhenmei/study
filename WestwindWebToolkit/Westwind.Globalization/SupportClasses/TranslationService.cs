using System;
using System.Collections.Generic;
using System.Text;
using Westwind.InternetTools;
using System.Web;
using Westwind.Utilities;

namespace Westwind.Globalization
{
    /// <summary>
    /// Provides basic translation features via several Web interfaces
    /// 
    /// NOTE: These services may change their format or otherwise fail.
    /// </summary>
    public class TranslationServices
    {
        /// <summary>
        /// Error message set when an error occurs in the translation service
        /// </summary>
        public string ErrorMessage
        {
            get { return _ErrorMessage; }
            set { _ErrorMessage = value; }
        }
        private string _ErrorMessage = "";

        /// <summary>
        /// Timeout for how long to wait for a translation
        /// </summary>
        public int TimeoutSeconds
        {
            get { return _TimeoutSeconds; }
            set { _TimeoutSeconds = value; }
        }
        private int _TimeoutSeconds = 10;


        /// <summary>
        /// Translates a string into another language using Google's Translation Pages.
        /// 
        /// &lt;&lt;a href=&quot;http://translate.google.com/translate_t&quot; 
        /// target=&quot;top&quot;&gt;&gt;http://translate.google.com/translate_t&lt;&l
        /// t;/a&gt;&gt;
        /// <seealso>Class TranslationServices</seealso>
        /// </summary>
        /// <param name="Text"></param>
        /// <param name="FromCulture">
        /// Two letter culture (en of en-us, fr of fr-ca, de of de-ch)
        /// </param>
        /// <param name="ToCulture">
        /// Two letter culture
        /// </param>
        public string TranslateGoogle(string Text, string FromCulture, string ToCulture)
        {
            FromCulture = FromCulture.ToLower();
            ToCulture = ToCulture.ToLower();

            string[] Tokens = FromCulture.Split('-');
            if (Tokens.Length > 1)
                FromCulture = Tokens[0];

            Tokens = ToCulture.Split('-');
            if (Tokens.Length > 1)
                ToCulture = Tokens[0];

            string LangPair = FromCulture + "|" + ToCulture;

            HttpClient Http = new HttpClient();
            Http.Timeout = this.TimeoutSeconds;
            
            // hl=en&ie=UTF8&text=The+big+black+apple+turned+green&langpair=en%7Cde
            Http.AddPostKey("text", Text);
            Http.AddPostKey("langpair", LangPair);
            Http.AddPostKey("ie", "UTF-8");
            Http.AddPostKey("hl", "en");

            string Html = Http.GetUrl("http://www.google.com/translate_t");
            if (string.IsNullOrEmpty(Html))
            {
                this.ErrorMessage = Http.ErrorMessage;
                return null;
            }
            
            // <div id=result_box dir="ltr">Bereit</div>
            string Result = StringUtils.ExtractString(Html,"<div id=result_box dir=\"ltr\">","</div>");

            if (Result == "")
            {
                this.ErrorMessage = "Invalid search result. Couldn't find marker.";
                return null;
            }

            return HttpUtility.HtmlDecode(Result);            
        }

        /// <summary>
        /// Translates a string using Yahoo's Babel fish service.
        /// 
        /// &lt;&lt;a href=&quot;http://babelfish.yahoo.com/&quot; 
        /// target=&quot;top&quot;&gt;&gt;http://babelfish.yahoo.com/&lt;&lt;/a&gt;&gt;
        /// <seealso>Class TranslationServices</seealso>
        /// </summary>
        /// <param name="Text">
        /// The text to translate
        /// </param>
        /// <param name="FromCulture">
        /// Two letter culture (en of en-us, fr of fr-ca, de of de-ch)
        /// </param>
        /// <param name="ToCulture">
        /// Two letter culture
        /// </param>
        public string TranslateBabelFish(string Text, string FromCulture, string ToCulture)
        {
            FromCulture = FromCulture.ToLower();
            ToCulture = ToCulture.ToLower();

            string[] Tokens = FromCulture.Split('-');
            if (Tokens.Length > 1)
                FromCulture = Tokens[0];

            Tokens = ToCulture.Split('-');
            if (Tokens.Length > 1)
                ToCulture = Tokens[0];

            string LangPair = FromCulture + "_" + ToCulture;

            HttpClient Http = new HttpClient();

            // ei=UTF-8&fr=bf-badge&trurl=&trtext=Hello&lp=en_de&submit=Translate
            Http.AddPostKey("ei", "UTF-8");
            Http.AddPostKey("fr", "bf-badge");
            Http.AddPostKey("trtext", Text);
            Http.AddPostKey("lp", LangPair);
            Http.AddPostKey("submit", "Translate");

            string Html = Http.GetUrl("http://babelfish.yahoo.com/translate_txt");
            if (string.IsNullOrEmpty(Html))
            {
                this.ErrorMessage = Http.ErrorMessage;
                return null;
            }
            
            // <div id="result"><div style="padding:0.6em;">Hallo</div></div>
            string Result = StringUtils.ExtractString(Html, "<div id=\"result\">", "</div>");
            if (Result == "")
            {
                this.ErrorMessage = "Invalid search result. Couldn't find marker.";
                return null;
            }
            Result = Result.Substring(Result.LastIndexOf(">") + 1);

            return HttpUtility.HtmlDecode(Result); 
        }


    }
}
