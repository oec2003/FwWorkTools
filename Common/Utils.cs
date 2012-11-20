/****************************************************************
 * 作者：冯威
 * 日期：2010.11.09 
 * 基类： 
 * 功能：系统常用方法 工具助手
 * 修改：修改日期,修改人加说明
****************************************************************/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Office.Interop.Excel;
using Microsoft.VisualBasic;
using System.Collections;
using System.Web.Security;
using Wuqi.Webdiyer;


using DataTable=System.Data.DataTable;
using Label=System.Web.UI.WebControls.Label;

namespace FW.CommonFunction
{
    public enum FlagEnum
    {
        Success,
        Fail,
        Error
    }


    /// <summary>
    /// 工具类
    /// </summary>
    public static class Utils
    {
        public static string _filePath = ConfigInfo.GetConfigValue("FilePath");

        /// <summary>
        /// 获得当前绝对路径
        /// </summary>
        /// <param name="strPath">指定的路径</param>
        /// <returns>绝对路径</returns>
        public static string GetMapPath(string strPath)
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
        /// 通过文件bytes
        /// 计算文件容量
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string GetFileWeightByBytes(int bytes)
        {
            if (bytes > 1073741824)
            {
                return ((double)(bytes / 1073741824)).ToString("0") + "G";
            }
            if (bytes > 1048576)
            {
                return ((double)(bytes / 1048576)).ToString("0") + "M";
            }
            if (bytes > 1024)
            {
                return ((double)(bytes / 1024)).ToString("0") + "K";
            }
            return bytes.ToString() + "Bytes";
        }
      
        #region 字符串截取及分割
        /// <summary>
        /// 返回字符串真实长度, 1个汉字长度为2
        /// </summary>
        /// <returns></returns>
        public static int GetStringLength(string str)
        {
            return Encoding.Default.GetBytes(str).Length;
        }

        /// <summary>
        /// 字符串如果操过指定长度则将超出的部分用指定字符串代替
        /// </summary>
        /// <param name="p_SrcString">要检查的字符串</param>
        /// <param name="p_Length">指定长度</param>
        /// <param name="p_TailString">用于替换的字符串</param>
        /// <returns>截取后的字符串</returns>
        public static string GetSubString(string p_SrcString, int p_Length, string p_TailString)
        {
            return GetSubString(p_SrcString, 0, p_Length, p_TailString);
        }


       /// <summary>
        /// 取指定长度的字符串
        /// </summary>
        /// <param name="p_SrcString">要检查的字符串</param>
        /// <param name="p_StartIndex">起始位置</param>
        /// <param name="p_Length">指定长度</param>
        /// <param name="p_TailString">用于替换的字符串</param>
        /// <returns>截取后的字符串</returns>
        public static string GetSubString(string p_SrcString, int p_StartIndex, int p_Length, string p_TailString)
        {


            string myResult = p_SrcString;

            ////当是日文或韩文时(注:中文的范围:\u4e00 - \u9fa5, 日文在\u0800 - \u4e00, 韩文为\xAC00-\xD7A3)
            //if (Regex.IsMatch(p_SrcString, "[\u0800-\u4e00]+") ||
            //    Regex.IsMatch(p_SrcString, "[\xAC00-\xD7A3]+"))
            //{
            //    //当截取的起始位置超出字段串长度时
            //    if (p_StartIndex >= p_SrcString.Length)
            //    {
            //        return "";
            //    }
            //    else
            //    {
            //        return p_SrcString.Substring(p_StartIndex,
            //                                       ((p_Length + p_StartIndex) > p_SrcString.Length) ? (p_SrcString.Length - p_StartIndex) : p_Length);
            //    }
            //}


            if (p_Length >= 0)
            {
                byte[] bsSrcString = Encoding.Default.GetBytes(p_SrcString);

                //当字符串长度大于起始位置
                if (bsSrcString.Length > p_StartIndex)
                {
                    int p_EndIndex = bsSrcString.Length;

                    //当要截取的长度在字符串的有效长度范围内
                    if (bsSrcString.Length > (p_StartIndex + p_Length))
                    {
                        p_EndIndex = p_Length + p_StartIndex;
                    }
                    else
                    {   //当不在有效范围内时,只取到字符串的结尾

                        p_Length = bsSrcString.Length - p_StartIndex;
                        p_TailString = "";
                    }



                    int nRealLength = p_Length;
                    int[] anResultFlag = new int[p_Length];
                    byte[] bsResult = null;

                    int nFlag = 0;
                    for (int i = p_StartIndex; i < p_EndIndex; i++)
                    {

                        if (bsSrcString[i] > 127)
                        {
                            nFlag++;
                            if (nFlag == 3)
                            {
                                nFlag = 1;
                            }
                        }
                        else
                        {
                            nFlag = 0;
                        }

                        anResultFlag[i] = nFlag;
                    }

                    if ((bsSrcString[p_EndIndex - 1] > 127) && (anResultFlag[p_Length - 1] == 1))
                    {
                        nRealLength = p_Length + 1;
                    }

                    bsResult = new byte[nRealLength];

                    Array.Copy(bsSrcString, p_StartIndex, bsResult, 0, nRealLength);

                    myResult = Encoding.Default.GetString(bsResult);

                    myResult = myResult + p_TailString;
                }
            }

            return myResult;
        }
        
