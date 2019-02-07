using CerberusMultiBranch.Models.Entities.Catalog;
using CerberusMultiBranch.Models.Entities.Operative;
using CerberusMultiBranch.Support;
using System;
using System.Collections.Generic;
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

        [Display(Name = "Producto")]
        public int ProductId { get; set; }

        [Display(Name = "Costo Unitario")]
        [DataType(DataType.Currency)]
        public double UnitPrice { get; set; }

    
        [Display(Name = "Pedidos")]
        public double OrderQty { get; set; }

        [Display(Name = "Recibidos")]
        public double ReceivedQty { get; set; }

        [Display(Name = "Complemento")]
        public double ComplementQty { get; set; }

        [Display(Name = "Inventariados")]
        public double StockedQty { get; set; }

        [Display(Name = "Comentario")]
        public string Comment { get; set; }

        public bool IsCompleated { get; set; }

        [Display(Name = "Total")]
        [DataType(DataType.Currency)]
        public double LineTotal { get; set; }

        [Display(Name = "Descuento")]
        public double Discount { get; set; }

        [Display(Name = "Creado")]
        public DateTime InsDate { get; set; }

        [Display(Name = "Creador por")]
        public string InsUser { get; set; }

        [Display(Name = "Editado")]
        public DateTime UpdDate { get; set; }

        [Display(Name = "Editado por")]
        public string UpdUser { get; set; }

        [NotMapped]
        public string ProviderCode { get; set; }

        #region Navigation Properties
        public virtual Product Product { get; set; }

        public virtual PurchaseOrder PurchaseOrder { get; set; }

        public ICollection<StockMovement> StockMovements { get; set; }


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