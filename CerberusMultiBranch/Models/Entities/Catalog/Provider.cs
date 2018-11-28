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
    [Table("Provider", Schema = "Catalog")]
    public class Provider
    {
        public int ProviderId { get; set; }

       
        [Display(Name = "Clave")]
        [Required]
        [MaxLength(10)]
        [Index("IDX_Code", IsUnique = true)]
        public string Code { get; set; }

        [Display(Name = "Nombre")]
        [Required]
        [MaxLength(100)]
        [Index("IDX_Name", IsUnique = false)]
        public string Name { get; set; }

        [Display(Name = "Razón Social")]
        [MaxLength(100)]
        public string BusinessName { get; set; }


        [Display(Name = "Página Web")]
        [DataType(DataType.Url, ErrorMessage ="error de formato! el sitio debe incluir http:// o https:// ejemplo: http://www.sitio.com")]
        [MaxLength(150)]
        public string WebSite { get; set; }

        //Federal Taxpayer register
        [Display(Name = "R.F.C.")]
        [MaxLength(15)]
        [Required]
        [Index("Unq_FTR", IsUnique = true)]
        public string FTR { get; set; }

    
        [Display(Name = "E-mail")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Ingresa una dirección de correo válida ej. nombre@tudominio.com")]
        [MaxLength(100)]
        [Index("IDX_Email", IsUnique = false)]
        public string Email { get; set; }

        [Display(Name = "E-mail 2")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Ingresa una dirección de correo válida ej. nombre@tudominio.com")]
        [MaxLength(100)]
        public string Email2 { get; set; }

        [Display(Name = "Teléfono 1")]
        [DataType(DataType.PhoneNumber)]
        [MaxLength(20)]
        public string Phone { get; set; }

        [Display(Name = "Teléfono 2")]
        [DataType(DataType.PhoneNumber)]
        [MaxLength(20)]
        public string Phone2 { get; set; }

        [Display(Name = "Teléfono 3")]
        [DataType(DataType.PhoneNumber)]        
        [MaxLength(20)]
        public string Phone3 { get; set; }

        [Display(Name = "Telefono Representante")]
        [DataType(DataType.PhoneNumber)]
        [MaxLength(20)]
        public string AgentPhone { get; set; }

        [Display(Name = "Representante")]
        [MaxLength(100)]
        public string Agent { get; set; }

        [Display(Name = "Línea")]
        [MaxLength(100)]
        public string Line { get; set; }

        public bool IsActive { get; set; }

        [Required]
        [Display(Name = "Editado")]
        public DateTime UpdDate { get; set; }

        [MaxLength(100)]
        [Display(Name = "Editado por")]
        public string UpdUser { get; set; }

        public DateTime? LockEndDate { get; set; }

        [MaxLength(100)]
        public string LockUser { get; set; }

        [Display(Name="Limite de Credito")]
        public double CreditLimit { get; set; }

        [Display(Name = "Días de Credito")]
        public int DaysToPay { get; set; }

        [Display(Name ="Productos")]
        public int Catalog { get; set; }

    
       public ICollection<Purchase> Purchases { get; set; }

        public ICollection<ExternalProduct> ExternalProducts { get; set; }

        
        public ICollection<Address> Addresses { get; set; }

        public Provider()
        {
            this.IsActive = true;
            this.UpdDate  = DateTime.Now.ToLocal();
            this.UpdUser = HttpContext.Current.User.Identity.Name;
            this.Code     = Cons.CodeSeqFormat;
        }

        public Provider Copy()
        {
            return new Provider
            {
                ProviderId = this.ProviderId,
                BusinessName = this.BusinessName,
                Code = this.Code,
                Email = this.Email,
                FTR = this.FTR,
                IsActive = this.IsActive,
                UpdDate = this.UpdDate,
                Name = this.Name,
                Phone = this.Phone,
                WebSite = this.WebSite
                
            };
        }
    }
}