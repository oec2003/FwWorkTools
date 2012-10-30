using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FW.CommonFunction;
using FW.WT.LinqDataModel;
using FW.WT.IDAL;
using System.Linq.Expressions;

namespace FW.WT.BLL
{
    public class SrcCodeManageBLL
    {
        private static readonly ISrcCodeManage dal = DALFactory.CreateSrcCodeManage();

        public IEnumerable<SrcCodeManage> FindAll(Expression<Func<SrcCodeManage, bool>> exp)
        {
            try
            {
                return dal.FindAll(exp);
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForApp("SrcCodeManageBLL.FindAll", ex.ToString());
                throw ex;
            }
        }

        public IEnumerable<SrcCodeManage> FindAll(int pageSize, int pageIndex, Expression<Func<SrcCodeManage, bool>> exp)
        {
            try
            {
                return dal.FindAll(pageSize, pageIndex, exp);
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForApp("SrcCodeManageBLL.FindAll", ex.ToString());
                throw ex;
            }
        }

        public IEnumerable<SrcCodeManage> FindAll(IQueryable<SrcCodeManage> source, int pageSize, int pageIndex, Expression<Func<SrcCodeManage, bool>> exp)
        {
            try
            {
                return dal.FindAll(source, pageSize, pageIndex, exp);
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForApp("SrcCodeManageBLL.FindAll", ex.ToString());
                throw ex;
            }
        }

        public SrcCodeManage FindByID(int id)
        {
            try
            {
                return dal.FindByID(id);
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForApp("SrcCodeManageBLL.FindByID", ex.ToString());
                throw ex;
            }
        }

        public int GetCount(Expression<Func<SrcCodeManage, bool>> exp)
        {
            try
            {
                return dal.GetCount(exp);
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForApp("SrcCodeManageBLL.GetCount", ex.ToString());
                throw ex;
            }
        }

        public void Add(SrcCodeManage srcCode)
        {
            try
            {
                dal.Add(srcCode);
                dal.Save();
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForApp("SrcCodeManageBLL.Add", ex.ToString());
                throw ex;
            }
        }

        public void Update(SrcCodeManage srcCode)
        {
            try
            {
                dal.Save();
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForApp("SrcCodeManageBLL.Update", ex.ToString());
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
                ErrorHandler.ExceptionHandlerForApp("SrcCodeManageBLL.Delete", ex.ToString());
                throw ex;
            }
        }

        public IQueryable<SrcCodeManage> GetSource()
        {
            try
            {
                return dal.GetSource();
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForApp("SrcCodeManageBLL.GetSource", ex.ToString());
                throw ex;
            }
        }
    }
}
