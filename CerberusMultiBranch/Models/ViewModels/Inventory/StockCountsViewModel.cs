using CerberusMultiBranch.Models.Entities.Config;
using CerberusMultiBranch.Models.Entities.Inventory;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace CerberusMultiBranch.Models.ViewModels.Inventory
{
    public class StockCountsViewModel
    {
        public int StockCountId { get; set; }

        [DisplayName("Nombre del recuento")]
        [Required(ErrorMessage ="Se requiere un nombre de recuento")]
        public string Name { get; set; }

        [DisplayName("Fecha de recuento")]
        public DateTime BeginDate { get; set; }

        [DisplayName("Observaciones")]
        public string Observations { get; set; }

        [DisplayName("Sucursal")]
        public int BranchId { get; set; }

        [DisplayName("Sistema")]
        public int? PartSysmtemId { get; set; }

        [DisplayName("Marca")]
        public int? MarkId { get; set; }

        public List<Branch> Branches { get; set; } 

        public List<PartSystem> Systems { get; set; }

        public List<Mark> Marks { get; set; }

        public List<StockCountDetail> StockCountsDetails { get; set; }

        public StockCount StockCount { get; set; }

        public StockCountsViewModel()
        {
            StockCount = new StockCount();

            StockCountsDetails = new List<StockCountDetail>();
            Branches = new List<Branch>();
            Systems = new List<PartSystem>();
            Marks = new List<Mark>();
            BeginDate = DateTime.Today.ToLocalTime();
        }
    }
}