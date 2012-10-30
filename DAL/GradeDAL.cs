using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using FW.CommonFunction;
using FW.DBHelper;
using FW.TrainingManage.DataModel;
using FW.TrainingManage.IDAL;

namespace FW.TrainingManage.SqlServerDAL
{
    public class GradeDAL:IGrade
    {
        public IList<Grade> GetGradeList(int pageSize, int pageindex, string sortField, string sortOrder, string condition)
        {
            List<Grade> list = new List<Grade>();

            SqlParameter[] parameters = new SqlParameter[5];
            parameters[0] = new SqlParameter("@Pagesize", SqlDbType.Int, 4);
            parameters[0].Value = pageSize;

            parameters[1] = new SqlParameter("@Pageindex", SqlDbType.Int, 4);
            parameters[1].Value = pageindex;

            parameters[2] = new SqlParameter("@SortField", SqlDbType.VarChar, 100);
            parameters[2].Value = sortField;

            parameters[3] = new SqlParameter("@SortOrder", SqlDbType.VarChar, 20);
            parameters[3].Value = sortOrder;

            parameters[4] = new SqlParameter("@Condition", SqlDbType.VarChar, 8000);
            parameters[4].Value = condition;
            using (IDataReader dataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionStringProfile, CommandType.StoredProcedure, "USP_Grade_Select", parameters))
            {
                while (dataReader.Read())
                {
                    list.Add(ReaderBind(dataReader));
                }
            }
            return list;
        }

        /// <summary>
        /// 对象实体绑定数据
        /// </summary>
        public Grade ReaderBind(IDataReader dataReader)
        {
            Grade grade = new Grade();
            object ojb;
            ojb = dataReader["GradeID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                grade.GradeID = (int)ojb;
            }
            grade.GradeName = dataReader["GradeName"].ToString();
            grade.Status = dataReader["Status"].ToString();
            grade.Description = dataReader["Description"].ToString();
            ojb = dataReader["CreateTime"];
            if (ojb != null && ojb != DBNull.Value)
            {
                grade.CreateTime = (DateTime)ojb;
            }
            ojb = dataReader["ModifyTime"];
            if (ojb != null && ojb != DBNull.Value)
            {
                grade.ModifyTime = (DateTime)ojb;
            }
            return grade;
        }

    }
}
