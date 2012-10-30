
using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Collections.Specialized;

namespace FW.CommonFunction
{
	/// <summary>
	/// ��¼�ļ���־������ָ���ļ����ƺ��ļ�����·�� 
    /// ���÷�����LogInFile objLogInFile =LogInFile.GetInstance (); 
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
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}
        static LogInFile myInstance;		
		UTF8Encoding utf8 = new UTF8Encoding();

		
		/// <summary>
		/// ��ȡSysRunLog��һ��ʵ������
		/// </summary>
		/// <returns></returns>
        public static LogInFile GetInstance()
		{
			if ( myInstance == null )
                myInstance = new LogInFile();
			return myInstance;
		}

		#region ����

		/// <summary>
		/// ��־������ļ�����
		/// </summary>
		private string logFileName="";

        /// <summary>
        /// ��־������ļ����ƣ���ָ����Ĭ��Ϊlog.txt
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
		/// ��־������ļ�·��
		/// </summary>
		private string logFileDir="";

        /// <summary>
        /// ��־������ļ�·���������֧�֣�Ĭ��Ϊ��ǰ·��
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
        /// �Ƿ�û�������µ���־�ļ�
	    /// </summary>
        private LogFileNewFlagType logFileNewFlag = LogFileNewFlagType.day;

        /// <summary>
        /// ��������־�ļ��ı�ǣ�none��second��minute��hour��day��month��Ĭ����day
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
		/// �򿪻򴴽��ļ���
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
		/// д����־��Ϣ
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
