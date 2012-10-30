using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Web.Caching;

namespace FW.CommonFunction
{
    /// <summary>
    /// 通道请求处理工厂类
    /// </summary>
    public class AjaxHandlerFactory : IHttpHandlerFactory
    {
        private System.Web.Caching.Cache _cache = HttpContext.Current.Cache;
        private int _cacheMinutes = 10;
        private string _urlPattern = null;
        private string _classNameReplacement = null;


        #region IHttpHandlerFactory 成员
        /// <summary>
        /// 获取处理Handler
        /// </summary>
        public IHttpHandler GetHandler(HttpContext context, string requestType, string url, string pathTranslated)
        {
            //根据请求地址解析出具体处理类，新建该类的实例进行处理
            string path = context.Request.Path;
            string className = ParseClassName(path);

            object handler = _cache.Get(className.ToUpper());

            if (handler != null)
            {
                return (IHttpHandler)handler;
            }

            Type t = Type.GetType(className, false, true);
            if (t != null)
            {
                if (t.GetInterface("IHttpHandler") != null)
                {
                    return (IHttpHandler)Activator.CreateInstance(t);
                }
            }
            return null;
        }
        /// <summary>
        /// 解析请求地址得到处理类
        /// </summary>
        private string ParseClassName(string path)
        {
            _urlPattern = System.Configuration.ConfigurationManager.AppSettings["AjaxHandler_UrlPattern"];
            if (_urlPattern == null)
            {
                throw new Exception("请在appSettings下配置AjaxHandler_UrlPattern项");
            }

            if (path.ToLower().StartsWith("/kenel"))
            { 
                _classNameReplacement = "FW.$1.$2, FW.$1";
            }
            else
            {
                _classNameReplacement = System.Configuration.ConfigurationManager.AppSettings["AjaxHandler_ClassNameReplacement"];
                if (_classNameReplacement == null)
                {
                    throw new Exception("请在appSettings下配置AjaxHandler_ClassNameReplacement项");
                }
            }

            return Regex.Replace(path, _urlPattern, _classNameReplacement).Replace("/", ".");
        }
        /// <summary>
        /// 释放Handler
        /// </summary>
        public void ReleaseHandler(IHttpHandler handler)
        {
            if (handler != null && handler.IsReusable)
            {
                //如果handler声明为可以重复使用，缓存该实例
                string classname = handler.GetType().FullName.ToUpper();
                _cache.Insert(classname, handler, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(0, _cacheMinutes, 0));
            }
        }
        #endregion
    }
}
