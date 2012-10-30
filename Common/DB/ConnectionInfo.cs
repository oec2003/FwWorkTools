using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Web;

namespace FW.CommonFunction
{
    public class ConnectionInfo
    {
        private string _strAppName;
        public ConnectionInfo()
        {
            _strAppName = System.Configuration.ConfigurationManager.AppSettings["AppName"];
            LoadSqlConnectionInfo(_strAppName);
        }

        public ConnectionInfo(string appName)
        {
            _strAppName = appName;
            LoadSqlConnectionInfo(_strAppName);
        }


        public string GetSqlConnectionString()
        {
            string strConn = string.Empty;
            strConn = HttpContext.Current.Application["ConnStr_" + _strAppName] as string;

            if (!string.IsNullOrEmpty(strConn))
            {
                return strConn;
            }
            else
            {
                if (this.ServerProt == "1433")
                {  //有些防火墙会禁用1433端口，为了用于兼容之前的模式做此判断
                    strConn = "server=" + this.ServerName + ";uid=" + this.UserName + ";pwd=" + this.Password + ";database=" + this.DBName;
                }
                else
                {
                    strConn = "server=" + this.ServerName + "," + this.ServerProt + ";uid=" + this.UserName + ";pwd=" + this.Password + ";database=" + this.DBName;
                }

                HttpContext.Current.Application.Lock();
                HttpContext.Current.Application["ConnStr_" + _strAppName] = strConn;
                HttpContext.Current.Application.UnLock();

                return strConn;
            }
        }

        private string UserName { get; set; }
        private string Password { get; set; }
        private string DBName { get; set; }
        private string ServerName { get; set; }
        private string ServerProt { get; set; }


        // 私有函数
        private void LoadSqlConnectionInfo(string appName)
        {
            RegistryKey RegKey = null;

            if (Detect3264() == "64")
            {
                //64位操作系统。由于数据库注册程序是pb做的，在64位操作系统中只能以32位方式运行，它写注册表只能写入到：Software\Wow6432Node\
                RegKey = Registry.LocalMachine.OpenSubKey("Software\\Wow6432Node\\mysoft\\" + appName, false);
            }
            else
            {
                //32位操作系统
                RegKey = Registry.LocalMachine.OpenSubKey("Software\\mysoft\\" + appName, false);
            }

            if ((RegKey != null))
            {
                this.Password = RegKey.GetValue("SaPassword", "").ToString();
                // 从注册表中读取密码
                this.Password = Cryptogram.DeCode(this.Password);
                // 进行解密反变换

                this.DBName = RegKey.GetValue("DBName", "").ToString();
                this.ServerName = RegKey.GetValue("ServerName", "").ToString();

                if (RegKey.GetValue("ServerProt", "") == null || RegKey.GetValue("ServerProt", "").ToString().Length < 1)
                {
                    this.ServerProt = "1433";
                }
                else
                {
                    this.ServerProt = RegKey.GetValue("ServerProt", "").ToString();
                }
                this.UserName = RegKey.GetValue("UserName", "").ToString();
            }
            else
            {
                Exception ex = new System.Exception("未配置注册表信息");
                TraceLog.WriteLine(string.Format("未配置注册表信息,键值:{0}未找到.无法获取数据库连接字符串!", appName));
                throw ex;
            }
        }

        /// <summary>
        /// 检查当前操作系统是32位还是64位
        /// </summary>
        /// <returns>"32"、"64"、"UNKNOWN"</returns>
        /// <remarks>
        /// 1、为了避免asp.net权限不够，所以没有使用 System.Management 和 WMI的方法进行版本判断。
        /// 2、使用System.IntPtr.Size只能判断当前DLL运行在32还是64位环境。该方法有一个缺点：当返回"32"时，有可能是在64位操作系统中以32位兼容模式运行。
        /// 但由于ERP是以AnyCpu方式编译，对于ERP来说，在64位操作系统中一定是以64位方式运行的，所以可以忽略此问题。
        /// </remarks>
        private string Detect3264()
        {
            if ((System.IntPtr.Size == 4))
            {
                return "32";
            }
            else if ((System.IntPtr.Size == 8))
            {
                return "64";
            }
            else
            {
                return "UNKNOWN";
            }
        }
    }
}
