using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRService.Utils
{
    public class ValidationUtils
    {
        #region string validation
        /// <summary>
        /// from System.Web.CrossSiteScriptingValidation
        /// </summary>
        /// <param name="s"></param>
        /// <param name="MatchIndex"></param>
        /// <returns></returns>
        private static char[] startingChars = new char[] { '<', '&' };
        public static bool IsDangerousString(string s, out int matchIndex)
        {
            matchIndex = 0;
            int startIndex = 0;
            while (true)
            {
                int num2 = s.IndexOfAny(startingChars, startIndex);
                if (num2 < 0)
                {
                    return false;
                }
                if (num2 == (s.Length - 1))
                {
                    return false;
                }
                matchIndex = num2;
                char ch = s[num2];
                if (ch != '&')
                {
                    if ((ch == '<') && ((IsAtoZ(s[num2 + 1]) || (s[num2 + 1] == '!')) || ((s[num2 + 1] == '/') || (s[num2 + 1] == '?'))))
                    {
                        return true;
                    }
                }
                else if (s[num2 + 1] == '#')
                {
                    return true;
                }
                startIndex = num2 + 1;
            }
        }
        private static bool IsAtoZ(char c)
        {
            return (((c >= 'a') && (c <= 'z')) || ((c >= 'A') && (c <= 'Z')));
        }
        #endregion

        
    }
}