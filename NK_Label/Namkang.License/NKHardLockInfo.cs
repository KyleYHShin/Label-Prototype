using System;

namespace Namkang.License
{
    public class NKHardLockInfo
    {
        public string VendorCode { get; set; }
        public int Feature { get; set; }
        public string Scope { get; set; }

        public string HaspID { get; set; }
        public string NetCountLimit { get; set; } // 0, 10, 50, unlimited
        public int MaxMemory { get; set; }
        public NKLicenseList.MemoryType MemoryType { get; set; }

        //public string OptionCode { get; set; } // 20자리 Hexa code(미사용)
        public DateTime ServiceExpirationDate { get; set; } // 4자리 Hexa code
        //public string LicenseCode { get; set; } // 24자리 Hexa code(미사용)
        public DateTime ManufacturedDate { get; set; } // 6자리 Hexa code
    }
}
