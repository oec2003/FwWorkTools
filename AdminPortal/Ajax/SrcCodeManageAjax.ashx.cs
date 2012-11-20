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
    /// Summary description for SrcCodeManageAjax1
    /// </summary>
    public class SrcCodeManageAjax : IHttpHandler,IRequiresSessionState
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

        public  void  GetSrcCodeManage()
        {
            Expression<Func<SrcCodeManage, bool>> exp = GetCondition();
            List<SrcCodeManage> list = _srcCodeBll.FindAll(_rows, _page, exp)
               .OrderBy(x => x.CustomerArea)
               .OrderBy(x => x.CustomerName)
               .ToList();
            int count=_srcCodeBll.GetCount(exp);
            _response.Write(_jc.ToDataGrid(list, count));
        }

        public Expression<Func<SrcCodeManage, bool>> GetCondition()
        {
            var exp = DynamicLinqExpressions.True<SrcCodeManage>();
            if (!string.IsNullOrWhiteSpace(_request["CustomerName"]))
            {
                exp = exp.And(g => g.CustomerName.Contains(_request["CustomerName"]));
            }
            if (!string.IsNullOrWhiteSpace(_request["CustomerArea"]))
            {
                exp = exp.And(g => g.CustomerArea.Contains(_request["CustomerArea"]));
            }
            if (!string.IsNullOrWhiteSpace(_request["ProjVersion"]))
            {
                exp = exp.And(g => g.ProjVersion.Contains(_request["ProjVersion"]));
            }
            return exp;
        }

        public void SaveSrcCodeManage()
        {
            try
            {
                SrcCodeManage srcCode;
                if (_request["Mode"] == "1")
                {
                    srcCode = new SrcCodeManage();
                }
                else
                {
                    srcCode = _srcCodeBll.FindByID(_request["ID"].ToInt32(0));
                }
                if (srcCode != null)
                {
                    srcCode.CustomerName = _request["KfZrr"];
                    srcCode.CustomerArea = _request["KfZrr"];
                    srcCode.ProjVersion = _request["KfZrr"];
                    srcCode.VSSAddress = _request["KfZrr"];
                    srcCode.DBName = _request["KfZrr"];
                    srcCode.ServerPort = _request["KfZrr"];
                    srcCode.ServerName = _request["KfZrr"];
                    srcCode.ApplicationName = _request["KfZrr"];
                    srcCode.UserName = _request["KfZrr"];
                    srcCode.UserPwd = _request["KfZrr"];
                    srcCode.Remark = _request["KfZrr"];
                    _srcCodeBll.Add(srcCode);
                }
                if (_request["Mode"] == "1")
                {
                    _srcCodeBll.Add(srcCode);
                    _response.Write(_jc.ToStatus(1));
                }
                else
                {
                    _srcCodeBll.Update(srcCode);
                    _response.Write(_jc.ToStatus(1));
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForWeb("SrcCodeManageAjax.SaveSrcCodeManage", ex.ToString());
                _response.Write(_jc.ToStatus(0));
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
                        _srcCodeBll.Delete(id.ToInt32(0));
                    }
                    _response.Write(_jc.ToStatus(1));
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForWeb("FeedBackLogAjax.DelSrcCodeManage", ex.ToString());
                _response.Write(_jc.ToStatus(0));
            }

        }
    }
}