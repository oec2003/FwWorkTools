using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FW.CommonFunction;
using FW.WT.LinqDataModel;
using System.Linq.Expressions;

namespace FW.WT.LinqToSqlServerDAL
{
    public class CodeLibRelationTagDAL:Repository<CodeLibRelationTag>
    {
        private static string _conStr = ConfigInfo.GetConfigValue("ConStr");
        private static WTDataContext<CodeLibRelationTag> _dataContext = new WTDataContext<CodeLibRelationTag>(_conStr);

        public CodeLibRelationTagDAL() : base(_dataContext) { }

        public int GetCount(Expression<Func<CodeLibRelationTag, bool>> exp)
        {
            return FindAll(exp).Count();
        }
    }
}
