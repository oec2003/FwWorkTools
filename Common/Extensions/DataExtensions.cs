using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FW.CommonFunction
{
    /// <summary>
    /// 数据类型的扩展方法类
    /// </summary>
    public static class DataExtensions
    {
        /// <summary>
        /// 判断DataSet中是否有数据 返回true 表示有数据
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static Boolean HasData(this DataSet ds)
        {
            Boolean result=false;
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                result = true;
            }
            return result;
        }

        /// <summary>
        /// 判断DataTable中是否有数据 返回true 表示有数据
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static Boolean HasData(this DataTable dt)
        {
            Boolean result = false;
            if (dt != null && dt.Rows.Count > 0)
            {
                result = true;
            }
            return result;
        }
    }
}
