using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Threading;

namespace FW.CommonFunction
{
    /// <summary>
    /// XmlHttpRequest请求处理抽象基类
    /// </summary>
    public abstract class AjaxHandler : IHttpHandler, IRequiresSessionState
    {
        public HttpRequest Request { get; private set; }
        public HttpResponse Response { get; private set; }
        public HttpServerUtility Server { get; private set; }
        public HttpSessionState Session { get; private set; }

        #region IHttpHandler 成员
        /// <summary>
        /// 当在子类中重写时指定当前实例是否可以重复使用
        /// </summary>
        public virtual bool IsReusable
        {
            get { return false; }
        }

        /// <summary>
        /// 请求处理
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            this.Request = context.Request;
            this.Response = context.Response;
            this.Server = context.Server;
            this.Session = context.Session;

            this.Response.Clear();

            CheckAuth();
            BindParams(context.Request.Params);
            context.Response.ContentType = this.ContentType;
            context.Response.Output.Write(this.ReturnBusinessData());
        }

        #endregion

        /// <summary>
        /// 绑定参数到属性
        /// </summary>
        /// <param name="requestparams">参数集合</param>
        protected void BindParams( NameValueCollection requestparams)
        {
            ParamMappinger.Mapping(this, requestparams);
        }
        /// <summary>
        /// 检查登录
        /// </summary>
        protected void CheckAuth()
        {

        }
        /// <summary>
        /// 当在子类中重写时返回业务处理结果
        /// </summary>
        /// <returns></returns>
        public abstract string ReturnBusinessData();
        /// <summary>
        /// 指定当前处理的返回类型
        /// </summary>
        public virtual string ContentType
        {
            get { return "text/html"; }
        }
    }
}
