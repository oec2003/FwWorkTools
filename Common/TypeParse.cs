/****************************************************************
 * 作者：冯威添加
 * 日期：2010.11.09 
 * 基类： 
 * 功能：系统强类型转换
 * 修改：修改日期,修改人加说明
****************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace FW.CommonFunction
{
    /// <summary>
    /// 
    /// </summary>
    public static class TypeParse
    {  
        /// <summary>
        /// 判断对象是否为Int32类型的数字
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public static bool IsNumeric(object Expression)
        {
            if (Expression != null)
            {
                string str = Expression.ToString();
                if (str.Length > 0 && str.Length <= 11 && Regex.IsMatch(str, @"^[-]?[0-9]*[.]?[0-9]*$"))
                {
                    if ((str.Length < 10) || (str.Length == 10 && str[0] == '1') || (str.Length == 11 && str[0] == '-' && str[1] == '1'))
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
        /// <param name="Expression"></param>
        /// <returns></returns>
        public static bool IsDouble(object Expression)
        {
            if (Expression != null)
            {
                return Regex.IsMatch(Expression.ToString(), @"^([0-9])[0-9]*(\.\w*)?$");
            }
            return false;
        }

        /// <summary>
        /// string型转换为bool型
        /// </summary>
        ///<param name="Expression"></param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的bool类型结果</returns>
        public static bool StrToBool(object Expression, bool defValue)
        {
            if (Expression != null)
            {
                if (string.Compare(Expression.ToString(), "true", true) == 0)
                {
                    return true;
                }
                else if (string.Compare(Expression.ToString(), "false", true) == 0)
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
        /// <param name="Expression"></param>
        /// <returns>转换后的int类型结果</returns>
        public static int StrToInt(object Expression, int defValue)
        {

            if (Expression != null)
            {
                string str = Expression.ToString();
                if (str.Length > 0 && str.Length <= 11 && Regex.IsMatch(str, @"^[-]?[0-9]*$"))
                {
                    if ((str.Length < 10) || (str.Length == 10 && str[0] == '1') || (str.Length == 11 && str[0] == '-' && str[1] == '1'))
                    {
                        return Convert.ToInt32(str);
                    }
                }
            }
            return defValue;
        }

        /// <summary>
        /// 将对象转换为Int16类型
        /// </summary>
        /// <param name="defValue">缺省值</param>
        /// <param name="Expression"></param>
        /// <returns>转换后的int类型结果</returns>
        public static Int16 StrToShort(object Expression, Int16 defValue)
        {

            if (Expression != null)
            {
                string str = Expression.ToString();
                if (str.Length > 0 && str.Length <= 11 && Regex.IsMatch(str, @"^[-]?[0-9]*$"))
                {
                    if ((str.Length < 10) || (str.Length == 10 && str[0] == '1') || (str.Length == 11 && str[0] == '-' && str[1] == '1'))
                    {
                        return Convert.ToInt16(str);
                    }
                }
            }
            return defValue;
        }

        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static float StrToFloat(object strValue, float defValue)
        {
            if ((strValue == null) || (strValue.ToString().Length > 10))
            {
                return defValue;
            }

            float intValue = defValue;
            if (strValue != null)
            {
                bool IsFloat = Regex.IsMatch(strValue.ToString(), @"^([-]|[0-9])[0-9]*(\.\w*)?$");
                if (IsFloat)
                {
                    intValue = Convert.ToSingle(strValue);
                    
                }
            }
            return intValue;
        }

        /// <summary>
        /// string型转换为decimal型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static decimal StrToDecimal(object strValue, decimal defValue)
        {
            if ((strValue == null))
            {
                return defValue;
            }

            decimal intValue = defValue;
            if (strValue != null)
            {
                bool IsFloat = Regex.IsMatch(strValue.ToString(), @"^([-]|[0-9])[0-9]*(\.\w*)?$");
                if (IsFloat)
                {
                    intValue = Convert.ToDecimal(strValue);

                }
            }
            return intValue;
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

        /// <summary>
        /// 去掉数据后面的.00，比如3.00去掉.00得到3
        /// </summary>
        /// <param name="strNumber">要去掉.00的数据</param>
        /// <returns></returns>
        public static string SubLastThreeZero(Object strNumber)
        {
            if (strNumber != null && !string.IsNullOrEmpty(strNumber.ToString().Trim()))
            {
                string[] Nums = strNumber.ToString().Trim().Split('.');
                if(Nums.Length==2)
                {
                    Byte[] NumBytes = Encoding.ASCII.GetBytes(Nums[1]);
                    Char[] NumChars = Encoding.ASCII.GetChars(NumBytes);
                    ArrayList arr = new ArrayList(NumChars);
                    for(int i=arr.Count;i>0;i--)
                    {
                        if(arr[i-1].ToString()=="0")
                        {
                            arr.RemoveAt(i - 1);
                        }
                        else
                        {
                            break;
                        }
                    }
                    if(arr.Count>0)
                    {
                        string Num1 = "";
                        for(int i=0;i<arr.Count;i++)
                        {
                            Num1 += arr[i];
                            
                        }
                        return Nums[0] + "." + Num1;
                    }
                    else
                    {
                        return Nums[0];
                    }
                }
                return strNumber.ToString();
                
            }
            return "";
        }
    }
}
