using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FW.CommonFunction;
using FW.WT.LinqDataModel;
using System.Linq.Expressions;


namespace FW.WT.LinqToSqlServerDAL
{
    public class FeedBackLogDAL : Repository<FeedBackLog>
    {
        private static string _conStr = ConfigInfo.GetConfigValue("ConStr");
        private static WTDataContext<FeedBackLog> _dataContext = new WTDataContext<FeedBackLog>(_conStr);

        public FeedBackLogDAL() : base(_dataContext) { }

        public int GetCount(Expression<Func<FeedBackLog, bool>> exp)
        {
            return FindAll(exp).Count();
        }

        public int GetCount()
        {
            return GetSource().Count();
        }

        public FeedBackLog FindByID(int? id)
        {
            var list = from c in FindAll(x => x.FeedBackLogID == id)
                       select c;
            return list.FirstOrDefault();
        }

        public IQueryable<FeedBackLog> GetSource()
        {
            IQueryable<FeedBackLog> feedBackLog = from c in _dataContext.FeedBackLogs
                                        select c;
            return feedBackLog;
        }
    }
}
