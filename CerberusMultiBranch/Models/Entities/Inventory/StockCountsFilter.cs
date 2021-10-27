using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Models.Entities.Inventory
{
    public class StockCountsFilter
    {
        public string Name { get; set; }

        //Falta el campo de la id de la sucursal

        public DateTime BeginDate { get; set; }

        public int MyProperty { get; set; }

    }
}