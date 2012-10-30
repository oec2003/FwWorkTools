using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FW.WT.LinqDataModel;
using FW.CommonFunction;
using FW.WT.BLL;
using System.Text;
using Menu = FW.WT.LinqDataModel.Menu;
namespace FW.WT.AdminPortal
{
    public partial class Main : System.Web.UI.MasterPage
    {
        public int MenuID { get; set; }
        MenuBLL menuBll = new MenuBLL();
        public string ParentMenuName { get; set; }
        public string MenuName { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            //this.ID = "Main";

            if (!IsPostBack)
            {
                litMenu.Text = GetMenu();

                litNav.Text = GetNav();
            }
        }

        /// <summary>
        /// 左侧菜单
        /// </summary>
        /// <returns></returns>
        public string GetMenu()
        {
            MenuBLL menuBll=new MenuBLL();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<div class=\"men\">");
            sb.AppendLine("<div class=\"men_01\">");
            sb.AppendLine("  <div class=\"men_02a\"><img src=\""+ResolveUrl("images/men05.gif")+"\" width=\"16\" height=\"14\" /></div>");
            sb.AppendLine("  <div class=\"men_02b\"><a href=\"#\" class=\"f07 bk\">系统首页</a></div>");
            sb.AppendLine("</div>");
            sb.AppendLine("<div class=\"men_03\">");
            IList<Menu> list = menuBll.FindAll(x => x.IsShow==true)
                .OrderBy(x => x.GroupID)
                .ThenBy(x=>x.ParentID)
                .ThenBy(x => x.OrderBy)
                .ToList();

            foreach (Menu menu in list)
            {
                if (menu.ParentID == 0) //目录
                {
                    sb.AppendLine("<div class=\"men_03_01\">");
                    sb.AppendLine(" <div class=\"men_03a\"><img src=\""+ResolveUrl("images/men03.gif")+"\" width=\"11\" height=\"9\" /></div>");
                    sb.AppendLine(" <div class=\"men_03b\"><a href=\"#\" class=\"bk bold\">"+menu.MenuName+"</a></div>");
                    sb.AppendLine("</div>");
                }
                else
                { 
                    sb.AppendLine("<div class=\"men_03_02\">");
                    if(menu.MenuID==this.MenuID)
                        sb.AppendLine(" <div class=\"men_03_02_01 men_bg\">");
                    else
                        sb.AppendLine(" <div class=\"men_03_02_01\">");
                    sb.AppendLine("     <div class=\"men_03a\"><img src=\""+ResolveUrl("images/men04.gif")+"\" width=\"11\" height=\"9\" /></div>");
                    sb.AppendLine("     <div class=\"men_03b\"><a href=\"" + ResolveUrl(menu.Url) + "\" class=\"bk\" target=\"_self\">" + menu.MenuName + "</a></div>");
                    sb.AppendLine(" </div>");
                    sb.AppendLine("</div>");
                }
            }

            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            return sb.ToString();
        }

        private string GetNav()
        {
            string nav = "首页-->常用功能-->源码地址管理";

            IList<Menu> list = menuBll.FindAll(x => x.MenuID == this.MenuID).ToList();
            if (list != null && list.Count > 0)
            {
                Menu menu = list[0];
                var parentID = menu.ParentID;
                var menuName = menu.MenuName;

                IList<Menu> parentList = menuBll.FindAll(x => x.MenuID == parentID).ToList();
                Menu parentMenu = parentList[0];
                var parentMenuName = parentMenu.MenuName;
                nav = "首页" + "-->" + parentMenuName + "-->" + menuName;
            }
            
            return nav;
        }
    }
}
