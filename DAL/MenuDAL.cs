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
    public class MenuDAL:IMenu
    {
        public IList<Menu> GetMenuList(int pageSize, int pageindex, string sortField, string sortOrder, string condition)
        {
            List<Menu> list=new List<Menu>();

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
            using (IDataReader dataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionStringProfile,CommandType.StoredProcedure,"USP_Menu_Select",parameters))
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
        public Menu ReaderBind(IDataReader dataReader)
        {
            Menu menu = new Menu();
            object ojb;
            ojb = dataReader["MenuID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                menu.MenuID = (int)ojb;
            }
            menu.MenuName = dataReader["MenuName"].ToString();
            menu.MenuType = dataReader["MenuType"].ToString();
            ojb = dataReader["ParentID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                menu.ParentID = (int)ojb;
            }
            ojb = dataReader["IsShow"];
            if (ojb != null && ojb != DBNull.Value)
            {
                menu.IsShow = (bool)ojb;
            }
            ojb = dataReader["OrderBy"];
            if (ojb != null && ojb != DBNull.Value)
            {
                menu.OrderBy = (int)ojb;
            }
            menu.Url = dataReader["Url"].ToString();
            ojb = dataReader["GroupID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                menu.GroupID = (int)ojb;
            }
            return menu;
        }
    }
}
