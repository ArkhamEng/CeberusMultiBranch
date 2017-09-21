using CerberusMultiBranch.Models.Entities.Catalog;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.Entities.Config
{
    [Table("EmployeeBranch", Schema = "Config")]
    public class EmployeeBranch
    {
        [Column(Order = 0), Key, ForeignKey("Branch")]
        public int BranchId { get; set; }

        [Column(Order = 1), Key, ForeignKey("Employee")]
        public int EmployeeId { get; set; }

        public virtual Branch Branch { get; set; }

        public virtual Employee Employee { get; set; }
    }
}