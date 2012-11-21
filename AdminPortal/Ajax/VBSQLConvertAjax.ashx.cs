using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FW.CommonFunction;
using FW.CommonFunction.Extensions;
using FW.WT.BLL;
using FW.WT.LinqDataModel;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.IO;
using Newtonsoft.Json;
using System.Reflection;
using System.Web.SessionState;

namespace FW.WT.AdminPortal.Ajax
{
    /// <summary>
    /// Summary description for VBSQLConvertAjax
    /// </summary>
    public class VBSQLConvertAjax : IHttpHandler, IRequiresSessionState
    {

        HttpRequest _request;
        HttpResponse _response;
        HttpSessionState _session;
        HttpServerUtility _server;

        SrcCodeManageBLL _srcCodeBll = new SrcCodeManageBLL();
        JsonConvert<SrcCodeManage> _jc = new JsonConvert<SrcCodeManage>();
        int _page = 1;
        int _rows = 10;

        public void ProcessRequest(HttpContext context)
        {
            //不让浏览器缓存
            context.Response.Buffer = true;
            context.Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);
            context.Response.AddHeader("pragma", "no-cache");
            context.Response.AddHeader("cache-control", "");
            context.Response.CacheControl = "no-cache";
            context.Response.ContentType = "text/plain";

            _request = context.Request;
            _response = context.Response;
            _session = context.Session;
            _server = context.Server;

            string method = _request["Method"].ToString();
            _page = _request["Page"].ToInt32(1);
            _rows = _request["Rows"].ToInt32(10);
            MethodInfo methodInfo = this.GetType().GetMethod(method);
            methodInfo.Invoke(this, null);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public void SqlToVb()
        {
            try
            {
                string content = _request["Content"];
                string str = content.Replace("\"", "\"\"");
                str = "\"" + str.Replace("\n", "\" & _\n\" ") + "\"";
                _response.Write(str);
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForWeb("VBSQLConvertAjax.SqlToVb", ex.ToString());
                _response.Write(FlagEnum.Error);
            }
        }

        public void VbToSql()
        {
            try
            {
                string content = _request["Content"];
                string str = content.Replace("\" & _\n", "\n") + "\"";
                str = str.Replace("\"", "");
                _response.Write(str);
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForWeb("VBSQLConvertAjax.VbToSql", ex.ToString());
                _response.Write(FlagEnum.Error);
            }
        }
    }
}