using CerberusMultiBranch.Models.Entities.Operative;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

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

        public ICollection<Sale> Sales { get; set; }

        public ICollection<BranchProduct> BranchProducts { get; set; }

        public ICollection<CashRegister> CashRegisters { get; set; }

        public ICollection<UserBranch> UserBranches { get; set; }

        //[NotMapped]
        //public ICollection<Operation> Sales { get { return this.Transactions.OfType<Operation>().ToList(); } }


        //[NotMapped]
        //public ICollection<Purchase> Purchases { get { return this.Transactions.OfType<Purchase>().ToList(); } }

        [NotMapped]
        public int Id { get { return this.BranchId; } }

        [NotMapped]
        [Display(Name ="Disponibles")]
        public double Quantity { get; set; }

        public Branch()
        {
            this.Sales = new List<Sale>();
        }
    }


}