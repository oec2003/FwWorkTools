using System;
using System.Collections.Generic;
using System.Web;
using System.Text;

namespace FW.CommonFunction
{
    public class WebUrl
    {
        /// <summary>
        /// 获取当前页面的URL完整路径：http://abc:8080/aaa/bbb/cccc.htm?p1=ddd  
        /// 返回 ： http://abc:8080/aaa/bbb/cccc.htm?p1=ddd
        /// </summary>
        /// <returns></returns>
        public static string GetCurrPageFullUrl()
        {
            try
            {
                string url = HttpContext.Current.Request.Url.ToString();
                return url;
            }
            catch
            {
                return "";
            }
        }
        /// <summary>
        /// 获取当前页面的URL短路径：http://abc:8080/aaa/bbb/cccc.htm?p1=ddd   aaa 是虚目录  ； 
        /// 返回 ： http://abc:8080/aaa/bbb
        /// </summary>
        /// <returns></returns>
        public static string GetCurrPageShortUrl()
        {
            try
            {
                string url = HttpContext.Current.Request.Url.ToString();
                url = url.Substring(0, url.LastIndexOf('/'));
                return url;
            }
            catch
            {
                return "";
            }
        }
        /// <summary>
        /// 获取当前页面的URL根路径（包含虚目录）：http://abc:8080/aaa/bbb/cccc.htm?p1=ddd  aaa 是虚目录  ；
        /// 返回 ： http://abc:8080/aaaa
        /// </summary>
        /// <returns></returns>
        public static string GetCurrPageRootUrlWithVirtualPath()
        {
            try
            {
                string url = HttpContext.Current.Request.Url.ToString();
                int i = -1;

                i = url.IndexOf("//");
                i = url.IndexOf("/", i + 2);
                url = url.Substring(0, i);

                string virtualPath = Config.VirtualPath().Trim ();
                if (virtualPath != "")
                {
                    url = url + "/" + virtualPath;
                }

                return url;
            }
            catch
            {
                return "";
            }
        }
       

        /// <summary>
        /// 获取当前页面的路径（不包括虚目录）：http://abc:8080/aaa/bbb/cccc.htm?p1=ddd  aaa 是虚目录    
        /// 返回 ： /bbb/ccc.htm
        /// </summary>
        /// <returns></returns>
        public static string GetCurrPagePathWithoutVirtualPath()
        {
            try
            {
                string url = HttpContext.Current.Request.Url.LocalPath;
                string virtualPath = Config.VirtualPath().Trim();
                if (virtualPath != "")
                {
                    url = url.Substring(virtualPath.Length + 1);
                }
                return url;
            }
            catch
            {
                return "";
            }
        }
        /// <summary>
        /// 获取当前页面的路径（包括虚目录）：http://abc:8080/aaa/bbb/cccc.htm?p1=ddd  aaa 是虚目录    
        /// 返回 ： /aaa/bbb/ccc.htm
        /// </summary>
        /// <returns></returns>
        public static string GetCurrPagePathWithVirtualPath()
        {
            try
            {
                string url = HttpContext.Current.Request.Url.LocalPath;
                string virtualPath = Config.VirtualPath().Trim();
                if (virtualPath != "")
                {
                    url = url.Substring(virtualPath.Length + 1);
                }
                return url;
            }
            catch
            {
                return "";
            }
        }
    }
}
