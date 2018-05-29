using Aladdin.HASP;
using System;
using System.Xml;

namespace Namkang.License
{
    public static class Controller
    {
        public static Hasp HaspData { get; private set; }
        public static NKHardLockInfo ProgramLicense { get; private set; }

        private static void InitializeLicense()
        {
            // Must change by released program
            ProgramLicense = new NKHardLockInfo()
            {
                HaspID = string.Empty,

                VendorCode = NKLicenseList.Code_YIOBI,
                Feature = (int)NKLicenseList.Feature_YIOBI.NK_Label,
                Scope = NKLicenseList.DefaultScope,
                NetMaxCount = string.Empty,
                IsNetKey = true,

                MaxMemory = (int)NKLicenseList.MaxMemory.SRM_PRO,
                MemoryType = NKLicenseList.MemoryType.ePM_Ex,

                ManufacturedDate = new DateTime(),
                ServiceExpirationDate = new DateTime(),
            };
            //

            HaspFeature feature = HaspFeature.FromFeature(ProgramLicense.Feature);
            HaspData = new Hasp(feature);
        }

        public static string Login()
        {
            InitializeLicense();

            HaspStatus loginResult = HaspData.Login(ProgramLicense.VendorCode, ProgramLicense.Scope);
            // Please note that there is no need to call a logout function explicitly - although it is recommended.
            // The garbage collector will perform the logout when disposing the object.
            // If you need more control over the logout procedure perform one of the more advanced tasks. - Gemalto

            if (HaspStatus.StatusOk == loginResult && HaspData.IsLoggedIn())
            {
                string sessionInfo = string.Empty;
                loginResult = HaspData.GetSessionInfo(Hasp.KeyInfo, ref sessionInfo);
                GetProgramLicenseDataFromKey(sessionInfo);
                GetProgramLicenseDataFromMemoryField();
            }

            return GetLicenseMessage();
        }

        public static string GetLicenseMessage()
        {

            HaspStatus loginSessionStatus = HaspData.Login(ProgramLicense.VendorCode, ProgramLicense.Scope);

            // Should update from test and user case
            if (ProgramLicense.IsNetKey && ProgramLicense.NetMaxCount.Equals("0"))
                return "라이선스 키: 네트워크 키가 아닙니다.";
                
            string msg = string.Empty;
            switch (loginSessionStatus)
            {
                case HaspStatus.AlreadyLoggedIn:
                case HaspStatus.StatusOk:
                    msg = string.Empty;
                    break;
                case HaspStatus.InvalidHandle:
                case HaspStatus.ContainerNotFound:
                    msg = "라이선스 키를 확인할 수 없습니다.";
                    break;
                case HaspStatus.InvalidFeature:
                    msg = "라이선스 키 : 사용 권한이 없습니다.";
                    break;
                case HaspStatus.TooManyUsers:
                    msg = "라이선스 키 : 최대 사용자 수를 초과하였습니다.";
                    break;
                case HaspStatus.BrokenSession:
                    msg = "라이선스 키 세션이 종료되었습니다.";
                    break;
                case HaspStatus.RemoteCommErr:
                    msg = "라이선스 키 : 네트워크 통신 오류";
                    break;
                default:
                    msg = "라이선스 키 : 알 수 없는 오류(" + loginSessionStatus + ")";
                    break;
            }
            return msg;
        }

        public static void GetProgramLicenseDataFromKey(string xmlString)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);

            XmlNodeList xmlList = xmlDoc.GetElementsByTagName("hasp");
            foreach (XmlNode item in xmlList)
            {
                ProgramLicense.HaspID = item["haspid"].InnerText;
                ProgramLicense.NetMaxCount = item["nethasptype"].InnerText;
            }
        }

        public static void GetProgramLicenseDataFromMemoryField()
        {
            HaspFile file = HaspData.GetFile(HaspFileId.ReadWrite);
            byte[] data = new byte[ProgramLicense.MaxMemory];
            HaspStatus status = file.Read(data, 0, data.Length);

            // Should redefine memory writting styles between 'ePM-Ex', 'Kohyoung', 'EAP' and  'MES' in 'NKHardLockInfo' class
            if (HaspStatus.StatusOk == status)
            {
                switch (ProgramLicense.MemoryType)
                {
                    case NKLicenseList.MemoryType.ePM_Ex:

                        // Check OptionCode

                        int preYear = DateTime.Now.Year / 100 * 100;
                        int year = ParseHexintToInt(data[10]);
                        int month = ParseHexintToInt(data[11]);
                        int day = DateTime.DaysInMonth(year, month);
                        ProgramLicense.ServiceExpirationDate = new DateTime(preYear + year, month, day, 23, 59, 59);

                        // Check LicenseCode

                        year = ParseHexintToInt(data[24]);
                        month = ParseHexintToInt(data[25]);
                        day = ParseHexintToInt(data[26]);
                        ProgramLicense.ManufacturedDate = new DateTime(preYear + year, month, day);
                        break;
                    case NKLicenseList.MemoryType.Kohyoung:
                        break;
                    case NKLicenseList.MemoryType.EAP:
                        break;
                    case NKLicenseList.MemoryType.MES:
                        break;
                    default:
                        break;
                }
            }
        }

        private static int ParseHexintToInt(int hexInt)
        {
            int Hex = 16;
            return hexInt / Hex * 10 + hexInt % Hex;
        }

    }
}
