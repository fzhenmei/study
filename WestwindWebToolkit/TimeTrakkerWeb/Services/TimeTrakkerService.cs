using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TimeTrakker;
using System.Data;
using System.Web.Script.Services;
using System.ServiceModel.Web;
using Westwind.Utilities;

namespace TimeTrakkerWeb
{
    [ServiceContract( Namespace="", Name="TimeTrakkerService")]
    public interface ITimeTrakkerService
    {
        [OperationContract]
        //[WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, 
        //           ResponseFormat = WebMessageFormat.Json, 
        //           UriTemplate = "helloworld/{name}")]
        string Helloworld(string name);

        //[WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest,
        //        ResponseFormat = WebMessageFormat.Json,
        //        UriTemplate = "loadcustomer")]
        //[WebGet(BodyStyle = WebMessageBodyStyle.WrappedRequest,
        //        ResponseFormat = WebMessageFormat.Json,
        //        UriTemplate = "loadcustomerGet/{Pk}")] 
        [OperationContract]        
        CustomerEntity LoadCustomer(string Pk);

        [OperationContract]
        List<CustomerEntity> GetCustomerList();

        //[OperationContract]
        //bool SaveCustomer(CustomerEntity customer);        
    }

    public class TimeTrakkerService : ITimeTrakkerService
    {
        public string Helloworld(string name)
        {
            return "Hello World, " + name;
        }

        public CustomerEntity LoadCustomer(string Pk)
        {
            busCustomer Customer = new busCustomer();
            Customer.Options.ThrowExceptions = true;

            CustomerEntity cust = Customer.Load(int.Parse(Pk));

            //cust.InvoiceEntities = null;
            //cust.ProjectEntities = null;            

            return cust;
        }

        public List<CustomerEntity> GetCustomerList()
        {
            busCustomer Customer = TimeTrakkerFactory.GetCustomer();
            return Customer.GetCustomerList().ToList();
        }

        public bool SaveCustomer(CustomerEntity custEntity)
        {
            busCustomer Customer = TimeTrakkerFactory.GetCustomer();

            // *** Save Single Entity
            return Customer.Save(custEntity);
        }

        public DataSet ReturnDs()
        {
            busCustomer Customer = new busCustomer();
            object query = Customer.GetRecentCustomers(10);

            DataTable table =  Customer.Converter.ToDataTable(query,"TCustomers");

            DataSet dataset = new DataSet();
            dataset.Tables.Add(table);

            return dataset;

            //return new DataSet();
        }

        
        public SimpleObject ReturnSimpleObject()
        {
            return new SimpleObject();        
        }
    }

    [DataContract]
    public class SimpleObject
    {
        [DataMember]
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        private string _Name = "Rick";


        [DataMember]
        public string Company
        {
            get { return _Company; }
            set { _Company = value; }
        }
        private string _Company = "West Wind";

        [DataMember]        
        public Address Address
        {
            get { return _Address; }
            set { _Address = value; }
        }
        private Address _Address = new Address();

        
    }

    [DataContract]
    public class Address
    {

        [DataMember]
        public string Street
        {
            get { return _Street; }
            set { _Street = value; }
        }
        private string _Street = "32 Kaiea";

        [DataMember]
        public string City
        {
            get { return _City; }
            set { _City = value; }
        }
        private string _City = "Paia";

        [DataMember]
        public string State
        {
            get { return _State; }
            set { _State = value; }
        }
        private string _State = "HI";

    }

}
