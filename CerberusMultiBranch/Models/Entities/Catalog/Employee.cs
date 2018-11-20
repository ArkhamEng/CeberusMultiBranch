using CerberusMultiBranch.Models.Entities.Config;
using CerberusMultiBranch.Support;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace CerberusMultiBranch.Models.Entities.Catalog
{
    [Table("Employee", Schema = "Catalog")]
    public class Employee: IUserAssignment
    {
        [Key]
        public int EmployeeId { get; set; }

        [Display(Name = "Ciudad ó Municipio")]
        public int? CityId { get; set; }


        [Display(Name = "Puesto")]
        public int? JobPositionId { get; set; }

        [MaxLength(128)]
        [Index("IDX_UserId", IsUnique = false)]
        [Display(Name = "Nombre de Usuario")]
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
        [MaxLength(15)]
        public string FTR { get; set; }

        [Display(Name = "N.S.S")]
        [MaxLength(15)]
        public string NSS { get; set; }

        [Display(Name = "Dirección")]
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
        [MaxLength(100)]
        public string Email { get; set; }

        [Display(Name = "Teléfono")]
        [DataType(DataType.PhoneNumber)]
        [MaxLength(20)]
        public string Phone { get; set; }

        [Display(Name = "Tel. Emergencia")]
        [DataType(DataType.PhoneNumber)]
        [MaxLength(20)]
        public string EmergencyPhone { get; set; }

        [Required]
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

        public byte[] Picture { get; set; }

        [MaxLength(20)]
        public string PictureType { get; set; }

        [Display(Name="Comisión")]
        public int ComissionForSale { get; set; }


        [Display(Name = "Salario")]
        public double Salary { get; set; }


        [NotMapped]
        public HttpPostedFileBase PostedFile { get; set; }


        #region Navigation Properties
        //public virtual City City { get; set; }

        public virtual JobPosition JobPosition { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        public ICollection<Address> Addresses { get; set; }

        [NotMapped]
        public string ImageFormated
        {
            get
            {
                if (this.Picture != null)
                {
                    var base64 = Convert.ToBase64String(Support.GzipWrapper.Decompress(this.Picture));
                    var imgSrc = String.Format("data:{0};base64,{1}", this.PictureType, base64);
                    return imgSrc;
                }
                else
                {
                    return "/Content/Images/sinimagen.jpg";
                }
            }
        }


        #endregion
        public Employee()
        {
            this.IsActive = true;
            this.Entrance = DateTime.Now;
            this.UpdDate = DateTime.Now;
            this.UpdUser = HttpContext.Current.User.Identity.Name;
            this.Code    = Cons.CodeSeqFormat;
        }
    }
}