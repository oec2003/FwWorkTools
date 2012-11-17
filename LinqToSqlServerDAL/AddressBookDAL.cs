using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FW.CommonFunction;
using FW.WT.LinqDataModel;
using System.Linq.Expressions;

namespace FW.WT.LinqToSqlServerDAL
{
    public class AddressBookDAL:Repository<AddressBook>
    {
        private static string _conStr = ConfigInfo.GetConfigValue("ConStr");
        private static WTDataContext<AddressBook> _dataContext = new WTDataContext<AddressBook>(_conStr);

        public AddressBookDAL() : base(_dataContext) { }

        public int GetCount(Expression<Func<AddressBook, bool>> exp)
        {
            return FindAll(exp).Count();
        }

        public AddressBook FindByID(int? id)
        {
            var list = from c in FindAll(x => x.AddressBookID == id)
                       select c;
            return list.FirstOrDefault();
        }

        public IQueryable<AddressBook> GetSource()
        {
            IQueryable<AddressBook> addressBook = from c in _dataContext.AddressBooks
                                        select c;
            return addressBook;
        }
    }
}
