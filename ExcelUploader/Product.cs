using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelUploader
{
    public class Product
    {
        public int ProductId { get; set; }

        public int CategoryId { get; set; }

        public int PartSystemId { get; set; }

        public string Code { get; set; }

        public string Reference { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        
        public double MinQuantity { get; set; }

        public string BarCode { get; set; }


        public double BuyPrice { get; set; }

        public int StorePercentage { get; set; }


        public int DealerPercentage { get; set; }

        public int WholesalerPercentage { get; set; }

        public double StorePrice { get; set; }

        public double WholesalerPrice { get; set; }


        public double DealerPrice { get; set; }

        
        public double MinimunPrice { get; set; }

        
        public string TradeMark { get; set; }

        
        public string Unit { get; set; }

     
      
    }
}
