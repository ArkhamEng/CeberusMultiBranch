﻿
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

        [Display(Name = "Factura")]
        public string Bill { get; set; }

        [Display(Name = "Estado de Compra")]
        public PStatus PurchaseStatusId { get; set; }

        [Display(Name = "Tipo de Compra")]
        public PType PurchaseTypeId { get; set; }

        [Display(Name = "Método de Envío")]
        public int ShipMethodId { get; set; }

        [Display(Name = "Fecha Pedido")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? OrderDate { get; set; }

        [Display(Name = "Fecha Vencimiento")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? DueDate { get; set; }

        [Display(Name = "Fecha Envío")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? ShipDate { get; set; }

        [Display(Name = "Fecha Entrega")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? DeliveryDate { get; set; }

        [Display(Name = "SubTotal")]
        [DataType(DataType.Currency)]
        public double SubTotal { get; set; }

        [Display(Name = "IVA")]
        public double TaxRate { get; set; }

        [Display(Name = "Monto IVA")]
        [DataType(DataType.Currency)]
        public double TaxAmount { get; set; }

        [Display(Name = "Total")]
        [DataType(DataType.Currency)]
        public double TotalDue { get; set; }

        [Display(Name = "Condiciones")]
        public int DaysToPay { get; set; }

        [Display(Name = "Costo de Envío")]
        public double Freight { get; set; }

        [Display(Name = "Seguro")]
        public double Insurance { get; set; }

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

        [Display(Name = "Comentario")]
        public string Comment { get; set; }

        public int Year { get; set; }

        public int Sequential { get; set; }

        #region Navigation Properties
        public virtual Provider Provider { get; set; }

        public virtual Branch Branch { get; set; }

        public virtual PurchaseType PurchaseType { get; set; }

        public virtual PurchaseStatus PurchaseStatus { get; set; }

        public virtual ShipMethod ShipMethod { get; set; }

        public ICollection<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }

        public ICollection<PurchaseOrderHistory> PurchaseOrderHistories { get; set; }

        public ICollection<Purchase> Purchases { get; set; }
        #endregion


        #region NotMapped

        public string DiscountAmount
        {
            get
            {
                var sub = (this.SubTotal + this.TaxAmount + this.Freight + this.Insurance);

                if (this.Discount > Cons.Zero)
                    return (sub * (this.Discount / Cons.OneHundred)).ToMoney();

                else
                    return ((double)Cons.Zero).ToMoney();
            }
        }

        public string Expenses
        {
            get { return (this.Freight + Insurance).ToMoney(); }
        }

        public bool CanSend
        {
            get
            {
                return (this.PurchaseStatusId == PStatus.Authorized ||
                        this.PurchaseStatusId == PStatus.SendingFailed &&
                        HttpContext.Current.User.IsInRole("Capturista"));
            }
        }

        public bool CanRevise
        {
            get
            {
                return (this.PurchaseStatusId == PStatus.InRevision &&
                    (HttpContext.Current.User.IsInRole("Almacenista") || HttpContext.Current.User.IsInRole("Administrador")));
            }
        }

        public bool CanAuthorize
        {
            get
            {
                return (this.PurchaseStatusId == PStatus.Revised &&
                    (HttpContext.Current.User.IsInRole("Supervisor") || HttpContext.Current.User.IsInRole("Administrador")));
            }
        }

        public bool CanReceive
        {
            get
            {
                return ((this.PurchaseStatusId == PStatus.Watting ||
                         this.PurchaseStatusId == PStatus.Partial) &&
                         (HttpContext.Current.User.IsInRole("Capturista") || HttpContext.Current.User.IsInRole("Administrador")));
            }
        }

        public bool CanViewDetail
        {
            get
            {
                return ((this.PurchaseStatusId >= PStatus.Watting &&
                    (HttpContext.Current.User.IsInRole("Capturista") || HttpContext.Current.User.IsInRole("Administrador"))));
            }
        }

        public string StatusStyle
        {
            get
            {
                //el si ya esta revisado y sigue el flujo normal, todo es verde, 
                //si se manda a revisar despues de haber sido autorizado el flujo será azul
                var style = "alert alert-success";

                switch (this.PurchaseStatusId)
                {
                    case PStatus.InRevision:
                        style = "alert alert-dark";
                        break;

                    case PStatus.Partial:
                        style = "alert alert-attention";
                        break;

                    case PStatus.SendingFailed:
                        style = "alert alert-warning";
                        break;

                    case PStatus.NotAuthorized:
                    case PStatus.Canceled:
                        style = "alert alert-danger";
                        break;

                    case PStatus.Authorized:
                    case PStatus.Revised:
                        if (!string.IsNullOrEmpty(this.Folio))
                            style = "alert alert-info";
                        break;
                }

                return style;
            }
        }

        public string FolioStyle
        {
            get
            {
                if (string.IsNullOrEmpty(this.Folio))
                    return "alert alert-dark";
                else
                    return "alert alert-success";
            }
        }

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