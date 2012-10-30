using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FW.WT.AdminPortal.Common;

namespace FW.WT.AdminPortal
{
    public partial class WebForm2 : QueryBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            this.TableName = "User";
            string condition = this.GetCondition();
            Response.Write(condition);
        }
    }
}
