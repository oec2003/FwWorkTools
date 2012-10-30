/****************************************************************
 * 作者：冯威添加
 * 日期：2010.11.09 
 * 基类： 
 * 功能：Cmwin系统文件上传
 * 修改：
****************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Security.Cryptography;
using System.Diagnostics;
using System.Drawing;
using System.Web;

namespace FW.CommonFunction
{
    /// <summary>
    /// 上传附件类
    /// </summary>
    public class UploadFile
    {
        //定义上传文件大小类变量
        private int _upFileSize = 0;

        /// <summary>
        /// 上传文件类型
        /// </summary>
        public enum UpFileType
        {
            /// <summary>
            /// 通用，万能，不限制任何格式
            /// </summary>
            Common,

            /// <summary>
            /// 图片格式
            /// </summary>
            Picture, 

            /// <summary>
            /// 限制格式（pdf,doc,jpg,png,gif,zip,rar） update by 冯岩　2007-06-26 增加几种类型　.swf及.dwf,.vob .mpeg .wmv
            /// </summary>
            Limited 
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public UploadFile()
        {
        }
        //获取上传文件大小属性
        /// <summary>
        /// 
        /// </summary>
        public int UpFileSize
        {
            get
            {
                return _upFileSize;
            }
        }



        /// <summary>
        /// 配合UpLoadImage方法 通过原始图片路径获取缩略图片路径
        /// </summary>
        /// <param name="bigPictureUrl">原始图路径</param>
        /// <returns>若存在，返回路径，否则返回空！</returns>
        public static string GetSmallPictureUrl(string bigPictureUrl)
        {
            //拼接缩略图
            string temp1 = bigPictureUrl.Substring(0, bigPictureUrl.LastIndexOf("/"));//../../CBPResource/EnterPrise/7/A2777/File/info
            string temp2 = temp1.Substring(0, temp1.LastIndexOf("/") + 1);//../../CBPResource/EnterPrise/7/A2777/File/
            string smallPic = temp2 + "smallPic/" + temp1.Substring(temp2.Length);//../../CBPResource/EnterPrise/7/A2777/File/smallPic/info			
            smallPic += "/" + bigPictureUrl.Substring(bigPictureUrl.LastIndexOf("/") + 1);
            if (File.Exists(System.Web.HttpContext.Current.Server.MapPath(smallPic)))
                return smallPic;//存在缩略图片，返回路径
            else
                return "";
        }
        /// <summary>
        /// 说明：图片上传及等比例缩放
        /// 根据给定的UI层显示的宽高对上传图片进行等比例缩放！
        /// 注意：在上传完成后，会在给定path路径下建立smallPic文件夹用于存放缩略图片
        /// 缩略图名字为fileName与原图相同
        /// 如20070725100632.jpg对应的缩略图路径为 smallPic/smallDirName/20070725100632.jpg
        /// </summary>
        /// <param name="upFile">上传控件</param>
        /// <param name="path">上传文件存放物理路径。</param>
        /// <param name="fileName">上传成功后返回的文件名，用于存入持久设备</param>
        /// <param name="width">UI层显示时的宽度</param>
        /// <param name="height">UI层显示时的高度</param>
        /// <param name="smallDirName"></param>
        /// <returns></returns>
        public string UpLoadImage(HtmlInputFile upFile, string path, out string fileName, int width, int height, string smallDirName)
        {
            int fileSize = 0; //定义文件大小
            string type = upFile.Value;
            type = type.Substring(type.LastIndexOf(".") + 1);

            type = type.ToLower();

            fileName = null;
            try
            {
                if (upFile.PostedFile.FileName.Trim() == "") //上传文件名为空时
                {
                    return "上传文件名为空";
                }
                if (upFile.PostedFile.FileName.Trim() != "")  //上传文件名不为空
                {
                    fileSize = upFile.PostedFile.ContentLength; //文件长度

                    if (fileSize == 0) //不是正确的文件
                    {
                        return "上传文件不是正确的文件";
                    }

                    //图片类型
                    if (type != "jpg" && type != "jpeg" && type != "png" && type != "gif" && type != "bmp")
                    {
                        return "上传文件格式不对";
                    }

                    if (fileSize > 5242880) //文件大小不能超过5M
                    {
                        return "上传文件大小不能超过5M";
                    }

                    fileName = upFile.PostedFile.FileName;

                    Image image = Image.FromStream(upFile.PostedFile.InputStream);
                    //将客户端文件保存在流中　解决　NTFS格式访问权限问题　

                    //创建位图
                    Bitmap bmp;

                    if (image.Width > width || image.Height > height)
                    {
                        //进行缩放
                        double flag = Convert.ToDouble(image.Height) / Convert.ToDouble(image.Width);//高与宽的比例　
                        double evel = Convert.ToDouble(height) / Convert.ToDouble(width);
                        if (flag == evel)
                            bmp = new Bitmap(image, width, height); //等比例缩放到　宽，高
                        else if (flag > evel)
                            //高过于，以高缩放
                            bmp = new Bitmap(image, Convert.ToInt32(Convert.ToDouble(height) / flag), height);
                        else
                            //过宽　以宽缩放
                            bmp = new Bitmap(image, width, Convert.ToInt32(Convert.ToDouble(width) * flag));
                    }
                    else
                    {
                        //图片本身比较小！需求暂不处理放大！
                        bmp = new Bitmap(image, image.Width, image.Height);
                    }


                    string extendName = fileName.Remove(0, fileName.LastIndexOf('.'));//获取扩展名

                    string nowTime = DateTime.Now.ToString("yyyyMMddHHmmssmm");

                    fileName = nowTime + extendName;//变量fileName会传出，生成新的数据库存储文件名

                    string smallFileName = nowTime + extendName;//缩略图片名字


                    //保存缩略图片
                    string smallImagePath = path + "/smallPic/";
                    if(!string.IsNullOrEmpty(smallDirName))
                    {
                        smallImagePath = path + "/smallPic/" + smallDirName + "/";
                    }

                    DirectoryInfo dir = new DirectoryInfo(smallImagePath);
                    if (!dir.Exists)
                    {
                        dir.Create();
                    }
                    bmp.Save(smallImagePath + smallFileName);


                    //保存原始图片
                   // dir = new DirectoryInfo(path + smallDirName + "/");
                    dir = new DirectoryInfo(path);
                    if (!dir.Exists)
                    {
                        dir.Create();
                    }
                    //upFile.PostedFile.SaveAs(path + smallDirName + "/" + fileName);
                    upFile.PostedFile.SaveAs(path +"/"+ fileName);
                }
                _upFileSize = fileSize;

                return "上传文件成功";
            }
            catch
            {
                return "上传失败";
            }
        }



        /// <summary>
        /// 传图片等比例缩放　用于企业会员上传LOGO　缩放到　高90 宽120　比例　0.75　
        /// BY 冯岩　2007-06-29
        /// </summary>
        /// <param name="upFile"></param>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string UploadLogo(HtmlInputFile upFile, string path, out string fileName)
        {
            int fileSize = 0; //定义文件大小
            string type = upFile.Value;
            type = type.Substring(type.LastIndexOf(".") + 1);

            type = type.ToLower();

            fileName = null;
            try
            {
                if (upFile.PostedFile.FileName.Trim() == "") //上传文件名为空时
                {
                    return "上传文件名为空";
                }
                if (upFile.PostedFile.FileName.Trim() != "")  //上传文件名不为空
                {
                    fileSize = upFile.PostedFile.ContentLength; //文件长度

                    if (fileSize == 0) //不是正确的文件
                    {
                        return "上传文件不是正确的文件";
                    }

                    //图片类型
                    if (type != "jpg" && type != "jpeg" && type != "png" && type != "gif" && type != "bmp")
                    {
                        return "上传文件格式不对";
                    }

                    if (fileSize > 5242880) //文件大小不能超过5M
                    {
                        return "上传文件大小不能超过5M";
                    }

                    fileName = upFile.PostedFile.FileName;

                    Image image = Image.FromStream(upFile.PostedFile.InputStream);
                    //将客户端文件保存在流中　解决　NTFS格式访问权限问题　

                    //创建位图
                    Bitmap bmp;

                    if (image.Width > 120 || image.Height > 90)
                    {
                        //进行缩放
                        double flag = Convert.ToDouble(image.Height) / Convert.ToDouble(image.Width);//高与宽的比例　

                        if (flag == 0.75)
                            bmp = new Bitmap(image, 120, 90); //等比例缩放到　宽，高
                        else if (flag > 0.75)
                            //高过于，以高缩放
                            bmp = new Bitmap(image, Convert.ToInt32(90 / flag), 90);
                        else
                            //过宽　以宽缩放
                            bmp = new Bitmap(image, 120, Convert.ToInt32(120 * flag));
                    }
                    else
                    {
                        //图片本身比较小！需求暂不处理放大！
                        bmp = new Bitmap(image, image.Width, image.Height);
                    }


                    string extendName = fileName.Remove(0, fileName.LastIndexOf('.'));//获取扩展名


                    string nowTime = DateTime.Now.ToString("yyyyMMddHHmmssmm");

                    fileName = nowTime + extendName;//变量fileName会传出，生成新的数据库存储文件名
                    //保存缩略图片
                    string smallImagePath = path.Substring(0, path.LastIndexOf("\\") - 3) + "smallPic\\LOGO\\";
                    DirectoryInfo dir = new DirectoryInfo(smallImagePath);
                    if (!dir.Exists)
                    {
                        dir.Create();
                    }
                    bmp.Save(smallImagePath + fileName);


                    //保存原始图片
                    dir = new DirectoryInfo(path);
                    if (!dir.Exists)
                    {
                        dir.Create();
                    }
                    upFile.PostedFile.SaveAs(path + fileName);
                }
                _upFileSize = fileSize;

                return "上传文件成功";
            }
            catch (Exception ex)
            {
                string m = ex.ToString();
                return "上传失败";
            }
        }

        /// <summary>
        /// 上传文件方法
        /// </summary>
        /// <param name="upFile">上传文件流</param>
        /// <param name="upFileType">上传文件类型方式</param>
        /// <param name="path">上传文件路径</param>
        /// <param name="fileName">上传后文件名称，输出参数</param>
        /// <returns>返回上传结果</returns>
        public string UpLoad(HtmlInputFile upFile, UpFileType upFileType, string path, out string fileName)
        {
            int fileSize = 0; //定义文件大小
            //			string strFileType; //文件类型

            string type = upFile.Value;
            type = type.Substring(type.LastIndexOf(".") + 1);
            type = type.ToLower();
            
            fileName = null;
            try
            {
                if (upFile.PostedFile.FileName.Trim() == "") //上传文件名为空时
                {
                    return "上传文件名为空";
                }
                if (upFile.PostedFile.FileName.Trim() != "")  //上传文件名不为空
                {
                    fileSize = upFile.PostedFile.ContentLength; //文件长度
                    //					strFileType = upFile.PostedFile.ContentType;//文件类型
                    if (fileSize == 0) //不是正确的文件
                    {
                        return "上传文件不是正确的文件";
                    }
                    switch (upFileType)
                    {
                        case UpFileType.Picture://图片类型
                            if (
                                type != "jpg"
                                && type != "png"
                                && type != "gif"
                                && type != "bmp"
                                )
                            {
                                return "上传文件格式不对";
                            }
                            break;
                        case UpFileType.Limited://限制类型
                            if (type != "pdf"
                                && type != "doc"
                                && type != "jpg"
                                && type != "png"
                                && type != "gif"
                                && type != "zip"
                                && type != "rar"
                                && type != "dwf"
                                && type != "swf"
                                && type != "vob"
                                && type != "wmv")
                            /*
                        if (strFileType!="application/pdf" 
                            && strFileType!="application/msword"
                            && strFileType!="image/gif"&&strFileType!="image/x-png"
                            &&strFileType!="image/pjpeg"
                            &&strFileType!="application/x-zip-compressed"
                            &&strFileType!="application/x-shockwave-flash"
                            &&strFileType!="application/octet-stream"
                            &&strFileType!="video/mpeg"
                            &&strFileType!="video/x-ms-wmv"
                            &&!(strFileType=="application/octet-stream"								
                            &&upFile.PostedFile.FileName.Trim().Substring(upFile.PostedFile.FileName.Trim().LastIndexOf(".")+1).ToLower()=="rar"))*/
                            {
                                return "上传文件格式不对!";
                            }
                            break;
                        case UpFileType.Common://万能类型
                            break;
                    }
                    //if (fileSize > 512000) //文件大小不能超过500KB
                    //{
                    //    return "上传文件大小不能超过500kb";
                    //}

                    fileName = upFile.PostedFile.FileName;
                    string name = fileName.Substring(fileName.LastIndexOf('\\') + 1,
                                                     fileName.LastIndexOf('.') - (fileName.LastIndexOf('\\') + 1));
                    string extendName = fileName.Remove(0, fileName.LastIndexOf('.'));//获取扩展名
                    fileName = name + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + extendName;//生成新的数据库存储文件名
            
                    DirectoryInfo dir = new DirectoryInfo(path);
                    if (!dir.Exists) dir.Create();
                    upFile.PostedFile.SaveAs(path + "\\" + fileName);

                }
                _upFileSize = fileSize;
                return "上传文件成功";
            }
            catch (Exception)
            {
                return "上传失败";
            }
        }



        /// <summary>
        /// 上传文件方法
        /// Add by 冯威
        /// 添加时fileNameEidt 为空 修改时传入原图片的名称
        /// </summary>
        /// <param name="upFile">上传文件流</param>
        /// <param name="upFileType">上传文件类型方式</param>
        /// <param name="path">上传文件路径</param>
        /// <param name="fileName">上传后文件名称，输出参数</param>
        /// <param name="fileNameEdit">修改时的文件名 该参数不为空表示是修改状态 不重新生成文件名</param>
        /// <returns>返回上传结果</returns>
        public string UpLoad(HtmlInputFile upFile, UpFileType upFileType, string path, out string fileName, string fileNameEdit)
        {
            int fileSize = 0; //定义文件大小
            //			string strFileType; //文件类型

            string type = upFile.Value;
            type = type.Substring(type.LastIndexOf(".") + 1).ToLower();
            fileName = null;
            try
            {
                if (upFile.PostedFile.FileName.Trim() == "") //上传文件名为空时
                {
                    return "上传文件名为空";
                }
                if (upFile.PostedFile.FileName.Trim() != "")  //上传文件名不为空
                {
                    fileSize = upFile.PostedFile.ContentLength; //文件长度
                    //					strFileType = upFile.PostedFile.ContentType;//文件类型
                    if (fileSize == 0) //不是正确的文件
                    {
                        return "上传文件不是正确的文件";
                    }
                    switch (upFileType)
                    {
                        case UpFileType.Picture://图片类型
                            if (
                                type != "jpg"
                                && type != "png"
                                && type != "gif"
                                && type != "bmp"
                                )
                            {
                                return "上传文件格式不对";
                            }
                            break;
                        case UpFileType.Limited://限制类型
                            if (type != "pdf"
                                && type != "doc"
                                && type != "jpg"
                                && type != "png"
                                && type != "gif"
                                && type != "zip"
                                && type != "rar"
                                && type != "dwf"
                                && type != "swf"
                                && type != "vob"
                                && type != "wmv")
                            /*
                        if (strFileType!="application/pdf" 
                            && strFileType!="application/msword"
                            && strFileType!="image/gif"&&strFileType!="image/x-png"
                            &&strFileType!="image/pjpeg"
                            &&strFileType!="application/x-zip-compressed"
                            &&strFileType!="application/x-shockwave-flash"
                            &&strFileType!="application/octet-stream"
                            &&strFileType!="video/mpeg"
                            &&strFileType!="video/x-ms-wmv"
                            &&!(strFileType=="application/octet-stream"								
                            &&upFile.PostedFile.FileName.Trim().Substring(upFile.PostedFile.FileName.Trim().LastIndexOf(".")+1).ToLower()=="rar"))*/
                            {
                                return "上传文件格式不对!";
                            }
                            break;
                        case UpFileType.Common://万能类型
                            break;
                    }
                    //if (fileSize > 512000) //文件大小不能超过500KB
                    //{
                    //    return "上传文件大小不能超过500kb";
                    //}
                    
                    //如果fileNameEdit不为空 为修改时 用原图片的名称作为新图片的名称
                    //if (fileNameEdit.Trim().Length==0)
                    //{
                    fileName = upFile.PostedFile.FileName;
                    string name = fileName.Substring(fileName.LastIndexOf('\\') + 1,
                                                     fileName.LastIndexOf('.') - (fileName.LastIndexOf('\\') + 1));
                    string extendName = fileName.Remove(0, fileName.LastIndexOf('.'));//获取扩展名
                    fileName =name+"_"+ DateTime.Now.ToString("yyyyMMddHHmmss") + extendName;//生成新的数据库存储文件名
                    //}
                    //else
                    //{
                    //    fileName = fileNameEdit;
                    //}

                    DirectoryInfo dir = new DirectoryInfo(path);
                    if (!dir.Exists) dir.Create();
                    upFile.PostedFile.SaveAs(path + "\\" + fileName);

                }
                _upFileSize = fileSize;
                return "上传文件成功";
            }
            catch (Exception)
            {
                return "上传失败";
            }
        }



        /// <summary>
        /// 复制上传文件
        /// </summary>
        /// <param name="strSrcFileName">原文件名</param>
        /// <param name="strPostfix">给原文件名加个后缀</param>
        /// <param name="strTarFileName">返回的文件名</param>
        /// <returns>ture/false</returns>
        static public bool CopyFile(string strSrcFileName, string strPostfix, out string strTarFileName)
        {
            try
            {
                FileInfo file = new FileInfo(strSrcFileName);
                strTarFileName = file.DirectoryName + "\\" + file.Name + strPostfix + "." + file.Extension;
                file.CopyTo(strTarFileName, false);
                return true;
            }
            catch (System.Exception)
            {
                strTarFileName = "";
                return false;
            }
        }
        /// <summary>
        /// 删除上传文件
        /// </summary>
        /// <param name="Page">Page页引用</param>
        /// <param name="strFileName">文件名</param>
        /// <returns>ture/false</returns>
        static public bool DeleteFile(System.Web.UI.Page Page, string strFileName)
        {
            try
            {
                if (strFileName.Trim().Equals(""))
                {
                    return true;
                }
                if (!File.Exists(Page.MapPath(strFileName)))
                    return true;

                FileInfo file = new FileInfo(Page.MapPath(strFileName));
                file.Delete();
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }

        }

        ///<summary>
        /// 删除文件
        /// </summary>
        /// <param name="strFilePath">文件的绝对路径</param>
        /// <returns>ture/false</returns>
        static public bool DeleteFile(string strFilePath)
        {
            try
            {
                if (strFilePath.Trim().Equals(""))
                {
                    return true;
                }
                if (!File.Exists(strFilePath))
                    return true;

                FileInfo file = new FileInfo(strFilePath);
                file.Delete();
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }

        }

        /// <summary>
        /// 删除目录
        /// </summary>
        /// <param name="Page">Page页引用</param>
        /// <param name="strPath"></param>
        /// <returns>ture/false</returns>
        static public bool DeleteDir(System.Web.UI.Page Page, string strPath)
        {
            try
            {
                // Determine whether the directory exists.
                if (Directory.Exists(Page.MapPath(strPath)))
                {
                    Directory.Delete(Page.MapPath(strPath), true);
                }

                return true;

            }
            catch (System.Exception)
            {
                return false;
            }
            finally { }
        }

        public static bool DownloadFile(string FileUrl, string strFileName)
        {
            byte[] buffer = new Byte[10000];
            long dataToRead = 1;
            int length;
            System.Uri myURi = new System.Uri(FileUrl);
            WebRequest req = WebRequest.Create(myURi);
            req.Method = "GET";
            req.Timeout = System.Threading.Timeout.Infinite;
            WebResponse res = req.GetResponse();
            Stream inStream = res.GetResponseStream();
            HttpContext.Current.Response.Clear();

            HttpContext.Current.Response.ClearContent();

            HttpContext.Current.Response.ClearHeaders();

            HttpContext.Current.Response.Charset = "GB2312";
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(strFileName));
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            HttpContext.Current.Response.ContentType = "application/octet-stream";
            //int dataToRead = 1;
            while (dataToRead > 0)
            {
                // Verify that the client is connected.
                if (HttpContext.Current.Response.IsClientConnected)
                {
                    // Read the data in buffer.
                    length = inStream.Read(buffer, 0, 10000);

                    // Write the data to the current output stream.
                    HttpContext.Current.Response.OutputStream.Write(buffer, 0, length);

                    // Flush the data to the HTML output.
                    HttpContext.Current.Response.Flush();

                    buffer = new Byte[10000];
                    dataToRead = dataToRead - length;
                }
                else
                {
                    //prevent infinite loop if user disconnects
                    dataToRead = -1;
                }
            }
            buffer = null;
            myURi = null;
            req = null;
            res = null;
            if (inStream != null)
                inStream.Close();
            HttpContext.Current.Response.End();
            return true;
        }
    }
}
