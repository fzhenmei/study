using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using Westwind.BusinessFramework.LinqToSql;

namespace TimeTrakker
{
    public class busLookup : BusinessObjectLinq<LookupEntity, TimeTrakkerContext>
    {
        /// <summary>
        /// Returns a list of states
        /// </summary>
        /// <returns></returns>
        public IQueryable<StateListItem> GetStates()
        {
            return from c in this.Context.LookupEntities
                   where c.type == "STATE"
                   orderby c.cdata1
                   select new StateListItem { StateCode = c.cdata, State = c.cdata1 };
        }

        /// <summary>
        /// Returns an alphabethized list of countries
        /// </summary>
        /// <returns></returns>
        public IQueryable<CountryListItem> GetCountries()
        {
            return from c in this.Context.LookupEntities
                   where c.type == "COUNTRY"
                   orderby c.cdata1
                   select new CountryListItem { CountryCode =  c.cdata, Country = c.cdata1 };
        }


    }


    public class StateListItem
    {
        public string StateCode { get; set; }
        public string State { get; set; }
    }

    public class CountryListItem
    {
        public string CountryCode { get; set; }
        public string Country { get; set; }
    }
}
