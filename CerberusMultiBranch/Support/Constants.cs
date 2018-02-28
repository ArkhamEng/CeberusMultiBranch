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

        public const double OneHundred = 100;

        public const int QuickResults = 300;

        public const string CodeMask = "000000";

        public const string BranchSession = "BranchSession";

        public const string SaleSession   = "SaleSession";

        public const string EmployeeProfilePath = "/Content/EmployeeProfile";

        public const string UserProfilePath = "/Content/UserProfile";

        public const string ProductImagesPath = "/Content/ProductImages";

        public const string ResponseOK = "OK";

        public const string DefaultPassword = "ADn9JXAPolz3R1QmEjw7chz42neVTLn426+eRtgaG2TvE2mR03Ri7TtoKH8iWJyuVw==";

        public const int DaysToCancel = 30;

        public const int DaysToModify = 30;

        public const int DescriptionLength = 300;
    }

    public struct FileStruct
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public byte[] Bytes { get; set; }

        public int Size { get { return this.Bytes != null ? this.Bytes.Length : Cons.Zero; } }

    }
}