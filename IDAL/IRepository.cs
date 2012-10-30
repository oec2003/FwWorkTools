using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FW.WT.LinqDataModel;
using System.Linq.Expressions;

namespace FW.WT.IDAL
{
    public interface IRepository<T> where T:class
    {
        IEnumerable<T> FindAll(int pageSize,int pageIndex,Expression<Func<T, bool>> exp);
        IEnumerable<T> FindAll(IQueryable<T> source, int pageSize, int pageIndex, Expression<Func<T, bool>> exp);
        IEnumerable<T> FindAll(Expression<Func<T, bool>> exp);
        void Add(T entity);
        void Delete(T entity);
        void Save();
    }
}
