using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Specialized;
using System.Reflection;

namespace FW.CommonFunction
{
    /// <summary>
    /// 参数映射绑定类
    /// </summary>
    public class ParamMappinger:IDisposable
    {
       
        /// <summary>
        /// 绑定对象
        /// </summary>
        public object MappingObject { get; set; }
        /// <summary>
        /// 参数集合
        /// </summary>
        public NameValueCollection RequestParams { get; set; }
        /// <summary>
        /// 创建一个参数映射绑定类实例
        /// </summary>
        /// <param name="mappingObject">绑定对象</param>
        /// <param name="requestParams">参数集合</param>
        public ParamMappinger(object mappingObject, NameValueCollection requestParams) 
        {
            this.MappingObject = mappingObject;
            this.RequestParams = requestParams;
        }
        /// <summary>
        /// 进行参数映射绑定
        /// </summary>
        public void Mapping()
        {
            ParamMappinger.Mapping(this.MappingObject, this.RequestParams);
        }
        /// <summary>
        /// 进行参数映射绑定
        /// </summary>
        /// <param name="obj">绑定对象</param>
        /// <param name="paramcllt">参数集合</param>
        public static void Mapping(object obj, NameValueCollection paramcllt)
        {
            Type t = obj.GetType();
            foreach (PropertyInfo pi in t.GetProperties())
            {
                //遍历寻找需要映射的属性
                Type valuetype = pi.PropertyType;
                object[] attrs = pi.GetCustomAttributes(typeof(ParamMappingAttribute), false);
                if (attrs.Length == 1)
                {
                    //获取set方法，如果没有定义则抛出异常
                    MethodInfo setfunc = pi.GetSetMethod();
                    if (setfunc != null)
                    {
                        ParamMappingAttribute mapping = attrs[0] as ParamMappingAttribute;
                        //参数名称，如果没有定义则默认属性名称
                        string paramname = mapping.ParamName == null ? pi.Name : mapping.ParamName;
                        //根据参数名称取得参数值
                        string paramvalue = System.Web.HttpUtility.UrlDecode(paramcllt[paramname]);
                        if (paramvalue == null)
                        {
                            //没有找到页面参数，且此参数为必需，抛出异常
                            if (mapping.IsRequired)
                            {
                              
                                throw new ArgumentNullException(string.Format("未提供页面参数{0}。", paramname));
                            }
                            else
                            {
                                continue;
                            }
                        }
                        //获取解析器解析参数值，调用set方法将结果赋值给属性
                        ParamMappingParser parser = mapping.GetParser();
                        object setvalue = parser == null ? paramvalue : parser.ParseValue(paramvalue);
                        setfunc.Invoke(obj, new object[] { setvalue });
                    }
                    else
                    {
                      
                        throw new MemberAccessException(string.Format("未定义属性的{0}公共set方法，无法进行赋值操作。", pi.Name));
                    }
                }
            }
        }

        #region IDisposable 成员
        /// <summary>
        /// 注销当前实例
        /// </summary>
        public void Dispose()
        {
            this.MappingObject = null;
            this.RequestParams = null;
        }

        #endregion
    }
}
