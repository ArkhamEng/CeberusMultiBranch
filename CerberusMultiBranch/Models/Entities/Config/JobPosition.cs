using CerberusMultiBranch.Models.Entities.Catalog;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.Entities.Config
{
    [Table("JobPosition", Schema = "Config")]
    public class JobPosition:ISelectable
    {
        public int JobPositionId { get; set; }

        [MaxLength(30)]
        public string Name { get; set; }

        public string UpdUser { get; set; }

        public DateTime UpdDate { get; set; }

        public ICollection<Employee> Employees { get; set; }

        public int Id
        {
            get
            {
                return JobPositionId;
            }
        }
    }
}