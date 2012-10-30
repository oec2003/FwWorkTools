using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace FW.CommonFunction
{
    /// <summary>
    /// 
    /// </summary>
    public class StringFormat
    {
         /// <summary>
		/// 得到指定字符串中含有多少个 c
		/// </summary>
		/// <param name="str"></param>
		/// <param name="c"></param>
		/// <returns></returns>
		public static int HasNumberChar(string str,string c)
		{
			if(str.IndexOf(c)!=-1)
			{
				return 1 + HasNumberChar(str.Substring(str.IndexOf(c)+c.Length),c);
			}
			else
			{
				return 0;
			}
		}

		/// <summary>
		/// </summary>
		/// <param name="html"></param>
		/// <returns></returns>
		public static string ClearHmtl(string html) 
		{ 
			html = html.ToLower();
			System.Text.RegularExpressions.Regex regex1 = new System.Text.RegularExpressions.Regex(@"<script[\s\S]+</script *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase); 
			System.Text.RegularExpressions.Regex regex2 = new System.Text.RegularExpressions.Regex(@" href *= *[\s\S]*script *:", System.Text.RegularExpressions.RegexOptions.IgnoreCase); 
			System.Text.RegularExpressions.Regex regex3 = new System.Text.RegularExpressions.Regex(@" no[\s\S]*=", System.Text.RegularExpressions.RegexOptions.IgnoreCase); 
			System.Text.RegularExpressions.Regex regex4 = new System.Text.RegularExpressions.Regex(@"<iframe[\s\S]+</iframe *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase); 
			System.Text.RegularExpressions.Regex regex5 = new System.Text.RegularExpressions.Regex(@"<frameset[\s\S]+</frameset *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase); 
			System.Text.RegularExpressions.Regex regex6 = new System.Text.RegularExpressions.Regex(@"\<img[^\>]+\>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);  
			System.Text.RegularExpressions.Regex regex7 = new System.Text.RegularExpressions.Regex(@"</p>", System.Text.RegularExpressions.RegexOptions.IgnoreCase); 
			System.Text.RegularExpressions.Regex regex8 = new System.Text.RegularExpressions.Regex(@"<p>", System.Text.RegularExpressions.RegexOptions.IgnoreCase); 
			System.Text.RegularExpressions.Regex regex9 = new System.Text.RegularExpressions.Regex(@"<[^>]*>", System.Text.RegularExpressions.RegexOptions.IgnoreCase); 
			
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
			html = html.Replace("&nbsp;","");
			html = html.Replace("　","");

			return html; 
		} 
		/// <summary>
		/// 得到中英文混合字符长度
		/// </summary>
		/// <param name="strInput"></param>
		/// <returns></returns>
		public static int GetRealLength(string strInput)
		{
			int byteLen = System.Text.Encoding.GetEncoding("gb2312").GetByteCount(strInput);
			return byteLen;
		}
		/// <summary>
		/// 按长度截取字符串
		/// </summary>
		/// <param name="strInput">输入字符串</param>
		/// <param name="intLen">截取长度</param>
		/// <returns></returns>
		public static string StringCut(string strInput ,int intLen )
		{
			strInput = strInput.Trim() ;
			int byteLen = Encoding.GetEncoding("gb2312").GetByteCount( strInput ) ;
			if ( byteLen > intLen )
			{
				string resultStr = "" ;
				for ( int i = 0 ; i < strInput.Length ; i++ )
				{
					if ( Encoding.GetEncoding("gb2312").GetByteCount( resultStr ) < intLen - 2) 
					{
						resultStr += strInput.Substring( i , 1 ) ;
					}
					else
					{
						break ;
					}
				}
				return resultStr + "..." ;
			}
			else
			{
				return strInput ;
			}
		}


		/// <summary>
		/// 按长度截取字符串
		/// </summary>
		/// <param name="strInput"></param>
		/// <param name="intLen"></param>
		/// <param name="strReplace">结尾替换字符串</param>
		/// <param name="nReplaceLen">结尾替换字符串长度（没有直接通过length获取，为了更灵活）</param>
		/// <returns></returns>
		public static string StringCut(string strInput ,int intLen,string strReplace,int nReplaceLen)
		{
			strInput = strInput.Trim() ;
			int byteLen = Encoding.GetEncoding("gb2312").GetByteCount( strInput ) ;
			if ( byteLen > intLen )
			{
				string resultStr = "" ;
				for ( int i = 0 ; i < strInput.Length ; i++ )
				{
					if ( Encoding.GetEncoding("gb2312").GetByteCount( resultStr ) < intLen +1- nReplaceLen) 
					{
						resultStr += strInput.Substring( i , 1 ) ;
					}
					else
					{
						break ;
					}
				}
				return resultStr + strReplace ;
			}
			else
			{
				return strInput ;
			}
		}


		/// <summary>
		/// 按长度截取字符串
		/// </summary>
		/// <param name="ob">输入字符串</param>
		/// <param name="intLen">截取长度</param>
		/// <returns></returns>
		public static string StringCut(object ob ,int intLen)
		{
			return StringCut(ob.ToString().Trim(),intLen);
		}

		/// <summary>
		/// 按长度截取字符串
		/// </summary>
		/// <param name="strInput">输入字符串</param>
		/// <param name="strExclude">排除(长度)字符串</param>
		/// <param name="intLen">截取长度</param>
		/// <returns></returns>
		public static string StringCut(string strInput ,string strExclude,int intLen)
		{
			int intLenExclude = Encoding.Default.GetByteCount( strExclude ) ;
			if (intLen-intLenExclude>0)
			{
				return StringCut(strInput,intLen-intLenExclude);
			}
			else
			{
				return "";
			}			
		}

		/// <summary>
		/// 按长度截取字符串
		/// </summary>
		/// <param name="ob">输入字符串</param>
		/// <param name="obExclude">排除(长度)字符串</param>
		/// <param name="intLen">截取长度</param>
		/// <returns></returns>
		public static string StringCut(object ob ,object obExclude,int intLen)
		{
			return StringCut(ob.ToString().Trim(),obExclude.ToString().Trim(),intLen);
		}
		/// <summary>
		/// 按样式获取日期的字符串
		/// </summary>
		/// <param name="dt">输入日期</param>
		/// <param name="format">样式</param>
		/// <returns></returns>
		public static string  StringDateTime(DateTime dt,string format)
		{
			return dt.ToString(format);
		}

		/// <summary>
		/// 按样式获取日期的字符串
		/// </summary>
		/// <param name="ob">输入日期</param>
		/// <param name="format">样式</param>
		/// <returns></returns>
		public static string StringDateTime(object ob,string format)
		{
			try
			{
				DateTime dt = Convert.ToDateTime(ob);
				return StringDateTime(dt,format);
			}
			catch(Exception)
			{
				return "";	
			}
		}

        /// <summary>
        /// cmwin系统用户密码加密方法
        /// </summary>
        /// <param name="input">原始字符串</param>
        /// <returns>加密后的字符串</returns>
        public static String ConvertStringByMD5(String input)
        {
            byte[] by = Encoding.Default.GetBytes(input.ToCharArray());
            MD5 md5 = new MD5CryptoServiceProvider();
            input = Convert.ToBase64String(md5.ComputeHash(by));
            return input;
        }
    }
}
