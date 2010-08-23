using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Text;
using Westwind.BusinessFramework.LinqToSql;


namespace Westwind.WebToolkit
{
    public partial class busUser :  BusinessObjectLinq<User,StockContext>
    {
        
        /// <summary>
        /// Authenticates a user based on username and pathword.
        /// Returns a simple true or false
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool Authenticate(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                this.SetError("Username or password cannot be empty");
                return false;
            }                        

            return 
                this.Context.Users
                    .Where(user => user.Email == email && user.Password == password)
                    .Count() > 0;
        }

        /// <summary>
        /// Authenticates a user based on username and password and
        /// returns a user entity and sets this.Entity on the userobject
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public User AuthenticateAndLoad(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                this.SetError("Username or password cannot be empty");
                return null;
            }

            User userEntity =
                this.Context.Users
                .Where(user => user.Email == email && user.Password == password).FirstOrDefault();


            if (userEntity != null)
                this.Entity = userEntity;

            return userEntity;
        }


        /// <summary>
        /// Returns a user PK from an email address
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public int GetPkFromEmail(string email)
        {
            return this.Context.Users
                        .Where(user => user.Email == email)
                        .Select(user => user.Pk)
                        .SingleOrDefault();
        }


    }
}
