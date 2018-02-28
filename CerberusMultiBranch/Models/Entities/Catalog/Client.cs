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
        [Index("IDX_CityId", IsUnique = false)]
        [Required]
        public int CityId { get; set; }

        [Display(Name = "Clave")]
        [Required]
        [MaxLength(10)]
        [Index("IDX_Code", IsUnique = true)]
        public string Code { get; set; }

        [Display(Name = "Nombre")]
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }


        [Display(Name = "Razón Social")]
        [MaxLength(100)]
        public string BusinessName { get; set; }

        //Federal Taxpayer register
        [Display(Name = "R.F.C.")]
        [MaxLength(15)]
        public string FTR { get; set; }

        [Display(Name = "Dirección")]
        [MaxLength(150)]
        [Required]
        public string Address { get; set; }

        [Display(Name = "C.P.")]
        [MaxLength(10)]
        [DataType(DataType.PostalCode)]
        public string ZipCode { get; set; }

        [Display(Name = "Fecha de Ingreso")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Entrance { get; set; }

        [Display(Name = "E-mail")]
        [DataType(DataType.EmailAddress)]
        [MaxLength(100)]
        public string Email { get; set; }

        [Display(Name = "Teléfono")]
        [DataType(DataType.PhoneNumber)]
        [MaxLength(20)]
        public string Phone { get; set; }

        public ClientType Type { get; set; }

        public bool IsActive { get; set; }

        [Required]
        public DateTime UpdDate { get; set; }

        [MaxLength(100)]
        public string UpdUser { get; set; }

        public virtual City City { get; set; }

        public ICollection<Sale> Sale { get; set; }

        [Display(Name="Credito Limite")]
        [DataType(DataType.Currency)]
        public double CreditLimit { get; set; }

        [Display(Name = "Credito Usado")]
        [DataType(DataType.Currency)]
        public double UsedAmount { get; set; }

        [Display(Name = "Dias de Crédito")]
        [Range(0, 60, ErrorMessage = "El crédito no puede ser por mas de 60 días")]
        public int CreditDays { get; set; }

        [NotMapped]
        [Display(Name = "Credito Disponible")]
        [DataType(DataType.Currency)]
        public  double CreditAvailable { get { return CreditLimit - UsedAmount; } }

        [Display(Name="Comentario Sobre Crédito")]
        public string CreditComment { get; set; }

        
      


        public Client()
        {
            this.IsActive  = true;
            this.Entrance  = DateTime.Now;
            this.UpdDate   = DateTime.Now;
            this.Code      = Cons.CodeMask;
        }

        public override string ToString()
        {
            var a = string.Empty;

            if (this.City != null && this.City.State != null)
                a= string.Format("{0} CP {1} {2}, {3} ", this.Address, this.ZipCode, this.City.State.Name, this.City.Name);
            else
               a= this.Address + " CP " + ZipCode;

            return a;
        }
    }

    public enum ClientType
    {
        Store  = 0,
        Dealer = 1,
        Wholesaler =2
    }
}