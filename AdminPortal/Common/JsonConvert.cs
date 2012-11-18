using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

using Newtonsoft.Json;
using FW.WT.LinqDataModel;

/// <summary>
/// 把对象转换成JSON对象
/// </summary>
public class JsonConvert<T> where T:class 
{
    /// <summary>
    ///系统树节点数据
    /// </summary>
    /// <param name="lsC">子信息</param>
    /// <param name="lsP">父信息</param>
    /// <returns></returns>
    public StringBuilder ToTreeNode(List<T> lsC, List<T> lsP)
    {
        StringBuilder jsonData = new StringBuilder();

        jsonData.Append("[");
        jsonData.Append(JsonConvert.SerializeObject(lsP[0]).ToLower());
        jsonData.Replace("}", ",");
        jsonData.Append("\"children\":[");

        foreach (T item in lsC)
        {
            string jsonGroup = JsonConvert.SerializeObject(item);
            jsonData.Append(jsonGroup.ToLower());
            jsonData.Append(",");
        }

        //去掉末尾“,”号
        if (lsC.Count > 0)
            jsonData = jsonData.Remove(jsonData.Length - 1, 1);

        jsonData.Append("]}]");
        jsonData.Replace("functionname", "text");
        jsonData.Replace("url", "attributes");

        return jsonData;
    }

    public StringBuilder ToCheckedTreeNode(List<T> lsC, List<string> lsP)
    {
        StringBuilder jsonData = new StringBuilder();

        jsonData.Append("[{");

        foreach (string str in lsP)
        {
            jsonData.Append("\"id\":");
            jsonData.Append("0");
            jsonData.Append(",");
            jsonData.Append("\"text\":");
            jsonData.Append(str);
            jsonData.Append(",");
            jsonData.Append("\"children\":[");
        }

        return jsonData;
    }

    /// <summary>
    /// 转换成datagrid格式的数据
    /// </summary>
    /// <param name="ls">数据集合</param>
    /// <param name="total">总数</param>
    /// <returns></returns>
    public StringBuilder ToDataGrid(List<T> ls, int total)
    {
        StringBuilder jsonData = new StringBuilder();

        jsonData.Append("{");
        jsonData.Append("\"total\":");
        jsonData.Append(total);
        jsonData.Append(",");
        jsonData.Append("\"rows\":");
        jsonData.Append("[");

        foreach (T item in ls)
        {
            string jsonGroup = JsonConvert.SerializeObject(item);
            jsonData.Append(jsonGroup);
            jsonData.Append(",");
        }

        if (ls.Count > 0)
            jsonData = jsonData.Remove(jsonData.Length - 1, 1);

        jsonData.Append("]}");

        return jsonData;
    }

    /// <summary>
    /// 转换成状态格式的JSON数据
    /// </summary>
    /// <param name="status"></param>
    /// <returns></returns>
    public StringBuilder ToStatus(int status)
    {
        StringBuilder jsonData = new StringBuilder();

        jsonData.Append("{");
        jsonData.Append("\"Flag\":");
        jsonData.Append("[{");
        jsonData.Append("\"Status\":");
        jsonData.Append(status);
        jsonData.Append("}]");
        jsonData.Append("}");
        return jsonData;
    }

}


