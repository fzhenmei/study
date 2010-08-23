using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Text;
using Westwind.BusinessFramework.LinqToSql;
using System.Data.SqlClient;

namespace Westwind.WebToolkit
{
    public class busUserToken : BusinessObjectLinq<UserToken,StockContext>
    {
        
        public int TokenTimeout
        {
          get { return _TokenTimeout; }
          set { _TokenTimeout = value; }
        }
        private int _TokenTimeout = 10000;


        /// <summary>
        /// Create a new token and write into database
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public string CreateToken(User user)
        {
            return this.CreateToken(user.Pk);
        }

        public bool UpdateTokenExpiration(string token)
        {
            int count = 
                this.ExecuteNonQuery("update " + this.TableInfo.Tablename + " set Expires=@Expires where token=@Token",
                    new SqlParameter("@Expires",DateTime.Now.AddDays(7)),
                    new SqlParameter("@Token",token) );

            if (count < 0)
                return false;
            return true;
        }

        
        public string CreateToken(int userPk)
        {
            string id = this.CreateTokenString();

            UserToken token = this.NewEntity();
            token.UserPk = userPk;
            token.Token = id;
            token.Entered = DateTime.Now;
            token.Expires = DateTime.Now.AddMinutes(this.TokenTimeout);

            if (!this.Save())
                return null;

            return id;
        }           

        /// <summary>
        /// Retrieves a user object from a token
        /// </summary>
        /// <param name="userToken"></param>
        /// <returns></returns>
        public User GetUserFromToken(string userToken)
        {
            busUser User = new busUser();

            User userEntity = 
                (from user in User.Context.Users
                     join tk in User.Context.UserTokens 
                     on user.Pk equals tk.UserPk
                where tk.Token == userToken && tk.Expires.CompareTo(DateTime.Now) > -1
                select user).FirstOrDefault();

            return userEntity;
        }

        /// <summary>
        /// Returns a token from a given user
        /// </summary>
        /// <param name="userPk"></param>
        /// <returns></returns>
        public string GetTokenFromUser(int userPk)
        {
            string token = 
             (from tk in this.Context.UserTokens
             where tk.UserPk == userPk &&
                 tk.Expires.CompareTo(DateTime.Now) > -1
             select tk.Token).FirstOrDefault();

            return token;
        }

        /// <summary>
        /// Retrieves a token for a user or creates a new token if it doesn't exist
        /// </summary>
        /// <param name="userPk"></param>
        /// <returns></returns>
        public string GetOrCreateToken(int userPk)
        {
            string token = this.GetTokenFromUser(userPk);
            if (string.IsNullOrEmpty(token))
                token = this.CreateToken(userPk);
            else
                this.UpdateTokenExpiration(token); // rolling expiration

            return token;
        }

        /// <summary>
        /// Checks to see whether a token is valid
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool ValidateToken(string userToken)
        {
            busUser User = new busUser();

            int count =
                (from user in User.Context.Users
                     join token in User.Context.UserTokens 
                     on user.Pk equals token.UserPk
                where token.Token == userToken && 
                      token.Expires.CompareTo(DateTime.Now) < 1
                 select user).Count();
            
            if (count > 0)
                return true;
            
            return false;
        }

        public bool PublicValidateToken(string token)
        {
            return true;
        }

        public void DeleteToken(string Token)
        {

        }

        /// <summary>
        /// Deletes timeout tokens
        /// </summary>
        /// <returns></returns>
        public int DeleteExpiredTokens()
        {
            return this.ExecuteNonQuery("delete from " + this.TableInfo.Tablename + " where expires > @expires",                                
                                 this.Context.CreateParameter("@expires",DateTime.Now));
        }

        /// <summary>
        /// Creates an actual token
        /// </summary>
        /// <returns></returns>
        protected string CreateTokenString()
        {
            return Guid.NewGuid().GetHashCode().ToString("x");
        }



    }
}
