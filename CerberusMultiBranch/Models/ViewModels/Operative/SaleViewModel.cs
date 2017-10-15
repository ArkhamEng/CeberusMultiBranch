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
            this.EmployeeName = sale.EmployeeName;
            this.EmployeeId = sale.EmployeeId;
            this.Folio = sale.Folio;
            this.IsPayed = sale.IsPayed;
            this.Compleated = sale.Compleated;
            this.Payments = sale.Payments;
            this.PaymentType = sale.PaymentType;
            this.TotalAmount = sale.TotalAmount;
            this.TransactionDate = sale.TransactionDate;
            this.TransactionDetails = sale.TransactionDetails;
            this.TransactionId = sale.TransactionId;
            this.UpdDate = sale.UpdDate;
            this.User = sale.User;
            this.UserId = sale.UserId;

            this.CarMakes = new List<ISelectable>().ToSelectList();
            this.Categories = new List<ISelectable>().ToSelectList();
            this.CarModels = new List<ISelectable>().ToSelectList();
            this.CarYears = new List<ISelectable>().ToSelectList();

        }
    }
}