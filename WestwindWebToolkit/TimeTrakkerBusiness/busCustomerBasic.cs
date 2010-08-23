#if false
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;

namespace TimeTrakker
{
    public class busCustomerBasic 
    {
        public TimeTrakkerContext context = null;
        
        public Customer Entity
        {
            get { return _Entity; }
            set { _Entity = value; }
        }
        private Customer _Entity = null;

        
        public string ErrorMessage
        {
            get { return _ErrorMessage; }
            set { _ErrorMessage = value; }
        }
        private string _ErrorMessage = "";

        
        public Exception ErrorException
        {
            get { return _ErrorException; }
            set { _ErrorException = value; }
        }
        private Exception _ErrorException = null;


        public busCustomerBasic()
        {
            this.context = new TimeTrakkerContext();
        }

        public Customer Load(int pk)
        {
            try
            {
                Customer cust = context.Customers.Single(c => c.Pk == pk);
                if (cust == null)
                {
                    this.SetError("Invalid Pk");
                    return null;
                }

                return cust;
            }
            catch (Exception ex)
            {
                this.SetError(ex);
                return null;
            }

            return null;
        }

        public Customer NewEntity()
        {            
            return new Customer();
        }

        public void Cancel()
        {            
            context = new TimeTrakkerContext();
        }

        public bool Save()
        {
            try
            {
                context.SubmitChanges();
            }
            catch(Exception ex)
            {
                this.SetError(ex);
                return false;
            }

            return true;
        }

        



        public void SetError(string Message)
        {
            this.ErrorException = null;

            if (string.IsNullOrEmpty(Message))
            {
                this.ErrorMessage = "";
                return;
            }

            this.ErrorMessage = Message;
        }

        public void SetError(Exception ex)
        {
            this.ErrorException = ex;
            if (ex == null)
               this.ErrorMessage = "";
            else
               this.ErrorMessage = ex.Message;
        }

        public bool Update(Customer customer)
        {
            context = new TimeTrakkerContext();
            
            //Customer Tcust = context.Customers.Single(c => c.Pk == customer.Pk);
            
            context.Customers.Attach(customer,true);
            customer.Address = customer.Address;            
            
            context.SubmitChanges();       
         
            return true;
        }

        public bool Insert(Customer customer)
        {
            context = new TimeTrakkerContext();
            context.Customers.Add(customer);
            context.SubmitChanges();

            return true;
        }        
    }
}

#endif