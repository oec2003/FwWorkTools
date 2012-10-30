using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Expressions;

namespace FW.CommonFunction
{
    /// <summary>
    /// 自定义参数映射属性类
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class ParamMappingAttribute : System.Attribute
    {

        /// <summary>
        /// 创建参数映射属性对象
        /// </summary>
        /// <param name="paramName">映射参数名称</param>
        /// <param name="parserType">解析器类型</param>
        /// <param name="isRequired">参数是否必须</param>
        public ParamMappingAttribute(string paramName, Type parserType, bool isRequired)
        {
            this.ParamName = paramName;
            this.ParserType = parserType;
            this.IsRequired = isRequired;
        }
        /// <summary>
        /// 创建参数映射属性对象
        /// </summary>
        /// <param name="paramName">映射参数名称</param>
        /// <param name="parserType">解析器类型</param>
        public ParamMappingAttribute(string paramName, Type parserType) : this(paramName, parserType, false) { }
        /// <summary>
        /// 创建参数映射属性对象
        /// </summary>
        /// <param name="parserType">解析器类型</param>
        /// <param name="isRequired">参数是否必须</param>
        public ParamMappingAttribute(Type parserType, bool isRequired) : this(null, parserType, isRequired) { }
        /// <summary>
        /// 创建参数映射属性对象
        /// </summary>
        /// <param name="paramName">映射参数名称</param>
        /// <param name="isRequired">参数是否必须</param>
        public ParamMappingAttribute(string paramName, bool isRequired) : this(paramName, null, isRequired) { }
        /// <summary>
        /// 创建参数映射属性对象
        /// </summary>
        /// <param name="paramName">映射参数名称</param>
        public ParamMappingAttribute(string paramName) : this(paramName, null, false) { }
        /// <summary>
        /// 创建参数映射属性对象
        /// </summary>
        /// <param name="parserType">解析器类型</param>
        public ParamMappingAttribute(Type parserType) : this(null, parserType, false) { }
        /// <summary>
        /// 创建参数映射属性对象
        /// </summary>
        /// <param name="isRequired">参数是否必须</param>
        public ParamMappingAttribute(bool isRequired) : this(null, null, isRequired) { }
        /// <summary>
        /// 创建参数映射属性对象
        /// </summary>
        public ParamMappingAttribute() : this(null, null, false) { }



        private static Dictionary<Type, ParamMappingParser> _parserCache;
        private Type _parserType;

        /// <summary>
        /// 解析器对象全局缓存
        /// </summary>
        protected static Dictionary<Type, ParamMappingParser> ParserCache
        {
            get
            {
                if (_parserCache == null)
                {
                    _parserCache = new Dictionary<Type, ParamMappingParser>();
                }
                return _parserCache;
            }
        }
        /// <summary>
        /// 映射参数名称
        /// </summary>
        public string ParamName { get; set; }
        /// <summary>
        /// 对应参数是否必须（如果设置为true且未找到指定页面参数将抛出异常）
        /// </summary>
        public bool IsRequired { get; set; }
        /// <summary>
        /// 解析器类型
        /// </summary>
        public Type ParserType
        {
            get
            {
                return _parserType;
            }
            set
            {
                if (value != null)
                {
                    if (!value.IsSubclassOf(typeof(ParamMappingParser)))
                    {
                        var message = String.Format("{0}类型错误,解析类必须继承自{1}.", value.FullName, typeof(ParamMappingParser).FullName);
                   
                        throw new ArgumentException(message);
                    }
                }
                _parserType = value;
            }
        }
        /// <summary>
        /// 获取解析器
        /// </summary>
        /// <returns>解析器实例</returns>
        public ParamMappingParser GetParser()
        {
            //如果没有指定解析器类型则返回空
            if (this.ParserType == null)
                return null;

            ParamMappingParser parser;
            //先从缓存读取，如果没有则实例化并缓存再返回
            if (ParserCache.ContainsKey(this.ParserType))
            {
                parser = ParserCache[this.ParserType];
            }
            else
            {
                parser = (ParamMappingParser)Activator.CreateInstance(this.ParserType);
                ParserCache.Add(this.ParserType, parser);
            }
            return parser;
        }
    }
}
