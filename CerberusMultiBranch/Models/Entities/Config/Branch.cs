﻿using CerberusMultiBranch.Models.Entities.Common;
using CerberusMultiBranch.Models.Entities.Operative;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace CerberusMultiBranch.Models.Entities.Config
{
    [Table("Branch", Schema = "Config")]
    public class Branch:ISelectable
    {
        public int BranchId { get; set; }

        [Display(Name="Sucursal")]
        public string Name { get; set; }

        [Display(Name = "Direción")]
        public string Address { get; set; }

        [Display(Name = "Ubicación")]
        public string Location { get; set; }

        public ICollection<ProductInventory> ProductInventories { get; set; }

        [NotMapped]
        public int Id { get { return this.BranchId; } }
       
    }
}