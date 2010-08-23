using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TimeTrakker
{
    /// <summary>
    /// Business object Factory class with static methods that
    /// feeds each of the business object classes
    /// </summary>
    public class TimeTrakkerFactory
    {
        public static busCustomer GetCustomer()
        {
            return new busCustomer();
        }

        public static busEntry GetEntry()
        {
            return new busEntry();
        }

        public static busInvoice GetInvoice()
        {
            return new busInvoice();
        }

        public static busProject GetProject()
        {
            return new busProject();
        }

        public static busUser GetUser()
        {
            return new busUser();
        }
    }
}