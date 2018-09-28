using HK.Pub.Externd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace cmvcdemo.Dal
{
    public class MyTest
    {
        static HK.Pub.Dal.Sql dbMachineDataCollection = new HK.Pub.Dal.Sql(HK.ConfigFile.ConnectiongString_Get("ShiMo"));

        static HK.Pub.Dal.Sql dbkanBanDataCollection = new HK.Pub.Dal.Sql(HK.ConfigFile.ConnectiongString_Get("KanBan"));
        public static string LshapeShow(string nowDate)
        {

            return dbMachineDataCollection.QueryDataTable(@"exec T_ScanRecord '','',@p1,@p2,1,30,10,100", new { p1 = nowDate, p2 = nowDate }).ToJson();

        }




        //返工看板数据

        public  static string FqcNg(string nowDate)
        {

            return dbMachineDataCollection.QueryDataTable(@"exec FqcNgDetail  '','1',@p1,@p2,'17',''", new { p1 = nowDate, p2 = nowDate }).ToJson();
        }

       
        //直通率获取数据
       public static string FqcRate(string nowDate)
        {

            return dbMachineDataCollection.QueryDataTable(@"EXEC T_Report_FqcRate @p1,'1'", new { p1 = nowDate}).ToJson(); 
        }

      public static string ProjectSch(string nowDate)
        {




            return dbMachineDataCollection.QueryDataTable(@"exec T_ProjectSchedule @p1,@p2,''", new { p1 = nowDate, p2 = nowDate }).ToJson();


        }



        public static string ModuleSch(string nowDate)
        {




            return dbMachineDataCollection.QueryDataTable(@"exec ModuleSchedule @p1,@p2", new { p1 = nowDate, p2 = nowDate }).ToJson();


        }


        //根据id查询数据库的看板地址



        public static string KanBanUrl(int id)
        {

           string url= (string)dbkanBanDataCollection.QuerySclar(@"select url from dbo.T_KanBan where Id=@p", new { p = id });

            return url;
        }













    }
}