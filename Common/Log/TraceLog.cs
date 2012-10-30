using System;
using System.Collections.Generic;

using System.Text;

namespace FW.CommonFunction
{
    /// <summary>
    /// 记录系统运行日志类，可以在config中配置日志记录的路径、文件名、文件重写标志；可以不配置，那么使用默认值
    ///<appSettings>
    ///<!-- 系统日志保存路径 -->
    ///<add key="LogPath" value="d:\log"/>
    ///<!-- 系统日志文件名，不要后缀名 -->
    ///<add key="LogFileName" value="WebLog"/>
    ///<!-- 系统日志文件重命名的周期， none,second,minute,hour,day,month-->
    ///<add key="LogFileRenameTime" value="hour"/>
    ///</appSettings>
    /// </summary>
    public class TraceLog
    {
        /// <summary>
        /// 写入运行Trace
        /// </summary>
        /// <param name="info"></param>
        public static void WriteLine(string info)
        {
            if (info == null || info == "")
            {
                return;
            }

            LogInFile objLog = LogInFile.GetInstance();
           
            try
            {
                objLog.LogFileName =  Config.LogFileName()+"_Trace";
            }
            catch
            {
                objLog.LogFileName = "Log_Trace";
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
                objLog.LogFileDir = Config.LogPath() + "Trace\\";
            }
            catch
            {
                objLog.LogFileDir = "";
            }

            objLog.WriteLog(info);
        }
    }
}
