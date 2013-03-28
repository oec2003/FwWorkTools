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
    /// Summary description for TypicalFuncAjax
    /// </summary>
    public class TypicalFuncAjax : IHttpHandler,IRequiresSessionState
    {

        HttpRequest _request;
        HttpResponse _response;
        HttpSessionState _session;
        HttpServerUtility _server;

        TypicalFuncBLL _typicalFuncBll = new TypicalFuncBLL();
        JsonConvert<TypicalFunc> _jc = new JsonConvert<TypicalFunc>();
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

        public void GetTypicalFunc()
        {
            Expression<Func<TypicalFunc, bool>> exp = GetCondition();
            List<TypicalFunc> list = _typicalFuncBll.FindAll(_rows, _page, exp)
               .OrderBy(x => x.CustomerArea)
               .OrderBy(x => x.CustomerName)
               .ToList();
            int count = _typicalFuncBll.GetCount(exp);
            _response.Write(_jc.ToDataGrid(list, count));
        }

        public Expression<Func<TypicalFunc, bool>> GetCondition()
        {
            var exp = DynamicLinqExpressions.True<TypicalFunc>();
            if (!string.IsNullOrWhiteSpace(_request["CustomerName"]))
            {
                exp = exp.And(g => g.CustomerName.Contains(_request["CustomerName"]));
            }
            if (!string.IsNullOrWhiteSpace(_request["CustomerArea"]))
            {
                exp = exp.And(g => g.CustomerArea.Contains(_request["CustomerArea"]));
            }
            if (!string.IsNullOrWhiteSpace(_request["TaskNo"]))
            {
                exp = exp.And(g => g.TaskNo.Contains(_request["TaskNo"]));
            }
            return exp;
        }

          public void SaveTypicalFunc()
        {
            try
            {
                TypicalFunc typicalFunc = null;
                string mode = CommonRequest.GetQueryString("Mode");
                int id = CommonRequest.GetQueryInt("ID", 0);

                if (mode == "1")
                {
                    typicalFunc = new TypicalFunc();
                }
                else if (mode == "2")
                {
                    typicalFunc = _typicalFuncBll.FindByID(id);
                }
               
                if (typicalFunc != null)
                {
                    typicalFunc.CustomerName = _request["CustomerName"];
                    typicalFunc.CustomerArea = _request["CustomerArea"];
                    typicalFunc.TaskNo = _request["TaskNo"];
                    typicalFunc.Url = _request["Url"];
                    typicalFunc.Fzr = _request["Fzr"];
                    typicalFunc.Title = _request["Title"];
                    typicalFunc.Remark = _request["Remark"];
                }
                if (mode == "1")
                {
                    _typicalFuncBll.Add(typicalFunc);
                    _response.Write(FlagEnum.Success);
                }
                else if (mode == "2")
                {
                    _typicalFuncBll.Update(typicalFunc);
                    _response.Write(FlagEnum.Success);
                }
                else
                {
                    _response.Write(FlagEnum.Error);
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForWeb("TypicalFuncAjax.SaveSrcCodeManage", ex.ToString());
                _response.Write(FlagEnum.Error);
            }
        }

        public void DelSrcCodeManage()
        {
            try
            {
                string ids = _request["IDs"];
                if (!string.IsNullOrWhiteSpace(ids))
                {
                    List<string> list = ids.Split(',').ToList<string>();
                    foreach (string id in list)
                    {
                        _typicalFuncBll.Delete(id.ToInt32(0));
                    }
                    _response.Write(FlagEnum.Success);
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForWeb("TypicalFuncAjax.DelSrcCodeManage", ex.ToString());
                _response.Write(FlagEnum.Error);
            }

        }

    }
}