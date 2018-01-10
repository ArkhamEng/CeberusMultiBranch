using CerberusMultiBranch.Models.Entities.Config;
using CerberusMultiBranch.Models.Entities.Operative;
using CerberusMultiBranch.Support;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CerberusMultiBranch.Models.ViewModels.Operative
{
    [NotMapped]
    public class SaleViewModel:Sale
    {
       public SelectList Systems { get; set; }

        public SelectList Categories { get; set; }

        public SelectList CarModels { get; set; }

        public SelectList CarMakes { get; set; }

        public SelectList CarYears { get; set; }
        public SaleViewModel()
        {
            this.CarMakes = new List<ISelectable>().ToSelectList();
            this.Categories = new List<ISelectable>().ToSelectList();
            this.CarModels = new List<ISelectable>().ToSelectList();
            this.CarYears = new List<ISelectable>().ToSelectList();
        }

        public SaleViewModel(Sale sale)
        {
            this.Branch = sale.Branch;
            this.BranchId = sale.BranchId;
            this.Client = sale.Client;
            this.ClientId = sale.ClientId;
            this.Folio = sale.Folio;
            this.LastStatus = sale.LastStatus;
            this.Status = sale.Status;
            this.Payments = sale.Payments;
            this.PaymentMethod = sale.PaymentMethod;
            this.TotalAmount = sale.TotalAmount;
            this.TransactionDate = sale.TransactionDate;
            this.SaleDetails = sale.SaleDetails;
            this.SaleId = sale.SaleId;
            this.UpdDate = sale.UpdDate;
            this.User = sale.User;
            this.UserId = sale.UserId;
            this.UpdUser = sale.UpdUser;
            this.Comment = sale.Comment;
            

            this.CarMakes = new List<ISelectable>().ToSelectList();
            this.Categories = new List<ISelectable>().ToSelectList();
            this.CarModels = new List<ISelectable>().ToSelectList();
            this.CarYears = new List<ISelectable>().ToSelectList();

        }
    }
}