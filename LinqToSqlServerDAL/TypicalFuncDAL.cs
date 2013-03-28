using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FW.CommonFunction;
using FW.WT.LinqDataModel;
using System.Linq.Expressions;


namespace FW.WT.LinqToSqlServerDAL
{
    public class TypicalFuncDAL : Repository<TypicalFunc>
    {
        private static string _conStr = ConfigInfo.GetConfigValue("ConStr");
        private static WTDataContext<TypicalFunc> _dataContext = new WTDataContext<TypicalFunc>(_conStr);

        public TypicalFuncDAL() : base(_dataContext) { }

        public int GetCount(Expression<Func<TypicalFunc, bool>> exp)
        {
            return FindAll(exp).Count();
        }

        public TypicalFunc FindByID(int? id)
        {
            var list = from c in FindAll(x => x.TypicalFuncID  == id)
                       select c;
            return list.FirstOrDefault();
        }

        public IQueryable<TypicalFunc> GetSource()
        {
            IQueryable<TypicalFunc> typicalFunc = from c in _dataContext.TypicalFuncs
                                        select c;
            return typicalFunc;
        }
    }
}
