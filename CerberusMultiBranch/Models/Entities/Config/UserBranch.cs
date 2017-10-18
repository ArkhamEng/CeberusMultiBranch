using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.Entities.Config
{
    [Table("UserBranch", Schema = "Config")]
    public class UserBranch
    {
        [Column(Order = 0), Key, ForeignKey("Branch")]
        public int BranchId { get; set; }

        [Column(Order = 1), Key, ForeignKey("User")]
        [MaxLength(128)]
        public string UserId { get; set; }

        public virtual Branch Branch { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}