using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HK.Pub.Dal
{
    public class Regex
    {
        static public string Replace(string input, string pattern, string replace) { return System.Text.RegularExpressions.Regex.Replace(input, pattern, replace); }

        static public string Match(string input, string pattern) { return System.Text.RegularExpressions.Regex.Match(input, pattern).Value; }
        static public bool IsMatch(string input, string pattern) { return System.Text.RegularExpressions.Regex.IsMatch(input, pattern); }

        static public bool IsPhone(string num) { return IsMatch(num, @"^[1][3,4,5,7,8][0-9]{9}$"); }
        static public bool IsCardId(string num) { return IsMatch(num, @"(^[1-9]\d{5}(18|19|([23]\d))\d{2}((0[1-9])|(10|11|12))(([0-2][1-9])|10|20|30|31)\d{3}[0-9Xx]$)|(^[1-9]\d{5}\d{2}((0[1-9])|(10|11|12))(([0-2][1-9])|10|20|30|31)\d{2}[0-9Xx]$)"); }
        static public bool IsEmail(string num) { return IsMatch(num, @"^[A-Za-z\d]+([-_.][A-Za-z\d]+)*@([A-Za-z\d]+[-.])+[A-Za-z\d]{2,4}$"); }
        static public bool IsTell(string num) { return IsMatch(num, @"^(0[0-9]{2,3}\-)?([2-9][0-9]{6,7})+(\-[0-9]{1,4})?$"); }
        static public bool IsChinese(string num) { return IsMatch(num, @"^[\u0391-\uFFE5]+$"); }
    }
}
