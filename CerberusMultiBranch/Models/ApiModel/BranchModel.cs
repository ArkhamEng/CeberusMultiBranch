using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.ApiModel
{
    public class BranchModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; } 

        public string Image { get; set; }


    }
}