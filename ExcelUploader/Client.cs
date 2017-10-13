using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelUploader
{
    public class Person
    {
        public int Id { get; set; }

        //[Display(Name = "Ciudad/Municipio")]
        //[Required]
        public int CityId { get; set; }

        //[Display(Name = "Clave")]
        //[Required]
        //[MaxLength(12)]
        //[Index("IDX_Code", IsUnique = true)]
        public string Code { get; set; }

        //[Display(Name = "Nombre")]
        //[Required]
        public string Name { get; set; }

        //[Display(Name = "Razón Social")]
        //[MaxLength(50)]
        public string BusinessName { get; set; }

        //[Display(Name = "Representante Legal")]
        //[MaxLength(50)]
        public string LegalRepresentative { get; set; }

        ////Federal Taxpayer register
        //[Display(Name = "R.F.C.")]
        //[MaxLength(13)]
        public string FTR { get; set; }

        //[Display(Name = "Dirección Fiscal")]
        //[DataType(DataType.MultilineText)]
        public string TaxAddress { get; set; }

        //[Display(Name = "Dirección")]
        //[Required]
        public string Address { get; set; }

        //[Display(Name = "C.P.")]
        //[DataType(DataType.PostalCode)]
        public string ZipCode { get; set; }

        //[Display(Name = "Fecha de Ingreso")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Entrance { get; set; }

        //[Display(Name = "E-mail")]
        //[DataType(DataType.EmailAddress)]
        //[MaxLength(30)]
        public string Email { get; set; }

        //[Display(Name = "Teléfono")]
        //[DataType(DataType.PhoneNumber)]
        //[Required]
        //[MaxLength(20)]
        public string Phone { get; set; }

        public bool IsActive { get; set; }

        //[Required]
        public DateTime UpdDate { get; set; }

    }
}
