using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FW.CommonFunction;
using FW.WT.LinqDataModel;
using System.Linq.Expressions;
using FW.WT.LinqToSqlServerDAL;


namespace FW.WT.BLL
{
    public class TypicalFuncBLL
    {
        private static readonly TypicalFuncDAL dal = new TypicalFuncDAL();

        public IEnumerable<TypicalFunc> FindAll(Expression<Func<TypicalFunc, bool>> exp)
        {
            try
            {
                return dal.FindAll(exp);
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForApp("TypicalFuncBLL.FindAll", ex.ToString());
                throw ex;
            }
        }

        public IEnumerable<TypicalFunc> FindAll(int pageSize, int pageIndex, Expression<Func<TypicalFunc, bool>> exp)
        {
            try
            {
                return dal.FindAll(pageSize, pageIndex, exp);
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForApp("TypicalFuncBLL.FindAll", ex.ToString());
                throw ex;
            }
        }

        public IEnumerable<TypicalFunc> FindAll(IQueryable<TypicalFunc> source, int pageSize, int pageIndex, Expression<Func<TypicalFunc, bool>> exp)
        {
            try
            {
                return dal.FindAll(source, pageSize, pageIndex, exp);
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForApp("TypicalFuncBLL.FindAll", ex.ToString());
                throw ex;
            }
        }

        public TypicalFunc FindByID(int id)
        {
            try
            {
                return dal.FindByID(id);
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForApp("TypicalFuncBLL.FindByID", ex.ToString());
                throw ex;
            }
        }

        public int GetCount(Expression<Func<TypicalFunc, bool>> exp)
        {
            try
            {
                return dal.GetCount(exp);
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForApp("TypicalFuncBLL.GetCount", ex.ToString());
                throw ex;
            }
        }

        public void Add(TypicalFunc typicalFunc)
        {
            try
            {
                dal.Add(typicalFunc);
                dal.Save();
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForApp("TypicalFuncBLL.Add", ex.ToString());
                throw ex;
            }
        }

        public void Update(TypicalFunc srcCode)
        {
            try
            {
                dal.Save();
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForApp("TypicalFuncBLL.Update", ex.ToString());
                throw ex;
            }
        }

        public void Delete(int id)
        {
            try
            {
                var typicalFunc = dal.FindByID(id);
                if (typicalFunc != null)
                {
                    dal.Delete(typicalFunc);
                    dal.Save();
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForApp("TypicalFuncBLL.Delete", ex.ToString());
                throw ex;
            }
        }

        public IQueryable<TypicalFunc> GetSource()
        {
            try
            {
                return dal.GetSource();
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForApp("TypicalFuncBLL.GetSource", ex.ToString());
                throw ex;
            }
        }
    }
}
