using CerberusMultiBranch.Models.Entities.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CerberusMultiBranch.Models.Entities.Config
{
    [Table("Make", Schema = "Config")]
    public class Make:ISelectable
    {
        public int MakeId { get; set; }

        public string Name { get; set; }

        public ICollection<Model> Models { get; set; }

        [NotMapped]
        public int Id { get { return this.MakeId; } }
     
    }

    [Table("Model", Schema = "Config")]
    public class Model : ISelectable
    {
        [NotMapped]
        public int Id { get { return this.ModelId; } }

        public int MakeId { get; set; }

        public int ModelId { get; set; }

        public string Name { get; set; }

        public virtual Model Make { get; set; }
    }

    [Table("Year", Schema = "Config")]
    public class Year : ISelectable
    {

        public int Id { get { return this.YearId; } }

        public int YearId { get; set; }

        public string Name { get; set; }
    }
}