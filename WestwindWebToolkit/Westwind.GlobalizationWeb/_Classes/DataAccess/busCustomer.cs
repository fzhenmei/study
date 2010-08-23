using System;
using System.Data;
using Westwind.BusinessFramework;
using System.Linq;

namespace Westwind.GlobalizationWeb
{

    /// <summary>
    /// Summary description for busCustomer
    /// </summary>
    public class busCustomer : Westwind.BusinessFramework.LinqToSql.BusinessObjectLinq<nw_Customer, NorthwindCustomersContext>
    {
        public busCustomer()
        {
        }


        /// <summary>
        /// Creates a TCustomerList Table of all customers in the local DataSet.
        /// </summary>
        /// <returns></returns>
        public IQueryable<nw_Customer> GetCustomerList()
        {
            return
                from c in this.Context.nw_Customers
                orderby c.CompanyName
                select c;
        }

        public override bool Validate()
        {
            if (!base.Validate())
                return false;

            nw_Customer Cust = this.Entity;

            this.ValidationErrors.Clear();

            if (Cust.CompanyName == null || Cust.CompanyName == string.Empty)
                this.ValidationErrors.Add("Company cannot be left blank");

            if (Cust.ContactName == null || Cust.ContactName == string.Empty)
                this.ValidationErrors.Add("Name cannot be left blank");

            if (Cust.Address.Length < 5)
                this.ValidationErrors.Add("Address is too short");

            if (this.ValidationErrors.Count > 0)
                return false;

            return true;
        }
    }
}