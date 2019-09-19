using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.ApiModel
{
    public class SearchFilter
    {
        public int Category { get; set; }

        public string Description { get; set; }

        public double MaxPrice { get; set; }

        public double MinPrice { get; set; }

        public List<string> TradeMarks { get; set; }

        public int Page { get; set; }

        public int PageSize { get; set; }
    }
}