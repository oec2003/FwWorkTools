using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FW.WT.LinqDataModel;
using System.Linq.Expressions;


namespace FW.WT.IDAL
{
    public interface IAddressBooks : IRepository<AddressBook>
    {
        int GetCount(Expression<Func<AddressBook, bool>> exp);
        AddressBook FindByID(int? id);
        IQueryable<AddressBook> GetSource();
    }
}
