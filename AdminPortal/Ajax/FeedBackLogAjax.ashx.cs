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
    /// Summary description for FeedBackLog
    /// </summary>
    public class FeedBackLogAjax : IHttpHandler, IRequiresSessionState
    {
        HttpRequest _request; 
        HttpResponse _response;
        HttpSessionState _session;
        HttpServerUtility _server;

        FeedBackLogBLL _feedBackLogBll = new FeedBackLogBLL();
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

        public  void GetFeedBackLog()
        {
            Expression<Func<FeedBackLog, bool>> exp = GetCondition();
            List<FeedBackLog> list = _feedBackLogBll.FindAll(_rows, _page, exp).ToList();
            int count = _feedBackLogBll.GetCount(exp);
            var json = list.ToJson<FeedBackLog>();
            JsonConvert<FeedBackLog> jc = new JsonConvert<FeedBackLog>();
            _response.Write(jc.ToDataGrid(list, count));
        }

        public Expression<Func<FeedBackLog, bool>> GetCondition()
        {
            var exp = DynamicLinqExpressions.True<FeedBackLog>();
            if (!string.IsNullOrWhiteSpace(_request["TaskNo"]))
            {
                exp = exp.And(g => g.TaskNo.Contains(_request["TaskNo"]));
            }
            return exp;
        }

        public void SaveFeedBackLog()
        {
            try
            {
                FeedBackLog fb;
                JsonConvert<string> jc = new JsonConvert<string>();
                if(_request["Mode"]=="1")
                {
                    fb = new FeedBackLog();
                }
                else
                {
                    fb=_feedBackLogBll.FindByID(_request["ID"].ToInt32(0));
                }

                if (fb != null)
                {
                    fb.KfZrr = _request["KfZrr"];
                    fb.IsKfCl = _request["IsKfCl"];
                    fb.FeedBackDate = _request["FeedBackDate"].ToDateTime(DateTime.Now);
                    fb.CstName = _request["CstName"];
                    fb.CsYzResult = _request["CsYzResult"];
                    fb.EndDate = _request["EndDate"].ToDateTime(DateTime.Now);
                    fb.CsZrr = _request["CsZrr"];
                    fb.FeedBackContent = _request["FeedBackContent"];
                    fb.KfClDate = _request["KfClDate"];
                    fb.TaskNo = _request["TaskNo"];
                    fb.Wtyy = _request["Wtyy"];
                }

                if (_request["Mode"] == "1")
                {
                    _feedBackLogBll.Add(fb);
                    _response.Write(jc.ToStatus(1));
                }
                else
                {
                    fb.FeedBackLogID = _request["ID"].ToInt32(0);
                    _feedBackLogBll.Update(fb);
                    _response.Write(jc.ToStatus(1));
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForWeb("FeedBackLogAjax.SaveFeedBackLog", ex.ToString());
                _response.Write("保存失败！");
            }
          
        }

        public void DelFeedBackLog()
        {
            try
            {
                string ids = _request["IDs"];
                if (!string.IsNullOrWhiteSpace(ids))
                {
                    List<string> list = ids.Split(',').ToList<string>();
                    foreach (string id in list)
                    {
                        _feedBackLogBll.Delete(id.ToInt32(0));
                    }
                    JsonConvert<string> jc = new JsonConvert<string>();
                    _response.Write(jc.ToStatus(1));
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForWeb("FeedBackLogAjax.DelFeedBackLog", ex.ToString());
                _response.Write("保存失败！");
            }

        }
    }
}