        /// <summary>
        /// 分割字符串
        /// </summary>
        public static string[] SplitString(string strContent, string strSplit)
        {
            if (strContent.IndexOf(strSplit) < 0)
            {
                string[] tmp = { strContent };
                return tmp;
            }
            return Regex.Split(strContent, Regex.Escape(strSplit), RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <returns></returns>
        public static string[] SplitString(string strContent, string strSplit, int p_3)
        {
            string[] result = new string[p_3];

            string[] splited = SplitString(strContent, strSplit);

            for (int i = 0; i < p_3; i++)
            {
                if (i < splited.Length)
                    result[i] = splited[i];
                else
                    result[i] = string.Empty;
            }

            return result;
        }

        #endregion

        #region 日期操作
        /// <summary>
        /// 返回标准日期格式string
        /// </summary>
        public static string GetDateyyyyMMdd()
        {
            return DateTime.Now.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 将指定日期转换成yyyyMMdd形式
        /// </summary>
        /// <param name="datetimestr"></param>
        /// <returns></returns>
        public static string GetDateyyyyMMdd(object datetimestr)
        {
            if (datetimestr == null || datetimestr.ToString().Equals(""))
            {
                return "";
            }

            try
            {
                datetimestr = Convert.ToDateTime(datetimestr).ToString("yyyy-MM-dd");
            }
            catch
            {
                return "";
            }
            return datetimestr.ToString();

        }

        /// <summary>
        /// 将指定日期转换成yyyy年MM月dd日形式
        /// </summary>
        /// <param name="datetimestr"></param>
        /// <returns></returns>
        public static string GetDateyyyyMMddH(object datetimestr)
        {
            string dateStr = "";
            if (datetimestr == null || datetimestr.ToString().Equals(""))
            {
                return "";
            }

            try
            {
                
                DateTime date = Convert.ToDateTime(datetimestr);
                string year = date.Year.ToString();
                string month = date.Month.ToString();
                string day = date.Day.ToString();
                dateStr= year + "年" + month + "月" + day + "日";
            }
            catch
            {
                return "";
            }
            return dateStr;

        }


        /// <summary>
        /// 返回指定日期格式
        /// </summary>
        public static string GetDateyyyyMMdd(string datetimestr, string replacestr)
        {
            if (datetimestr == null)
            {
                return replacestr;
            }

            if (datetimestr.Equals(""))
            {
                return replacestr;
            }

            try
            {
                datetimestr = Convert.ToDateTime(datetimestr).ToString("yyyy-MM-dd").Replace("1900-01-01", replacestr);
            }
            catch
            {
                return replacestr;
            }
            return datetimestr;

        }
        
       
        /// <summary>
        /// 返回标准时间格式string
        /// </summary>
        public static string GetDateTimeyyyyMMddHHmmss()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 返回标准时间格式string
        /// </summary>
        public static string GetExcelDateTimeyyyyMMddHHmmss()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmss");
        }

        #endregion
                     
        #region 字符串编码
        /// <summary>
        /// 返回 HTML 字符串的编码结果
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>编码结果</returns>
        public static string HtmlEncode(string str)
        {
            return HttpUtility.HtmlEncode(str);
        }

        /// <summary>
        /// 返回 HTML 字符串的解码结果
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>解码结果</returns>
        public static string HtmlDecode(string str)
        {
            return HttpUtility.HtmlDecode(str);
        }

        /// <summary>
        /// 返回 URL 字符串的编码结果
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>编码结果</returns>
        public static string UrlEncode(string str)
        {
            return HttpUtility.UrlEncode(str);
        }

        /// <summary>
        /// 返回 URL 字符串的编码结果
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>解码结果</returns>
        public static string UrlDecode(string str)
        {
            return HttpUtility.UrlDecode(str);
        }
        #endregion
        
        #region Cookie操作
        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        public static void WriteCookie(string strName, string strValue)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie.Value = strValue;
            HttpContext.Current.Response.AppendCookie(cookie);

        }
        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        /// <param name="expires">过期时间(分钟)</param>
        public static void WriteCookie(string strName, string strValue, int expires)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie.Value = strValue;
            cookie.Expires = DateTime.Now.AddMinutes(expires);
            HttpContext.Current.Response.AppendCookie(cookie);

        }

        /// <summary>
        /// 读cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <returns>cookie值</returns>
        public static string GetCookie(string strName)
        {
            if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies[strName] != null)
            {
                return HttpContext.Current.Request.Cookies[strName].Value.ToString();
            }

            return "";
        }

        #endregion

        #region 判断类型
        /// <summary>
        /// 判断字符串是否是yy-mm-dd字符串
        /// </summary>
        /// <param name="str">待判断字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsDateString(string str)
        {
            return Regex.IsMatch(str, @"(\d{4})-(\d{1,2})-(\d{1,2})");
        }

        /// <summary>
        /// 验证是否为正整数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsInt(string str)
        {
            return Regex.IsMatch(str, @"^[0-9]*$");
        }

        /// <summary>
        /// 判断对象是否为Int32类型的数字
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public static bool IsNumeric(object Expression)
        {
            return TypeParse.IsNumeric(Expression);
        }

