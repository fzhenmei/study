#region License
/*
 **************************************************************
 *  Author: Rick Strahl 
 *          © West Wind Technologies, 2008 - 2009
 *          http://www.west-wind.com/
 * 
 * Created: 09/04/2008
 *
 * Permission is hereby granted, free of charge, to any person
 * obtaining a copy of this software and associated documentation
 * files (the "Software"), to deal in the Software without
 * restriction, including without limitation the rights to use,
 * copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following
 * conditions:
 * 
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
 * OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 * OTHER DEALINGS IN THE SOFTWARE.
 **************************************************************  
*/
#endregion

using System;
using System.Data;

using System.Text;
using System.Globalization;
using System.Reflection;
using System.Collections;
using System.IO;
using System.Collections.Specialized;

using System.Text.RegularExpressions;
using Westwind.Utilities;
using System.Web.Script.Serialization;

namespace Westwind.Web.JsonSerializers
{

    /// <summary>
    /// The high level JSON Serializer instance that provides 
    /// serialization and deserialization services to the application. This is 
    /// the core serializer that the application or components interact with.
    /// 
    /// This serializer defers operation to the specified JSON parsing implementation.
    /// Supported parsers include:
    /// 
    /// * West Wind Native that's built-in (no dependencies)   (This is the default)
    /// * JSON.NET   (requires JSON.NET assembly to be included and JSONNET_REFERENCE global Define
    /// * JavaScriptSerializer (ASP.NET JavaScript Serializer - requires System.Web.Extension + WEBEXTENSIONS_REFERENCE global Define)
    /// </summary>
    public class JSONSerializer
    {
        /// <summary>
        /// This property determines the default parser that is created when
        /// using the default constructor. This is also the default serializer
        /// used when using the AjaxMethodCallback control.
        /// 
        /// This property should be set only once at application startup typically
        /// </summary>
        public static SupportedJsonParserTypes DefaultJsonParserType = SupportedJsonParserTypes.WestWindJsonSerializer;

        private IJSONSerializer _serializer = null;

        /// <summary>
        /// Encodes Dates as a JSON string value that is compatible
        /// with MS AJAX and is safe for JSON validators. If false
        /// serializes dates as new Date() expression instead.
        /// 
        /// The default is true.
        /// </summary>
        public JsonDateEncodingModes DateSerializationMode
        {
            get { return _SerializeDateAsFormatString; }
            set { _SerializeDateAsFormatString = value; }
        }
        private JsonDateEncodingModes _SerializeDateAsFormatString = JsonDateEncodingModes.ISO;

        /// <summary>
        /// Determines if there are line breaks inserted into the 
        /// JSON to make it more easily human readable.
        /// </summary>
        public bool FormatJsonOutput
        {
            get { return _FormatJsonOutput; }
            set { _FormatJsonOutput = value; }
        }
        private bool _FormatJsonOutput = false;

        

        /// <summary>
        /// Default Constructor - assigns default 
        /// </summary>
        public JSONSerializer() : this(DefaultJsonParserType)
        { }

        public JSONSerializer(IJSONSerializer serializer)
        {
            _serializer = serializer;
        }

        public JSONSerializer(SupportedJsonParserTypes parserType)
        {
            // The West Wind Parser is native
            if (parserType == SupportedJsonParserTypes.WestWindJsonSerializer)
                _serializer = new WestwindJsonSerializer(this);

#if (JSONNET_REFERENCE)
            else if (parserType == SupportedJsonParserTypes.JsonNet)
                _serializer = new JsonNetJsonSerializer(this);
#endif
            else if (parserType == SupportedJsonParserTypes.JavaScriptSerializer)
                _serializer = new WebExtensionsJavaScriptSerializer(this);

            else
                throw new InvalidOperationException("Unsupported JSON Serializer specified. JsonNet and System.Web.Extensions must be explicitly compiled in.");
        }

        public string Serialize(object value)
        {
            return _serializer.Serialize(value);
        }

        public object Deserialize(string jsonString, Type type)
        {
            return _serializer.Deserialize(jsonString, type);
        }

        public TType Deserialize<TType>(string jsonString)
        {
            return (TType)Deserialize(jsonString, typeof( TType) );      
        }
    }

    public enum SupportedJsonParserTypes
    {
        WestWindJsonSerializer,
        JsonNet,
        JavaScriptSerializer
    }


    /// <summary>
    /// Enumeration that determines how JavaScript dates are
    /// generated in JSON output
    /// </summary>
    public enum JsonDateEncodingModes
    {
        NewDateExpression,
        MsAjax,
        ISO
    }
}
