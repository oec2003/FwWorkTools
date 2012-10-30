using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Web;
using System.Drawing;
using System.Collections;
using System.Text.RegularExpressions;

namespace FW.CommonFunction
{
    public class ExcelHelper
    {

        #region 操作說明

        //声明ExcelHelper
        //LogisticsManage.ExcelHelper excel = new LogisticsManage.ExcelHelper();

        //指定當前Page对象
        //excel.Page = this.Page;

        //指定Excel数据源(DataTable)
        //excel.DataSource = ExcelTable;

        //指定Excel文件名称(全名)
        //excel.FileName = "MyExcel.xls";

        //调用导出方法
        //excel.OutExcel();

        //-------------------------------------------------------------
        //如何修改列标题
        //-------------------------------------------------------------

        //指定列标题为自定义列
        //excel.ThisColumns = true;

        //设置列标题列表(备注:按顺序修改列标题)
        //excel.ColumnsName = new string[] { "", "", "服务编码", "问题类型", "问题描述", "发问人", "责任单位", "恢复状态", "询问日期" };

        //设置列标题背景颜色
        //excel.HeaderBackColor = System.Drawing.Color.DarkBlue;

        //设置字体颜色
        //excel.HeaderTextColor = System.Drawing.Color.White;

        #endregion

        #region Excel私有属性

        private DataTable m_DataSource;

        /// <summary>
        /// Excel数据源
        /// </summary>
        public DataTable DataSource
        {
            get { return this.m_DataSource; }
            set { this.m_DataSource = value; }
        }
 
        private string m_FileName;
        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName
        {
            get { return this.m_FileName; }
            set { this.m_FileName = value; }
        }

        private System.Web.UI.Page m_Page;
        /// <summary>
        /// 页面Page对象
        /// </summary>
        public System.Web.UI.Page Page
        {
            set { this.m_Page = value; }
            get { return this.m_Page; }
        }

        private bool m_thisColumus = false;
        /// <summary>
        /// 是否按指定列名输出
        /// </summary>
        public bool ThisColumns
        {
            get { return this.m_thisColumus; }
            set { this.m_thisColumus = value; }
        }

        private System.Drawing.Color m_HeaderTextColor = System.Drawing.Color.White;
        /// <summary>
        /// 列标题文本颜色
        /// </summary>
        public System.Drawing.Color HeaderTextColor
        {
            set { this.m_HeaderTextColor = value; }
            get { return this.m_HeaderTextColor; }
        }

        private System.Drawing.Color m_HeaderBackColor = System.Drawing.Color.Gray;
        /// <summary>
        /// 列标题背景色
        /// </summary>
        public System.Drawing.Color HeaderBackColor
        {
            set { this.m_HeaderBackColor = value; }
            get { return this.m_HeaderBackColor; }
        }

        private string[] m_ColumnsName;
        /// <summary>
        /// 标题列表
        /// </summary>
        public string[] ColumnsName
        {
            get { return m_ColumnsName; }
            set { m_ColumnsName = value; }
        }

        private System.Text.Encoding m_Encoding = System.Text.Encoding.GetEncoding("utf-8");
        /// <summary>
        /// 流字符集
        /// </summary>
        public Encoding EEncoding
        {
            get { return m_Encoding; }
            set { m_Encoding = value; }
        }

        #endregion

        #region Excel公共方法

        /// <summary>
        /// 導出Excel公共方法
        /// </summary>
        public void OutExcel()
        {
            //DataGrid对象
            System.Web.UI.WebControls.DataGrid dgExport = null;
            //Http输出流对象
            System.Web.HttpResponse httpResponse = Page.Response;
            //設置輸出文件名稱
            httpResponse.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(FileName, System.Text.Encoding.UTF8));
            httpResponse.ContentEncoding = EEncoding;
            httpResponse.ContentType = "application/ms-excel";
            System.IO.StringWriter tw = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
            dgExport = new System.Web.UI.WebControls.DataGrid();

            //是否按指定列名輸出
            if (ThisColumns)
            {
                DataTable tempTable = new DataTable();
                tempTable.Columns.Clear();
                tempTable.Rows.Clear();

                for (int i = 0; i < DataSource.Columns.Count; i++)
                {
                    try
                    {
                        tempTable.Columns.Add(ColumnsName[i]);
                    }
                    catch
                    {
                        tempTable.Columns.Add("TempName" + i);
                    }
                }


                for (int i = 0; i < DataSource.Rows.Count; i++)
                {
                    DataRow dr = tempTable.NewRow();
                    for (int j = 0; j < DataSource.Columns.Count; j++)
                    {

                        string tempDate = DataSource.Rows[i][j].ToString();
                        dr[j] = tempDate;

                    }
                    tempTable.Rows.Add(dr);
                }

                dgExport.DataSource = tempTable.DefaultView;
            }
            else//按源输出
            {
                dgExport.DataSource = DataSource.DefaultView;
            }
            dgExport.AllowPaging = false;
            dgExport.HeaderStyle.ForeColor = HeaderTextColor;
            dgExport.HeaderStyle.BackColor = HeaderBackColor;
            dgExport.DataBind();
            // 返回客户端 
            dgExport.RenderControl(hw);

            //httpResponse.Write(tw.ToString());
            //httpResponse.End();

            string filePath = Page.Server.MapPath("..") + "\\" + FileName;

            System.IO.StreamWriter sw = System.IO.File.CreateText(filePath);
            //修改栏位格式

