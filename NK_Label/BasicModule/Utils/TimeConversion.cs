using System;
using System.Globalization;

namespace BasicModule.Utils
{
    public static class TimeConversion
    {
        public static string DateToString(string pattern)
        {
            try
            {
                // InvariantCulture : 문화권 독립(고정)적인 개체
                // DefaultThreadCurrentCulture : 현재 응용 프로그램 도메인의 스레드에 대한 기본 문화권(시스템 시간 변경 시 자동 변경)
                // InstalledUICulture : 운영 체제에 설치된 문화권
                // CurrentUICulture : 리소스 관리자가 런타임에 문화권 관련 리소스를 찾기 위해 사용하는 문화권
                // CurrentCulture : 현재 스레드에서 사용하는 문화권
                return DateTime.Now.ToString(pattern, CultureInfo.DefaultThreadCurrentCulture);
            }
            catch (Exception e)
            {
                return "패턴오류";
            }
        }
    }
}
