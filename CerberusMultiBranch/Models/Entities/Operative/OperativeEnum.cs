using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.Entities.Operative
{
    public enum TranStatus : int
    {
        Canceled = -1, //Cancelado en venta y compra
        InProcess = 0, //Abierto-venta, En proceso -compra
        Reserved = 1,
        Revision = 2,
        Compleated = 3, //Pagado-Venta, Inventariado-Compra
    }

    public enum PaymentMethod
    {
        Ninguno = 0,
        Tarjeta = 1, //Tarjeda Credito o débito
        Efectivo = 2, // Efectivo
        Cheque = 3, // Con cheque
        Mixto = 4, //Efectivo y Tarjeta
        Transferencia = 5
    }

    public enum MovementType
    {
        Entry = 1,
        Exit = 2,
        Reservation = 3,
        Release = 4
    }
}