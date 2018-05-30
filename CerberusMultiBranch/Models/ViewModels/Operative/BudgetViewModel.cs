using CerberusMultiBranch.Models.Entities.Config;
using CerberusMultiBranch.Models.Entities.Operative;
using CerberusMultiBranch.Support;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CerberusMultiBranch.Models.ViewModels.Operative
{
    [NotMapped]
    public class BudgetViewModel
    {
       public SelectList Branches { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? BeginDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? EndDate { get; set; }

        public ICollection<Budget> Budgets { get; set; }

        public BudgetViewModel()
        {
            this.Budgets = new List<Budget>();
        }
    }
}