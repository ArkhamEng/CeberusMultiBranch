using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.Entities
{
    [Table("Branch", Schema = "Configuration")]
    public class Branch
    {
        public int BranchId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Address { get; set; }
    }
}