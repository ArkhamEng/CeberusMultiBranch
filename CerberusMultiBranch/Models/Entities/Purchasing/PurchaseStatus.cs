using CerberusMultiBranch.Models.Entities.Config;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace CerberusMultiBranch.Models.Entities.Purchasing
{

    [Table("PurchaseStatus", Schema = "Purchasing")]
    public class PurchaseStatus:ISelectable
    {
        public PStatus PurchaseStatusId { get; set; }

        [MaxLength(30)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string Description { get; set; }

        public ICollection<PurchaseOrder> PurchaseOrders { get; set; }

        public int Id
        {
            get
            {
                return (int)this.PurchaseStatusId;
            }
        }
    }



    public enum PStatus:int
    {

        [Display(Name = "Pedido Cancelado")]
        Canceled = -4,

        [Display(Name = "Pedido Caducado")]
        Expired = -3,

        [Display(Name = "Error en e envio")]
        SendingFailed = -2,

        [Display(Name = "No Autorizado")]
        NotAuthorized = -1,

        [Display(Name = "En revisión")]
        InRevision = 1,

        [Display(Name = "Revisado")]
        Revised = 2,

        [Display(Name = "Autorizado")]
        Authorized = 3,

        [Display(Name = "En espera")]
        Watting  = 4,

        [Display(Name = "Recibido Parcial")]
        Partial = 5,

        [Display(Name = "Pedido Recibido")]
        Received = 6,

       
    }

}