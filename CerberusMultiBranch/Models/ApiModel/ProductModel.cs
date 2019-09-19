using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.ApiModel
{
    public class CategoryModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<ProductModel> Products {get; set; }
    }

    public class ProductModel
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string TradeMark { get; set; }

        public string Unit { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public List<string> Images { get; set; }

        public double Stock  { get; set; }

        public CategoryModel Category { get; set; }

        public List<StockInformation> StockInfo { get; set; }

        public ProductModel()
        {
            this.Category = new CategoryModel();
            this.StockInfo = new List<StockInformation>();
        }
    }

    public class StockInformation
    {
        public double Quantity { get; set; }

        public string BranchName { get; set; }
    }
}