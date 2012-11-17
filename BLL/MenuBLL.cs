using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using FW.CommonFunction;
using FW.WT.LinqDataModel;
using FW.WT.LinqToSqlServerDAL;

namespace FW.WT.BLL
{
    public class MenuBLL
    {
        private static readonly MenuDAL dal = new MenuDAL();

        public IEnumerable<Menu> FindAll(Expression<Func<Menu, bool>> exp)
        {
            try
            {
                return dal.FindAll(exp);
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForApp("MenuBLL.FindAll", ex.ToString());
                throw ex;
            }
        }
    }
}
