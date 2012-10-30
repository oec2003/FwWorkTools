using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FW.TrainingManage.DataModel
{
   	/// <summary>
	/// 实体类Grade 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
    [Serializable]
    public class Grade
    {
        public Grade()
        { }
        #region Model
        private int _gradeid;
        private string _gradename;
        private string _status;
        private string _description;
        private DateTime? _createtime;
        private DateTime? _modifytime;
        /// <summary>
        /// 
        /// </summary>
        public int GradeID
        {
            set { _gradeid = value; }
            get { return _gradeid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string GradeName
        {
            set { _gradename = value; }
            get { return _gradename; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Status
        {
            set { _status = value; }
            get { return _status; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Description
        {
            set { _description = value; }
            get { return _description; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? CreateTime
        {
            set { _createtime = value; }
            get { return _createtime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? ModifyTime
        {
            set { _modifytime = value; }
            get { return _modifytime; }
        }
        #endregion Model
    }
}
