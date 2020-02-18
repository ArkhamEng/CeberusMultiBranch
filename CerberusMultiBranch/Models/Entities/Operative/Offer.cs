using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CerberusMultiBranch.Models.Entities.Operative
{
    [Table("Offer", Schema = "Operative")]
    public class Offer
    {
        public int OfferId { get; set; }

        [Display(Name ="Tipo")]
        public OfferType Type { get; set; }

        [Display(Name = "Clave")]
        public string Name { get; set; }

        [Display(Name = "Descripción")]
        public string Description { get; set; }


        public string ImagePath { get; set; }

        public string TextColor { get; set; }

        public string ShadowColor { get; set; }

        [Display(Name = "Descuento")]
        public int Discount { get; set; }

        [Display(Name = "Cant. Min")]
        public int MinQty { get; set; }

        [Display(Name = "Cant. Max")]
        public int MaxQty { get; set; }

        [Display(Name = "Inicio")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [Index("IDX_BeginDate")]
        public DateTime BeginDate { get; set; }

        [Display(Name = "Fin")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [Index("IDX_EndDate")]
        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; }

        public int Year { get; set; }

        public int Consecutive { get; set; }

        public DateTime InsDate { get; set; }

        public string InsUser { get; set; }

        [NotMapped]
        public bool HasImage { get; set; }

        [NotMapped]
        public string Base64 { get; set; }

        public Offer()
        {
            IsActive = true;
        }
    }

    public enum OfferType
    {
        [Display(Name = "Sin Descuento")]
        NoDiscount = 0,

        [Display(Name = "Descuento por Volumen")]
        Volume = 1,

        [Display(Name = "Descuento de Temporada")]
        Season = 2,

        [Display(Name = "Producto Nuevo")]
        NewProduct = 3,

        [Display(Name = "Producto Descontinuado")]
        Discontinued = 4,
    }
}