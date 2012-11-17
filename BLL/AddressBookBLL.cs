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
    public class AddressBookBLL
    {
        private static readonly AddressBookDAL dal = new AddressBookDAL();
        
        public IEnumerable<AddressBook> FindAll(Expression<Func<AddressBook, bool>> exp)
        {
            try
            {
                return dal.FindAll(exp);
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForApp("AddressBookBLL.FindAll", ex.ToString());
                throw ex;
            }
        }

        public IEnumerable<AddressBook> FindAll(int pageSize, int pageIndex, Expression<Func<AddressBook, bool>> exp)
        {
            try
            {
                return dal.FindAll(pageSize, pageIndex, exp);
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForApp("AddressBookBLL.FindAll", ex.ToString());
                throw ex;
            }
        }

        public IEnumerable<AddressBook> FindAll(IQueryable<AddressBook> source, int pageSize, int pageIndex, Expression<Func<AddressBook, bool>> exp)
        {
            try
            {
                return dal.FindAll(source, pageSize, pageIndex, exp);
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForApp("AddressBookBLL.FindAll", ex.ToString());
                throw ex;
            }
        }

        public AddressBook FindByID(int id)
        {
            try
            {
                return dal.FindByID(id);
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForApp("AddressBookBLL.FindByID", ex.ToString());
                throw ex;
            }
        }

        public int GetCount(Expression<Func<AddressBook, bool>> exp)
        {
            try
            {
                return dal.GetCount(exp);
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForApp("AddressBookBLL.GetCount", ex.ToString());
                throw ex;
            }
        }

        public void Add(AddressBook course)
        {
            try
            {
                dal.Add(course);
                dal.Save();
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForApp("AddressBookBLL.Add", ex.ToString());
                throw ex;
            }
        }

        public void Update(AddressBook course)
        {
            try
            {
                dal.Save();
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForApp("AddressBookBLL.Update", ex.ToString());
                throw ex;
            }
        }

        public void Delete(int id)
        {
            try
            {
                var course = dal.FindByID(id);
                dal.Delete(course);
                dal.Save();
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForApp("AddressBookBLL.Delete", ex.ToString());
                throw ex;
            }
        }

        public IQueryable<AddressBook> GetSource()
        {
            try
            {
                return dal.GetSource();
            }
            catch (Exception ex)
            {
                ErrorHandler.ExceptionHandlerForApp("AddressBookBLL.GetSource", ex.ToString());
                throw ex;
            }
        }
    
    }
}
