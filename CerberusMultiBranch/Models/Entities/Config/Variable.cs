using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.Entities.Config
{
    [Table("Variable", Schema = "Config")]
    public class Variable
    {
        public int VariableId { get; set; }

        [MaxLength(25)]
        public string Name { get; set; }

        public string Value { get; set; }
    }
}