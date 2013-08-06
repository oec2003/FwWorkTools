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
    public class CodeLibBLL
    {
        private static readonly CodeLibDAL dal = new CodeLibDAL();

        public IEnumerable<CodeLib> FindAll(Expression<Func<CodeLib, bool>> exp)
        {
            try
            {
                return dal.FindAll(exp);
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForApp("CodeLibBLL.FindAll", ex.ToString());
                throw ex;
            }
        }

        public IEnumerable<CodeLib> FindAll(int pageSize, int pageIndex, Expression<Func<CodeLib, bool>> exp)
        {
            try
            {
                return dal.FindAll(pageSize, pageIndex, exp);
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForApp("CodeLibBLL.FindAll", ex.ToString());
                throw ex;
            }
        }

        public IEnumerable<CodeLib> FindAll(IQueryable<CodeLib> source, int pageSize, int pageIndex, 
                Expression<Func<CodeLib, bool>> exp)
        {
            try
            {
                return dal.FindAll(source, pageSize, pageIndex, exp);
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForApp("CodeLibBLL.FindAll", ex.ToString());
                throw ex;
            }
        }

        public CodeLib FindByID(int id)
        {
            try
            {
                return dal.FindByID(id);
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForApp("CodeLibBLL.FindByID", ex.ToString());
                throw ex;
            }
        }

        public int GetCount(Expression<Func<CodeLib, bool>> exp)
        {
            try
            {
                return dal.GetCount(exp);
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForApp("CodeLibBLL.GetCount", ex.ToString());
                throw ex;
            }
        }

        public void Add(CodeLib codeLib)
        {
            try
            {
                dal.Add(codeLib);
                dal.Save();
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForApp("CodeLibBLL.Add", ex.ToString());
                throw ex;
            }
        }

        public void Update(CodeLib codeLib)
        {
            try
            {
                dal.Save();
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForApp("CodeLibBLL.Update", ex.ToString());
                throw ex;
            }
        }

        public void Delete(int id)
        {
            try
            {
                var codeLib = dal.FindByID(id);
                if (codeLib != null)
                {
                    dal.Delete(codeLib);
                    dal.Save();
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForApp("CodeLibBLL.Delete", ex.ToString());
                throw ex;
            }
        }

        public IQueryable<CodeLib> GetSource()
        {
            try
            {
                return dal.GetSource();
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForApp("CodeLibBLL.GetSource", ex.ToString());
                throw ex;
            }
        }

        public string GetCodeContent()
        {
            string codeContent = "";

            return codeContent;
        }
    }
}
