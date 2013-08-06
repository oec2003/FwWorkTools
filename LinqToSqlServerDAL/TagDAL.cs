using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FW.CommonFunction;
using FW.WT.LinqDataModel;
using System.Linq.Expressions;

namespace FW.WT.LinqToSqlServerDAL
{
    public class TagDAL:Repository<Tags>
    {
        private static string _conStr = ConfigInfo.GetConfigValue("ConStr");
        private static WTDataContext<Tags> _dataContext = new WTDataContext<Tags>(_conStr);

        public TagDAL() : base(_dataContext) { }

        public int GetCount(Expression<Func<Tags, bool>> exp)
        {
            return FindAll(exp).Count();
        }

        public Tags FindByID(int? id)
        {
            var list = from c in FindAll(x => x.TagID == id)
                       select c;
            return list.FirstOrDefault();
        }

        public IQueryable<Tags> GetSource()
        {
            IQueryable<Tags> tags = from c in _dataContext.Tags
                                        select c;
            return tags;
        }
    }
}
