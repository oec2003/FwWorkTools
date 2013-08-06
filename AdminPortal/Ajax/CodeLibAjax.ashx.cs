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
    /// CodeLibAjax 的摘要说明
    /// </summary>
    public class CodeLibAjax : IHttpHandler,IRequiresSessionState
    {   
        HttpRequest _request;
        HttpResponse _response;
        HttpSessionState _session;
        HttpServerUtility _server;

        CodeLibBLL _codeLibBLL = new CodeLibBLL();
        JsonConvert<CodeLib> _jc = new JsonConvert<CodeLib>();
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

        public void SaveCodeLib()
        {
            try
            {
                CodeLib codeLib=null;
                string mode = CommonRequest.GetQueryString("Mode");
                int id = CommonRequest.GetQueryInt("ID", 0);

                if (mode == "1")
                {
                    codeLib = new CodeLib();
                }
                else if (mode == "2")
                {
                    codeLib = _codeLibBLL.FindByID(id);
                }
               
                if (codeLib != null)
                {
                    codeLib.Title = _request["Title"];
                    codeLib.CreateBy = CommonRequest.GetIP();
                    codeLib.CreateIP =CommonRequest.GetIP();
                    codeLib.CreateOn=DateTime.Now;
                    codeLib.Summary = _request["Summary"];
                    codeLib.CodeContent = _request["Content"];
                }
                if (mode == "1")
                {
                    _codeLibBLL.Add(codeLib);
                    _response.Write(FlagEnum.Success);
                }
                else if (mode == "2")
                {
                    _codeLibBLL.Update(codeLib);
                    _response.Write(FlagEnum.Success);
                }
                else
                {
                    _response.Write(FlagEnum.Error);
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForWeb("CodeLibAjax.SaveCodeLib", ex.ToString());
                _response.Write(FlagEnum.Error);
            }
        }
    }
}