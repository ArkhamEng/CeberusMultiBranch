﻿using CerberusMultiBranch.Models.Entities.Catalog;
using CerberusMultiBranch.Models.Entities.Config;
using CerberusMultiBranch.Support;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.Entities.Operative
{
    [Table("Budget", Schema = "Operative")]
    public class Budget
    {
        [Display(Name = "Folio")]
        public int BudgetId { get; set; }

        [Display(Name = "Sucursal")]
        public int BranchId { get; set; }

        [Display(Name = "Cliente")]
        public int ClientId { get; set; }
      
        [Display(Name ="Fecha")]
        public DateTime BudgetDate { get; set; }

        [Display(Name = "Expira")]
        public DateTime DueDate { get; set; }

        [MaxLength(30)]
        [Display(Name = "Agente")]
        public string UserName { get; set; }

       
        public virtual Branch Branch { get; set; }

        public virtual Client Client { get; set; }

        public ICollection<BudgetDetail> BudgetDetails { get; set; }

        public Budget()
        {
            this.BudgetDate = DateTime.Now.ToLocal();
            this.UserName   = HttpContext.Current.User.Identity.Name;
            this.BranchId = HttpContext.Current.User.Identity.GetBranchId();
            this.DueDate = new DateTime(BudgetDate.Year,BudgetDate.Month, 
                               DateTime.DaysInMonth(this.BudgetDate.Year, BudgetDate.Month),23,0,0);
            this.BudgetDetails = new List<BudgetDetail>();
        }
    }
}