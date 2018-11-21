using CerberusMultiBranch.Models.Entities.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.Entities.Catalog
{
    [Table("Address", Schema = "Catalog")]
    public class Address
    {
        public int AddressId { get; set; }

        [Display(Name = "Tipo de dirección")]
        [MaxLength(20)]
        public string AddressType { get; set; }

        [Display(Name = "Entidad")]
        [MaxLength(20)]
        public string Entity { get; set; }

        [Display(Name = "Ciudad ó Municipio")]
        [Index("IDX_CityId", IsUnique = false)]
        [Required]
        public int CityId { get; set; }

        [Display(Name = "Colonia")]
        [MaxLength(150)]
        [Required]
        public string Location { get; set; }

        [Display(Name = "Calle")]
        [MaxLength(150)]
        [Required]
        public string Street { get; set; }

        [Display(Name = "C.P.")]
        [MaxLength(10)]
        [DataType(DataType.PostalCode)]
        public string ZipCode { get; set; }

        [Display(Name = "Referencia")]
        [MaxLength(250)]        
        public string Reference { get; set; }

        [ForeignKey("Client")]
        public int? ClientId { get; set; }

        [ForeignKey("Employee")]
        public int? EmployeeId { get; set; }

        [ForeignKey("Provider")]
        public int? ProviderId { get; set; }


        public virtual City City { get; set; }

        public virtual Provider Provider { get; set; }

        public virtual Employee Employee { get; set; }

        public virtual Client Client { get; set; }

        public override string ToString()
        {
            return string.Format("{0}, {1}  {2}, {3}  CP. {4}",
                this.Street, this.Location, this.City.State.Name, this.City.Name, this.ZipCode);
        }

        public  string WithReference()
        {

            return string.Format("{0}, {1}  {2}, {3}  CP. {4} | Referencia {5}",
                  this.Street, this.Location, this.City.State.Name, this.City.Name, this.ZipCode, this.Reference);
        }
    }
    
}