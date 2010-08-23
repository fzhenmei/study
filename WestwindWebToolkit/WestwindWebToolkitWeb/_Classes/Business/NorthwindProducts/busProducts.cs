using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Westwind.BusinessFramework.LinqToSql;

namespace Westwind.WebToolkit
{
    public class busProducts : BusinessObjectLinq<nw_Product,NorthwindProductsContext>
    {

        public IQueryable<nw_Product> GetAllProducts()
        {
            return
                from prod in this.Context.nw_Products
                orderby prod.ProductName.ToLower()
                select prod;
        }

        public IQueryable<nw_Category> GetAllCategories()
        {
            return
                from cat in this.Context.nw_Categories
                orderby cat.CategoryName.ToLower()
                select cat;                    
        }

        public IQueryable<nw_Supplier> GetAllSuppliers()
        {
            return
                from sup in this.Context.nw_Suppliers
                orderby sup.CompanyName
                select sup;
        }

        /// <summary>
        /// implement validation rule checking for this entity
        /// </summary>
        /// <param name="entity"></param>
        protected override void OnValidate(nw_Product entity)
        {
            base.OnValidate(entity);

            if (string.IsNullOrEmpty(entity.ProductName))
                this.ValidationErrors.Add("Product name should not be empty","txtProductName");

            if (entity.UnitsInStock < 0)
                this.ValidationErrors.Add("Stock values should not be smaller than 0","txtUnitsInStock");
            
        }
    }

}
