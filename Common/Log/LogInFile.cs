
using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Collections.Specialized;

namespace FW.CommonFunction
{
	/// <summary>
	/// 记录文件日志，可以指定文件名称和文件保存路径 
    /// 调用方法：LogInFile objLogInFile =LogInFile.GetInstance (); 
	/// </summary>
	public class LogInFile
	{

        public enum LogFileNewFlagType
        {
            none,second,minute,hour,day,month
        }

        public LogInFile()
		{
            
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}
        static LogInFile myInstance;		
		UTF8Encoding utf8 = new UTF8Encoding();

		
		/// <summary>
		/// 获取SysRunLog的一个实例对象
		/// </summary>
		/// <returns></returns>
        public static LogInFile GetInstance()
		{
			if ( myInstance == null )
                myInstance = new LogInFile();
			return myInstance;
		}

		#region 属性

		/// <summary>
		/// 日志保存的文件名称
		/// </summary>
		private string logFileName="";

        /// <summary>
        /// 日志保存的文件名称，不指定，默认为log.txt
        /// </summary>
		public string LogFileName
		{
			get
			{
				return logFileName;
			}

			set
			{
				logFileName=value;
				if(logFileName.Trim ()=="")
				{
					
				}
				else
				{
					if(logFileName.IndexOf (".")==-1)
					{
						logFileName=logFileName.Trim ()+".txt";
					}
				}
			}
		}

		/// <summary>
		/// 日志保存的文件路径
		/// </summary>
		private string logFileDir="";

        /// <summary>
        /// 日志保存的文件路径，如果不支持，默认为当前路径
        /// </summary>
		public string LogFileDir
		{
			get
			{
				return logFileDir;
			}

			set
			{
				logFileDir=value;
				logFileDir=logFileDir.Trim ();
				if(logFileDir=="")
				{
					
				}
				else
				{
					if(logFileDir.Substring (logFileDir.Length -1)=="\\")
					{
						
					}
					else
					{
						logFileDir=logFileDir+"\\";
					}
				}
			}
		}

	    /// <summary>
        /// 是否没有生成新的日志文件
	    /// </summary>
        private LogFileNewFlagType logFileNewFlag = LogFileNewFlagType.day;

        /// <summary>
        /// 生成新日志文件的标记；none，second，minute，hour，day，month；默认是day
        /// </summary>
        public LogFileNewFlagType LogFileNewFlag
		{
			get
			{
                return logFileNewFlag;
			}

			set
			{
                logFileNewFlag = value;
			}
		}

		#endregion
		
		/// <summary>
		/// 打开或创建文件。
		/// </summary>
		private FileStream OpenSysLogFile()
		{	
			if(this.logFileName .Trim ()=="")
			{
                logFileName = "Log.txt";
         
			}
			if(this.logFileDir .Trim ()=="")
			{
				logFileDir=System.Threading.Thread.GetDomain().BaseDirectory+"Log\\";;
			}

			string fullFileName="";
            switch (logFileNewFlag)
            { 
                case  LogFileNewFlagType.second  :
                    fullFileName = logFileDir + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + "_" + logFileName;
                    break;
                case LogFileNewFlagType.minute:
                    fullFileName = logFileDir + DateTime.Now.ToString("yyyy-MM-dd HH-mm") + "_" + logFileName;
                    break;
                case LogFileNewFlagType.hour :
                    fullFileName = logFileDir + DateTime.Now.ToString("yyyy-MM-dd HH") + "_" + logFileName;
                    break;
                case LogFileNewFlagType.day :
                    fullFileName = logFileDir + DateTime.Now.ToString("yyyy-MM-dd") + "_" + logFileName;
                    break;
                case LogFileNewFlagType.month :
                    fullFileName = logFileDir + DateTime.Now.ToString("yyyy-MM") + "_" + logFileName;
                    break;
                case LogFileNewFlagType.none:
                    fullFileName = logFileDir + "_" + logFileName;
                    break;
                default :
                    fullFileName = logFileDir + DateTime.Now.ToString("yyyy-MM-dd") + "_" + logFileName;
                    break;
            }



			DirectoryInfo theFolder=new DirectoryInfo (logFileDir);
			if(!theFolder.Exists )
			{
				Directory.CreateDirectory (logFileDir);
			}

			FileStream 	sysLogFileStream=null;
			try
			{
				sysLogFileStream = new FileStream(fullFileName,FileMode.OpenOrCreate,FileAccess.ReadWrite,FileShare.ReadWrite);
			}
			catch(Exception er)
			{
				throw er;
			}
			return sysLogFileStream;
		}

		/// <summary>
		/// 写入日志信息
		/// </summary>
		/// <param name="pInfo"></param>
		public void WriteLog(string pInfo)
		{
			lock(this)
			{
				FileStream writer=this.OpenSysLogFile ();

				string content;
				byte[] buf;
				long len;
				try
				{
					content=DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"\r\n"+pInfo+"\r\n\r\n";
					buf = utf8.GetBytes(content);
					len = writer.Length;
					writer.Lock(0,len);
					writer.Seek(0,SeekOrigin.End);
					writer.Write(buf,0,buf.Length);
					writer.Unlock(0,len);
					writer.Flush();
					writer.Close ();
					writer=null;
				}
				catch(Exception er)
				{
					throw er;
				}
			}
		}
	}
}
