using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.Entities.Operative
{
    [Table("StockMovement", Schema = "Operative")]
    public class StockMovement
    {
        public int StockMovementId { get; set; }

        [Column(Order = 0), ForeignKey("BranchProduct")]
        public int BranchId { get; set; }

        [Column(Order = 1), ForeignKey("BranchProduct")]
        public int ProductId { get; set; }

        [Display(Name = "Unidades")]
        public double Quantity { get; set; }

        [Display(Name = "Fecha Movimiento")]
        public DateTime MovementDate { get; set; }

        [Display(Name = "Hecho por")]
        public string User { get; set; }

        [Display(Name="Flujo")]
        public MovementType MovementType { get; set; }


        [Display(Name = "Comentarios")]
        public string Comment { get; set; }

        public virtual BranchProduct BranchProduct { get; set; }


        [Display(Name = "Tipo")]
        public string OperationType
        {
            get { return 
                         this.Comment.ToUpper().Contains("SALIDA AUTOMATICA")  ? "Venta"   :
                         this.Comment.ToUpper().Contains("ENTRADA AUTOMATICA") ? "Compra" :
                         this.Comment.ToUpper().Contains("TRANSFERENCIA")  ? "Transferencia" : "Manual"; }
        }

        public string FlowStyle
        {
            get { return this.MovementType == MovementType.Entry ? "bgDataTable-success" : 
                         this.MovementType == MovementType.Exit  ? "bgDataTable-danger": string.Empty;
            }
        }
    }

  
}