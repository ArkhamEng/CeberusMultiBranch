using CerberusMultiBranch.Models.Entities.Operative;
using CerberusMultiBranch.Models.Entities.Purchasing;
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

        [MaxLength(4)]
        public string Code { get; set; }

        [MaxLength(500)]
        public string NoteMemberHtml { get; set; }

        [MaxLength(500)]
        public string NoteLocalHtml { get; set; }


        [MaxLength(50)]
        public string LogoPath { get; set; }

        public ICollection<Sale> Sales { get; set; }

        public ICollection<PurchaseOrder> PurchaseOrder { get; set; }

        public ICollection<BranchProduct> BranchProducts { get; set; }

        public ICollection<CashRegister> CashRegisters { get; set; }

        public ICollection<UserBranch> UserBranches { get; set; }

        public ICollection<PurchaseItem> PurchaseItems { get; set; }

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