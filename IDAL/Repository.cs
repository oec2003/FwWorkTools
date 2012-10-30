using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FW.WT.LinqDataModel;
using System.Linq.Expressions;

namespace FW.WT.IDAL
{
    public class Repository<T>:IRepository<T> where T:class
    {
        protected WTDataContext<T> _dataContext;

        public Repository(WTDataContext<T> dataContext)
        {
            this._dataContext = dataContext;
           
        }

        public IEnumerable<T> FindAll(int pageSize,int pageIndex,Expression<Func<T,bool>> exp)
        {
            return _dataContext.GetTable<T>().Where(exp).Skip((pageIndex-1)*pageSize).Take(pageSize);
        }

        public IEnumerable<T> FindAll(Expression<Func<T, bool>> exp)
        {
            return _dataContext.GetTable<T>().Where(exp);
        }

        public IEnumerable<T> FindAll(IQueryable<T> source, int pageSize, int pageIndex, Expression<Func<T, bool>> exp)
        {
            return source.Where(exp).Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        public void Add(T entity)
        {
            _dataContext.GetTable<T>().InsertOnSubmit(entity);
        }

        public void Delete(T entity)
        {
            _dataContext.GetTable<T>().DeleteOnSubmit(entity);
        }

        public void Save()
        {
            _dataContext.SubmitChanges();
        }
       
    }
}