        /// <summary>
        /// 判断对象是否为Double类型的数字
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public static bool IsDouble(object Expression)
        {
            return TypeParse.IsDouble(Expression);
        }

        /// <summary>
        /// 判断给定的字符串数组(strNumber)中的数据是不是都为数值型
        /// </summary>
        /// <param name="strNumber">要确认的字符串数组</param>
        /// <returns>是则返加true 不是则返回 false</returns>
        public static bool IsNumericArray(string[] strNumber)
        {
            return TypeParse.IsNumericArray(strNumber);
        }

        /// <summary>
        /// 是否为ip
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIP(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");

        }
        /// <summary>
        /// 检测是否符合email格式
        /// </summary>
        /// <param name="strEmail">要判断的email字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsEmail(string strEmail)
        {
            return Regex.IsMatch(strEmail, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }   
        /// <summary>
        /// 检测是否是正确的Url
        /// </summary>
        /// <param name="strUrl">要验证的Url</param>
        /// <returns>判断结果</returns>
        public static bool IsURL(string strUrl)
        {
            return Regex.IsMatch(strUrl, @"^(http|https)\://([a-zA-Z0-9\.\-]+(\:[a-zA-Z0-9\.&%\$\-]+)*@)*((25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])|localhost|([a-zA-Z0-9\-]+\.)*[a-zA-Z0-9\-]+\.(com|edu|gov|int|mil|net|org|biz|arpa|info|name|pro|aero|coop|museum|[a-zA-Z]{1,10}))(\:[0-9]+)*(/($|[a-zA-Z0-9\.\,\?\'\\\+&%\$#\=~_\-]+))*$");
        }
        /// <summary>
        /// 判断是否为base64字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsBase64String(string str)
        {
            //A-Z, a-z, 0-9, +, /, =
            return Regex.IsMatch(str, @"[A-Za-z0-9\+\/\=]");
        }

        /// <summary>
        /// 检测是否有Sql危险字符
        /// </summary>
        /// <param name="str">要判断字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsSafeSqlString(string str)
        {

            return !Regex.IsMatch(str, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']");
        }
       
        #endregion

        #region 字符串过滤
        /// <summary>
        /// 替换html字符
        /// </summary>
        public static string EncodeHtml(string strHtml)
        {
            if (strHtml != "")
            {
                strHtml = strHtml.Replace(",", "&def");
                strHtml = strHtml.Replace("'", "&dot");
                strHtml = strHtml.Replace(";", "&dec");
                return strHtml;
            }
            return "";
        }

        /// <summary>
        /// 从HTML中获取文本,保留br,p,img
        /// </summary>
        /// <param name="HTML"></param>
        /// <returns></returns>
        public static string GetTextFromHTML(string HTML)
        {
            Regex regEx = new Regex(@"</?(?!br|/?p|img)[^>]*>", RegexOptions.IgnoreCase);

            return regEx.Replace(HTML, "");
        }

        ///<summary>
        /// 传入指定的HTML的标签
        /// </summary>
        /// <param name="InputHTML">要过滤的HTML标签，以","分割如</param>
        /// <param name="Content">需要过滤的内容</param>
        public static string RemoveInputHtml(string Content,string InputHTML)
        {
            Content = Content.ToLower();
            Regex regex;
            string[] InputHtmls = InputHTML.Split(',');
            for(int i=0;i<InputHtmls.Length;i++)
            {
                if(!string.IsNullOrEmpty(InputHtmls[i]))
                {
                    regex = new Regex(@InputHtmls[i], RegexOptions.IgnoreCase);
                    Content = regex.Replace(Content, "");
                }
            }
            Content = Content.Replace(" ", "");
            Content = Content.Replace("&nbsp;", "");
            Content = Content.Replace("　", "");
            return Content;
        }

        /// <summary>
        /// 移除Html中的标记
        /// </summary>
        /// <param name="html">html字符串</param>
        /// <returns>返回移除后的文本</returns>
        public static string RemoveHtml(string html)
        {
            html = html.ToLower();
            Regex regex1 = new Regex(@"<script[\s\S]+</script *>", RegexOptions.IgnoreCase);
            Regex regex2 = new Regex(@" href *= *[\s\S]*script *:", RegexOptions.IgnoreCase);
            Regex regex3 = new Regex(@" no[\s\S]*=", RegexOptions.IgnoreCase);
            Regex regex4 = new Regex(@"<iframe[\s\S]+</iframe *>", RegexOptions.IgnoreCase);
            Regex regex5 = new Regex(@"<frameset[\s\S]+</frameset *>", RegexOptions.IgnoreCase);
            Regex regex6 = new Regex(@"\<img[^\>]+\>", RegexOptions.IgnoreCase);
            Regex regex7 = new Regex(@"</p>", RegexOptions.IgnoreCase);
            Regex regex8 = new Regex(@"<p>", RegexOptions.IgnoreCase);
            Regex regex9 = new Regex(@"<[^>]*>", RegexOptions.IgnoreCase);

            html = regex1.Replace(html, ""); //过滤<script></script>标记 
            html = regex2.Replace(html, ""); //过滤href=javascript: (<A>) 属性 
            html = regex3.Replace(html, " _disibledevent="); //过滤其它控件的on...事件 
            html = regex4.Replace(html, ""); //过滤iframe 
            html = regex5.Replace(html, ""); //过滤frameset 
            html = regex6.Replace(html, ""); //过滤frameset 
            html = regex7.Replace(html, ""); //过滤frameset 
            html = regex8.Replace(html, ""); //过滤frameset 
            html = regex9.Replace(html, "");
            html = html.Replace(" ", "");
            html = html.Replace("</strong>", "");
            html = html.Replace("<strong>", "");
            html = html.Replace("&nbsp;", "");
            html = html.Replace("　", "");

            return html;
        }

        ///// <summary>
        ///// 移除标题中的Html标记
        ///// 2008-12-05 冯威 add
        ///// </summary>
        ///// <param name="html">html字符串</param>
        ///// <returns>返回移除后的文本</returns>
        //public static string RemoveTitleHtml(string html)
        //{
        //    string stroutput = html;
        //    Regex regex = new Regex(@"<[^>]+>|</[^>]+>");

        //    stroutput = regex.Replace(stroutput, "");
        //    return stroutput;
        //}

        /// <summary>
        /// 移除标题中的Html标记 
        /// 2008-12-05 冯威 add
        /// </summary>
        /// <param name="htmlCode">html字符串</param>
        /// <returns>返回移除后的文本</returns>
        public static string RemoveTitleHtml(string htmlCode)
        {
            string regularStr = "<.+?>|[ ]+";
            //			string regularStr	=	"<[^>]*>";
            string temp = htmlCode.Replace("\"", "'");
            string coninfo = System.Text.RegularExpressions.Regex.Replace(temp, regularStr, "").Replace("\r", "").
                                    Replace("\n", "").Replace("&nbsp;", "").Replace("   ", "").Trim();
            return coninfo;
        }


        /// <summary>
        /// 过滤HTML中的不安全标签
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string RemoveUnsafeHtml(string content)
        {
            content = Regex.Replace(content, @"(\<|\s+)o([a-z]+\s?=)", "$1$2", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, @"(script|frame|form|meta|behavior|style)([\s|:|>])+", "$1.$2", RegexOptions.IgnoreCase);
            return content;
        }
        /// <summary>
        /// 改正sql语句中的转义字符
        /// </summary>
        public static string MashSQL(string str)
        {
            string str2;

            if (str == null)
            {
                str2 = "";
            }
            else
            {
                str = str.Replace("\'", "'");
                str2 = str;
            }
            return str2;
        }

        /// <summary>
        /// 替换sql语句中的有问题符号
        /// </summary>
        public static string SQLSafe(string str)
        {
            string str2;

            if (str == null)
            {
                str2 = "";
            }
            else
            {
                str = str.Replace("'", "''");
                str2 = str;
            }
            return str2;
        }
        #endregion

        #region 类型转换
        /// <summary>
        /// string型转换为bool型
        /// </summary>
        /// <param name="Expression">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的bool类型结果</returns>
        public static bool StrToBool(object Expression, bool defValue)
        {
            return TypeParse.StrToBool(Expression, defValue);
        }

        /// <summary>
        /// 将对象转换为Int32类型
        /// </summary>
        /// <param name="Expression">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int StrToInt(object Expression, int defValue)
        {
            return TypeParse.StrToInt(Expression, defValue);
        }

        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static float StrToFloat(object strValue, float defValue)
        {
            return TypeParse.StrToFloat(strValue, defValue);
        }

        /// <summary>
        /// 自动将DBNull数据类型转换为空字符串,如果为非DBNull则返回去掉前后空格的字符串
        /// </summary>
        /// <param name="dtRowOjbect">传入一个object数据类型</param>
        /// <returns>返回转换后去掉前后空格的字字符串类型</returns>
        public static string DBNullToString(object dtRowOjbect)
        {
            if (dtRowOjbect!=null && dtRowOjbect != DBNull.Value)
            {
                return dtRowOjbect.ToString().Trim();
            }
            else
            {
                return "";
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public class CreatePage
        {
            /// <summary>
            /// 生成静态页面
            /// </summary>
            /// <param name="pageUrl">页地址</param>
            /// <param name="savePageName">保存地址（包括文件名）</param>
            public static void create(string pageUrl, string savePageName)
            {
                try
                {
                    WebRequest request = WebRequest.Create(pageUrl);
                    request.Timeout = 500000;
                    WebResponse response = request.GetResponse();
                    Stream resStream = response.GetResponseStream();

                    StreamReader sr = new StreamReader(resStream, Encoding.UTF8);

                     StreamWriter sw = null;
                    if (File.Exists(savePageName)) 
                    {
                        sw = new StreamWriter(savePageName, false, Encoding.GetEncoding("GB2312"));
                    }
                    else
                    {
                        sw = new StreamWriter(savePageName, true, Encoding.GetEncoding("GB2312"));
                    }

                    Encoding enc = Encoding.GetEncoding("GB2312");

                    byte[] bPageCode;
                    bPageCode = Encoding.Convert(Encoding.Default, enc, Encoding.Default.GetBytes(sr.ReadToEnd()));

                    sw.Write(enc.GetString(bPageCode));
                    sw.Flush();
                    sw.Close();

                    sr.Close();
                    
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }


     

        /// <summary>
        /// 针对特殊字符串进行处理
        /// </summary>
        public static string sqlencode20(string str)
        {
            str = str.Replace("'", "''");
            //只在LIKE查询时才用到这些
            //			str=str.Replace("[","[[]"); //此句一定要在最先
            //			
            //			str=str.Replace("_","[_]");
            //			str=str.Replace("%","[%]");
            //			str=str.Replace("@#%nbsp","+");
            return str;
        }

        /// <summary>
        /// 生成参数搜索辅助表时，将备选项中的特殊符号替换成可以做为列名的符号
        /// 2009-07-07 Add By Eric.Feng
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ReplaceSpecialSymbol(string str)
        {
            return str.Replace('-', '_');
        }

        ///// <summary>
        ///// 绑定下拉框
        ///// </summary>
        ///// <param name="listControl">要绑定的下拉框控件</param>
        ///// <param name="dt">数据源</param>
        ///// <param name="textField">text</param>
        ///// <param name="valueField">value</param>
        //public static void BindListControl(ListControl listControl, DataTable dt, String textField, String valueField)
        //{
        //    BindListControl(listControl, dt, textField, valueField, "", "");
        //}

        /// <summary>
        /// 绑定下拉框
        /// </summary>
        /// <param name="listControl">要绑定的下拉框控件</param>
        /// <param name="dt">数据源</param>
        /// <param name="textField">text</param>
        /// <param name="valueField">value</param>
        public static void BindListControl(ListControl listControl, IList list, String textField, String valueField)
        {
            BindListControl(listControl, list, textField, valueField, "", "");
        }

        ///// <summary>
        ///// 绑定下拉框 单选框列表 复选框列表
        ///// </summary>
        ///// <param name="listControl">要绑定的下拉框控件</param>
        ///// <param name="dt">数据源</param>
        ///// <param name="textField">text</param>
        ///// <param name="valueField">value</param>
        ///// <param name="defaultText">默认值text</param>
        ///// <param name="defaultValue">默认值value</param>
        //public static void BindListControl(ListControl listControl, DataTable dt, String textField, String valueField,
        //    String defaultText,String defaultValue)
        //{
        //    if (dt.HasData())
        //    {
        //        listControl.DataSource = dt;
        //        listControl.DataTextField = textField;
        //        listControl.DataValueField = valueField;
        //    }
        //    else
        //    {
        //        listControl.DataSource = null;
        //    }
        //    listControl.DataBind();
        //    if (defaultText.Trim().Length > 0 && defaultValue.Trim().Length > 0)
        //    {
        //        listControl.Items.Insert(0, new ListItem(defaultText, defaultValue));
        //    }
        //}

        /// <summary>
        /// 绑定下拉框 单选框列表 复选框列表
        /// </summary>
        /// <param name="listControl">要绑定的下拉框控件</param>
        /// <param name="dt">数据源</param>
        /// <param name="textField">text</param>
        /// <param name="valueField">value</param>
        /// <param name="defaultText">默认值text</param>
        /// <param name="defaultValue">默认值value</param>
        public static void BindListControl(ListControl listControl, IList list, String textField, String valueField,
            String defaultText, String defaultValue)
        {
            if (list.Count>0)
            {
                listControl.DataSource = list;
                listControl.DataTextField = textField;
                listControl.DataValueField = valueField;
            }
            else
            {
                listControl.DataSource = null;
            }
            listControl.DataBind();
            if (defaultText.Trim().Length > 0 && defaultValue.Trim().Length > 0)
            {
                listControl.Items.Insert(0, new ListItem(defaultText, defaultValue));
            }
        }


        /// <summary>
        /// 绑定数据源控件
        /// </summary>
        /// <param name="ds">数据源</param>
        /// <param name="bindControl">数据源控件</param>
        public static void BindToDataBoundControl(DataSet ds, Repeater bindControl)
        {
            BindToDataBoundControl(ds, bindControl, null, null, null, null);
        }


        /// <summary>
        /// 绑定数据源控件
        /// </summary>
        /// <param name="ds">数据源</param>
        /// <param name="bindControl">数据源控件</param>
        /// <param name="pager">分页控件</param>
        /// <param name="lblTotal">分页显示所以记录条数Label</param>
        /// <param name="lblPageCurrent">每页条数Label</param>
        /// <param name="lblPageCount">页数Label</param>
        public static void BindToDataBoundControl(DataSet ds, Repeater bindControl, AspNetPager pager, Label lblTotal, Label lblPageCurrent, Label lblPageCount)
        {
            if (ds.HasData())
            {
                int count = ds.Tables[1].Rows[0][0].ToString().ToInt32(0);
                int rowCount = ds.Tables[1].Rows[0][1].ToString().ToInt32(0);
                bindControl.DataSource = ds.Tables[0].DefaultView;
                if (pager != null && lblTotal != null && lblPageCurrent != null && lblPageCount != null)
                {
                    pager.RecordCount = count;

                    lblTotal.Text = " "+count.ToString()+" ";
                    int rowStart = (pager.CurrentPageIndex - 1)*pager.PageSize + 1;
                    int rowEnd = rowStart + rowCount - 1;
                    lblPageCurrent.Text =" "+ rowStart + "-" + rowEnd+" ";
                    lblPageCount.Text =" "+  pager.PageCount.ToString()+" ";
                }
              
            }
            else
            {
                bindControl.DataSource = null;
                if (pager != null && lblTotal != null && lblPageCurrent != null && lblPageCount != null)
                {
                    pager.RecordCount = 0;
                    lblTotal.Text = " 0 ";
                    lblPageCurrent.Text = " 0-0 ";
                    lblPageCount.Text = " 0 ";
                }
            }
            bindControl.DataBind();

        }

        /// <summary>
        /// 绑定数据源控件
        /// </summary>
        /// <param name="ds">数据源</param>
        /// <param name="bindControl">数据源控件</param>
        /// <param name="pager">分页控件</param>
        /// <param name="lblTotal">分页显示所以记录条数Label</param>
        /// <param name="lblPageCurrent">每页条数Label</param>
        /// <param name="lblPageCount">页数Label</param>
        /// <param name="lbExcelExport">页面上如果有excel导出功能时，显示出来的条数</param>
        public static void BindToDataBoundControl(DataSet ds, Repeater bindControl, AspNetPager pager, Label lblTotal, Label lblPageCurrent, Label lblPageCount,Label lbExcelExport)
        {
            if (ds.HasData())
            {
                int count = ds.Tables[1].Rows[0][0].ToString().ToInt32(0);
                int rowCount = ds.Tables[1].Rows[0][1].ToString().ToInt32(0);
                bindControl.DataSource = ds.Tables[0].DefaultView;
                if (pager != null && lblTotal != null && lblPageCurrent != null && lblPageCount != null)
                {
                    pager.RecordCount = count;

                    lblTotal.Text = " " + count.ToString() + " ";
                    lbExcelExport.Text = " " + count.ToString() + " ";
                    int rowStart = (pager.CurrentPageIndex - 1) * pager.PageSize + 1;
                    int rowEnd = rowStart + rowCount - 1;
                    lblPageCurrent.Text = " " + rowStart + "-" + rowEnd + " ";
                    lblPageCount.Text = " " + pager.PageCount.ToString() + " ";
                }

            }
            else
            {
                bindControl.DataSource = null;
                if (pager != null && lblTotal != null && lblPageCurrent != null && lblPageCount != null)
                {
                    pager.RecordCount = 0;
                    lblTotal.Text = " 0 ";
                    lblPageCurrent.Text = " 0-0 ";
                    lblPageCount.Text = " 0 ";
                }
            }
            bindControl.DataBind();

        }


        #region 根据字典里需要排序的字段列出要排序的LinkButton
        /// <summary>
        /// 根据字典里需要排序的字段列出要排序的LinkButton
        /// </summary>
        /// <param name="dic">排序字段的字典</param>
        /// <param name="btnList_OrderBy">BulletedList控件</param>
        public static void BindBulletedList(BulletedList btnList_OrderBy, Dictionary<string, string> dic)
        {
            if (dic != null && dic.Count > 0)
            {
                DataTable dt = new DataTable();
                DataColumn dc = new DataColumn("Key", typeof(string));
                dt.Columns.Add(dc);
                dc = new DataColumn("Value", typeof(string));
                dt.Columns.Add(dc);

                foreach (KeyValuePair<string, string> k in dic)
                {
                    if (!string.IsNullOrEmpty(k.Key) && !string.IsNullOrEmpty(k.Value))
                    {
                        DataRow dr = dt.NewRow();
                        dr["Key"] = "按" + k.Value + "排序(降)";
                        dr["Value"] = k.Key + "$DESC";
                        dt.Rows.Add(dr);

                        dr = dt.NewRow();
                        dr["Key"] = "按" + k.Value + "排序(升)";
                        dr["Value"] = k.Key + "$ASC";
                        dt.Rows.Add(dr);
                    }
                }
                btnList_OrderBy.DisplayMode = BulletedListDisplayMode.LinkButton;
                btnList_OrderBy.DataSource = dt;
                btnList_OrderBy.DataTextField = "Key";
                btnList_OrderBy.DataValueField = "Value";
            }
            else
            {
                btnList_OrderBy.DataSource = null;
            }
            btnList_OrderBy.DataBind();
        }
        #endregion

        /// <summary>
        /// 获取客户编号
        /// </summary>
        /// <returns></returns>
        public static string GetClientNumber()
        {
            return DateTime.Now.Year.ToString() +
                DateTime.Now.Month.ToString() +
                DateTime.Now.Day.ToString() +
                DateTime.Now.Hour.ToString() +
                DateTime.Now.Minute.ToString() +
                DateTime.Now.Second.ToString() +
                DateTime.Now.Millisecond.ToString();
        }

        /// <summary>
        /// Excel导出
        /// </summary>
        /// <param name="dt">要导出的表</param>
        /// <param name="excelName">导出的excel名称</param>
        /// <param name="page">来自哪个页面</param>
        /// <param name="dic">列名DIC</param>
        public static void DataTableToExcel(DataTable dt, string excelName, Page page, Dictionary<string, string> dic)
        {
            if (dt.HasData())
            {
                GC.Collect();
                Application excel;
                int rowIndex = 1;
                int colIndex = 1;
                _Workbook xBk;
                _Worksheet xSt;

                excel = new ApplicationClass();

                xBk = excel.Workbooks.Add(true);

                xSt = (_Worksheet) xBk.ActiveSheet;
                xSt.Name = excelName;
                //设置标题列名
                foreach (DataColumn col in dt.Columns)
                {
                    if (dic.ContainsKey(col.ColumnName))
                    {
                        //设置标题格式并赋值
                        excel.Cells[1, colIndex] = dic[col.ColumnName];
                        xSt.get_Range(excel.Cells[1, colIndex], excel.Cells[1, colIndex]).HorizontalAlignment = XlVAlign.xlVAlignCenter; 
                        xSt.get_Range(excel.Cells[1, colIndex], excel.Cells[1, colIndex]).Font.Size = 12;
                        xSt.get_Range(excel.Cells[1, colIndex], excel.Cells[1, colIndex]).Font.Name = "黑体";
                        xSt.get_Range(excel.Cells[1, colIndex], excel.Cells[1, colIndex]).Interior.ColorIndex = 24;
                        xSt.get_Range(excel.Cells[1, colIndex], excel.Cells[1, colIndex]).RowHeight = 35;
                        xSt.get_Range(excel.Cells[1, colIndex], excel.Cells[1, colIndex]).Borders.LineStyle = 1; 
                        colIndex++;
                    }
                }

                foreach (DataRow row in dt.Rows)
                {
                    rowIndex++;
                    colIndex = 1;
                    foreach (DataColumn col in dt.Columns)
                    {
                        if (dic.ContainsKey(col.ColumnName))
                        {
                            if (col.DataType == System.Type.GetType("System.DateTime"))
                            {
                                excel.Cells[rowIndex, colIndex] = string.IsNullOrEmpty(row[col.ColumnName].ToString()) ? "": (Convert.ToDateTime(row[col.ColumnName].ToString())).ToString("yyyy-MM-dd");
                                xSt.get_Range(excel.Cells[rowIndex, colIndex], excel.Cells[rowIndex, colIndex]).HorizontalAlignment = XlVAlign.xlVAlignCenter; //设置日期型的字段格式为居中对齐 
                            }
                            else if (col.DataType == System.Type.GetType("System.String"))
                            {
                                excel.Cells[rowIndex, colIndex] = "'" + row[col.ColumnName].ToString();
                                xSt.get_Range(excel.Cells[rowIndex, colIndex], excel.Cells[rowIndex, colIndex]).HorizontalAlignment = XlVAlign.xlVAlignCenter; //设置字符型的字段格式为居中对齐 
                            }
                            else
                            {
                                excel.Cells[rowIndex, colIndex] = row[col.ColumnName].ToString();
                            }
                            colIndex++;
                        }
                    }
                }

                excel.get_Range(excel.Cells[2, 1], excel.Cells[dt.Rows.Count + 1, dic.Count]).Select();
                excel.ActiveWindow.FreezePanes = true;

                DirectoryInfo dir = new DirectoryInfo(page.Server.MapPath(_filePath + "DownExcel/"));
                if (!dir.Exists)
                {
                    dir.Create();
                }
                xBk.SaveCopyAs(page.Server.MapPath(_filePath + "DownExcel/") + excelName + ".xls");

                dt = null;
                xBk.Close(false, null, null);
                excel.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xBk);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xSt);
                xBk = null;
                excel = null;
                xSt = null;
                GC.Collect();

                string path = page.Server.MapPath(_filePath + "DownExcel/" + excelName + ".xls");

                System.IO.FileInfo file = new System.IO.FileInfo(path);
                HttpResponse contextResponse = HttpContext.Current.Response;
                contextResponse.Clear();
                contextResponse.Charset = "GB2312";
                contextResponse.ContentEncoding = System.Text.Encoding.UTF8;
                // 添加头信息，为"文件下载/另存为"对话框指定默认文件名 
                contextResponse.AddHeader("Content-Disposition",
                                          "attachment; filename=" + page.Server.UrlEncode(file.Name));
                // 添加头信息，指定文件大小，让浏览器能够显示下载进度 
                contextResponse.AddHeader("Content-Length", file.Length.ToString());

                // 指定返回的是一个不能被客户端读取的流，必须被下载 
                contextResponse.ContentType = "application/ms-excel";

                // 把文件流发送到客户端 
                contextResponse.WriteFile(file.FullName);
                // 停止页面的执行 

                contextResponse.End();
                GC.Collect();
                UploadFile.DeleteFile(page.Server.MapPath(_filePath + "DownExcel/") + excelName + ".xls");
            }
        }

        /// <summary>
        /// Excel导出
        /// </summary>
        /// <param name="ds">要导出的数据集</param>
        /// <param name="excelName">导出的excel名称</param>
        /// <param name="page">来自哪个页面</param>
        /// <param name="dic">列名DIC</param>
        public static void DataTableToExcel(DataSet ds, string excelName, Page page, Dictionary<string, string> dic)
        {
            if (ds.Tables.Count > 0)
            {
                GC.Collect();
                Application excel;
                excel = new ApplicationClass();
                List<string> list_TableName = new List<string>();
                Workbook xBk = excel.Workbooks.Add(true);
                int SheetCount = 0;

                #region 循环生成表格
                foreach (DataTable dt in ds.Tables)
                {
                    if (dt.HasData())
                    {
                        int rowIndex = 1;
                        int colIndex = 1;
                        _Worksheet xSt;
                        SheetCount++;
                        xSt = (Worksheet)xBk.Sheets.get_Item(SheetCount);
                        xBk.Sheets.Add(Type.Missing, xSt, Type.Missing, Type.Missing);
                        xSt.Name = dt.TableName;
                        list_TableName.Add(dt.TableName);
                        //设置标题列名
                        foreach (DataColumn col in dt.Columns)
                        {
                            if (dic.ContainsKey(col.ColumnName))
                            {
                                //设置标题格式并赋值
                                xSt.Cells[1, colIndex] = dic[col.ColumnName];
                                xSt.get_Range(xSt.Cells[1, colIndex], xSt.Cells[1, colIndex]).HorizontalAlignment =
                                    XlVAlign.xlVAlignCenter;
                                xSt.get_Range(xSt.Cells[1, colIndex], xSt.Cells[1, colIndex]).Font.Size = 12;
                                xSt.get_Range(xSt.Cells[1, colIndex], xSt.Cells[1, colIndex]).Font.Name = "黑体";
                                xSt.get_Range(xSt.Cells[1, colIndex], xSt.Cells[1, colIndex]).Interior.ColorIndex =
                                    24;
                                xSt.get_Range(xSt.Cells[1, colIndex], xSt.Cells[1, colIndex]).RowHeight = 35;
                                xSt.get_Range(xSt.Cells[1, colIndex], xSt.Cells[1, colIndex]).Borders.LineStyle = 1;
                                colIndex++;
                            }
                        }

                        foreach (DataRow row in dt.Rows)
                        {
                            rowIndex++;
                            colIndex = 1;
                            foreach (DataColumn col in dt.Columns)
                            {
                                if (dic.ContainsKey(col.ColumnName))
                                {
                                    if (col.DataType == System.Type.GetType("System.DateTime"))
                                    {
                                        xSt.Cells[rowIndex, colIndex] =
                                            string.IsNullOrEmpty(row[col.ColumnName].ToString())
                                                ? ""
                                                : (Convert.ToDateTime(row[col.ColumnName].ToString())).ToString(
                                                      "yyyy-MM-dd");
                                        xSt.get_Range(xSt.Cells[rowIndex, colIndex], xSt.Cells[rowIndex, colIndex]).
                                            HorizontalAlignment = XlVAlign.xlVAlignCenter; //设置日期型的字段格式为居中对齐 
                                    }
                                    else if (col.DataType == System.Type.GetType("System.String"))
                                    {
                                        xSt.Cells[rowIndex, colIndex] = "'" + row[col.ColumnName].ToString();
                                        xSt.get_Range(xSt.Cells[rowIndex, colIndex], xSt.Cells[rowIndex, colIndex]).
                                            HorizontalAlignment = XlVAlign.xlVAlignCenter; //设置字符型的字段格式为居中对齐 
                                    }
                                    else
                                    {
                                        xSt.Cells[rowIndex, colIndex] = row[col.ColumnName].ToString();
                                    }
                                    colIndex++;
                                }
                            }
                        }
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(xSt);
                        xSt = null;
                    }
                }

                #endregion

                try
                {
                    
                    for (int i = 1; i <= xBk.Sheets.Count; i++)
                    {
                        Worksheet sheet = null;
                        sheet = (Worksheet) xBk.Sheets.get_Item(i);
                        if (!list_TableName.Contains(sheet.Name))
                        {
                            sheet.Delete();
                        }
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(sheet);
                    }
                }
                catch (Exception ex)
                {
                    GC.Collect();
                    throw ex;
                }

                DirectoryInfo dir = new DirectoryInfo(page.Server.MapPath(_filePath + "DownExcel/"));
                if (!dir.Exists)
                {
                    dir.Create();
                }
                xBk.SaveCopyAs(page.Server.MapPath(_filePath + "DownExcel/") + excelName + ".xls");

                ds = null;
                xBk.Close(false, null, null);
                excel.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xBk);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
                xBk = null;
                excel = null;
                
                GC.Collect();

                string path = page.Server.MapPath(_filePath + "DownExcel/" + excelName + ".xls");

                System.IO.FileInfo file = new System.IO.FileInfo(path);
                HttpResponse contextResponse = HttpContext.Current.Response;
                contextResponse.Clear();
                contextResponse.Charset = "GB2312";
                contextResponse.ContentEncoding = System.Text.Encoding.UTF8;
                // 添加头信息，为"文件下载/另存为"对话框指定默认文件名 
                contextResponse.AddHeader("Content-Disposition",
                                          "attachment; filename=" + page.Server.UrlEncode(file.Name));
                // 添加头信息，指定文件大小，让浏览器能够显示下载进度 
                contextResponse.AddHeader("Content-Length", file.Length.ToString());

                // 指定返回的是一个不能被客户端读取的流，必须被下载 
                contextResponse.ContentType = "application/ms-excel";

                // 把文件流发送到客户端 
                contextResponse.WriteFile(file.FullName);
                // 停止页面的执行 

                contextResponse.End();
                GC.Collect();
                UploadFile.DeleteFile(page.Server.MapPath(_filePath + "DownExcel/") + excelName + ".xls");
            }

        }

        
    }
}

