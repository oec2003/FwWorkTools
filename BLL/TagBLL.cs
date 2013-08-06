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
    public class TagBLL
    {
        private static readonly TagDAL dal = new TagDAL();

        public IEnumerable<Tags> FindAll(Expression<Func<Tags, bool>> exp)
        {
            try
            {
                return dal.FindAll(exp);
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForApp("TagBLL.FindAll", ex.ToString());
                throw ex;
            }
        }

        public IEnumerable<Tags> FindAll(int pageSize, int pageIndex, Expression<Func<Tags, bool>> exp)
        {
            try
            {
                return dal.FindAll(pageSize, pageIndex, exp);
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForApp("TagBLL.FindAll", ex.ToString());
                throw ex;
            }
        }

        public IEnumerable<Tags> FindAll(IQueryable<Tags> source, int pageSize, int pageIndex,
                Expression<Func<Tags, bool>> exp)
        {
            try
            {
                return dal.FindAll(source, pageSize, pageIndex, exp);
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForApp("TagBLL.FindAll", ex.ToString());
                throw ex;
            }
        }

        public Tags FindByID(int id)
        {
            try
            {
                return dal.FindByID(id);
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForApp("TagBLL.FindByID", ex.ToString());
                throw ex;
            }
        }

        public int GetCount(Expression<Func<Tags, bool>> exp)
        {
            try
            {
                return dal.GetCount(exp);
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForApp("TagBLL.GetCount", ex.ToString());
                throw ex;
            }
        }

        public void Add(Tags tags)
        {
            try
            {
                dal.Add(tags);
                dal.Save();
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForApp("TagBLL.Add", ex.ToString());
                throw ex;
            }
        }

        public void Update(Tags tags)
        {
            try
            {
                dal.Save();
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForApp("TagBLL.Update", ex.ToString());
                throw ex;
            }
        }

        public void Delete(int id)
        {
            try
            {
                var tags = dal.FindByID(id);
                if (tags != null)
                {
                    dal.Delete(tags);
                    dal.Save();
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForApp("TagBLL.Delete", ex.ToString());
                throw ex;
            }
        }

        public IQueryable<Tags> GetSource()
        {
            try
            {
                return dal.GetSource();
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForApp("TagBLL.GetSource", ex.ToString());
                throw ex;
            }
        }
    }
}
