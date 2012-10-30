using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FW.TrainingManage.DataModel
{
    /// <summary>
    /// 实体类Menu 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class Menu
    {
        public Menu()
        { }
        #region Model
        private int _menuid;
        private string _menuname;
        private string _menutype;
        private int? _parentid;
        private bool _isshow;
        private int? _orderby;
        private string _url;
        private int? _groupid;
        /// <summary>
        /// 
        /// </summary>
        public int MenuID
        {
            set { _menuid = value; }
            get { return _menuid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string MenuName
        {
            set { _menuname = value; }
            get { return _menuname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string MenuType
        {
            set { _menutype = value; }
            get { return _menutype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? ParentID
        {
            set { _parentid = value; }
            get { return _parentid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IsShow
        {
            set { _isshow = value; }
            get { return _isshow; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? OrderBy
        {
            set { _orderby = value; }
            get { return _orderby; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Url
        {
            set { _url = value; }
            get { return _url; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? GroupID
        {
            set { _groupid = value; }
            get { return _groupid; }
        }
        #endregion Model

    }
}
