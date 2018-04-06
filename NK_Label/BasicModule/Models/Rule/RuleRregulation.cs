using System;
using System.Collections.Generic;

namespace BasicModule.Models.Rule
{
    public static class RuleRregulation
    {
        private static char Symbol = '*';
        private static char SymbolPre = '{';
        private static char SymbolPost = '}';

        public static string Prefix = Symbol.ToString() + SymbolPre.ToString();
        public static string Postfix = SymbolPost.ToString() + Symbol.ToString();

        public static int MIN_NAME_LEN = 2;

        public enum RuleFormat
        {
            TIME = 1,
            SEQUENTIAL_NUM = 10,
            MANUAL_LIST = 20
        }

        //미사용 시 삭제
        public static List<string> TimeFormatList
        {
            get
            {
                return new List<string>()
                {
                    "yyyy", "yy", "MM", "dd"
                    , "hh", "HH", "mm", "ss"
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

        public static string[] RuleNameSeperatorToList(string originText)
        {
            try
            {
                List<int> startIndex = new List<int>();
                List<int> endIndex = new List<int>();
                int blockCount = 0;
                for (int i = 0; i < originText.Length; i++)
                {
                    if (originText[i] == Symbol && i + 1 < originText.Length - 1 && originText[i + 1] == SymbolPre)
                    {
                        startIndex.Add(i);
                        blockCount++;
                    }
                    else if (originText[i] == Symbol && i - 1 >= 0 && originText[i - 1] == SymbolPost)
                    {
                        endIndex.Add(i);
                        blockCount++;
                    }
                }
                if (startIndex.Count != endIndex.Count)
                    throw new Exception();

                string[] blocks = new string[blockCount + 1];
                int blockIndex = 0;
                int prevIndex = 0;
                for (int i = 0; i < originText.Length; i++)
                {
                    if (startIndex[0] == i)
                    {
                        if (i > 0)
                        {
                            blocks[blockIndex] = originText.Substring(prevIndex, i - prevIndex);
                            blockIndex++;
                        }
                        var ruleName = originText.Substring(i, endIndex[0] - i + 1);
                        if (!RuleNameVerifier(ruleName))
                            throw new Exception();
                        blocks[blockIndex] = ruleName;
                        blockIndex++;

                        prevIndex = endIndex[0] + 1;
                        i = endIndex[0] + 1;
                        startIndex.RemoveAt(0);
                        endIndex.RemoveAt(0);
                    }
                    if (endIndex.Count == 0 && i < originText.Length - 1)
                    {
                        blocks[blockIndex] = originText.Substring(i, originText.Length - i);
                        break;
                    }
                }
                return blocks;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return null;
            }
        }

    }
}