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

namespace FW.WT.AdminPortal.Ajax
{
    public class VBSQLConvertAjax : AjaxHandler
    {
        [ParamMapping("action")]
        public string Action { get; set; }
        public override string ReturnBusinessData()
        {
            if (Action == "VbToSql")
            {
                return VbToSql();
            }
            else if (Action == "SqlToVb")
            {
                return SqlToVb();
            }
          
            return string.Empty;
        }


        [ParamMapping("Content")]
        public string Content { get; set; }
        private string SqlToVb()
        {
            string str = Content.Replace("\"", "\"\"");
            str = "\"" + str.Replace("\n", "\" & _\n\" ") + "\"";
            return str;
        }

        private string VbToSql()
        {
            string str = Content.Replace("\" & _\n", "\n") + "\"";
            str = str.Replace("\"", "");
            return str;
        }
    }
}