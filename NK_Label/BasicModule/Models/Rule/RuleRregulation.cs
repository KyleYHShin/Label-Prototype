using System;
using System.Collections.Generic;

namespace BasicModule.Models.Rule
{
    public static class RuleRegulation
    {
        private static char SymbolStart = '*';
        private static char SymbolPre = '{';
        private static char SymbolPost = '}';
        private static char SymbolEnd = '*';

        public static string Prefix = SymbolStart.ToString() + SymbolPre.ToString();
        public static string Postfix = SymbolPost.ToString() + SymbolEnd.ToString();

        public static int MIN_NAME_LEN = 2;

        public enum RuleFormat
        {
            TIME = 1,
            SEQUENTIAL_NUM = 10,
            MANUAL_LIST = 20
        }

        public static readonly Dictionary<string, RuleFormat> BarcodeFormatList = new Dictionary<string, RuleFormat>
        {
            { "Time", RuleFormat.TIME }
            , { "Sequential Number", RuleFormat.SEQUENTIAL_NUM }
            , { "Manual List", RuleFormat.MANUAL_LIST }
        };

        public static Dictionary<string, string> TimeFormatList
        {
            get
            {
                // 사용 불가 목록 : %
                return new Dictionary<string, string>()
                {
                    { "yyyy" , "4자리 연도"}
                    , { "yy" , "2자리 연도"}
                    , { "MM" , "월 (숫자)"}
                    , { "MMM" , "월 (약식 문자)"}
                    , { "MMMM" , "월 (전체 문자)"}
                    , { "dd" , "일"}
                    , { "hh" , "시 (12시간 형식)"}
                    , { "HH" , "시 (24시간 형식)"}
                    , { "tt" , "AM / PM"}
                    , { "mm" , "분"}
                    , { "ss" , "초"}
                };
            }
        }

        public static bool RuleNameVerifier(string combinedName)
        {
            try
            {
                int startIndex = Prefix.Length;
                int endIndex = combinedName.Length - Postfix.Length;
                string name = combinedName.Substring(startIndex, endIndex - startIndex);

                string symbols = @"[~!@\#$%^&*\()\=+|\\/:;?""<>']";
                if (combinedName.StartsWith(Prefix) && combinedName.EndsWith(Postfix)
                    && !string.IsNullOrEmpty(name) && name.Length >= MIN_NAME_LEN
                    && !new System.Text.RegularExpressions.Regex(symbols).IsMatch(name)) //특수기호 없이 문자와 숫자로만 된
                    return true;

                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static string RuleNameCombiner(string name)
        {
            return Prefix + name + Postfix;
        }

        public static string RuleNameExtractor(string combinedName)
        {
            if (RuleNameVerifier(combinedName))
            {
                int startIndex = Prefix.Length;
                int endIndex = combinedName.Length - Postfix.Length;
                return combinedName.Substring(startIndex, endIndex - startIndex);
            }
            else
                return "";
        }

        private static int GetNextStartIndex(int startIndex, string text)
        {
            string str = text.Substring(startIndex);
            int ret = str.IndexOf(Prefix);
          
            return ret;
        }

        public static List<string> RuleNameSeperatorToList(string text)
        {
            List<string> wordList = new List<string>();
            try
            {
                while (true)
                {
                    int startIndex;
                    int endIndex;

                    int x = text.IndexOf(Prefix);
                    if (x >= 0)
                        startIndex = x;
                    else
                    {
                        wordList.Add(text);
                        break;
                    }

                    x = text.IndexOf(Postfix);
                    if (x >= 0)
                        endIndex = x;
                    else
                    {
                        wordList.Add(text);
                        break;
                    }

                    if (startIndex > endIndex)
                    {
                        wordList.Add(text.Substring(0, startIndex));
                        text = text.Substring(startIndex);
                    }
                    else
                    {
                        if (startIndex > 0)
                            wordList.Add(text.Substring(0, startIndex));

                        wordList.Add(text.Substring(startIndex, endIndex - startIndex + Postfix.Length));
                        text = text.Substring(endIndex + Postfix.Length);
                    }
                    if (string.IsNullOrEmpty(text))
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                wordList.Clear();
            }
            return wordList;
        }

    }
}