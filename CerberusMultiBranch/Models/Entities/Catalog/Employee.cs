using CerberusMultiBranch.Models.Entities.Config;
using CerberusMultiBranch.Models.Entities.Operative;
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
    public class Employee: IUserAssignment
    {
        [Key]
        public int EmployeeId { get; set; }

        [Display(Name = "Ciudad/Municipio")]
        [Required]
        [Index("IDX_CityId", IsUnique = false)]
        public int CityId { get; set; }


        [MaxLength(128)]
        [Index("IDX_UserId", IsUnique = true)]
        public string UserId { get; set; }

        [Display(Name = "Clave")]
        [Required]
        [MaxLength(12)]
        [Index("IDX_Code", IsUnique = true)]
        public string Code { get; set; }

        [Display(Name = "Nombre")]
        [Required]
        [MaxLength(100)]
        [Index("IDX_Name", IsUnique = true)]
        public string Name { get; set; }
      
        //Federal Taxpayer register
        [Display(Name = "R.F.C.")]
        [MaxLength(13)]
        public string FTR { get; set; }


        [Display(Name = "Dirección")]
        [Required]
        [MaxLength(100)]
        public string Address { get; set; }

        [Display(Name = "C.P.")]
        [DataType(DataType.PostalCode)]
        [MaxLength(6)]
        public string ZipCode { get; set; }

        [Display(Name = "Fecha de Ingreso")]
        [DataType(DataType.Date)]
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Entrance { get; set; }

        [Display(Name = "E-mail")]
        [DataType(DataType.EmailAddress)]
        [MaxLength(30)]
        [Index("IDX_Email", IsUnique = true)]
        public string Email { get; set; }

        [Display(Name = "Teléfono")]
        [DataType(DataType.PhoneNumber)]
        [Required]
        [MaxLength(12)]
        [Index("IDX_Phone", IsUnique = true)]
        public string Phone { get; set; }


        [Required]
        public bool IsActive { get; set; }

        [Required]
        public DateTime UpdDate { get; set; }

        public byte[] Picture { get; set; }

        [MaxLength(20)]
        public string PictureType { get; set; }

        [NotMapped]
        public HttpPostedFileBase PostedFile { get; set; }


        #region Navigation Properties
        public virtual City City { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        public ICollection<EmployeeBranch> EmployeeBranches { get; set; }

        #endregion


        public Employee()
        {
            this.IsActive = true;
            this.Entrance = DateTime.Now;
            this.UpdDate = DateTime.Now;
            this.Code    = Cons.CodeMask;
            this.EmployeeBranches = new List<EmployeeBranch>();
        }
    }
}