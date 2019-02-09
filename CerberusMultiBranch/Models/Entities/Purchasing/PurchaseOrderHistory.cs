using CerberusMultiBranch.Support;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.Entities.Purchasing
{
    [Table("PurchaseOrderHistory", Schema = "Purchasing")]
    public class PurchaseOrderHistory
    {
        public int PurchaseOrderId { get; set; }

        public int PurchaseOrderHistoryId { get; set; }

        [Display(Name = "Comentario")]
        public string Comment { get; set; }

        [Display(Name = "Estado de Compra")]
        [MaxLength(50)]
        public string Status { get; set; }

        [Display(Name = "Tipo de Compra")]
        [MaxLength(50)]
        public string Type { get; set; }

        [Display(Name = "Método de Envío")]
        [MaxLength(50)]
        public string ShipMethod { get; set; }

        [Display(Name = "Fecha Inicio")]
        public DateTime BeginDate { get; set; }

        [Display(Name = "Fecha Fin")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Usuario")]
        [MaxLength(50)]
        public string ModifyByUser { get; set; }

        public virtual PurchaseOrder PurchaseOrder { get; set; }

        public PurchaseOrderHistory()
        {
            this.EndDate = DateTime.Now.ToLocal();
        }

    }
}