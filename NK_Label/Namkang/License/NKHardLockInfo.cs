using System;

namespace Namkang.License
{
    public class NKHardLockInfo
    {
        public string VendorCode { get; set; }
        public int Feature { get; set; }
        public string Scope { get; set; }

        public string HaspID { get; set; }
        public int MaxMemory { get; set; }
        public NKLicenseList.MemoryType MemoryType { get; set; }

        public bool IsNetKey { get; set; }
        public string NetCountLimit { get; set; } // 0, 10, 50, unlimited

        public DateTime ServiceExpirationDate { get; set; }
        public DateTime ManufacturedDate { get; set; }

        /* Add some properties for program's option */
        //public bool HasSpecificOption { get; set; }
        //public string OptionCode { get; set; }
        //public string LicenseCode { get; set; }
    }
}
