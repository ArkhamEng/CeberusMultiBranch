using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CerberusMultiBranch.Support
{
    public static class Cons
    {
        public struct Responses
        {
            public const string  Success = "success";

            public const string Warning = "warning";

            public const string Danger = "danger";

            public const string Info = "info";

            public struct Codes
            {
                public const int ConditionMissing = -5;
                public const int InvalidData = -4;
                public const int ErroSaving     = -3;
                public const int RecordNotFound = -2;
                public const int RecordLocked = -1;
                public const int Success = 200;
                public const int ServerError = 500;
                public const int UnAuthorized = 401;
                

            }
        }

        public const int QuickResults = 300;

        public const int MaxResults  = 30000;

        public const int Zero = 0;
        public const int One = 1;
        public const int Two = 2;

        public const int RefundId = 3;

        public const string BudgetFormat = "000000000";

        public const int Decimals = 4; //cantidad de decimales en redondeo de dinero

        public const double DeIVA = 1.16;

        public const int LockTimeOut = 5;

        public const double OneHundred = 100;

        

        public const string CodeSeqFormat = "000000";

        public const string VariableIVA = "IVA";

        public const string CodeMask = "{0}{1}{2}";

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

        public const string LogingUrl = "/Account/Login";
    }

    public struct FileStruct
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public byte[] Bytes { get; set; }

        public int Size { get { return this.Bytes != null ? this.Bytes.Length : Cons.Zero; } }

    }
}