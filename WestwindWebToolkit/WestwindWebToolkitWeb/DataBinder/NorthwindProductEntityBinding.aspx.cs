using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Runtime.Serialization;
using Westwind.Utilities;

namespace Westwind.WebToolkit.DataBinder
{
    public partial class NorthwindProductEntityBinding : System.Web.UI.Page
    {
        // Used as BindingSource - make sure it's public so it's visible even low trust environments
        public busProducts Product;
        public int ProductID = -1;

        public CustomType customType = new CustomType();
        
        /// <summary>
        /// Determines whether a product has changed
        /// Used to avoid ViewState for event tracking and to know when to bind
        /// </summary>
        protected bool IsProductChange
        {
            get
            {
                if (Request.Form["__EVENTTARGET"].Contains("txtProductID"))
                    return true;

                return false;
            }                
        }


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);                       

            this.Product = new busProducts();

            // Bind the related lookup lists
            this.GetProductList();
                        
            IQueryable<nw_Supplier> supplierList = this.Product.GetAllSuppliers();
            IQueryable filteredSupplierList = supplierList.Select(supp => new { CompanyName = supp.CompanyName, SupplierID=supp.SupplierID });
            this.txtSupplierID.DataSource = this.Product.Converter.ToDataReader(filteredSupplierList);
            this.txtSupplierID.DataTextField = "CompanyName";
            this.txtSupplierID.DataValueField = "SupplierID";
            this.txtSupplierID.DataBind();

            IQueryable<nw_Category> categoryList = this.Product.GetAllCategories();
            IQueryable filteredCategoryList = categoryList.Select(cat => new { CategoryName = cat.CategoryName, CategoryID = cat.CategoryID });
            this.txtCategoryID.DataSource = this.Product.Converter.ToDataReader(filteredCategoryList);
            this.txtCategoryID.DataTextField = "CategoryName";
            this.txtCategoryID.DataValueField = "CategoryID";
            this.txtCategoryID.DataBind();
        }

        private void GetProductList()
        {
            IQueryable<nw_Product> productList = this.Product.GetAllProducts();
            IQueryable filteredProductList = productList.Select(prod => new { ProductName = prod.ProductName, ProductID = prod.ProductID });
            this.txtProductID.DataSource = this.Product.Converter.ToDataReader(filteredProductList);
            this.txtProductID.DataTextField = "ProductName";
            this.txtProductID.DataValueField = "ProductID";
            this.txtProductID.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ((WestWindWebToolkitMaster)this.Master).SubTitle = "Databinder Examples";

            string strId = Request.Params["Id"];
            if (string.IsNullOrEmpty(strId))
                strId = this.txtProductID.SelectedValue;
            
            if (strId == null || strId == "-1")  // Temporary New ID
            {
                if (this.Product.NewEntity() == null)
                {
                    this.ErrorDisplay.ShowError("Unable to create a new Product.");
                    return;
                }
            }
            else
            {
                int.TryParse(strId, out this.ProductID);
                if (this.Product.Load(this.ProductID) == null)
                {
                    this.ErrorDisplay.ShowError("Unable to load Product. Invalid Id.");
                    return;
                }
            }

            // bind only on first load or if the product is changed
            if (!this.IsPostBack || this.IsProductChange)
            {                
                this.DataBinder.DataBind();
            }

            // Manually bind this control always - it's ReadOnly and so doesn't post back
            this.DataBinder.GetDataBindingItem(this.txtPk).DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            // unbind back into the underlying data source: Product.Entity for most fields
            this.DataBinder.Unbind();
                        
            // check for binding errors and display if there's a problem
            if (this.DataBinder.BindingErrors.Count > 0)
            {
                this.ErrorDisplay.Text = this.DataBinder.BindingErrors.ToHtml();
                return;
            }

            // validate the business object - check product entity for rule violations
            if (!this.Product.Validate())
            {
                // Automatically add binding errors from ValidationErrors - requires
                // IList that has ControlID and Message properties
                this.DataBinder.AddValidationErrorsToBindingErrors(this.Product.ValidationErrors);

                // You can also manually add binding error messages and assign to a control
                //this.DataBinder.AddBindingError("Invalid Country Code",this.txtCountry);

                this.ErrorDisplay.Text = this.DataBinder.BindingErrors.ToHtml();
                return;
            }

            bool isNew = false;
            if (this.Product.Entity.ProductID < 1)
                isNew = true;

            if (!this.Product.Save())
            {
                this.ErrorDisplay.ShowError("Couldn't save Product:<br/>" + this.Product.ErrorMessage);
                return;
            }

            this.ErrorDisplay.ShowMessage("Product information has been saved." +
                                (this.chkShowValues.Checked ? "<p/>" + this.customType.ToString() : ""));

            if (isNew)
            {
                // rebind the product list
                this.GetProductList();

                // select the new product 
                this.txtProductID.SelectedValue = this.Product.Entity.ProductID.ToString();
            }

        }

        protected void btnNewProduct_Click(object sender, EventArgs e)
        {
            if (this.Product.NewEntity() == null)
            {
                this.ErrorDisplay.ShowError("Couldn't create new customer.");
                return;
            }

            // bind the new entity values to the page
            this.DataBinder.DataBind();
        }
    }


    /// <summary>
    /// Sample complex type to bind to which includes
    /// a few different types of values and nullables
    /// </summary>
    public class CustomType
    {
        public string StringVar = "Hello World";
        public DateTime ?DateVar = DateTime.Now.AddDays(-7);
        public int ?IntVar = null;
        public decimal ?DecimalVar = 112.33M;
        public float ?FloatVar = 112.33F;
        public bool BoolVar = false;

        public override string ToString()
        {
            return SerializationUtils.ObjectToString(this, "<br/>\r\n", ObjectToStringTypes.Fields);
        }
    }
}
