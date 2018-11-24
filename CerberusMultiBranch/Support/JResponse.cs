using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Support
{
    public class JResponse
    {
        public int Id { get; set; }

        public string Result { get; set; }

        public string Header { get; set; }

        public string Body { get; set; }

        public string Extra { get; set; }

        public object JProperty { get; set; }

        public int Code { get; set; }
    }
}