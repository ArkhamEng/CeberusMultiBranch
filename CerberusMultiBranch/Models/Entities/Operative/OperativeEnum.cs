using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.Entities.Operative
{
    public enum TranStatus : int
    {
        PreCancel = -2,//se termina
        Canceled = -1, //Cancelado en venta y compra
        InProcess = 0, //Abierto-venta, En proceso -compra
        Reserved = 1, // compra / venta concluida pero no pagada | (compra inventariada, venta con producto reservado)
        Revision = 2,
        Compleated = 3, //Compra-Venta pagada en su totalidad
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
        Contado  = 0,
        Credito  = 1,
        Preventa = 2,
        Apartado = 3,
    }

    public enum MovementType
    {
        [Display(Name="Entrada")]
        Entry = 1,

        [Display(Name = "Salida")]
        Exit = 2,
        Reservation = 3,
        Release = 4
    }
}