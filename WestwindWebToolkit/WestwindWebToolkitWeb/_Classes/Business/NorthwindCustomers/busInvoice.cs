using System.Data;
using Westwind.BusinessFramework.LinqToSql;
using System.Text;
using Westwind.WebToolkit.NorthwindCustomers;

namespace Westwind.WebToolkit
{

    /// <summary>
    /// Summary description for busInvoice
    /// </summary>
    public class busInvoice : BusinessObjectLinq<nw_Order, NorthwindCustomersContext>
    {
        /// <summary>
        /// Creates a DataTable called TInvoiceList in the internal DataSet
        /// </summary>
        /// <param name="MaxInvoices"></param>
        /// <returns></returns>
        public DataTable GetLastInvoices(int MaxInvoices)
        {
            if (MaxInvoices == 0)
                MaxInvoices = 100;

            string Sql = @"
SELECT  TOP " + MaxInvoices.ToString() + @"
	MAX(nw_Orders.OrderID) AS OrderId, MAX(nw_Customers.CompanyName) AS Company, MAX(nw_Customers.ContactName) AS Name, MAX(nw_Orders.OrderDate) AS OrderDate, 
    SUM( (nw_OrderDetails.UnitPrice * [nw_OrderDetails].Quantity) )  AS OrderTotal, Max(freight) as Freight
FROM nw_Customers INNER JOIN
     nw_Orders   ON  nw_Customers.CustomerID = nw_Orders.CustomerID INNER JOIN
     nw_OrderDetails on nw_Orders.OrderID = nw_OrderDetails.OrderID
group by nw_Orders.Orderid
order by OrderDate Desc
";

            return this.Context.ExecuteDataTable(Sql, "TInvoiceList");
        }
        /// <summary>
        /// Returns a list of on invoices in a DataTable called TInvoiceList
        /// 
        /// Old code that - not converted to a LINQ query
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public DataTable GetInvoicesForCustomer(string CustomerId)
        {
            string Sql =
    @"
SELECT  
	MAX(nw_Orders.OrderID) AS OrderId, MAX(nw_Customers.CompanyName) AS Company, MAX(nw_Customers.ContactName) AS Name, MAX(nw_Orders.OrderDate) AS OrderDate, 
    SUM( (nw_OrderDetails.UnitPrice * [nw_OrderDetails].Quantity) )  AS OrderTotal, Max(freight) as Freight
FROM nw_Customers INNER JOIN
     nw_Orders   ON  nw_Customers.CustomerID = nw_Orders.CustomerID INNER JOIN
     nw_OrderDetails on nw_Orders.OrderID = nw_OrderDetails.OrderID
WHERE nw_Orders.CustomerId=@CustomerId
group by nw_Orders.Orderid
order by OrderDate Desc
";

            return Context.ExecuteDataTable(Sql, "TCustomerInvoiceList", Context.CreateParameter("@CustomerId", CustomerId));
        }

        //    /// <summary>
        //    /// Displays the lineitems of the currently loaded invoice
        //    /// </summary>
        //    /// <param name="OrderId"></param>
        //    /// <returns></returns>
        //    public string HtmlLineItems()
        //    {
        //        int OrderId = this.Entity.OrderID;


        //        int Result = this.Execute(@"
        //select OrderId,Products.ProductId, ProductName, [order details].UnitPrice,Quantity, (Quantity * [Order details].UnitPrice) as ItemTotal 
        // from [Order Details],Products
        //where OrderId=@OrderId and [Order Details].OrderId = @OrderId and [Order Details].ProductId = Products.ProductId","TLineItems",
        //this.CreateParameter("@OrderId",OrderId) );     

        //        if (Result < 0) 
        //        {
        //            return "";
        //        }             

        //        StringBuilder sb = new StringBuilder();

        //        sb.Append("<table class='blackborder' width='500' cellpadding='4' bgcolor='white'>");
        //        sb.Append("<tr class='gridheader'><th>Description</th><th>Qty</th><th>Price</th><th>Total</th></tr>");

        //        bool AlternateRow = false;
        //        decimal OrderTotal = 0.00M;
        //        foreach (DataRow dr in this.DataSet.Tables["TLineItems"].Rows)
        //        {
        //            sb.Append("<tr" + (AlternateRow ? " class='gridalternate'" : "") + "><td>" + dr["productname"] + "</td><td align='center'>" +  dr["Quantity"].ToString() + "</td><td align='right'>" + ((decimal)dr["UnitPrice"]).ToString("n2") + "</td><td align='right'>" + ((decimal)dr["ItemTotal"]).ToString("n2") + "</td></tr>");

        //            AlternateRow = !AlternateRow;
        //            OrderTotal += (decimal)dr["ItemTotal"];
        //        }

        //        decimal Freight = ((decimal)this.DataRow["Freight"]);

        //        // *** Separator line
        //        sb.Append("<tr><td colspan='4' class='blockheader' style='height:1px;border:0px;padding:0px;'></td></tr>");

        //        sb.Append("<tr style='border-top:1px solid black'><td colspan='3' align='right'>Shipping:</td><td align='right'>" + Freight.ToString("n2") + "</td></tr>");
        //        sb.Append("<tr style='border-top:1px solid black'><td colspan='3' align='right'><b>Order Total:</b></td><td align='right' style='background:silver;color:maroon;font-weight:bold'>" + (OrderTotal + Freight).ToString("c2") + "</td></tr>");        
        //        sb.Append("</table>");
        //        return sb.ToString();                                       
        //    }


        /// <summary>
        /// Returns an invoice list for the current invoice.
        /// 
        /// Doesn't belong here but since this is used for demos
        /// in various places this is an easy way to create the detail
        /// data quickly and reusably.
        /// </summary>
        /// <returns></returns>
        public string HtmlLineItems()
        {
            StringBuilder sb = new StringBuilder();

            var lineItems = this.Entity.nw_OrderDetails;

            if (lineItems.Count < 1)
                return "";

            sb.Append("<table class='blackborder' cellspacing='0' style='background: white;width: 100%;border: 0px;'>");
            sb.Append("<tr class='dialog-header'><th>Description</th><th>Qty</th><th>Price</th><th>Total</th></tr>");

            bool AlternateRow = false;
            decimal OrderTotal = 0.00M;

            foreach (nw_OrderDetail item in lineItems)
            {
                decimal itemTotal = item.Quantity * item.UnitPrice;
                sb.Append("<tr" + (AlternateRow ? " class='gridalternate'" : "") + "><td>" +
                    item.nw_Product.ProductName + "</td><td align='center'>" +
                    item.Quantity.ToString() + "</td><td align='right'>" +
                    item.UnitPrice.ToString("n2") + "</td><td align='right'>" +
                    itemTotal.ToString("n2") + "</td></tr>");

                AlternateRow = !AlternateRow;
                OrderTotal += itemTotal;
            }

            // *** Separator line
            sb.Append("<tr><td colspan='4' class='blockheader' style='height:1px;border:0px;padding:0px;'></td></tr>");

            sb.Append("<tr style='border-top:1px solid black'><td colspan='3' align='right'>Shipping:</td><td align='right'>" + this.Entity.Freight.Value.ToString("n2") + "</td></tr>");
            sb.Append("<tr style='border-top:1px solid black'><td colspan='3' align='right'><b>Order Total:</b></td><td align='right' style='background:silver;color:maroon;font-weight:bold'>" + (OrderTotal + this.Entity.Freight).Value.ToString("c2") + "</td></tr>");
            sb.Append("</table>");

            return sb.ToString();
        }


    }
}