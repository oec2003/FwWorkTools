using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using Microsoft.VisualBasic;
using System.Collections;
using System.Web.Security;
using System.Linq;

namespace FW.CommonFunction
{
    /// <summary>
    /// String类型的扩展方法类
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// 获得当前绝对路径
        /// </summary>
        /// <param name="strPath">指定的路径</param>
        /// <returns>绝对路径</returns>
        public static string GetMapPath(this string strPath)
        {
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Server.MapPath(strPath);
            }
            else //非web程序引用
            {
                return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
            }
        }

        /// <summary>
        /// 返回字符串真实长度, 1个汉字长度为2
        /// </summary>
        /// <returns></returns>
        public static int GetStringRealLength(this string str)
        {
            return Encoding.Default.GetBytes(str).Length;
        }

        /// <summary>
        /// 判断对象是否为Int32类型的数字
        /// </summary>
        /// <param name="s">传入的字符串</param>
        /// <returns></returns>
        public static bool IsNumeric(this string s)
        {
            if (s != null)
            {
                if (s.Length > 0 && s.Length <= 11 && Regex.IsMatch(s, @"^[-]?[0-9]*[.]?[0-9]*$"))
                {
                    if ((s.Length < 10) || (s.Length == 10 && s[0] == '1') || (s.Length == 11 && s[0] == '-' && s[1] == '1'))
                    {
                        return true;
                    }
                }
            }
            return false;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s">传入的字符串</param>
        /// <returns></returns>
        public static bool IsDouble(this string s)
        {
            if (s != null)
            {
                return Regex.IsMatch(s, @"^([0-9])[0-9]*(\.\w*)?$");
            }
            return false;
        }

        /// <summary>
        /// string型转换为bool型
        /// </summary>
        ///<param name="s">传入的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的bool类型结果</returns>
        public static bool ToBoolean(this string s, bool defValue)
        {
            if (s != null)
            {
                if (string.Compare(s.ToUpper(), "TRUE", true) == 0)
                {
                    return true;
                }
                else if (string.Compare(s.ToUpper(), "FALSE", true) == 0)
                {
                    return false;
                }
            }
            return defValue;
        }

        /// <summary>
        /// 将对象转换为Int32类型
        /// </summary>
        /// <param name="defValue">缺省值</param>
        /// <param name="s">传入的字符串</param>
        /// <returns>转换后的int类型结果</returns>
        public static Int32 ToInt32(this string s, int defValue)
        {

            if (s != null)
            {
                if (s.Length > 0 && s.Length <= 11 && Regex.IsMatch(s, @"^[-]?[0-9]*$"))
                {
                    if ((s.Length < 10) || (s.Length == 10 && s[0] == '1') || (s.Length == 11 && s[0] == '-' && s[1] == '1'))
                    {
                        return Convert.ToInt32(s);
                    }
                }
            }
            return defValue;
        }

        /// <summary>
        /// 将对象转换为Int16类型
        /// </summary>
        /// <param name="defValue">缺省值</param>
        /// <param name="s">传入的字符串</param>
        /// <returns>转换后的int类型结果</returns>
        public static Int16 ToInt16(this string s, Int16 defValue)
        {

            if (s != null)
            {
                if (s.Length > 0 && s.Length <= 11 && Regex.IsMatch(s, @"^[-]?[0-9]*$"))
                {
                    if ((s.Length < 10) || (s.Length == 10 && s[0] == '1') || (s.Length == 11 && s[0] == '-' && s[1] == '1'))
                    {
                        return Convert.ToInt16(s);
                    }
                }
            }
            return defValue;
        }

        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="s">传入的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static Single ToSingle(this string s, Single defValue)
        {
            if ((s == null) || (s.ToString().Length > 10))
            {
                return defValue;
            }

            Single value = defValue;
            if (s != null)
            {
                bool isFloat = Regex.IsMatch(s.ToString(), @"^([-]|[0-9])[0-9]*(\.\w*)?$");
                if (isFloat)
                {
                    value = Convert.ToSingle(s);
                }
            }
            return value;
        }

        /// <summary>
        /// string型转换为decimal型
        /// </summary>
        /// <param name="s">传入的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static decimal ToDecimal(this string s, decimal defValue)
        {
            if ((s == null))
            {
                return defValue;
            }

            decimal value = defValue;
            if (s != null)
            {
                bool isDecimal = Regex.IsMatch(s.ToString(), @"^([-]|[0-9])[0-9]*(\.\w*)?$");
                if (isDecimal)
                {
                    value = Convert.ToDecimal(s);
                }
            }
            return value;
        }

        /// <summary>
        /// string型转换为DateTime类型
        /// </summary>
        /// <param name="s">传入的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的DateTime类型结果</returns>
        public static DateTime ToDateTime(this string s, DateTime defValue)
        {
            if (s == null)
            {
                return defValue;
            }
            else
            {
                try
                {
                    return  Convert.ToDateTime(s);
                }
                catch
                {
                    return defValue;
                }
            }
        }


        /// <summary>
        /// 判断给定的字符串数组(strNumber)中的数据是不是都为数值型
        /// </summary>
        /// <param name="strNumber">要确认的字符串数组</param>
        /// <returns>是则返加true 不是则返回 false</returns>
        public static bool IsNumericArray(string[] strNumber)
        {
            if (strNumber == null)
            {
                return false;
            }
            if (strNumber.Length < 1)
            {
                return false;
            }
            foreach (string id in strNumber)
            {
                if (!IsNumeric(id))
                {
                    return false;
                }
            }
            return true;

        }
    }
}
