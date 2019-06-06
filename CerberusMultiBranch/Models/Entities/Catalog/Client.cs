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


        [Display(Name = "Fecha de Ingreso")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Entrance { get; set; }

        [Display(Name = "Correo Electrónico")]
        [DataType(DataType.EmailAddress, ErrorMessage ="Ingresa una dirección de correo válida ej. nombre@tudominio.com")]
        [MaxLength(100)]
        public string Email { get; set; }

        [Display(Name = "Correo Adicional")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Ingresa una dirección de correo válida ej. nombre@tudominio.com")]
        [MaxLength(100)]
        public string Email2 { get; set; }

        [Display(Name = "Teléfono Principal")]
        [DataType(DataType.PhoneNumber)]
        [MaxLength(20)]
        public string Phone { get; set; }

        [Display(Name = "Teléfono Adicional")]
        [DataType(DataType.PhoneNumber)]
        [MaxLength(20)]
        public string Phone2 { get; set; }

        [Display(Name = "Tipo de cliente")]
        public ClientType Type { get; set; }

        [Display(Name = "Persona Fiscal")]
        public string PersonType { get; set; }

      
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

        public bool IsActive { get; set; }

        [Required]
        [Display(Name = "Fecha Edición")]
        [DataType(DataType.DateTime)]
        public DateTime UpdDate { get; set; }

        [MaxLength(100)]
        [Display(Name = "Editado por")]
        public string UpdUser { get; set; }


        public DateTime? LockEndDate { get; set; }

        [MaxLength(100)]
        public string LockUser { get; set; }

        //public virtual City City { get; set; }

        public ICollection<Sale> Sale { get; set; }

        public ICollection<Address> Addresses { get; set; }


        public string ClientType { get { return this.Type.GetName(); } }

        public Client()
        {
            this.IsActive  = true;
            this.Entrance  = DateTime.Now.ToLocal();
            this.UpdDate   = DateTime.Now.ToLocal();
            this.UpdUser    = HttpContext.Current.User.Identity.Name;
            this.Code      = Cons.CodeSeqFormat;

            //temp
            this.Addresses = new List<Address>();
            
        }

        public override string ToString()
        {
            //var a = string.Empty;

            //if (this.City != null && this.City.State != null)
            //    a= string.Format("{0} CP {1} {2}, {3} ", this.Address, this.ZipCode, this.City.State.Name, this.City.Name);
            //else
            //   a= this.Address + " CP " + ZipCode;

            //return a;
            return base.ToString();
        }
    }

    public enum ClientType
    {
        [Display(Name = "Mostrador")]
        Store  = 0,

        [Display(Name = "Distribuidor")]
        Dealer = 1,

        [Display(Name = "Mayorista")]
        Wholesaler =2
    }
}