using System;
using System.Web;

using System.Collections.Generic;
using Westwind.Utilities;
using System.Data;
using Westwind.Web.Controls;

using System.Text;
using System.Web.SessionState;
using System.Linq;
using Westwind.Web;


namespace Westwind.WebToolkit
{

    /// <summary>
    /// Web Service that services the ATLAS Chat.aspx page for reading and retrieving
    /// messages.
    /// 
    /// Note that some of these methods explicitly break common 'service rules' by
    /// returning content that is very specific to the client. This was a concious
    /// choice to keep the code simple to use on the client by providing some pre
    /// formatting of the data server side. In a real life scenario there probably 
    /// should be two sets of services - one for pure data services and one for
    /// application specific responses like the ones used here. In the future it
    /// might be possible with ATLAS to application service methods into the page
    /// itself using the page level [WebMethod] attribute while still getting 
    /// the lightweight model of only passing the parameter signatures (as opposed
    /// to also passing Page data) and thereby making that distinction more explicit.
    /// </summary>
    public class ChatService : CallbackHandler, IRequiresSessionState
    {
        public ChatService()
        {
        }

        /// <summary>
        /// Called when a new message is written from the client to
        /// the server.
        /// </summary>
        /// <param name="MessageText"></param>
        /// <param name="Handle"></param>
        /// <param name="ChatId"></param>
        /// <returns></returns>
        [CallbackMethod]
        public bool WriteMessage(string MessageText, string Handle, string ChatId)
        {
            busChatMessage Message = new busChatMessage();

            Message.WriteChatActivity(ChatId, Handle, ChatActivityTypes.WriteMessage);

            if (MessageText == "")
            {
                // Clear out old message
                Message.ClearMessages();
                return true;
            }

            Message.NewEntity();
            string Encoded = StringUtils.DisplayMemoEncoded(MessageText);
            Message.Entity.Message = StringUtils.ExpandUrls(Encoded, "_blank");
            Message.Entity.Sender = Handle;
            Message.Entity.ChatId = ChatId;
            Message.Entity.Time = DateTime.UtcNow;

            if (!Message.Save())
                throw new ApplicationException(Message.ErrorMessage);

            return true;
        }

        /// <summary>
        /// Retrieves all messages since the last time on and returns the
        /// messages in HTML format for direct appending to the Message label.
        /// 
        /// Uses Session to track the last time the user was on.
        /// </summary>
        /// <param name="ChatId"></param>
        /// <param name="Username"></param>
        /// <returns></returns>
        [CallbackMethod]
        public string GetMessages(string ChatId, string Username)
        {
            DateTime LastOn;
            string SessionKey = ChatId + "_" + Username + "_LastOn";

            object TVal = HttpContext.Current.Session[SessionKey];
            if (TVal == null)
                LastOn = DateTime.UtcNow.AddMinutes(-5);
            else
                LastOn = (DateTime)TVal;

            busChatMessage Message = new busChatMessage();

            Message.WriteChatActivity(ChatId, Username, ChatActivityTypes.ReadMessages);

            string Result = Message.GetMessagesHtml(LastOn, ChatId);            

            HttpContext.Current.Session[SessionKey] = DateTime.UtcNow;

            return Result;
        }

        /// <summary>
        /// Returns a simple HTML Link list of all users that are currently active.
        /// Users are considered active if they have posted in less than 10 minutes
        /// and 'passive' if they are just lurking. There are essentially three modes:
        /// 
        /// Active  (green image)
        /// Recently Active (orange image)
        /// Lurking  (red image)
        /// </summary>
        /// <param name="ChatId"></param>
        /// <returns></returns>
        [CallbackMethod]
        public string GetActiveUsers(string ChatId)
        {
            busChatMessage Message = new busChatMessage();

            IQueryable<ChatUser> messages = Message.GetRecentUsers(ChatId);
            
            StringBuilder sb = new StringBuilder();
            foreach (ChatUser msg in messages) 
            {
                String Sender = msg.Sender;
                if (Sender == "")
                    continue;

                DateTime LastOn = msg.LastOn;
                DateTime LastPost = msg.LastPost;
                sb.Append("<div ");

                if (LastPost.CompareTo(DateTime.UtcNow.AddMinutes(-5)) >= 0)
                    sb.Append("class='ActiveUser' ");
                    //sb.Append(@"<img src='../images/ChatLive.gif'>");
                else if (LastPost.CompareTo(DateTime.UtcNow.AddMinutes(-10)) >= 0)
                    sb.Append("class='InactiveUser' ");
                    //sb.Append(@"<img src='../images/ChatNotActive.gif'>");
                else if (LastOn.CompareTo(DateTime.UtcNow.AddMinutes(-1)) >= 0)
                    sb.Append("class='OfflineUser' ");
                    //sb.Append(@"<img src='../images/ChatOff.gif'>");
                else
                    continue;

                sb.Append(">" + Sender + "</div>");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Returns a list of links for all the active chats available
        /// as an HTML <br> separated list. This list is used by the
        /// client to pop up a selection drop down below ChatId field.
        /// </summary>
        /// <returns></returns>
        [CallbackMethod]
        public string GetChatListHtml()
        {
            busChatMessage Message = new busChatMessage();

            List<string> chats = Message.GetChatList();
            if (chats == null)
                return Message.ErrorMessage;

            if (chats.Count < 1)
                return "";

            StringBuilder sb = new StringBuilder();
            foreach (string chat in chats)
            {                             
                sb.AppendFormat(@"<a href='javascript:SetChatId(""{0}"")'>{1}</a><br>", chat,chat);
            }

            return sb.ToString();
        }
    }
}