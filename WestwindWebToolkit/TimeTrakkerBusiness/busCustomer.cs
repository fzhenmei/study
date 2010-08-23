using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using Westwind.BusinessFramework.LinqToSql;

namespace TimeTrakker
{
  
    public class busCustomer : BusinessObjectLinq<CustomerEntity, TimeTrakkerContext>
    {

        /// <summary>
        /// Returns a query result of recent customers
        /// </summary>
        /// <param name="customerCount"></param>
        /// <returns></returns>
        public IQueryable<CustomerEntity> GetRecentCustomers(int customerCount)
        {
            

            
            
            IQueryable<CustomerEntity> query = from c in Context.CustomerEntities
                                                orderby c.LastOrder descending
                                                select c;
            query.Take(customerCount);
            return query;
        }

        /// <summary>
        /// Returns a full list of customers with company and name properties only set
        /// </summary>
        /// <returns></returns>
        public IQueryable<CustomerEntity> GetCustomerList()
        {
            return
                from c in this.Context.CustomerEntities
                orderby c.Company
                select c;
        }

        /// <summary>
        /// Assign defaults to Customer entity
        /// </summary>
        /// <returns></returns>
        public override CustomerEntity NewEntity()
        {
            CustomerEntity entity = base.NewEntity();
            if (entity == null)    
                return null;
            
            entity.Entered = DateTime.Now;
            entity.Updated = DateTime.Now;
            entity.LastOrder = App.MIN_DATE_VALUE;
            entity.BillingRate = 150M;
            entity.CountryId = "US";
            entity.State = "CA";

            return entity;
        }

        public override bool Save()
        {
            string country = App.CountryList.Where(c => c.CountryCode == this.Entity.CountryId).Single().Country as string ?? string.Empty;
            this.Entity.Country = country;

            return base.Save();
        }


        protected override void OnValidate(CustomerEntity entity)
        {            
            if (string.IsNullOrEmpty(entity.Company))
                this.ValidationErrors.Add("Company should be filled in.", "txtCompany");

            if (string.IsNullOrEmpty(entity.Email))
                this.ValidationErrors.Add("Email address cannot be omitted.", "txtEmail");

            if (entity.Entered < App.MIN_DATE_VALUE)
                this.ValidationErrors.Add("Entered date must be after " + App.MIN_DATE_VALUE.ToShortDateString());

            if (entity.Updated < App.MIN_DATE_VALUE)
                this.ValidationErrors.Add("Updated date must be after " + App.MIN_DATE_VALUE.ToShortDateString());

            if (entity.LastOrder < App.MIN_DATE_VALUE)
                this.ValidationErrors.Add("Last Order Date must be after " + App.MIN_DATE_VALUE.ToShortDateString());
        }

        
    }
}
