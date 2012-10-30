using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using FW.CommonFunction;
using FW.WT.LinqDataModel;
using FW.WT.IDAL;

namespace FW.WT.LinqToSqlServerDAL
{
    public class MenuDAL:Repository<Menu>,IMenu
    {
        private static string _conStr = ConfigInfo.GetConfigValue("ConStr");
        private static WTDataContext<Menu> _dataContext = new WTDataContext<Menu>(_conStr);

        public MenuDAL() : base(_dataContext) { }
    }
}
