using CerberusMultiBranch.Models.Entities.Common;
using CerberusMultiBranch.Support;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.Entities.Catalog
{
    [Table("Employee", Schema = "Catalog")]
    public class Employee
    {
        public int EmployeeId { get; set; }

        [Display(Name = "Clave")]
        [Required]
        [MaxLength(12)]
        [Index("IDX_Code", IsUnique = true)]
        public string Code { get; set; }

        [Display(Name = "Nombre")]
        [Required]
        public string Name { get; set; }
      
        //Federal Taxpayer register
        [Display(Name = "R.F.C.")]
        [MaxLength(13)]
        [Index("IDX_FTR", IsUnique = false)]
        public string FTR { get; set; }


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

        public virtual IdentityUser User { get; set; }

        [NotMapped]
        public HttpPostedFileBase PostedFile { get; set; }


        public Employee()
        {
            this.IsActive = true;
            this.Entrance = DateTime.Now;
            this.InsDate = DateTime.Now;
            this.UpdDate = DateTime.Now;
            this.Code    = Cons.CodeMask;
        }

       
    }
}