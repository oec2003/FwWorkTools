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
    public class SrcCodeManageAjax:AjaxHandler 
    {
        SrcCodeManageBLL srcCodeBll = new SrcCodeManageBLL();

        [ParamMapping("action")]
        public string Action { get; set; }
        public override string ReturnBusinessData()
        {
            if (Action == "GetSrcCodeManage")
            {
                return GetSrcCodeManage();
            }
            else if (Action == "AddSrcCodeManage")
            {
                return AddSrcCodeManage();
            }
            else if (Action == "UpdateSrcCodeManage")
            {
                return UpdateSrcCodeManage();
            }
            else if (Action == "DelSrcCodeManage")
            {
                return DelSrcCodeManage();
            }
            else if (Action == "RegisterAndDownLoadReg")
            {
                return RegisterAndDownLoadReg();
            }
            else if (Action == "IsExistName")
            {
                return IsExistName();
            }
            return string.Empty;
        }

        private string GetSrcCodeManage()
        {
            List<SrcCodeManage> list = srcCodeBll.GetSource()
               .OrderBy(x => x.CustomerArea)
               .OrderBy(x => x.CustomerName)
               .ToList();
            var json = list.ToJson<SrcCodeManage>();
            return json;
        }

        [ParamMapping("JsonData")]
        public string JsonData { get; set; }
        private string AddSrcCodeManage()
        {
            try
            {
                string[] arrJsonData = JsonData.Split('|');
                SrcCodeManage srcCode = new SrcCodeManage();
                srcCode.CustomerName = arrJsonData[0];
                srcCode.CustomerArea = arrJsonData[1];
                srcCode.ProjVersion = arrJsonData[2];
                srcCode.VSSAddress = arrJsonData[3];
                srcCode.DBName = arrJsonData[4];
                srcCode.ServerPort = arrJsonData[5];
                srcCode.ServerName = arrJsonData[6];
                srcCode.ApplicationName = arrJsonData[7];
                srcCode.UserName = arrJsonData[8];
                srcCode.UserPwd = arrJsonData[9];
                srcCode.Remark = arrJsonData[10];
                srcCodeBll.Add(srcCode);
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForWeb("SrcCodeManageAjax.AddSrcCodeManage", ex.ToString());
                return "ERROR";
            }
            return "OK";
        }

        private string UpdateSrcCodeManage()
        {
            try
            {
                string[] arrJsonData = JsonData.Split('|');
                int id = arrJsonData[11].ToInt32(0);
                SrcCodeManage srcCode = srcCodeBll.FindByID(id);
                srcCode.CustomerName = arrJsonData[0];
                srcCode.CustomerArea = arrJsonData[1];
                srcCode.ProjVersion = arrJsonData[2];
                srcCode.VSSAddress = arrJsonData[3];
                srcCode.DBName = arrJsonData[4];
                srcCode.ServerPort = arrJsonData[5];
                srcCode.ServerName = arrJsonData[6];
                srcCode.ApplicationName = arrJsonData[7];
                srcCode.UserName = arrJsonData[8];
                srcCode.UserPwd = arrJsonData[9];
                srcCode.Remark = arrJsonData[10];
                srcCodeBll.Update(srcCode);
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForWeb("SrcCodeManageAjax.UpdateSrcCodeManage", ex.ToString());
                return "ERROR";
            }
            return "OK";
        }

        private string RegisterAndDownLoadReg()
        {
            try
            {
                string[] arrJsonData = JsonData.Split('|');
                string dbName = arrJsonData[0];
                string appName = arrJsonData[1];
                string serverName = arrJsonData[2];
                string userName = arrJsonData[3];
                string saPassword = arrJsonData[4];
                string serverProt = arrJsonData[5];
                string customerName = arrJsonData[6];
                string subkey = @"software\mysoft\" + appName;
                string fullRegPath = @"[HKEY_LOCAL_MACHINE\SOFTWARE\mysoft\" + appName+"]";
                string regName = customerName + ".reg";

                saPassword = Cryptogram.EnCode(saPassword);

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Windows Registry Editor Version 5.00");
                sb.AppendLine(fullRegPath);
                sb.AppendLine("\"DBName\"=\"" + dbName + "\"");
                sb.AppendLine("\"IsConnectSl\"=\"0\"");
                sb.AppendLine("\"IsRead\"=\"1\"");
                sb.AppendLine("\"SaPassword\"=\"" + saPassword + "\"");
                sb.AppendLine("\"ServerName\"=\"" + serverName.Replace(@"\",@"\\") + "\"");
                sb.AppendLine("\"ServerProt\"=\"" + serverProt + "\"");
                sb.AppendLine("\"UserName\"=\"" + userName + "\"");

                string filePath = System.Threading.Thread.GetDomain().BaseDirectory + "RegFile\\"; ;
                FileHelper fileHelper = new FileHelper(regName, filePath, regName);
                fileHelper.DeleteFile();
                fileHelper.WriteFile(sb.ToString());
               // fileHelper.DownFile(Response);


                return filePath + "|" + regName;


                //saPassword = Cryptogram.EnCode(saPassword);
                //RegHelper reg = new RegHelper(subkey, RegDomain.LocalMachine);
                //reg.CreateSubKey(subkey);
                //reg.WriteRegeditKey("DBName", dbName);
                //reg.WriteRegeditKey("IsConnectSl", "0");
                //reg.WriteRegeditKey("IsRead", "1");
                //reg.WriteRegeditKey("SaPassword", saPassword);
                //reg.WriteRegeditKey("ServerName", serverName);
                //reg.WriteRegeditKey("ServerProt", serverProt);
                //reg.WriteRegeditKey("UserName", userName);
                //RegHelper.ExportReg(@"c:\" + regName, fullRegPath);
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForWeb("SrcCodeManageAjax.RegisterAndDownLoadReg", ex.ToString());
                return "ERROR";
            }
            return "OK";
        }

        [ParamMapping("SrcCodeManageID")]
        public string SrcCodeManageID { get; set; }
        private string DelSrcCodeManage()
        {
            try
            {
                srcCodeBll.Delete(SrcCodeManageID.ToInt32(0));
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForWeb("SrcCodeManageAjax.DelSrcCodeManage", ex.ToString());
                return "ERROR";
            }
            return "OK";
        }


        [ParamMapping("CustomerName")]
        public string CustomerName { get; set; }
        [ParamMapping("Mode")]
        public string Mode { get; set; }
      
        private string IsExistName()
        {
            try
            {
                var exp = DynamicLinqExpressions.True<SrcCodeManage>();
                exp=exp.And(g => g.CustomerName.ToUpper()==CustomerName.ToUpper());
                if (Mode == "2")
                { 
                    exp=exp.And(g=>g.SrcCodeManageID!=SrcCodeManageID.ToInt32(0));
                }
                Expression<Func<SrcCodeManage, bool>> condition = exp;
                int count = srcCodeBll.GetCount(condition);
                if(count>0)
                {
                    return "NO";
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForWeb("SrcCodeManageAjax.IsExistName", ex.ToString());
                return "ERROR";
            }
            return "OK";
        }

    }
}