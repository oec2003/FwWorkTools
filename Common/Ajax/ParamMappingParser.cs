using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FW.CommonFunction
{
    /// <summary>
    /// 参数解析抽象类
    /// </summary>
    public abstract class ParamMappingParser
    {
        /// <summary>
        /// 在子类中重写时，返回参数解析结果
        /// </summary>
        /// <param name="paramValue">参数值</param>
        /// <returns>解析结果</returns>
        public abstract object ParseValue(string paramValue);
    }
    /// <summary>
    /// bool类型解析器
    /// </summary>
    public class BooleanParamMappingParser : ParamMappingParser
    {
        public override object ParseValue(string paramValue)
        {
            if (paramValue.Equals("0"))
                return false;
            if (paramValue.Equals("1"))
                return true;
            return bool.Parse(paramValue);
        }
    }
    /// <summary>
    /// 32位int类型解析器
    /// </summary>
    public class Int32ParamMappingParser : ParamMappingParser
    {
        public override object ParseValue(string paramValue)
        {
            return int.Parse(paramValue);
        }
    }
    /// <summary>
    /// 64位int类型解析器
    /// </summary>
    public class Int64ParamMappingParser : ParamMappingParser
    {
        public override object ParseValue(string paramValue)
        {
            return long.Parse(paramValue);
        }
    }
    /// <summary>
    /// float类型解析器
    /// </summary>
    public class FloatParamMappingParser : ParamMappingParser
    {
        public override object ParseValue(string paramValue)
        {
            return float.Parse(paramValue);
        }
    }
    /// <summary>
    /// double类型解析器
    /// </summary>
    public class DoubleParamMappingParser : ParamMappingParser
    {
        public override object ParseValue(string paramValue)
        {
            return double.Parse(paramValue);
        }
    }
    /// <summary>
    /// 枚举类型解析器
    /// </summary>
    /// <typeparam name="T">枚举类型</typeparam>
    public class EnumParamMappingParser<T> : ParamMappingParser
    {
        public EnumParamMappingParser()
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException(string.Format("{0}不是枚举类型", typeof(T).FullName));
            }
        }
        public override object ParseValue(string paramValue)
        {
            return Enum.Parse(typeof(T), paramValue);
        }
    }
    /// <summary>
    /// Guid类型解析器
    /// </summary>
    public class GuidParamMappingParser : ParamMappingParser
    {
        public override object ParseValue(string paramValue)
        {
            try
            {
                return new Guid(paramValue);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
