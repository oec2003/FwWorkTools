using System;
using System.Collections.Generic;

using System.Text;
using System.Configuration;


namespace FW.CommonFunction
{
    /// <summary>
    /// 
    /// </summary>
	public class Config
	{
		/// <summary>
		/// 数据库连接字符串 - ConStr
		/// </summary>
		public static string DBConStr()
		{
			string tempStr = string.Empty;
            try
            {
                tempStr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
            }
            catch (Exception e)
            {
                ErrorHandler.ExceptionHandlerForApp("获取数据库连接字符串错误", e.ToString());
                throw e;
			}
			return tempStr;
		}

		/// <summary>
		/// 系统运行日志保存路径 - LogPath
		/// </summary>
		public static string LogPath()
		{
			string tempStr = "";

			try
			{
				tempStr = ConfigurationManager.AppSettings["LogPath"].ToString();
				if (!tempStr.EndsWith("\\"))
				{
                    tempStr = tempStr + "\\";
				}
			}
			catch (Exception e)
			{
				throw e;
			}
			return tempStr;
		}

		/// <summary>
		/// 系统日志文件名，不要后缀名
		/// </summary>
		/// <returns></returns>
		public static string LogFileName()
		{
			string tempStr = string.Empty;
			try
			{
				tempStr = ConfigurationManager.AppSettings["LogFileName"].ToString();
			}
			catch (Exception e)
			{
				throw e;
			}
			return tempStr;
		}
		/// <summary>
		/// 系统日志文件重命名的周期， none,second,minute,hour,day,month
		/// </summary>
		/// <returns></returns>
		public static string LogFileNewFlag()
		{
			string tempStr = string.Empty;

			try
			{
				tempStr = ConfigurationManager.AppSettings["LogFileNewFlag"].ToString();
			}
			catch (Exception e)
			{
				throw e;
			}
			return tempStr;
		}
		/// <summary>
		/// 菜单XML文件保存路径
		/// </summary>
		/// <returns></returns>
		public static string MenuXMLPath()
		{
			string tempStr = string.Empty;

			try
			{
				tempStr = ConfigurationManager.AppSettings["MenuXMLPath"].ToString();
				if (!tempStr.EndsWith("\\"))
				{
                    tempStr = tempStr + "\\";
				}
			}
			catch
			{
				tempStr = @"c:\MenuXMLPath\";
			}
			return tempStr;
		}
		/// <summary>
		/// 是否校验登录情况
		/// </summary>
		/// <returns></returns>
		public static bool CheckLogin()
		{
			bool temp = true;
			try
			{
				if (ConfigurationManager.AppSettings["CheckLogin"].ToString() == "N")
					temp = false;
				else
					temp = true;
			}
			catch
			{
				temp = true;
			}
			return temp;
		}

		/// <summary>
		/// 获取系统的Cookie过期时间，单位：分钟，默认30
		/// </summary>
		/// <returns></returns>
		public static int CookieExpiresMinute()
		{
			int temp = 30;

			try
			{
                temp = int.Parse(ConfigurationManager.AppSettings["CookieExpiresMinute"].ToString());
			}
			catch
			{
				temp = 30;
			}
			return temp;
		}

		/// <summary>
		/// 站点虚拟路径
		/// </summary>
		/// <returns></returns>
		public static string VirtualPath()
		{
			string temp = string.Empty;

			try
			{
				temp = ConfigurationManager.AppSettings["VirtualPath"].ToString();
			}
			catch
			{
				temp = string.Empty;
			}
			return temp;
		}

		/// <summary>
		/// 附件最大值
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
        public static int AttachmentMaxLength()
		{
			int temp = 100;

			try
			{
                temp = int.Parse(ConfigurationManager.AppSettings["AttachmentMaxLength"].ToString());

			}
			catch
			{
				temp = 100;
			}
			return temp;
		}

		/// <summary>
		/// 附件类型
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
        public static string AttachmentType()
		{
			string temp = string.Empty;
			try
			{
                temp = ConfigurationManager.AppSettings["AttachmentType"].ToString();
			}
			catch
			{
                temp = string.Empty;
			}
			return temp;
		}


        /// <summary>
        /// 图片文件存储根目录
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string FolderPhotoRoot()
        {
            string temp = string.Empty;

            try
            {
                temp = ConfigurationManager.AppSettings["FolderPhotoRoot"].ToString();
            }
            catch
            {
            }
            return temp;
        }

        /// <summary>
        /// 图片文件存储根目录
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string DefaultAlbumIconFile()
        {
            string temp = "";

            try
            {
                temp = ConfigurationManager.AppSettings["DefaultAlbumIconFile"].ToString();
            }
            catch
            {
            }
            return temp;
        }

        /// <summary>
        /// 图片文件存储根目录
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string PhotoTypes()
        {
            string temp = "jpeg, jpg";

            try
            {
                temp = ConfigurationManager.AppSettings["PhotoTypes"].ToString();
            }
            catch
            {
            }
            return temp;
        }




		/// <summary>
		/// 缓存AppSettings对应节点的Value
		/// </summary>
		private static Dictionary<string, string> appSettingValues = new Dictionary<string, string>();

		/// <summary>
		/// 获取配置文件的AppSettings的值
		/// </summary>
		/// <param name="key">键</param>
		/// <returns>值</returns>
		public static string GetAppSettingsValue(string key)
		{
			if (appSettingValues.ContainsKey(key))
			{
				return appSettingValues[key];
			}
			else
			{
				string value = ConfigurationManager.AppSettings[key];
				appSettingValues[key] = value;
				return value;
			}
		}
	}
}
