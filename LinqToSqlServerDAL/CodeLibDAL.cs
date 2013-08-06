using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FW.CommonFunction;
using FW.WT.LinqDataModel;
using System.Linq.Expressions;

namespace FW.WT.LinqToSqlServerDAL
{
    public class CodeLibDAL:Repository<CodeLib>
    {
        private static string _conStr = ConfigInfo.GetConfigValue("ConStr");
        private static WTDataContext<CodeLib> _dataContext = new WTDataContext<CodeLib>(_conStr);

        public CodeLibDAL() : base(_dataContext) { }

        public int GetCount(Expression<Func<CodeLib, bool>> exp)
        {
            return FindAll(exp).Count();
        }

        public CodeLib FindByID(int? id)
        {
            var list = from c in FindAll(x => x.CodeLibID == id)
                       select c;
            return list.FirstOrDefault();
        }

        public IQueryable<CodeLib> GetSource()
        {
            IQueryable<CodeLib> codeLib = from c in _dataContext.CodeLib
                                        select c;
            return codeLib;
        }
    }
}
