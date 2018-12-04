using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.ViewModels.Operative
{
    public class SetProviderCodeViewModel
    {
        [Display(Name ="Código Proveedor")]
        [Required(ErrorMessage ="Se requiere un código de proveedor válido")]
        public string ProviderCode { get; set; }

        public string InternalCode { get; set; }

        [Display(Name = "Precio Proveedor")]
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "Es necesario un precio proveedor")]
        public double  Price { get; set; }

        public int   ProviderId { get; set; }

        public int ProductId { get; set; }
    }
}