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
                SrcCodeManage srcCode=null;
                string mode = CommonRequest.GetQueryString("Mode");
                int id = CommonRequest.GetQueryInt("ID", 0);

                if (mode == "1")
                {
                    srcCode = new SrcCodeManage();
                }
                else if (mode == "2")
                {
                    srcCode = _srcCodeBll.FindByID(id);
                }
               
                if (srcCode != null)
                {
                    srcCode.CustomerName = _request["CustomerName"];
                    srcCode.CustomerArea = _request["CustomerArea"];
                    srcCode.ProjVersion = _request["ProjVersion"];
                    srcCode.VSSAddress = _request["VSSAddress"];
                    srcCode.DBName = _request["DBName"];
                    srcCode.ServerPort = _request["ServerPort"];
                    srcCode.ServerName = _request["ServerName"];
                    srcCode.ApplicationName = _request["ApplicationName"];
                    srcCode.UserName = _request["UserName"];
                    srcCode.UserPwd = _request["UserPwd"];
                    srcCode.Remark = _request["Remark"];
                }
                if (mode == "1")
                {
                    _srcCodeBll.Add(srcCode);
                    _response.Write(FlagEnum.Success);
                }
                else if (mode == "2")
                {
                    _srcCodeBll.Update(srcCode);
                    _response.Write(FlagEnum.Success);
                }
                else
                {
                    _response.Write(FlagEnum.Error);
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForWeb("SrcCodeManageAjax.SaveSrcCodeManage", ex.ToString());
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
                        _srcCodeBll.Delete(id.ToInt32(0));
                    }
                    _response.Write(FlagEnum.Success);
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForWeb("SrcCodeManageAjax.DelSrcCodeManage", ex.ToString());
                _response.Write(FlagEnum.Error);
            }

        }

        public void IsExistName()
        {
            try
            {
                string mode = CommonRequest.GetFormString("Mode");
                string cstName = CommonRequest.GetFormString("CstName");
                int id = CommonRequest.GetFormInt("id",0);
                var exp = DynamicLinqExpressions.True<SrcCodeManage>();
                exp = exp.And(g => g.CustomerName.ToUpper() == cstName.ToUpper());
                if (mode == "2")
                {
                    exp = exp.And(g => g.SrcCodeManageID != id);
                }
                Expression<Func<SrcCodeManage, bool>> condition = exp;
                int count = _srcCodeBll.GetCount(condition);
                if (count > 0)
                {
                    _response.Write(_jc.ToStatus(1));
                }
                else
                {
                    _response.Write(_jc.ToStatus(0));
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForWeb("SrcCodeManageAjax.IsExistName", ex.ToString());
                _response.Write(_jc.ToStatus(-1));
            }

        }

        public void RegisterAndDownLoadReg()
        {
            try
            {
                string dbName = _request["dbName"];
                string appName = _request["appName"];
                string serverName = _request["serverName"];
                string userName = _request["userName"];
                string saPassword = _request["saPassword"];
                string serverProt = _request["serverProt"];
                string customerName = _request["customerName"];
                string subkey = @"software\mysoft\" + appName;
                string fullRegPath = @"[HKEY_LOCAL_MACHINE\SOFTWARE\mysoft\" + appName + "]";
                string regName = customerName + ".reg";

                saPassword = Cryptogram.EnCode(saPassword);

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Windows Registry Editor Version 5.00");
                sb.AppendLine(fullRegPath);
                sb.AppendLine("\"DBName\"=\"" + dbName + "\"");
                sb.AppendLine("\"IsConnectSl\"=\"0\"");
                sb.AppendLine("\"IsRead\"=\"1\"");
                sb.AppendLine("\"SaPassword\"=\"" + saPassword + "\"");
                sb.AppendLine("\"ServerName\"=\"" + serverName.Replace(@"\", @"\\") + "\"");
                sb.AppendLine("\"ServerProt\"=\"" + serverProt + "\"");
                sb.AppendLine("\"UserName\"=\"" + userName + "\"");

                string filePath = System.Threading.Thread.GetDomain().BaseDirectory + "RegFile\\"; ;
                FileHelper fileHelper = new FileHelper(regName, filePath, regName);
                fileHelper.DeleteFile();
                fileHelper.WriteFile(sb.ToString());
                // fileHelper.DownFile(Response);

                _response.Write(filePath + "|" + regName);
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForWeb("SrcCodeManageAjax.RegisterAndDownLoadReg", ex.ToString());
                _response.Write(FlagEnum.Error);
            }
        }
    }
}