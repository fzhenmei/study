using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using Westwind.BusinessFramework.LinqToSql;

namespace TimeTrakker
{
    public class busUser : BusinessObjectLinq<UserEntity, TimeTrakkerContext>
    {
        
       /// <summary>
       /// Loads an individual User by id
       /// </summary>
       /// <param name="userId"></param>
       /// <returns>Entity or Null</returns>
        public UserEntity LoadUserByUserId(string userId)
        {                
            return this.Context.UserEntities.SingleOrDefault( u => u.UserId == userId);
        }

        /// <summary>
        /// Loads and Authenticates a user based on userId and password.       
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        /// <returns>Entity or null on failure</returns>
        public UserEntity AuthenticateAndLoad(string userId, string password)
        {
            this.Entity  = this.Context.UserEntities.SingleOrDefault(u => u.UserId == userId && u.Password == password);
            return this.Entity;
        }

        /// <summary>
        /// Saves user settings for a given user
        /// </summary>
        /// <param name="LastCustomerPk"></param>
        /// <param name="LastProjectPk"></param>
        /// <returns></returns>
        public bool SaveUserPreferences(int userPk, int lastCustomerPk, int lastProjectPk)
        {
            if (this.Load(userPk) != null)
            {
                this.Entity.LastProject = lastProjectPk;
                this.Entity.LastCustomer = lastCustomerPk;
                this.Save();
            }

            return false;
        }

    }
}
