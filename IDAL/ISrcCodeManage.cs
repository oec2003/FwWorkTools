using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FW.WT.LinqDataModel;
using System.Linq.Expressions;

namespace FW.WT.IDAL
{
    public interface ISrcCodeManage : IRepository<SrcCodeManage>
    {
        int GetCount(Expression<Func<SrcCodeManage, bool>> exp);
        SrcCodeManage FindByID(int? id);
        IQueryable<SrcCodeManage> GetSource();
    }
}
