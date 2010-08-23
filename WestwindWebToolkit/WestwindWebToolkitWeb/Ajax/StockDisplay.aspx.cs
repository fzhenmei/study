using System;

namespace Westwind.WebToolkit
{

    public partial class StockDisplay : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            string action = Request.Params["Action"];
            if (action == "Chart")
                this.GetStockChart();

            this.GetStockQuote();
        }


        /// <summary>
        /// Demonstrates how to generically return a JSON response
        /// using the ASP.NET AJAX JsonSerializer 
        /// (there are better ways to do this but `this demonstrates
        ///  how you can do this easily from any callback)
        /// </summary>
        void GetStockQuote()
        {
            string symbol = Request.Params["Symbol"] ?? "";

            StockServer stockServer = new StockServer();
            StockQuote quote = stockServer.GetStockQuote(symbol);
            if (quote == null)
            {
                Response.Write("<div class='errordisplay'>Unable to load stock data for " + symbol + ".</div>");
                Response.End();
            }

            this.lblCompany.Text = quote.Company + " - " + quote.Symbol;
            this.lblPrice.Text = quote.LastPrice.ToString();
            this.lblOpenPrice.Text = quote.OpenPrice.ToString();
            this.lblNetChange.Text = quote.NetChange.ToString();
            this.lblQuoteTime.Text = quote.LastQuoteTimeString;

            // this will call this page to retrieve the chart    
            this.imgStockQuoteGraph.ImageUrl = "StockDisplay.aspx?Symbol=" + quote.Symbol + "&action=Chart";
        }

        void GetStockChart()
        {
            string symbol = Request.Params["Symbol"] ?? "";

            StockServer stockServer = new StockServer();
            byte[] img = stockServer.GetStockHistoryGraph(new string[1] { symbol },
                                              "Stock History for " + symbol,
                                              400, 250, 2);
            Response.ContentType = "application/jpg";
            Response.BinaryWrite(img);
            Response.End();
        }

    }
}
