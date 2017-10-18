using CerberusMultiBranch.Models.Entities.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.Entities.Operative
{
    public class Transference:Transaction
    {
        [ForeignKey("OriginBranch")]
        public int OriginBranchId { get; set; }

        public string AuthUser { get; set; }

        public DateTime AuthDate { get; set; }

        public virtual Branch OriginBranch { get; set; }
    }
}