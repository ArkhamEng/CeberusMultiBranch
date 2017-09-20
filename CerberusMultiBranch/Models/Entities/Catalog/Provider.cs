using CerberusMultiBranch.Models.Entities.Common;
using CerberusMultiBranch.Models.Entities.Operative;
using CerberusMultiBranch.Support;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.Entities.Catalog
{
    [Table("Provider", Schema = "Catalog")]
    public class Provider
    {
        public int ProviderId { get; set; }

        [Display(Name = "Clave")]
        [Required]
        [MaxLength(12)]
        [Index("IDX_Code", IsUnique = true)]
        public string Code { get; set; }

        [Display(Name = "Nombre")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Razón Social")]
        [MaxLength(50)]
        [Index("IDX_BussinessName", IsUnique = false)]
        public string BusinessName { get; set; }


        [Display(Name = "Página Web")]
        [DataType(DataType.Url)]
        public string WebSite { get; set; }

        //Federal Taxpayer register
        [Display(Name = "R.F.C.")]
        [MaxLength(13)]
        [Index("IDX_FTR", IsUnique = false)]
        public string FTR { get; set; }

     
        [Display(Name = "Dirección")]
        [DataType(DataType.MultilineText)]
        [Required]
        public string Address { get; set; }

        [Display(Name = "C.P.")]
        [DataType(DataType.PostalCode)]
        public string ZipCode { get; set; }


        [Display(Name = "E-mail")]
        [DataType(DataType.EmailAddress)]
        [MaxLength(30)]
        [Index("IDX_Email", IsUnique = false)]
        public string Email { get; set; }

        [Display(Name = "Teléfono")]
        [DataType(DataType.PhoneNumber)]
        [Required]
        [MaxLength(20)]
        [Index("IDX_Phone", IsUnique = true)]
        public string Phone { get; set; }

        [Display(Name = "Ciudad/Municipio")]
        [Required]
        public int CityId { get; set; }

        public bool IsActive { get; set; }

        [Required]
        public DateTime InsDate { get; set; }

        [Required]
        public DateTime UpdDate { get; set; }


        public virtual City City { get; set; }

        public ICollection<Purchase> Purchases { get; set; }

        public Provider()
        {
            this.IsActive = true;
            this.InsDate  = DateTime.Now;
            this.UpdDate  = DateTime.Now;
            this.Code     = Cons.CodeMask;
        }

        public Provider Copy()
        {
            return new Provider
            {
                ProviderId = this.ProviderId,
                Address = this.Address,
                BusinessName = this.BusinessName,
                CityId = this.CityId,
                Code = this.Code,
                Email = this.Email,
                FTR = this.FTR,
                IsActive = this.IsActive,
                InsDate = this.InsDate,
                UpdDate = this.UpdDate,
                Name = this.Name,
                Phone = this.Phone,
                ZipCode = this.ZipCode,
                WebSite = this.WebSite
                
            };
        }
    }
}