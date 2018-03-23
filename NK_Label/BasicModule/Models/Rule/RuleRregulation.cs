namespace BasicModule.Models.Rule
{
    public static class RuleRregulation
    {
        public static string Prefix = "*{";
        public static string Postfix = "}*";

        public enum RuleFormat
        {
            TIME = 1,
            SEQUENTIAL_NO = 10,
            MANUAL_LIST = 20
        }
    }
}
