using CerberusMultiBranch.Models.Entities.Catalog;
using CerberusMultiBranch.Support;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace CerberusMultiBranch.Models.Entities.Purchasing
{
    [Table("PurchaseOrderDetail", Schema = "Purchasing")]
    public class PurchaseOrderDetail
    {
        public int PurchaseOrderDetailId { get; set; }

        public int PurchaseOrderId { get; set; }

        public int ProductId { get; set; }

        public double OrderQty { get; set; }

        public double UnitPrice { get; set; }

        public double LineTotal { get; set; }

        public double ReceivedQty { get; set; }

        public double RejectedQty { get; set; }

        public double StokedQty { get; set; }

        [Display(Name = "Creado")]
        public DateTime InsDate { get; set; }

        [Display(Name = "Creador por")]
        public string InsUser { get; set; }

        [Display(Name = "Editado")]
        public DateTime UpdDate { get; set; }

        [Display(Name = "Editado por")]
        public string UpdUser { get; set; }


        #region Navigation Properties
        public virtual Product Product { get; set; }

        public virtual PurchaseOrder PurchaseOrder { get; set; }

        #endregion

        public PurchaseOrderDetail()
        {
            this.InsDate = DateTime.Now.ToLocal();
            this.UpdDate = DateTime.Now.ToLocal();
            this.InsUser = HttpContext.Current.User.Identity.Name;
            this.UpdUser = HttpContext.Current.User.Identity.Name;
        }

    }
}