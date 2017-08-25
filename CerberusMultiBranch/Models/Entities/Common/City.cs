﻿using CerberusMultiBranch.Models.Entities.Catalog;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.Entities.Common
{
    [Table("City", Schema = "Common")]
    public class City : ISelectable
    {
        public int CityId { get; set; }

        public string Code { get; set; }

        [Display(Name = "Nombre")]
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public bool IsActive { get; set; }


        [Display(Name = "Estado")]
        public int StateId { get; set; }

        public virtual State State { get; set; }

        public IEnumerable<Client> Clients { get; set; }

      


        [NotMapped]
        public int Id { get { return this.CityId; } }

        public City()
        {
            this.Clients = new List<Client>();
        }
    }
}