            string s = FormatDateHTML(tw.ToString());
            sw.Write(s);
            sw.Close();

            DownFile(httpResponse, FileName, filePath);
            httpResponse.End();

        }

        /// <summary>
        /// 導出CSV公共方法
        /// </summary>
        public void OutCSV()
        {

            System.Web.HttpResponse httpResponse = Page.Response;

            System.Text.StringBuilder strData = new StringBuilder();

            for (int i = 0; i < DataSource.Columns.Count; i++)
            {
                try
                {
                    strData.Append(ColumnsName[i]);
                    strData.Append(",");
                }
                catch
                {
                    strData.Append("TempName" + i);
                }
            }

            strData.Append("\n");


            for (int i = 0; i < DataSource.Rows.Count; i++)
            {

                for (int j = 0; j < DataSource.Columns.Count; j++)
                {

                    strData.Append(DataSource.Rows[i][j].ToString());
                    strData.Append(",");


                }
                strData.Append("\n");
            }



            //string temp = string.Format("attachment;filename={0}", "ExportData.csv");
            //httpResponse.ClearHeaders();
            //httpResponse.AppendHeader("Content-disposition", temp);
            //httpResponse.Write(strData);
            //httpResponse.End(); 

            // 设置编码和附件格式
            httpResponse.ContentType = "application/ms-excel";
            httpResponse.ContentEncoding = System.Text.Encoding.GetEncoding("BIG5");
            httpResponse.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(FileName, Encoding.GetEncoding("BIG5")).ToString());
            httpResponse.Charset = "";

            httpResponse.Write(strData);
            httpResponse.End();
        }

        private bool DownFile(System.Web.HttpResponse Response, string fileName, string fullPath)
        {
            System.IO.FileStream fs = null;
            try
            {
                Response.ContentType = "application/octet-stream";

                Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8) + ";charset=utf-8");
                fs = System.IO.File.OpenRead(fullPath);
                long fLen = fs.Length;
                int size = 102400;//每100K同时下载数据 
                byte[] readData = new byte[size];//指定缓冲区的大小 
                if (size > fLen) size = Convert.ToInt32(fLen);
                long fPos = 0;
                bool isEnd = false;
                while (!isEnd)
                {
                    if ((fPos + size) > fLen)
                    {
                        size = Convert.ToInt32(fLen - fPos);
                        readData = new byte[size];
                        isEnd = true;
                    }
                    fs.Read(readData, 0, size);//读入一个压缩块 

                    Response.BinaryWrite(readData);
                    fPos += size;
                }
                fs.Close();
                System.IO.File.Delete(fullPath);
                return true;
            }
            catch
            {
                fs.Close();
                System.IO.File.Delete(fullPath);
                return false;
            }
        }
        #endregion


        #region Excel格式轉換私有方法 Jieen 2008.6.5 添加
        /// <summary>
        /// 修改栏位格式
        /// </summary>
        /// <param name="_html">文本内容</param>
        /// <returns></returns>
        public static string FormatDateHTML(string _html)
        {
            //数字转换
            string ReplaceAll = "<td>(?<key>[0]+\\w)</td>";
            Regex r1 = new Regex(ReplaceAll, RegexOptions.None);
            string[] i1 = System.Text.RegularExpressions.Regex.Split(_html, ReplaceAll);
            Match mc1 = r1.Match(_html);
            string s1 = mc1.Groups[1].Value;
            s1 = "<td style='mso-number-format:\"\\@\";'>" + s1 + "</td>";

            //mso-number-format:"\@";

            //日期转换
            string ReplaceReg = "<td>(?<key>[0-9]{1,4}/[0-9]{1,2}/[0-9]{1,2})</td>";
            Regex r = new Regex(ReplaceReg, RegexOptions.None);
            string[] i = System.Text.RegularExpressions.Regex.Split(_html, ReplaceReg);

            Match mc = r.Match(_html);
            string s = mc.Groups[1].Value;

            s = "<td style='mso-number-format:\"yyyy\\0022\\5E74\\0022m\\0022\\6708\\0022d\\0022\\65E5\\0022\\;\\@\"'>" + s + "</td>";

            _html = System.Text.RegularExpressions.Regex.Replace(_html, "<td>[0-9]{1,4}/[0-9]{1,2}/[0-9]{1,2}</td>", s);

            return System.Text.RegularExpressions.Regex.Replace(_html, "<td>[0]+\\w</td>", s1);

        }
        #endregion

        /// <summary>
        /// 將HTML串導出為Excel
        /// 撰寫人：Eric.Feng
        /// 時間：2008-06-11
        /// </summary>
        /// <param name="html"></param>
        public void OutHtmlToExcel(string html)
        {
            //Http輸出流對象
            System.Web.HttpResponse httpResponse = this.Page.Response;
            //設置輸出文件名稱
            httpResponse.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(FileName, System.Text.Encoding.UTF8));
            httpResponse.ContentEncoding = EEncoding;
            httpResponse.ContentType = "application/ms-excel";
            System.IO.StringWriter tw = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);

            string filePath = Page.Server.MapPath("..") + "\\" + FileName;

            System.IO.StreamWriter sw = System.IO.File.CreateText(filePath);

            // string s = FormatDateHTML(tw.ToString());
            sw.Write(html);
            sw.Close();

            //DownFile(httpResponse, FileName, filePath);
            httpResponse.Write(html);
            httpResponse.End();
        }
    }
}
