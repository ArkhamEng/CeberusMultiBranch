using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Support
{
    public static class Cons
    {
        public const int Zero = 0;
        public const int One = 1;
        public const int Two = 2;

        public const string CodeMask = "000000";

        public const string SessionBranchId = "SessionBranchId";

        public const string SessionBranchName   = "SessionBranchName";
    }

    public struct FileStruct
    {
        public string Name { get;set;}

        public string Type { get; set; }

        public byte[] Bytes { get; set; }

        public int Size { get {return this.Bytes!=null? this.Bytes.Length : Cons.Zero; } }

    }
}