
using CerberusMultiBranch.Models.Entities.Catalog;
using CerberusMultiBranch.Models.Entities.Config;
using CerberusMultiBranch.Support;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.Entities.Purchasing
{
    [Table("PurchaseOrder", Schema = "Purchasing")]
    public class PurchaseOrder
    {
        public int PurchaseOrderId { get; set; }

        [Display(Name = "Sucursal")]
        public int BranchId { get; set; }

        [Display(Name = "Proveedor")]
        public int ProviderId { get; set; }

        [Display(Name = "Folio")]
        public string Folio { get; set; }

        [Display(Name = "Estado de Compra")]
        public PStatus PurchaseStatusId { get; set; }

        [Display(Name="Tipo de Compra")]
        public PType PurchaseTypeId { get; set; }

        [Display(Name = "Método de Envío")]
        public int ShipMethodId { get; set; }

        [Display(Name = "Fecha de Pedido")]
        public DateTime OrderDate { get; set; }

        [Display(Name = "Fecha de Vencimiento")]
        public DateTime DueDate { get; set; }

        [Display(Name = "Fecha Envío")]
        public DateTime ShipDate { get; set; }

        [Display(Name = "Fecha de Entrega")]
        public DateTime DeliveryDate { get; set; }

        [DataType(DataType.Currency)]
        public double SubTotal { get; set; }

        public double TaxRate { get; set; }

        [DataType(DataType.Currency)]
        public double TaxAmount { get; set; }

        [DataType(DataType.Currency)]
        public double TotalDue { get; set; }


        [Display(Name = "Creado")]
        public DateTime InsDate { get; set; }

        [Display(Name = "Creador por")]
        public string   InsUser { get; set; }

        [Display(Name = "Editado")]
        public DateTime UpdDate { get; set; }

        [Display(Name = "Editado por")]
        public string UpdUser { get; set; }

        #region Navigation Properties
        public virtual Provider Provider { get; set; }

        public virtual Branch Branch { get; set; }

        public virtual PurchaseType PurchaseType { get; set; }

        public virtual PurchaseStatus PurchaseStatus { get; set; }

        public virtual ShipMethod ShipMethod { get; set; }

        public ICollection<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }
        #endregion

        public PurchaseOrder()
        {
            this.InsDate = DateTime.Now.ToLocal();
            this.UpdDate = DateTime.Now.ToLocal();
            this.InsUser = HttpContext.Current.User.Identity.Name;
            this.UpdUser = HttpContext.Current.User.Identity.Name;
        }
    }
}