using CerberusMultiBranch.Models.Entities.Config;
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
    [Table("Client", Schema = "Catalog")]
    public class Client
    {
        public int ClientId { get; set; }

        [Display(Name = "Ciudad/Municipio")]
        [Required]
        public int CityId { get; set; }

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
        public string BusinessName { get; set; }

        [Display(Name = "Representante Legal")]
        [MaxLength(50)]
        public string LegalRepresentative { get; set; }

        //Federal Taxpayer register
        [Display(Name = "R.F.C.")]
        [MaxLength(13)]
        public string FTR { get; set; }

        [Display(Name = "Dirección Fiscal")]
        [DataType(DataType.MultilineText)]
        public string TaxAddress { get; set; }

        [Display(Name = "Dirección")]
        [Required]
        public string Address { get; set; }

        [Display(Name = "C.P.")]
        [DataType(DataType.PostalCode)]
        public string ZipCode { get; set; }

        [Display(Name = "Fecha de Ingreso")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Entrance { get; set; }

        [Display(Name = "E-mail")]
        [DataType(DataType.EmailAddress)]
        [MaxLength(30)]
        public string Email { get; set; }

        [Display(Name = "Teléfono")]
        [DataType(DataType.PhoneNumber)]
        [Required]
        [MaxLength(20)]
        public string Phone { get; set; }

        public bool IsActive { get; set; }

        [Required]
        public DateTime UpdDate { get; set; }

        
        public virtual City City { get; set; }

        public ICollection<Sale> Sale { get; set; }

        public Client()
        {
            this.IsActive  = true;
            this.Entrance  = DateTime.Now;
            this.UpdDate   = DateTime.Now;
            this.Code      = Cons.CodeMask;
        }

        public Client Copy()
        {
            return new Client
            {
                ClientId  = this.ClientId,
                Address   = this.Address,
                BusinessName = this.BusinessName,
                CityId   = this.CityId,
                Code = this.Code,
                Email = this.Email,
                Entrance = this.Entrance,
                FTR = this.FTR,
                IsActive = this.IsActive,
                UpdDate = this.UpdDate,
                LegalRepresentative = this.LegalRepresentative,
                Name = this.Name,
                Phone = this.Phone,
                TaxAddress = this.TaxAddress,
                ZipCode = this.ZipCode
            };
        }
    }
}