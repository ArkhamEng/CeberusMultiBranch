using CerberusMultiBranch.Models.Entities.Catalog;
using CerberusMultiBranch.Models.Entities.Common;
using CerberusMultiBranch.Support;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CerberusMultiBranch.Models.ViewModels.Catalog
{
    [NotMapped]
    public class ProductViewModel:Product
    {
        public SelectList Categories { get; set; }

        public SelectList SubCategories { get; set; }

        [Display(Name="Categoría")]
        public int CategoryId { get; set; }

        public ProductViewModel()
        {
            this.Categories = new List<Category>().ToSelectList();
            this.SubCategories = new List<SubCategory>().ToSelectList();
        }
    }
} 