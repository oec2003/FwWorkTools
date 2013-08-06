using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FW.CommonFunction;
using FW.WT.LinqDataModel;
using System.Linq.Expressions;


namespace FW.WT.LinqToSqlServerDAL
{
    public class SrcCodeManageDAL:Repository<SrcCodeManage>
    {
        private static string _conStr = ConfigInfo.GetConfigValue("ConStr");
        private static WTDataContext<SrcCodeManage> _dataContext = new WTDataContext<SrcCodeManage>(_conStr);

        public SrcCodeManageDAL() : base(_dataContext) { }

        public int GetCount(Expression<Func<SrcCodeManage, bool>> exp)
        {
            return FindAll(exp).Count();
        }

        public SrcCodeManage FindByID(int? id)
        {
            var list = from c in FindAll(x => x.SrcCodeManageID == id)
                       select c;
            return list.FirstOrDefault();
        }

        public IQueryable<SrcCodeManage> GetSource()
        {
            IQueryable<SrcCodeManage> srcCodeManage = from c in _dataContext.SrcCodeManages
                                        select c;
            return srcCodeManage;
        }
    }
}
