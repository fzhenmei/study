#region License
/*
 **************************************************************
 *  Author: Rick Strahl 
 *          © West Wind Technologies, 2009
 *          http://www.west-wind.com/
 * 
 * Created: 09/12/2009
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

using System.Collections;
using System.Text;

namespace Westwind.Utilities
{
	/// <summary>
	/// A collection of ValidationError objects that is used to collect
	/// errors that occur duing calls to the Validate method.
	/// </summary>
	public class ValidationErrorCollection : CollectionBase
	{
		
		/// <summary>
		/// Indexer property for the collection that returns and sets an item
		/// </summary>
		public ValidationError this[int index]
		{
			get 
			{
				return (ValidationError) List[index]; 
			}
			set 
			{
				List[index] = value;
			}
		}

		/// <summary>
		/// Adds a new error to the collection
		/// <seealso>Class ValidationError</seealso>
		/// </summary>
		/// <param name="Error">
		/// Validation Error object
		/// </param>
		/// <returns>Void</returns>
		public void Add(ValidationError Error) 
		{
			List.Add(Error);
		}



		/// <summary>
		/// Adds a new error to the collection
		/// <seealso>Class ValidationErrorCollection</seealso>
		/// </summary>
		/// <param name="Message">
		/// Message of the error
		/// </param>
		/// <param name="FieldName">
		/// optional field name that it applies to (used for Databinding errors on 
		/// controls)
		/// </param>
		/// <param name="ID">
		/// An optional ID you assign the error
		/// </param>
		/// <returns>Void</returns>
		public void Add(string Message,string FieldName, string ID) 
		{
			ValidationError Error = new ValidationError();
			Error.Message = Message;
			Error.ControlID = FieldName;
			Error.ID = ID;
			Add(Error);
		}

        /// <summary>
        /// Adds a new error to the collection
        /// <seealso>Class ValidationErrorCollection</seealso>
        /// </summary>
        /// <param name="Message">
        /// Message of the error
        /// </param>
        /// <returns>Void</returns>
        public void Add(string Message)
        {
            Add(Message, "", "");
        }



        /// <summary>
        /// Adds a new error to the collection
        /// </summary>
        /// <param name="Message">Message of the error</param>
        /// <param name="FieldName">optional field name that it applies to (used for Databinding errors on controls)</param>
        public void Add(string Message, string FieldName)
        {
            Add(Message, FieldName, "");
        }



        /// <summary>
        /// Removes the item specified in the index from the Error collection
        /// </summary>
        /// <param name="Index"></param>
        public void Remove(int Index)
        {
            if (Index > List.Count - 1 || Index < 0)
                List.RemoveAt(Index);
        }

		/// <summary>
		/// Adds a validation error if the condition is true. Otherwise no item is 
		/// added.
		/// <seealso>Class ValidationErrorCollection</seealso>
		/// </summary>
		/// <param name="Condition">
		/// If true this error is added. Otherwise not.
		/// </param>
		/// <param name="Message">
		/// The message for this error
		/// </param>
		/// <param name="FieldName">
		/// Name of the UI field (optional) that this error relates to. Used optionally
		///  by the databinding classes.
		/// </param>
		/// <param name="ID">
		/// An optional Error ID.
		/// </param>
		/// <returns>value of condition</returns>
		public bool Assert(bool Condition, string Message, string FieldName, string ID) 
		{
			if (Condition)
				Add(Message,FieldName,ID);

			return Condition;
		}

        /// <summary>
        /// Adds a validation error if the condition is true. Otherwise no item is 
        /// added.
        /// <seealso>Class ValidationErrorCollection</seealso>
        /// </summary>
        /// <param name="Condition">
        /// If true the Validation Error is added.
        /// </param>
        /// <param name="Message">
        /// The Error Message for this error.
        /// </param>
        /// <returns>value of condition</returns>
        public bool Assert(bool Condition, string Message)
        {
            if (Condition)
                Add(Message);

            return Condition;
        }

        /// <summary>
        /// Adds a validation error if the condition is true. Otherwise no item is 
        /// added.
        /// <seealso>Class ValidationErrorCollection</seealso>
        /// </summary>
        /// <param name="Condition">
        /// If true the Validation Error is added.
        /// </param>
        /// <param name="Message">
        /// The Error Message for this error.
        /// </param>
        /// <returns>string</returns>
        public bool Assert(bool Condition, string Message, string FieldName)
        {
            if (Condition)
                Add(Message, FieldName);

            return Condition;
        }


        /// <summary>
        /// Asserts a business rule - if condition is true it's added otherwise not.
        /// <seealso>Class ValidationErrorCollection</seealso>
        /// </summary>
        /// <param name="Condition">
        /// If this condition evaluates to true the Validation Error is added
        /// </param>
        /// <param name="Error">
        /// Validation Error Object
        /// </param>
        /// <returns>value of condition</returns>
        public bool Assert(bool Condition, ValidationError Error)
        {
            if (Condition)
                List.Add(Error);

            return Condition;
        }


		/// <summary>
		/// Returns a string representation of the errors in this collection.
		/// The string is separated by CR LF after each line.
		/// </summary>
		/// <returns></returns>
		public override string ToString() 
		{
			if (Count < 1)
				return "";

			StringBuilder sb = new StringBuilder(128);
			
			foreach(ValidationError Error in this) 
			{
				sb.Append(Error.Message);
				sb.Append("\r\n");
			}
			
			return sb.ToString();
		}

		/// <summary>
		/// Returns an HTML representation of the errors in this collection.
		/// The string is returned as an HTML list.
		/// </summary>
		/// <returns></returns>
		public string ToHtml() 
		{
			if (Count < 1)
				return "";

			StringBuilder sb = new StringBuilder(256);
			sb.Append("<ul>\r\n");

			foreach(ValidationError Error in this) 
			{
				sb.Append("<li> ");

				if (Error.ControlID != null && Error.ControlID != "")
					sb.Append("<a href='javascript:;' onclick=\"document.getElementById('" + Error.ControlID+ "').focus();document.getElementById('" + Error.ControlID + "').style.background='cornsilk';\" class='errordisplay' style='border:0px;text-decoration:none'>" + Error.Message + "</a>");
				else
					sb.Append(Error.Message);
                
                sb.Append("</li>\r\n");
			}

			sb.Append("</ul>\r\n");
			return sb.ToString();
		}
	}

	/// <summary>
	/// Object that holds a single Validation Error for the business object
	/// </summary>
	public class ValidationError 
	{
		
		/// <summary>
		/// The error message for this validation error.
		/// </summary>
		public string Message
		{
			get
			{
				return cMessage;
			}
			set
			{
				cMessage = value;
			}
		}
		string cMessage = "";

		/// <summary>
		/// The name of the field that this error relates to.
		/// </summary>
		public string ControlID 
		{
			get { return this .cFieldName; }
			set { cFieldName = value; }
		}
		string cFieldName = "";
		
		/// <summary>
		/// An ID set for the Error. This ID can be used as a correlation between bus object and UI code.
		/// </summary>
		public string ID 
		{
			get { return cID; }
			set { cID = value; }
		}
		string cID = "";
		
		public ValidationError() : base() {}
		public ValidationError(string message) 
		{
			Message = message;
		}
		public ValidationError(string message, string fieldName) 
		{
			Message = message;
			ControlID = fieldName;
		}
		public ValidationError(string message, string fieldName, string id) 
		{
			Message = message;
			ControlID = fieldName;
			ID = id;
		}

	}
}
