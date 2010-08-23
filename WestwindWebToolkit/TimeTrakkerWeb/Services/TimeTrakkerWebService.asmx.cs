using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Xml.Linq;
using TimeTrakker;
using System.Collections.Generic;
using Westwind.Utilities;

namespace TimeTrakkerWeb.Services
{
    /// <summary>
    /// Summary description for TimeTrakkerWebService
    /// </summary>
    [WebService(Namespace = "http://www.west-wind.com/timetrakker")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class TimeTrakkerWebService : System.Web.Services.WebService
    {


        [WebMethod]
        public List<CustomerListResult> GetCustomerList()
        {
            busCustomer Customer = TimeTrakkerFactory.GetCustomer();

            IQueryable<CustomerEntity> q = Customer.GetCustomerList();

            // *** Select just a subset so we push minimal data to the client
            IQueryable<CustomerListResult> cl =
                q.Select(c => new CustomerListResult()
                {
                    Company = c.Company,
                    Pk = c.Pk,
                    FirstName = c.FirstName,
                    LastName = c.LastName
                });

            return cl.ToList();
        }

        [WebMethod]
        public CustomerEntity GetCustomer(int custPk)
        {
            busCustomer Customer = TimeTrakkerFactory.GetCustomer();

            if (custPk == 0)
                // *** Allow creation of new customer entity
                Customer.NewEntity();
            else
            {
                if (Customer.Load(custPk) == null)
                    new SoapException("Invalid Customer ID passed", SoapException.ServerFaultCode);
            }

            return Customer.Entity;
        }

        [WebMethod]
        public bool SaveCustomer(CustomerEntity customer)
        {
            busCustomer Customer = TimeTrakkerFactory.GetCustomer();
            return Customer.Save(customer);
        }

        [WebMethod]     
        public List<ProjectListResult> GetProjects(int custPk)
        {
            busProject project = TimeTrakkerFactory.GetProject();
            IQueryable<ProjectListResult> query;
            if (custPk > 0)
                query = project.GetProjectsForCustomer(custPk)
                               .Select(p => new ProjectListResult { ProjectName = p.ProjectName, Pk = p.Pk } );
            else
                query = project.GetOpenProjects()
                               .Select(p => new ProjectListResult { ProjectName = p.ProjectName, Pk = p.Pk }); ;

            return query.ToList();
        }

        [WebMethod]
        public bool PunchIn(DateTime DateIn, string TimeIn,
                            string Title, string Description, 
                            int ProjectPk, int CustomerPk)
        {
            busEntry Entry = TimeTrakkerFactory.GetEntry();

            Entry.NewEntity();

            Entry.Entity.Title = Title;
            Entry.Entity.Description = Description;
            Entry.Entity.TimeIn = TimeUtils.DateTimeFromDateAndTime(DateIn,TimeIn);
            Entry.Entity.ProjectPk = ProjectPk;
            Entry.Entity.CustomerPk = CustomerPk;
            
            // *** Hard coded for demos
            Entry.Entity.UserPk = 1;

            Entry.PunchIn();

            return true;
        }

        [WebMethod]
        public EntryEntity GetEntry(int entryPk)
        {
            busEntry Entry = TimeTrakkerFactory.GetEntry();

            if (Entry.Load(entryPk) == null )
                throw new SoapException("Invalid Entry",SoapException.ServerFaultCode);

            Entry.Entity.UserEntity = null;
            Entry.Entity.ProjectEntity.Customer = null;            

            return Entry.Entity;
        }


        [WebMethod]
        public bool PunchOut(int entryPk, string Title, string Description, 
                             DateTime DateIn, string TimeIn,
                             DateTime DateOut, string TimeOut,
                             int ProjectPk, int CustomerPk)
        {
            busEntry Entry = TimeTrakkerFactory.GetEntry();

            EntryEntity entity = Entry.Load(entryPk);
            if (entity == null)
                throw new SoapException("Invalid Entry", SoapException.ServerFaultCode);

            entity.Title = Title;
            entity.Description = Description;
            entity.TimeIn = TimeUtils.DateTimeFromDateAndTime(DateIn,TimeIn);
            entity.TimeOut = TimeUtils.DateTimeFromDateAndTime(DateOut,TimeOut);
            entity.CustomerPk = CustomerPk;
            entity.ProjectPk = ProjectPk;

            if (Entry.Entity.Rate == 0.00M)
                Entry.Entity.Rate = 150.00M;
            
            
            if (Entry.Entity.UserPk == 0)
                Entry.Entity.UserPk = 1;  // Hardcoded for demos

            return Entry.PunchOut();
        }

        [WebMethod]
        public List<EntryListResult> GetRecentEntries()
        {
            busEntry entry = TimeTrakkerFactory.GetEntry();

            // *** Hard coded for a single user in this demo
            IQueryable<EntryEntity> entryList = entry.GetEntries(1);

            // *** Create  a concrete list with custom type selection applied
            List<EntryListResult> entries = entryList.Take(20).Select(c => 
                  new EntryListResult() { Pk = c.Pk, Title = c.Title, 
                            TimeIn=c.TimeIn, PunchedOut=c.PunchedOut, 
                            TimeOut=c.TimeOut,
                            TotalHours = c.TotalHours,}  ).ToList();

            // *** Update the time strings for the client explicitly
            // *** Note: Can't do in the query because function not support in LINQ TO SQL
            foreach (EntryListResult e in entries)
            {
                e.TimeInString = TimeUtils.ShortDateString(e.TimeIn, true);
                e.TimeOutString = TimeUtils.ShortDateString(e.TimeOut, true);
            }

            return entries;
        }
    }

    /// <summary>
    /// Customized result set that returns only the specified result data.
    /// Notice: It's best to explicitly declare the type rather than return
    /// an anonymous type so that the type shows up in the WSDL!
    /// </summary>
    public class CustomerListResult
    {
        public string Company;
        public int Pk;
        public string FirstName;
        public string LastName;
    }

    public class EntryListResult
    {
        public int Pk;
        public string Title;
        public DateTime TimeIn;
        public DateTime TimeOut;
        public decimal TotalHours;
        public bool PunchedOut;

        public string TimeInString;
        public string TimeOutString;
    }

}
