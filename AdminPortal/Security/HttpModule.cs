
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using FW.CommonFunction;

namespace FW.WT.AdminPortal.Security
{

    class HttpModule : IHttpModule
    {
        #region IHttpModule 成员
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            //throw new NotImplementedException();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void Init(HttpApplication context)
        {
            context.AcquireRequestState += new EventHandler(context_AcquireRequestState);

        }

        #endregion

        /// <summary>
        /// url重写
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReUrl_BeginRequest(object sender, EventArgs e)
        {
            /*
             * 以下功能演示，具体转向逻辑待以后确定 
            HttpContext context = ((HttpApplication)sender).Context;
            string requestPath = context.Request.Path.ToLower();
            string requestParameters = "";
            if (requestPath.IndexOf(".htm") != -1)
            {
                requestParameters = context.Request.QueryString.ToString().ToLower();
                if (requestParameters.Length > 0)
                {
                    requestParameters = "&" + requestParameters;
                }
                requestPath = "/Default.aspx";
            }

            context.RewritePath(requestPath, string.Empty, requestParameters);
            return;
            */
        }


        private void context_AcquireRequestState(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication)sender;
            StartProcessRequest(application);
        }

        string _sqlstr = "";
        private void StartProcessRequest(HttpApplication application)
        {
            try
            {
                HttpContext context = application.Context;

                string userName = "";
                string getkeys = "";
                //string sqlErrorPage = "~/";//转向的错误提示页面 
                string keyvalue = "";

                string requestUrl = context.Request.Path.ToString();
                string userIP = CommonRequest.GetIP().Trim();

                if (context.Request.QueryString != null &&
                    context.Request.FilePath != "/WT/AdminPortal/Ajax/VBSQLConvertAjax.ajax")
                {
                    for (int i = 0; i < context.Request.QueryString.Count; i++)
                    {
                        getkeys = context.Request.QueryString.Keys[i];
                        keyvalue = context.Server.UrlDecode(context.Request.QueryString[getkeys]);

                        if (!ProcessSqlStr(keyvalue))
                        {
                            //危害

                            //Utils.WriteLogFile(file, userIP + "：\r\n" + userName + "：\r\n" + requestUrl + "：\r\n[" + getkeys + "]：\r\n[" + "[" + keyvalue + "]：\r\n有害词为:[" + "[" + _sqlstr + "]");

                            //context.Response.Redirect("~/Error.aspx");
                            context.Response.Write("不要输入非法字符哦！");
                            context.Response.End();
                            return;
                        }
                    }
                }


                if (context.Request.Form != null &&
                    context.Request.FilePath != "/WT/AdminPortal/Ajax/VBSQLConvertAjax.ajax")
                {
                    for (int i = 0; i < context.Request.Form.Count; i++)
                    {
                        getkeys = context.Request.Form.Keys[i];
                        keyvalue = context.Server.HtmlDecode(context.Request.Form[i]);
                        if (getkeys == "__VIEWSTATE") continue;
                        if (!ProcessSqlStr(keyvalue))
                        {

                            //Utils.WriteLogFile(file, userIP + "：\r\n" + userName + "：\r\n" + requestUrl + "：\r\n[" + getkeys + "]：\r\n[" + "[" + keyvalue + "]：\r\n有害词为:[" + "[" + _sqlstr + "]");

                            //context.Response.Redirect("~/Error.aspx");
                            context.Response.Write("不要输入非法字符哦！");
                            context.Response.End();
                            return;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
            }
        }

        private bool ProcessSqlStr(string Str)
        {
            bool ReturnValue = true;
            try
            {
                if (Str.Trim() != "")
                {
                    //string SqlStr = "exec|insert |delete |update |count|chr|mid|master|truncate|char|declare |drop |creat |table|select |--|for|set|where |end|cursor|begin|into";
                    string SqlStr = ConfigInfo.GetConfigValue("FilterSql"); ;//"exec|insert |chr|master|truncate|declare |drop |creat |select |for|set|varchar|end|cursor|begin|into|open|cast";

                    string[] anySqlStr = SqlStr.Split('|');
                    foreach (string ss in anySqlStr)
                    {
                        if (Str.ToLower().IndexOf(ss)>=0)
                        {
                            _sqlstr = ss;
                            ReturnValue = false;
                            break;
                        }
                    }
                }
            }
            catch
            {
                ReturnValue = false;
            }
            return ReturnValue;
        }

    }
}

