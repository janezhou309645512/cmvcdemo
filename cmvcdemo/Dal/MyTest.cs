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


        public static string LshapeShow()
        {

            return dbMachineDataCollection.QueryDataTable(@"exec T_ScanRecord '','','2018-09-14','2018-09-14',1,30,10,100").ToJson();

        }




        //返工看板数据

        public  static string FqcNg()
        {

            return dbMachineDataCollection.QueryDataTable(@"exec FqcNgDetail  '','1','2018-09-19','2018-09-19','',''").ToJson();
        }

       
        //直通率获取数据
       public static string FqcRate()
        {

            return dbMachineDataCollection.QueryDataTable(@"EXEC T_Report_FqcRate '2018-07-26','1'").ToJson(); 
        }


















    }
}