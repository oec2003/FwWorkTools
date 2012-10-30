using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;


namespace FW.CommonFunction.Extensions
{
    public static class ListExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static string ToJson<T>(this List<T> list)
        {
            try
            {
                return JsonHelper.EntityListToJson<T>(list);
            }
            catch
            {
                return "";
            }
        }
    }
}
