
using System;
using System.Web;
using System.Configuration;


namespace FW.CommonFunction
{
    /// <summary>
    /// 异常处理类：是处理异常的类
    /// </summary>
    public class ErrorHandler
    {
        /// <summary>
        /// 处理错误信息（WEB门户） 
        /// </summary>
        /// <param name="errorInfo">对错误的描述，现实给用户的友好信息</param>
        /// <param name="sysInfo">系统的线程信息，可以不填，如果是错误，那么用er.tostring()</param>
        public static void ExceptionHandlerForWeb(string errorInfo, string sysInfo)
        {
            string messageURL = WebUrl.GetCurrPageRootUrlWithVirtualPath() + "/ErrorInfo.aspx";

            if (errorInfo == null || errorInfo == "")
            {
                return;
            }
            LogInFile objLog = LogInFile.GetInstance();

            try
            {
                objLog.LogFileName =Config.LogFileName()+"_WebError";
            }
            catch
            {
                objLog.LogFileName = "Log_WebError";
            }
            try
            {
                objLog.LogFileNewFlag  =(LogInFile .LogFileNewFlagType)Enum.Parse(typeof(LogInFile .LogFileNewFlagType), Config.LogFileNewFlag(), true);
            }
            catch
            {
                objLog.LogFileNewFlag = LogInFile.LogFileNewFlagType.day;
            }

            try
            {
                objLog.LogFileDir = Config.LogPath()+"Error\\";
            }
            catch
            {
                objLog.LogFileDir = "";
            }
            objLog.WriteLog(errorInfo + "\r\n" + sysInfo);



            sysInfo = sysInfo.Replace("\n", "");
            sysInfo = sysInfo.Replace("\r", "");
            sysInfo = sysInfo.Replace("\t", "");
            sysInfo = sysInfo.Replace(":", "");
            HttpCookie myCookie = new HttpCookie("FBI_EXCEPTIONSYSINFO");
            myCookie.Value =Cryptogram.UrlEncode ( sysInfo);
            myCookie.Expires = DateTime.Now.AddMinutes(10);
            HttpContext.Current.Response.Cookies.Add(myCookie);
            try
            {
                HttpContext.Current.Response.Redirect(messageURL + "?ErrorInfo=" +  Cryptogram .UrlEncode ( errorInfo));
            }
            catch (System.Threading.ThreadAbortException)
            {
                //由Response.Redirect引起的线程中止异常，不予捕获
                ;
            }
        }


        /// <summary>
        /// 处理错误信息（服务或者应用程序）
        /// 出现异常的时候，异常需要外层的应用来处理异常的业务逻辑
        /// </summary>
        /// <param name="errorInfo">对错误的描述，现实给用户的友好信息</param>
        /// <param name="sysInfo">系统的线程信息，可以不填，如果是错误，那么用er.tostring()</param>
        public static void ExceptionHandlerForApp(string errorInfo, string sysInfo)
        {
            if (errorInfo == null || errorInfo == "")
            {
                return;
            }

            LogInFile objLog = LogInFile.GetInstance();

            try
            {
                objLog.LogFileName =  Config.LogFileName()+"_AppError";
            }
            catch
            {
                objLog.LogFileName = "Log_AppError";
            }
            try
            {
                objLog.LogFileNewFlag = (LogInFile.LogFileNewFlagType)Enum.Parse(typeof(LogInFile.LogFileNewFlagType), Config.LogFileNewFlag(), true);
            }
            catch
            {
                objLog.LogFileNewFlag = LogInFile.LogFileNewFlagType.hour;
            }

            try
            {
                objLog.LogFileDir = Config.LogPath() + "Error\\";
            }
            catch
            {
                objLog.LogFileDir = "";
            }
            objLog.WriteLog(errorInfo + "\r\n" + sysInfo);
        }
    }
}
