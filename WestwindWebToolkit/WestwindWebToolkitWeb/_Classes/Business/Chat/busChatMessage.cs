using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Linq;
using System.Data.Linq;

using Westwind.BusinessFramework.LinqToSql;
using System.Text;
using System.Collections.Generic;


namespace Westwind.WebToolkit
{
    /// <summary>
    /// The Chat Message Business object used to load and manage
    /// chat messages.
    /// </summary>
    public class busChatMessage : BusinessObjectLinq<ChatMessage,ChatContext>
    {
        /// <summary>
        /// Returns messages for a given ChatId since a given time
        /// </summary>
        /// <param name="since"></param>
        /// <param name="chatId"></param>
        /// <returns></returns>
        public IQueryable<ChatMessage> GetMessages(DateTime since,string chatId)
        {
            return 
                from msg in this.Context.ChatMessages
                where msg.Time > since && msg.ChatId == chatId
                select msg;     
        }

        /// <summary>
        /// Returns the last x messges for a give chat
        /// </summary>
        /// <param name="messageCount">number of last messages to retrieve</param>
        /// <param name="chatId"></param>
        /// <returns></returns>
        public IQueryable<ChatMessage> GetMessages(int messageCount,string chatId)
        {
            return
                (from msg in this.Context.ChatMessages
                where msg.ChatId == chatId
                select msg).Take(messageCount);     
        }

        public string GetMessagesHtml(DateTime since, string chatId)
        {
            IQueryable<ChatMessage> messages = this.GetMessages(since, chatId);
            var msgs = messages.Select(msg => new { Sender = msg.Sender, Message = msg.Message } );
 
            StringBuilder sb = new StringBuilder();
            foreach (var msg in msgs)
            {
                sb.Append("<div class='MessageWrapper'>\r\n");
                sb.Append("<div class='MessageAuthor'>" + msg.Sender + " says:</div>");
                sb.Append("<div class='Message'>" + msg.Message + "</div>\r\n");
                sb.Append("</div>");
            }

            if (sb.Length == 0)
                return null;

            return sb.ToString();
        }

        /// <summary>
        /// Get users that have been on in the last minute.
        /// 
        /// Should catch anybody who's still connected since the clients
        /// are pinging the server every few seconds.
        /// </summary>
        /// <param name="ChatId"></param>
        /// <returns></returns>
        public IQueryable<ChatUser> GetRecentUsers(string ChatId)
        {
            return
                from user in Context.ChatUsers
                where user.ChatId == ChatId && user.LastOn > DateTime.UtcNow.AddMinutes(-1)
                select user;

        }


        /// <summary>
        /// Writes user status to the user file.
        /// 
        /// Note this is for simplicity - this really should live in a separate class but is hooked here
        /// since this is hte only operation on the user context.
        /// </summary>
        /// <param name="chatId"></param>
        /// <param name="sender"></param>
        /// <param name="chatActitivityType"></param>
        /// <returns></returns>
        public int WriteChatActivity(string chatId, string sender,ChatActivityTypes chatActitivityType) 
        {
            int Result = 0;
            if (sender == "" || chatId == "")
                return Result;

            // Manual updates since I didn't map Users to business object - more efficient anyway
            if (chatActitivityType == ChatActivityTypes.ReadMessages)
                Result = this.ExecuteNonQuery("update ChatUsers " +
                                              "set LastOn=@Time " +
                                              "where ChatId=@ChatId AND Sender=@Sender",
                            Context.CreateParameter("@ChatId", chatId),
                            Context.CreateParameter("@Sender", sender),
                            Context.CreateParameter("@Time",DateTime.UtcNow) );

            else if (chatActitivityType == ChatActivityTypes.WriteMessage)
                Result = this.ExecuteNonQuery("update ChatUsers " +
                                              "set LastOn=@Time,LastPost=@Time " +
                                              "where ChatId=@ChatId AND Sender=@Sender",
                            Context.CreateParameter("@ChatId", chatId),
                            Context.CreateParameter("@Sender", sender),
                            Context.CreateParameter("@Time",DateTime.UtcNow) );                                  
            if (Result < 1)
                this.ExecuteNonQuery("insert into ChatUsers " +
                                  "(ChatId,Sender,LastOn,LastPost,SignedOn) VALUES " +
                                  "(@ChatId,@Sender,@Time,@Time,@Time)",
                            Context.CreateParameter("@ChatId", chatId),
                            Context.CreateParameter("@Sender", sender),
                            Context.CreateParameter("@Time", DateTime.UtcNow) );   
            
            return Result;       
        }

        /// <summary>
        /// Returns a list of active chats in a table called TChats
        /// </summary>
        /// <returns></returns>
        public List<string> GetChatList()
        {
            return
                (from user in Context.ChatUsers                
                where user.LastOn > DateTime.UtcNow.AddMinutes(-1)
                group user by user.ChatId into chats
                select chats.Key).ToList();            
        }

        /// <summary>
        /// Removes messages older than 15 minutes
        /// </summary>
        public void ClearMessages()
        {                        
            this.ExecuteNonQuery("delete from ChatMessages WHERE Time < @Time",
                                  this.Context.CreateParameter("@Time",DateTime.UtcNow.AddMinutes(-15)) );

            this.ExecuteNonQuery("delete from ChatActivity WHERE LastOn < @Time",
                       this.Context.CreateParameter("@Time", DateTime.UtcNow.AddMinutes(-15)));

        }

   }

   public enum ChatActivityTypes
   {
       ReadMessages,
       WriteMessage,
       ChatEntered
   }
}