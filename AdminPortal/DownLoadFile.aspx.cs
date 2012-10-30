using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using FW.CommonFunction;
using System.Text;
namespace FW.WT.AdminPortal
{
    public partial class DownLoadFile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string filePath = CommonRequest.GetQueryString("FilePath");
            string fileName = CommonRequest.GetQueryString("FileName");
            if (File.Exists(filePath+fileName))
            {
                FileHelper fileHelper = new FileHelper(fileName, filePath, fileName);
                fileHelper.DownFile(Response);
            }
            else 
            {
                Response.Write("文件不存在！");
            }
        }

    }
}
