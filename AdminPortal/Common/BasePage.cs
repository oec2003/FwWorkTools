using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Data.SqlClient;

namespace FW.WT.AdminPortal.Common
{
    public class BasePage:Page
    {
        //protected readonly string _website= 
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string LoginName { get; set; }

        

        #region 判断页面是否是刷新提交
        /// <summary>
        /// 标致页面是否刷新提交
        /// </summary>
        public bool IsRefreshed
        {
            get
            {
                if (_flag.HasValue)
                {
                    return _flag.Value;
                }
                else
                {
                    _flag = this.Cache[Num] != null; return _flag.Value;
                };
            }
        }

        private bool? _flag;

        /// <summary>
        /// 刷新次数
        /// </summary>
        private string Num
        {
            get
            {
                if (ViewState["num"] == null)
                {
                    return Guid.NewGuid().ToString();
                }
                else
                {
                    return (string)ViewState["num"];
                }
            }
            set { ViewState["num"] = value; }
        }


        protected void Page_PreRender(object sender, EventArgs e)
        {
            this.Page.Unload += new EventHandler(Page_Unload);
            _num = Num;
            Num = Guid.NewGuid().ToString();
        }

        private string _num;

        void Page_Unload(object sender, EventArgs e)
        {
            if (_num != null)
                this.Cache[_num] = 1;
        }
        #endregion

        /// <summary>
        /// 检查用户登录是否过期
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            //if (Session["User"] != null)
            //{
            //    Users u = Session["User"] as Users;
            //    if (u != null)
            //    {
            //        if (u.Users_ID != 1 && Session["UserFunct"] == null)
            //        {
            //            Response.Redirect("~/Login.aspx?type=-2");
            //        }

            //        //2010-10-11 Eric.Feng Modify
            //        string userGrouName = string.Empty;
            //        DataTable dt = Session["UserFunct"] as DataTable;
            //        if (dt != null)
            //        {
            //            userGrouName = dt.Rows[0]["GrouName"].ToString();
            //            if (Application[userGrouName] == null)
            //            {
            //                userGrouName = UserGrou.GetUserGrouNameByUserId(u.Users_ID);
            //            }
            //        }
            //        else
            //        {
            //            userGrouName = UserGrou.GetUserGrouNameByUserId(u.Users_ID);
            //        }

            //        if (Application[userGrouName] != null)
            //        {
            //            Application.Lock();
            //            Session["UserFunct"] = Application[userGrouName];
            //            Application.UnLock();
            //        }

            //        if (Session["UserFunct"] == null)
            //        {
            //            Response.Redirect("~/Login.aspx?type=-2");
            //        }

            //        Users user = Session["User"] as Users;
            //        if (user != null)
            //        {
            //            _User_ID = user.Users_ID.ToString();
            //            _UserName = user.UserName;
            //            LoginName = user.LoginName;
            //        }


            //        if (Application["InfoCategory"] != null)
            //        {
            //            Application.Lock();
            //            Session["InfoCategory"] = Application["InfoCategory"];
            //            Application.UnLock();
            //        }

            //        if (Application["CommInfoType"] != null)
            //        {
            //            Application.Lock();
            //            Session["CommInfoType"] = Application["CommInfoType"];
            //            Application.UnLock();
            //        }

            //        base.OnLoad(e);
            //    }
            //    else
            //    {
            //        Response.Redirect("~/Login.aspx");
            //    }
            //}
            //else
            //{
            //    Response.Redirect("~/Login.aspx");
            //}

            base.OnLoad(e);
        }

        /// <summary>
        /// 显示信息然后跳转url
        /// </summary>
        /// <param name="msg">警告信息</param>
        protected void ShowMsgAndToUrl(string msg, string url)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<script language='javascript' defer>");
            sb.Append("operatorSuccess(\""+msg+"\",\"" + url + "\");");
            sb.Append("</script>");
            Page.ClientScript.RegisterStartupScript(this.GetType(), "ShowMsgAndToUrl", sb.ToString());
        }

        /// <summary>
        /// 显示信息
        /// </summary>
        /// <param name="msg">操作后的提示信息</param>
        /// <param name="url">转向地址</param>
        protected void ShowMsg(string msg)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<script language='javascript' defer>");
            sb.Append("showOperateResult(\"" + msg + "\");");
            sb.Append("</script>");
            Page.ClientScript.RegisterStartupScript(this.GetType(), "ShowMsg", sb.ToString());
        }

        /// <summary>
        /// 往页面输出执行脚本
        /// </summary>
        /// <param name="jsScript">脚本</param>
        protected void ExecuteScript(string jsScript)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<script language='javascript' defer>");
            sb.Append(jsScript);
            sb.Append("</script>");
            Page.ClientScript.RegisterStartupScript(this.GetType(), "ExecuteScript", sb.ToString());
        }
     
    }
}
