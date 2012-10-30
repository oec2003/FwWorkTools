using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FW.CommonFunction;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI;
using Wuqi.Webdiyer;

namespace FW.WT.AdminPortal.Common
{
    public enum QueryType { Equal, Like, Greater, Less, GreaterEqual, LessEqual, LessEqualDate, Other, NoField, In, NotIn }

    public class QueryBasePage:BasePage
    {
        private StringBuilder _sbWhereClause = new StringBuilder();

        //protected 

        /// <summary>
        /// 数据列表控件
        /// </summary>
        public GridView DataControl { get; set; }
        /// <summary>
        /// 查询的表名称
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// 查询的表主键名称
        /// </summary>
        public string DataKeyField { get; set; }
        /// <summary>
        /// 默认排序字段
        /// </summary>
        public string DefaultSortField { get; set; }

        public void Query()
        { 
        
        }

        /// <summary>
        /// 获取查询条件
        /// </summary>
        /// <returns></returns>
        public string GetCondition()
        {
            _sbWhereClause.Append(" 1=1 ");
           
            //ControlCollection htmlForm = GetHtmlForm(Page);
            ControlCollection htmlForm = Page.Controls[0].FindControl("ContentPlaceHolderMain").Controls;
            if (htmlForm !=null)
            {
                try
                {
                    foreach (Control c in htmlForm)
                    {
                        if (!c.Visible) continue;
                        string id = c.ID;
                        if (string.IsNullOrEmpty(id)) continue;
                        string[] temp = id.Split('_');
                        if (temp.Length < 3) continue;
                        string fieldName = temp[1];//要查询的字段

                        if (temp.Length == 4)//指定了表名的字段
                        {
                            fieldName = temp[3] + "." + fieldName;
                        }
                        string queryType = temp[2];//查询类型
                        string queryValue = string.Empty;
                        switch (c.GetType().ToString())
                        {
                            case "System.Web.UI.WebControls.TextBox":
                                queryValue = ((TextBox)c).Text.Trim();
                                if (queryValue != "")
                                {
                                    AppendWhereClause(fieldName, queryValue, queryType);
                                }
                                break;
                            case "System.Web.UI.WebControls.DropDownList":
                                //如果Value为"noQuery"则不查询
                                if (!"NOQUERY".Equals(((DropDownList)c).SelectedItem.Value.ToUpper()))
                                {
                                    queryValue = ((DropDownList)c).SelectedItem.Value;
                                    AppendWhereClause(fieldName, queryValue, queryType);
                                }
                                break;

                            case "System.Web.UI.WebControls.CheckBox":
                                CheckBox cb = (CheckBox)c;
                                if (cb.Checked)
                                {
                                    queryValue = cb.ToolTip;
                                    if (queryValue.Length>0)
                                    {
                                        AppendWhereClause(fieldName, queryValue, queryType);
                                    }
                                }
                                break;
                            case "System.Web.UI.WebControls.RadioButtonList":
                                RadioButtonList rbl = (RadioButtonList)c;
                                if (rbl.SelectedIndex > -1)
                                {
                                    //如果Value为"noQuery"则不查询
                                    if (rbl.SelectedItem.Value.ToUpper() != "NOQUERY")
                                    {
                                        queryValue = rbl.SelectedItem.Value;
                                        AppendWhereClause(fieldName, queryValue, queryType);
                                    }

                                }
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    TraceLog.WriteLine("拼接查询条件时出错：" + ex.Message);
                    throw ex;
                }
            }
            return this._sbWhereClause.ToString();
        }

        /// <summary>
        /// 添加查询条件，固定的查询条件通过此方法添加
        /// </summary>
        /// <param name="fieldName">字段名</param>
        /// <param name="clause">查询的值</param>
        /// <param name="queryType">Equal/Like/Other</param>
        public void AppendWhereClause(string fieldName, string clause, QueryType queryType)
        {
            if (queryType == QueryType.NoField)
            {
                _sbWhereClause.Append(" AND (" + clause + ")");
            }
            else
            {
                fieldName = fieldName.ToUpper();
                if (fieldName.IndexOf('.') == -1)
                {
                    fieldName = this.TableName + "." + fieldName;
                }
                switch (queryType)
                {
                    case QueryType.Equal:
                        _sbWhereClause.Append(" AND (" + fieldName + " ='" + clause.Replace("'", "''") + "')");
                        break;
                    case QueryType.Like:
                        _sbWhereClause.Append(" AND (" + fieldName + " LIKE '%" + clause.Replace(" ", "%").Replace("*", "%").Replace("'", "''") + "%')");
                        break;
                    case QueryType.Greater:
                        _sbWhereClause.Append(" AND (" + fieldName + " > '" + clause.Replace("'", "''") + "')");
                        break;
                    case QueryType.Less:
                        _sbWhereClause.Append(" AND (" + fieldName + " < '" + clause.Replace("'", "''") + "')");
                        break;
                    case QueryType.GreaterEqual:
                        _sbWhereClause.Append(" AND (" + fieldName + " >= '" + clause.Replace("'", "''") + "')");
                        break;
                    case QueryType.LessEqual:
                        _sbWhereClause.Append(" AND (" + fieldName + " <= '" + clause.Replace("'", "''") + "')");
                        break;
                    case QueryType.LessEqualDate:
                        DateTime date = DateTime.Parse(clause);
                        if (date.TimeOfDay.Seconds == 0)
                        {
                            clause = date.AddDays(1).ToString("yyyy-MM-dd");
                        }
                        _sbWhereClause.Append(" AND (" + fieldName + " < '" + clause + "')");
                        break;
                    case QueryType.Other:
                        _sbWhereClause.Append(" AND (" + fieldName + " " + clause + ")");
                        break;
                    case QueryType.In:
                        _sbWhereClause.Append(" AND (" + fieldName + " IN (" + clause + "))");
                        break;
                    case QueryType.NotIn:
                        _sbWhereClause.Append(" AND (" + fieldName + " NOT IN (" + clause + "))");
                        break;
                }
            }

        }

        /// <summary>
        /// 根据查询类型添加查询条件
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="clause"></param>
        /// <param name="queryType"></param>
        private void AppendWhereClause(string fieldName, string clause, string queryType)
        {
            AppendWhereClause(fieldName, clause, ParsQueryType(queryType));
        }

        private QueryType ParsQueryType(string queryType)
        {
            try
            {
                return (QueryType)Enum.Parse(typeof(QueryType), queryType, true);
            }
            catch
            {
                throw new ApplicationException("QueryType不支持" + queryType);
            }
        }


        private ControlCollection GetHtmlForm(Control c)
        {
            ControlCollection result = null;
            if (c is HtmlForm)
            {
                return c.Controls;   
            }
            
            foreach (Control c1 in c.Controls)
            {
                if (!c1.HasControls()) continue;
                GetHtmlForm(c1);
            }
            
           
            return result;
        }
    }
}
