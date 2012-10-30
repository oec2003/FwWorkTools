using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FW.CommonFunction;
using FW.WT.LinqDataModel;
using System.Linq.Expressions;
namespace TestConsoleApp
{
    public class Program
    {
        private static string _conStr = ConfigInfo.GetConfigValue("ConStr");
        private static WTDataContext<SrcCodeManage> tm = new WTDataContext<SrcCodeManage>(_conStr);
        static void Main(string[] args)
        {
            Ts();
            Console.ReadLine();
              
        }
        private static void Print(string s)
        {
            Console.WriteLine(s);
        }

        public static void Ts()
        {
            IQueryable<SrcCodeManage> query = from c in tm.SrcCodeManages
                                       select c;

            foreach (SrcCodeManage cc in query)
            {
                Print(cc.SrcCodeManageID.ToString());
                Print(cc.Remark);
                Print(cc.ProjVersion);
                Print(cc.ServerName.ToString());
                Print(cc.ServerPort.ToString());

            }
                                     
        }
    }

    

    public class Point
    {
        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
        public int X { get; set; }
        public int Y { get; set; }
    }
}



