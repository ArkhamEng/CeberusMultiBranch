using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Support
{
    public class DisplayStyle:Attribute
    {
        public string Alert { get; set; }

        public string Icon { get; set; }
    }
}