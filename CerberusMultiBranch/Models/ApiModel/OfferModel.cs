using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.ApiModel
{
    public class OfferModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string TextShadow { get; set; }

        public string TextColor { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public int Discount { get; set; }

        public double Saving { get; set; }

        public string Image { get; set; }
    }
}