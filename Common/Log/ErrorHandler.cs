
using System;
using System.Web;
using System.Configuration;


namespace FW.CommonFunction
{
    /// <summary>
    /// �쳣�����ࣺ�Ǵ����쳣����
    /// </summary>
    public class ErrorHandler
    {
        /// <summary>
        /// ���������Ϣ��WEB�Ż��� 
        /// </summary>
        /// <param name="errorInfo">�Դ������������ʵ���û����Ѻ���Ϣ</param>
        /// <param name="sysInfo">ϵͳ���߳���Ϣ�����Բ������Ǵ�����ô��er.tostring()</param>
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
                //��Response.Redirect������߳���ֹ�쳣�����貶��
                ;
            }
        }


        /// <summary>
        /// ���������Ϣ���������Ӧ�ó���
        /// �����쳣��ʱ���쳣��Ҫ����Ӧ���������쳣��ҵ���߼�
        /// </summary>
        /// <param name="errorInfo">�Դ������������ʵ���û����Ѻ���Ϣ</param>
        /// <param name="sysInfo">ϵͳ���߳���Ϣ�����Բ������Ǵ�����ô��er.tostring()</param>
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
