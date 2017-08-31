using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.Entities.Config
{
    [Table("Company", Schema = "Config")]
    public class Company
    {
        public int CompanyId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string WebSite { get; set; }
    }
}