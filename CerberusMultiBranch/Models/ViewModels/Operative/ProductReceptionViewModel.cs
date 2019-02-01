using CerberusMultiBranch.Models.Entities.Purchasing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.ViewModels.Operative
{
    public class ProductReceptionViewModel
    {
        public int DetailId { get; set; }

        [Display(Name="Producto")]
        public string Description { get; set; }

        [Display(Name = "Unidad")]
        public string MeasureUnit { get; set; }


        [Display(Name = "Ordenados")]
        public double RequestedQty { get; set; }

        [Display(Name = "Recibidos")]
        public double ReceivedQty { get; set; }

        [Display(Name = "Descuento")]
        public double Discount { get; set; }

        [Display(Name = "Rechazados")]
        public double RejectedQty { get; set; }

        [Display(Name = "Almacenados")]
        public double StocketQty { get; set; }

        [Display(Name = "Observaciones")]
        public string Comment { get; set; }

        [Display(Name = "Con Serie")]
        public bool IsTrackable { get; set; }

        public bool HasValues { get; set; }

        public bool IsCompleated { get; set; }

        public List<SerialItemViewModel> Serials { get; set; }

        public List<SerialItemViewModel> SerialsSaved { get; set; }
    }


    public class SerialItemViewModel
    {
        [Display(Name = "Consecutivo")]
        public int Consecutive { get; set; }

        [Display(Name ="Número de Serie")]
        public string SerialNumber { get; set; }

        [Display(Name = "Fecha Ingreso")]
        public DateTime InsDate { get; set; }


        [Display(Name = "Usuario Ingreso")]
        public string InsUser { get; set; }

    }
}