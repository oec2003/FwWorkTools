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
    public class FeedBackLogBLL
    {
        private static readonly FeedBackLogDAL dal = new FeedBackLogDAL();

        public IEnumerable<FeedBackLog> FindAll(Expression<Func<FeedBackLog, bool>> exp)
        {
            try
            {
                return dal.FindAll(exp);
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForApp("FeedBackLogBLL.FindAll", ex.ToString());
                throw ex;
            }
        }

        public IEnumerable<FeedBackLog> FindAll(int pageSize, int pageIndex, Expression<Func<FeedBackLog, bool>> exp)
        {
            try
            {
                return dal.FindAll(pageSize, pageIndex, exp);
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForApp("FeedBackLogBLL.FindAll", ex.ToString());
                throw ex;
            }
        }

        public IEnumerable<FeedBackLog> FindAll(int pageSize, int pageIndex)
        {
            try
            {
                return dal.FindAll(pageSize, pageIndex);
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForApp("FeedBackLogBLL.FindAll", ex.ToString());
                throw ex;
            }
        }

        public IEnumerable<FeedBackLog> FindAll(IQueryable<FeedBackLog> source, int pageSize, int pageIndex, Expression<Func<FeedBackLog, bool>> exp)
        {
            try
            {
                return dal.FindAll(source, pageSize, pageIndex, exp);
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForApp("FeedBackLogBLL.FindAll", ex.ToString());
                throw ex;
            }
        }

        public FeedBackLog FindByID(int id)
        {
            try
            {
                return dal.FindByID(id);
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForApp("FeedBackLogBLL.FindByID", ex.ToString());
                throw ex;
            }
        }

        public int GetCount(Expression<Func<FeedBackLog, bool>> exp)
        {
            try
            {
                return dal.GetCount(exp);
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForApp("FeedBackLogBLL.GetCount", ex.ToString());
                throw ex;
            }
        }

        public int GetCount()
        {
            try
            {
                return dal.GetCount();
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForApp("FeedBackLogBLL.GetCount", ex.ToString());
                throw ex;
            }
        }

        public void Add(FeedBackLog srcCode)
        {
            try
            {
                dal.Add(srcCode);
                dal.Save();
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForApp("FeedBackLogBLL.Add", ex.ToString());
                throw ex;
            }
        }

        public void Update(FeedBackLog srcCode)
        {
            try
            {
                dal.Save();
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForApp("FeedBackLogBLL.Update", ex.ToString());
                throw ex;
            }
        }

        public void Delete(int id)
        {
            try
            {
                var srcCode = dal.FindByID(id);
                dal.Delete(srcCode);
                dal.Save();
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForApp("FeedBackLogBLL.Delete", ex.ToString());
                throw ex;
            }
        }

        public IQueryable<FeedBackLog> GetSource()
        {
            try
            {
                return dal.GetSource();
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForApp("FeedBackLogBLL.GetSource", ex.ToString());
                throw ex;
            }
        }
    }
}
