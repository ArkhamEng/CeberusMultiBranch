using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.Entities.Operative
{
    public class Purchase
    {
        public int PurchaseId { get; set; }

        public int ProviderId { get; set; }

        public int BranchId { get; set; }

        public DateTime InsDate { get; set; }

        public DateTime UpdDate { get; set; }

        public int  EmployeeId { get; set; }
    }

    public class PurchaseDetail
    {
        public int PurchaseDetailId { get; set; }

        public int PurchaseId { get; set; }

        public int ProductId { get; set; }

        public double Quantity { get; set; }

        public int MyProperty { get; set; }
    }
}