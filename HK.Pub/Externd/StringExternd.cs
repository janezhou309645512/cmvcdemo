using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// 扩展字符串类的一些方法
    /// </summary>
    static public class StringExternd
    {
        /// <summary>
        /// 字符串做正则匹配
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pattern">正则字符串</param>
        /// <returns></returns>
        static public string Match(this string input, string pattern) { return HK.Pub.Dal.Regex.Match(input, pattern); }
        static public bool IsMatch(this string input, string pattern) { return HK.Pub.Dal.Regex.IsMatch(input, pattern); }
        /// <summary>
        /// 判断字符串是否是手机号
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        static public bool IsPhone(this string input) { return HK.Pub.Dal.Regex.IsPhone(input); }
        /// <summary>
        /// 判断字符串是否是邮件
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        static public bool IsEMail(this string input) { return HK.Pub.Dal.Regex.IsEmail(input); }
        /// <summary>
        /// 判断字符串是否是身份证号码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        static public bool IsCardId(this string input) { return HK.Pub.Dal.Regex.IsCardId(input); }
        /// <summary>
        /// 判断字符串是否是中文
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        static public bool IsChinese(this string input) { return HK.Pub.Dal.Regex.IsChinese(input); }
        /// <summary>
        /// 判断字符串是否是电话号码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        static public bool IsTell(this string input) { return HK.Pub.Dal.Regex.IsTell(input); }
        /// <summary>
        /// 判断字符串是否为空
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        static public bool IsNullOrEmpty(this string input) { return string.IsNullOrEmpty(input); }
    }



}
