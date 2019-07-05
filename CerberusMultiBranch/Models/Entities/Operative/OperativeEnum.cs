using CerberusMultiBranch.Support;
using System.ComponentModel.DataAnnotations;

namespace CerberusMultiBranch.Models.Entities.Operative
{
    public enum TranStatus : int
    {
        [Display(Name ="En Modificación")]
        [DisplayStyle(Alert ="alert alert-warning", Icon = "fa fa-edit")]
        OnChange = -3,

        [Display(Name = "En Cancelación")]
        [DisplayStyle(Alert = "alert alert-danger", Icon = "fa fa-exclamation-triangle")]
        PreCancel  = -2,//se termina

        [Display(Name = "Cancelada")]
        [DisplayStyle(Alert = "alert alert-danger", Icon = "fa fa-ban")]
        Canceled   = -1, //Cancelado en venta y compra

        [Display(Name = "En Proceso")]
        [DisplayStyle(Alert = "alert alert-dark", Icon = "fa fa-pencil")]
        InProcess  = 0, //Abierto-venta, En proceso -compra

        [Display(Name = "Reservado")]
        [DisplayStyle(Alert = "alert alert-info", Icon = "fa fa-gift")]
        Reserved   = 1, // compra / venta concluida pero no pagada | (compra inventariada, venta con producto reservado)

        [Display(Name = "En Seguimiento")]
        [DisplayStyle(Alert = "alert alert-attention", Icon = "fa fa-eye")]
        Revision   = 2, // venta / compra en revisión

        [Display(Name = "Completado")]
        [DisplayStyle(Alert = "alert alert-success", Icon = "fa fa-thumbs-up")]
        Compleated = 3, //Compra-Venta pagada en su totalidad

        [Display(Name = "Modificado")]
        [DisplayStyle(Alert = "alert alert-info", Icon = "fa fa-exclamation")]
        Modified = 4, //Compra-Venta pagada en su totalidad

    }

    public enum PaymentMethod
    {
        Ninguno = 0,
        Tarjeta = 1, //Tarjeda Credito o débito
        Efectivo = 2, // Efectivo
        Cheque = 3, // Con cheque
        Mixto = 4, //Efectivo y Tarjeta
        Transferencia = 5,
        Vale = 6
    }

    public enum TransactionType
    {
        [Display(Name = "Contado")]
        [DisplayStyle(Alert = "alert alert-success", Icon = "fa fa-dollar")]
        Cash       = 0,

        [Display(Name = "Crédito")]
        [DisplayStyle(Alert = "alert alert-info", Icon = "fa fa-credit-card-alt")]
        Credit      = 1,

        [Display(Name = "Preventa")]
        [DisplayStyle(Alert = "alert alert-dark", Icon = "glyphicon glyphicon-time")]
        Presale     = 2,

        [Display(Name = "Apartado")]
        [DisplayStyle(Alert = "alert alert-warning", Icon = "fa fa-archive")]
        Reservation = 3,
    }

    public enum MovementType
    {
        [Display(Name="Entrada")]
        Entry = 1,

        [Display(Name = "Salida")]
        Exit = 2,

        [Display(Name = "Reservación")]
        Reservation = 3,

        [Display(Name = "Liberación")]
        Release = 4
    }

    public enum DispatchMethod : int
    {
        [Display(Name ="En Sucursal")]
        OnSite = 0,

        [Display(Name = "A Domicilio")]
        HomeDelivery = 1
    }
}