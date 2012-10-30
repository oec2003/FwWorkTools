using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FW.CommonFunction;
using System.Reflection;

namespace FW.WT.IDAL
{
    public class DALFactory
    {
        private static readonly string _path = ConfigInfo.GetConfigValue("DALPath");

        public static IAddressBooks CreateAddressBook()
        {
            string className = _path + ".AddressBookDAL";
            return (IAddressBooks)Assembly.Load(_path).CreateInstance(className);
        }

        public static ISrcCodeManage CreateSrcCodeManage()
        {
            string className = _path + ".SrcCodeManageDAL";
            return (ISrcCodeManage)Assembly.Load(_path).CreateInstance(className);
        }

        public static IMenu CreateMenu()
        {
            string className = _path + ".MenuDAL";
            return (IMenu)Assembly.Load(_path).CreateInstance(className);
        }
    }
}
