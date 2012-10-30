
using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Web.Configuration;
using System.Web;
using System.Web.UI;
using System.Xml;

namespace FW.CommonFunction
{
    /// <summary>
    /// 读取系统配置信息
    /// </summary>
    public class ConfigInfo
    {
 
        /// <summary>
        /// 通过传入WebConfig中的Key值返回Values值
        /// </summary>
        /// <param name="key">传入一个WebConfig中的Key值</param>
        /// <returns>返回的Key中的Values值</returns>
       public static string GetConfigValue(string key)
       {
           if (string.IsNullOrEmpty(key))
           {
               return "";
           }

           if (ConfigurationSettings.AppSettings[key] != null)
           {
               return ConfigurationSettings.AppSettings[key].ToString().Trim();
           }
           else
           {
               return "";
           }
       }

        /// <summary>
        /// 设置appSetting的值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="filePath">web.config文件路径</param>
        public static void SetConfiguration(Page page,string key, string value,string filePath)
        {

            Configuration configuration = null;                 //Configuration对象
            AppSettingsSection appSection = null;               //AppSection对象 
          
            configuration =WebConfigurationManager.OpenWebConfiguration(filePath);
        
            //取得AppSetting节
            appSection = configuration.AppSettings;

            //赋值并保存
            if (appSection.Settings[key] != null)
            {
                appSection.Settings[key].Value = value;
                configuration.Save();
            }
        }

        /// <summary>
        /// 获取appSetting的值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="filePath">web.config文件路径</param>
        public static string GetConfiguration(Page page, string key, string filePath)
        {

            Configuration configuration = null;                 //Configuration对象
            AppSettingsSection appSection = null;               //AppSection对象 

            configuration = WebConfigurationManager.OpenWebConfiguration(filePath);

            //取得AppSetting节
            appSection = configuration.AppSettings;

            //赋值并保存
            if (appSection.Settings[key] != null)
            {
                return appSection.Settings[key].Value;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取xml中指定节点的值
        /// </summary>
        /// <param name="page"></param>
        /// <param name="key"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetXmlValue(Page page, string key, string filePath)
        {
            string result = string.Empty;
            XmlTextReader reader = new XmlTextReader(HttpContext.Current.Server.MapPath(filePath)); 
            XmlDocument doc = new XmlDocument();
            doc.Load(reader);//
            reader.Close();

            XmlNode oNode = doc.DocumentElement;
            XmlNodeList nodeList = oNode.ChildNodes;
            foreach (XmlNode n in nodeList)
            {
                if (n.Name.Equals("appSettings"))
                {
                    foreach (XmlNode n1 in n.ChildNodes)
                    {
                        if (n1.Attributes != null)
                        {
                            if (n1.Attributes["key"].Value == key)
                            {
                                result = n1.Attributes["value"].Value;
                            }
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        ///设置xml中指定节点的值
        /// </summary>
        /// <param name="page"></param>
        /// <param name="key"></param>
        /// <param name="filePath"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static void SetXmlValue(Page page, string key,string value, string filePath)
        {
            string path = HttpContext.Current.Server.MapPath(filePath);
            XmlTextReader reader = new XmlTextReader(path); 
            XmlDocument doc = new XmlDocument();
            doc.Load(reader);
            reader.Close();

            XmlNode oNode = doc.DocumentElement;
            XmlNodeList nodeList = oNode.ChildNodes;
            foreach (XmlNode n in nodeList)
            {
                if (n.Name.Equals("appSettings"))
                {
                    foreach (XmlNode n1 in n.ChildNodes)
                    {
                        if (n1.Attributes != null)
                        {
                            XmlElement xe = n1 as XmlElement;
                            if (xe != null)
                            {
                                if (xe.GetAttribute("key") == key)
                                {
                                    xe.SetAttribute("value", value);
                                }
                            }
                        }
                    }
                }
               
            }
            doc.Save(path);
        }
    }
}
